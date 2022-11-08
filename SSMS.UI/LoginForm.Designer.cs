namespace SSMS.UI;

partial class LoginForm {
  /// <summary>
  /// Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  /// Clean up any resources being used.
  /// </summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing) {
    if (disposing && (components != null)) {
      components.Dispose();
    }
    base.Dispose(disposing);
  }

  #region Windows Form Designer generated code

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent() {
      this.BtnCancel = new System.Windows.Forms.Button();
      this.BtnConnect = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.TbServerName = new System.Windows.Forms.TextBox();
      this.TbLogin = new System.Windows.Forms.TextBox();
      this.TbPassword = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // BtnCancel
      // 
      this.BtnCancel.BackColor = System.Drawing.SystemColors.Control;
      this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.BtnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.BtnCancel.Location = new System.Drawing.Point(380, 219);
      this.BtnCancel.Name = "BtnCancel";
      this.BtnCancel.Size = new System.Drawing.Size(92, 30);
      this.BtnCancel.TabIndex = 0;
      this.BtnCancel.Text = "Cancel";
      this.BtnCancel.UseVisualStyleBackColor = false;
      // 
      // BtnConnect
      // 
      this.BtnConnect.BackColor = System.Drawing.SystemColors.Control;
      this.BtnConnect.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.BtnConnect.Location = new System.Drawing.Point(282, 219);
      this.BtnConnect.Name = "BtnConnect";
      this.BtnConnect.Size = new System.Drawing.Size(92, 30);
      this.BtnConnect.TabIndex = 1;
      this.BtnConnect.Text = "Connect";
      this.BtnConnect.UseVisualStyleBackColor = false;
      this.BtnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 70);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(101, 21);
      this.label1.TabIndex = 2;
      this.label1.Text = "Server name:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 110);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(52, 21);
      this.label2.TabIndex = 3;
      this.label2.Text = "Login:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 150);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(79, 21);
      this.label3.TabIndex = 4;
      this.label3.Text = "Password:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.label4.Location = new System.Drawing.Point(177, 9);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(130, 32);
      this.label4.TabIndex = 5;
      this.label4.Text = "SQL Server";
      // 
      // TbServerName
      // 
      this.TbServerName.Location = new System.Drawing.Point(169, 67);
      this.TbServerName.Name = "TbServerName";
      this.TbServerName.Size = new System.Drawing.Size(303, 29);
      this.TbServerName.TabIndex = 6;
      // 
      // TbLogin
      // 
      this.TbLogin.Location = new System.Drawing.Point(169, 107);
      this.TbLogin.Name = "TbLogin";
      this.TbLogin.Size = new System.Drawing.Size(303, 29);
      this.TbLogin.TabIndex = 7;
      // 
      // TbPassword
      // 
      this.TbPassword.Location = new System.Drawing.Point(169, 147);
      this.TbPassword.Name = "TbPassword";
      this.TbPassword.PasswordChar = '*';
      this.TbPassword.Size = new System.Drawing.Size(303, 29);
      this.TbPassword.TabIndex = 8;
      // 
      // LoginForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.CancelButton = this.BtnCancel;
      this.ClientSize = new System.Drawing.Size(484, 261);
      this.Controls.Add(this.TbPassword);
      this.Controls.Add(this.TbLogin);
      this.Controls.Add(this.TbServerName);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.BtnConnect);
      this.Controls.Add(this.BtnCancel);
      this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
      this.KeyPreview = true;
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "LoginForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Connect to Server";
      this.TopMost = true;
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginForm_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();

  }

  #endregion

  private Button BtnCancel;
  private Button BtnConnect;
  private Label label1;
  private Label label2;
  private Label label3;
  private Label label4;
  private TextBox TbServerName;
  private TextBox TbLogin;
  private TextBox TbPassword;
}