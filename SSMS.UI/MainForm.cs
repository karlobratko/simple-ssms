using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

using Nito.AsyncEx;

using SSMS.UI.DAL;
using SSMS.UI.Helpers;
using SSMS.UI.Models;

namespace SSMS.UI;

public partial class MainForm : Form {

  private LoginInformation? _loginInformation;
  private AsyncLazy<IList<Database>> Databases = new(async () => await RepositoryFactory.Repository.GetDatabasesAsync().ToArrayAsync());

  public MainForm() => InitializeComponent();

  private void MainForm_Shown(object sender, EventArgs e) => ShowLoginPopup();

  private void ConnectObjectExplorer_Click(object sender, EventArgs e) => ShowLoginPopup();

  private void Disconnect_Click(object sender, EventArgs e) {
    _loginInformation = null;
    DdlDatabases.Items.Clear();
    TsEditors.Enabled = false;
    TwServer.Nodes.Clear();
    DgvResults.DataSource = null;
    TbMessages.Text = "";
    Databases = new(async () => await RepositoryFactory.Repository.GetDatabasesAsync().ToArrayAsync());
  }

  private async void NewQuery_Click(object sender, EventArgs e) => await NewQueryAsync();

  private async void Execute_Click(object sender, EventArgs e) {
    TextBox? textBox = TcEditors.SelectedTab.Controls.OfType<TextBox>().FirstOrDefault();
    if (textBox is null)
      return;

    string content = textBox.SelectedText;
    if (content is "")
      _ = MessageBox.Show("SQL Server Management Studio",
                          "Select query to execute",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);

    try {
      int rowsAffected = 0;
      if (content.Contains("SELECT") || content.Contains("EXEC")) {
        DataTable dataTable = await RepositoryFactory.Repository
                                                     .ExecuteQueryAsync((DdlDatabases.SelectedItem as Database)?.Name ?? "",
                                                                        content.Trim());
        DgvResults.DataSource = null;
        DgvResults.DataSource = dataTable;
        rowsAffected = dataTable.Rows.Count;
      } else {
        rowsAffected = await RepositoryFactory.Repository
                                              .ExecuteNonQueryAsync((DdlDatabases.SelectedItem as Database)?.Name ?? "",
                                                                    content.Trim());
        DgvResults.DataSource = null;
      }

      TbMessages.Text = new StringBuilder().AppendLine($"({rowsAffected} rows affected)")
                                           .AppendLine()
                                           .Append($"Completion time: {DateTime.Now.ToString("o", CultureInfo.InvariantCulture)}")
                                           .ToString();
    } catch (Exception ex) {
      DgvResults.DataSource = null;
      TbMessages.Text = new StringBuilder().AppendLine(ex.Message)
                                           .AppendLine()
                                           .Append($"Completion time: {DateTime.Now.ToString("o", CultureInfo.InvariantCulture)}")
                                           .ToString();
    }
  }

  private async Task NewQueryAsync(string? fileName = null) {
    TabPageTag lastTag = TcEditors.TabPages
                                  .OfType<TabPage>()
                                  .Select(tab => tab.Tag as TabPageTag ?? TabPageTag.Empty())
                                  .OrderBy(tag => tag.Id)
                                  .LastOrDefault(TabPageTag.Empty());

    TabPageTag tag = lastTag with {
      Id = fileName is not null ? 0 : lastTag.Id + 1,
      FileName = fileName is not null ? fileName : $"SQLQuery{lastTag.Id + 1}.sql"
    };
    var tabPage = new TabPage($"{tag.FileName.Split(Path.DirectorySeparatorChar).Last()} - ({_loginInformation?.Credentials.Login})") {
      Tag = tag,
    };
    tabPage.Controls.Add(new TextBox() {
      Dock = DockStyle.Fill,
      Multiline = true,
      ScrollBars = ScrollBars.Both,
      AcceptsTab = true,
      AcceptsReturn = true,
      WordWrap = true,
      Lines = fileName is not null ? await File.ReadAllLinesAsync(fileName) : Array.Empty<string>(),
      Font = new Font(new FontFamily("Consolas"), 10)
    });
    TcEditors.TabPages.Add(tabPage);
    TcEditors.SelectedIndex = TcEditors.TabCount - 1;
    if (TcEditors.TabCount == 1)
      TsmiSave.Text = $"Save {tag.FileName.Split(Path.DirectorySeparatorChar).Last()}";

    if (_loginInformation is not null) {
      TsEditors.Enabled = true;

      await DdlDatabases_Init();
    }
  }

  private void TcEditors_SelectedIndexChanged(object sender, EventArgs e) {
    if (TcEditors.SelectedTab is null || TcEditors.SelectedTab.Tag is not TabPageTag tag)
      return;

    TsmiSave.Text = $"Save {tag.FileName.Split(Path.DirectorySeparatorChar).Last()}";
  }

  private async void OpenFile_Click(object sender, EventArgs e) {
    using var ofd = new OpenFileDialog() {
      Filter = "SQL Server Files (*.sql)|*.sql|All Files (*.*)|*.*|Text Files (*.txt)|*.txt",
      Title = "Open File",
      CheckFileExists = true,
      CheckPathExists = true,
      RestoreDirectory = true,
      ValidateNames = true
    };

    if (ofd.ShowDialog() is DialogResult.OK)
      await NewQueryAsync(ofd.FileName);
  }

  private async void Save_Click(object sender, EventArgs e) {
    if (TcEditors.SelectedTab is null || TcEditors.SelectedTab.Tag is not TabPageTag tag)
      return;

    TextBox? textBox = TcEditors.SelectedTab.Controls.OfType<TextBox>().FirstOrDefault();
    if (textBox is null)
      return;

    await SaveFile(tag.FileName, textBox.Lines, TcEditors.SelectedTab);
  }

  private async void SaveAll_Click(object sender, EventArgs e) {
    foreach (TabPage tab in TcEditors.TabPages) {
      if (tab.Tag is not TabPageTag tag)
        break;

      TextBox? textBox = TcEditors.SelectedTab.Controls.OfType<TextBox>().FirstOrDefault();
      if (textBox is null)
        break;

      await SaveFile(tag.FileName, textBox.Lines, TcEditors.SelectedTab);
    }
  }

  private void Cut_Click(object sender, EventArgs e) {
    TextBox? textBox = TcEditors.SelectedTab.Controls.OfType<TextBox>().FirstOrDefault();
    if (textBox is null)
      return;

    Clipboard.SetText(textBox.SelectedText);
    textBox.Text = textBox.Text.Replace(textBox.SelectedText, "");
  }

  private void Copy_Click(object sender, EventArgs e) {
    TextBox? textBox = TcEditors.SelectedTab.Controls.OfType<TextBox>().FirstOrDefault();
    if (textBox is null)
      return;

    Clipboard.SetText(textBox.SelectedText);
  }
  private void Paste_Click(object sender, EventArgs e) {
    TextBox? textBox = TcEditors.SelectedTab.Controls.OfType<TextBox>().FirstOrDefault();
    if (textBox is null)
      return;

    string text = Clipboard.GetText();
    int newSelectionStart = textBox.SelectionStart + text.Length;
    textBox.Text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
    textBox.Text = textBox.Text.Insert(textBox.SelectionStart, Clipboard.GetText());
    textBox.SelectionStart = newSelectionStart;
  }

  private async Task SaveFile(string path, string[] content, TabPage tabPage) {
    if (File.Exists(path)) {
      File.Delete(path);
      await File.WriteAllLinesAsync(path, content);
    } else {
      using var sfd = new SaveFileDialog() {
        DefaultExt = "sql",
        Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*|Text Files (*.txt)|*.txt",
        AddExtension = true,
        CheckPathExists = true,
        FileName = path.Split(Path.DirectorySeparatorChar).Last(),
        RestoreDirectory = true,
        Title = "Save As"
      };

      if (sfd.ShowDialog() is DialogResult.OK) {
        using Stream stream = sfd.OpenFile();
        foreach (string line in content) {
          await stream.WriteAsync(Encoding.Unicode.GetBytes(line));
        }

        tabPage.Tag = TabPageTag.Empty() with { FileName = sfd.FileName };
        tabPage.Text = $"{sfd.FileName.Split(Path.DirectorySeparatorChar).Last()} - ({_loginInformation?.Credentials.Login})";
      }
    }
  }

  private async void TcEditors_MouseClickAsync(object sender, MouseEventArgs e) {
    if (e.Button is MouseButtons.Middle) {
      TabPage? tabPage = TcEditors.TabPages
                                  .OfType<TabPage>()
                                  .Where((_, idx) => TcEditors.GetTabRect(idx).Contains(e.Location))
                                  .FirstOrDefault();
      if (tabPage is not null) {
        TextBox? textBox = tabPage.Controls.OfType<TextBox>().FirstOrDefault();
        if (textBox is not null && textBox.Text.Length > 0) {
          TabPageTag? tag = (tabPage.Tag as TabPageTag) ?? TabPageTag.Empty();
          DialogResult mbResult = MessageBox.Show($"Save changes of the {tag.FileName} file?",
                                                  "SQL Server Management Studio",
                                                  MessageBoxButtons.YesNoCancel,
                                                  MessageBoxIcon.Question);
          switch (mbResult) {
            case DialogResult.Yes:
              await SaveFile(tag.FileName, textBox.Lines, tabPage);
              break;
            case DialogResult.Cancel:
              return;
            default:
              break;
          }
        }

        TcEditors.TabPages.Remove(tabPage);

        if (TcEditors.TabPages.Count is 0) {
          TsEditors.Enabled = false;
        }
      }
    }
  }

  private void ShowLoginPopup() {
    using var loginForm = new LoginForm();
    if (loginForm.ShowDialog() is DialogResult.Cancel
     || loginForm.LoginInformation is null)
      return;

    _loginInformation = loginForm.LoginInformation;
    LoggedIn(_loginInformation);
  }

  private void LoggedIn(LoginInformation loginInformation) {
    TsEditors_Init();
    TwServer_Init(loginInformation);
  }

  private async Task DdlDatabases_Init() {
    DdlDatabases.Items.Clear();
    DdlDatabases.Items.AddRange((await Databases).ToArray());
    DdlDatabases.SelectedIndex = 0;
  }

  private void TsEditors_Init() {
    if (TcEditors.TabPages.Count > 0)
      TsEditors.Enabled = true;
  }

  private void TwServer_Init(LoginInformation loginInformation) {
    var dbsNode = new TreeNode("Databases", new[] { TreeNodes.Empty }) { Tag = "dbs" };
    var mainNode = new TreeNode($"{loginInformation.ServerName} ({loginInformation.Credentials.Login})",
                                new[] { dbsNode }) { Tag = "main" };
    _ = TwServer.Nodes.Add(mainNode);
    TwServer.BeforeExpand += TwServer_BeforeExpandAsync;
    TwServer.BeforeCollapse += TwServer_BeforeCollapse;
    TwServer.AfterSelect += TwServer_AfterSelect;
  }

  private void TwServer_AfterSelect(object? sender, TreeViewEventArgs e) {
    if (e.Node is null)
      return;

    switch (e.Node) {
      case { Tag: Database db }:
        DdlDatabases.SelectedIndex = DdlDatabases.Items.IndexOf(db);
        break;
      default:
        break;
    }

  }

  private void TwServer_BeforeCollapse(object? sender, TreeViewCancelEventArgs e) {
    if (e.Node is null || e.Node.Tag is "main")
      return;

    e.Node.Nodes.Clear();
    _ = e.Node.Nodes.Add(TreeNodes.Empty);
  }

  private async void TwServer_BeforeExpandAsync(object? _, TreeViewCancelEventArgs e) {
    if (e.Node is null)
      return;

    TwServer.BeginUpdate();

    switch (e.Node) {
      case { Tag: "dbs" }:
        (await Databases).ToList().ForEach(db => e.Node.Nodes.Add(new TreeNode(db.ToString(), new[] { TreeNodes.Empty }) { Tag = db }));

        e.Node.Nodes.RemoveAt(0);
        break;

      case { Tag: Database db }:
        _ = e.Node.Nodes.Add(new TreeNode("Tables", new[] { TreeNodes.Empty }) { Tag = "tbls" });
        _ = e.Node.Nodes.Add(new TreeNode("Views", new[] { TreeNodes.Empty }) { Tag = "vws" });

        e.Node.Nodes.RemoveAt(0);
        break;

      case { Tag: "tbls", Parent.Tag: Database db }:
        (await db.Tables).ToList().ForEach(db => e.Node.Nodes.Add(new TreeNode(db.ToString())));

        e.Node.Nodes.RemoveAt(0);
        break;

      case { Tag: "vws", Parent.Tag: Database db }:
        (await db.Views).ToList().ForEach(db => e.Node.Nodes.Add(new TreeNode(db.ToString())));

        e.Node.Nodes.RemoveAt(0);
        break;
      default:
        break;
    }

    TwServer.EndUpdate();
  }

  private async void Refresh_Click(object sender, EventArgs e) {
    TwServer.CollapseAll();
    Databases = new(async () => await RepositoryFactory.Repository.GetDatabasesAsync().ToArrayAsync());
    TwServer.ExpandAll();

    DdlDatabases.Items.Clear();
    if (TcEditors.TabCount > 0)
      await DdlDatabases_Init();
  }
}