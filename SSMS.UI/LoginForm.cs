
using SSMS.UI.DAL;
using SSMS.UI.Models;

namespace SSMS.UI;
public partial class LoginForm : Form {
  public LoginInformation? LoginInformation { get; private set; }

  public LoginForm() => InitializeComponent();

  private async void BtnConnect_Click(object sender, EventArgs e) => await LoginAsync();

  private async void LoginForm_KeyDown(object sender, KeyEventArgs e) {
    if (e.KeyCode == Keys.Enter)
      await LoginAsync();
  }

  private async Task LoginAsync() {
    LoginInformation = new LoginInformation(TbServerName.Text.Trim(),
                                            new Credentials(TbLogin.Text.Trim(), TbPassword.Text.Trim()));
    try {
      await RepositoryFactory.Repository.LoginAsync(LoginInformation);
      DialogResult = DialogResult.Continue;
    } catch (Exception exception) {
      LoginInformation = null;
      _ = MessageBox.Show(exception.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }
}
