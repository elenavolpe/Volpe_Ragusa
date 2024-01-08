namespace Volpe_Ragusa
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            textBoxEmail = new TextBox();
            textBoxPassword = new TextBox();
            label2 = new Label();
            buttonLogin = new Button();
            linkRegistrazione = new LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(237, 143);
            label1.Name = "label1";
            label1.Size = new Size(121, 25);
            label1.TabIndex = 0;
            label1.Text = "Inserisci email";
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(379, 137);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(150, 31);
            textBoxEmail.TabIndex = 1;
            textBoxEmail.Text = "email";
            textBoxEmail.TextChanged += textBoxEmail_TextChanged;
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(379, 197);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(150, 31);
            textBoxPassword.TabIndex = 3;
            textBoxPassword.Text = "password";
            textBoxPassword.TextChanged += textBoxPassword_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(202, 203);
            label2.Name = "label2";
            label2.Size = new Size(156, 25);
            label2.TabIndex = 2;
            label2.Text = "Inserisci password";
            // 
            // buttonLogin
            // 
            buttonLogin.Location = new Point(521, 304);
            buttonLogin.Name = "buttonLogin";
            buttonLogin.Size = new Size(112, 34);
            buttonLogin.TabIndex = 4;
            buttonLogin.Text = "Login";
            buttonLogin.UseVisualStyleBackColor = true;
            buttonLogin.Click += buttonLogin_Click;
            // 
            // linkRegistrazione
            // 
            linkRegistrazione.AutoSize = true;
            linkRegistrazione.Location = new Point(38, 313);
            linkRegistrazione.Name = "linkRegistrazione";
            linkRegistrazione.Size = new Size(298, 25);
            linkRegistrazione.TabIndex = 5;
            linkRegistrazione.TabStop = true;
            linkRegistrazione.Text = "Non sei ancora registrato? Clicca qui";
            linkRegistrazione.LinkClicked += linkRegistrazione_LinkClicked;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(linkRegistrazione);
            Controls.Add(buttonLogin);
            Controls.Add(textBoxPassword);
            Controls.Add(label2);
            Controls.Add(textBoxEmail);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBoxEmail;
        private TextBox textBoxPassword;
        private Label label2;
        private Button buttonLogin;
        private LinkLabel linkRegistrazione;
    }
}
