namespace Volpe_Ragusa.csharp
{
    partial class Registrazione
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            textBoxNome = new TextBox();
            label2 = new Label();
            textBoxCognome = new TextBox();
            label3 = new Label();
            textBoxEmail = new TextBox();
            label4 = new Label();
            textBoxPassword = new TextBox();
            label5 = new Label();
            textBoxConfermaPassword = new TextBox();
            label6 = new Label();
            textBoxEta = new TextBox();
            checkBoxMuscoli = new CheckBox();
            buttonRegistrazione = new Button();
            ListBoxMuscoli = new CheckedListBox();
            labelMuscoli = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(228, 15);
            label1.Name = "label1";
            label1.Size = new Size(125, 25);
            label1.TabIndex = 0;
            label1.Text = "Inserisci nome";
            // 
            // textBoxNome
            // 
            textBoxNome.Location = new Point(386, 14);
            textBoxNome.Name = "textBoxNome";
            textBoxNome.Size = new Size(150, 31);
            textBoxNome.TabIndex = 1;
            textBoxNome.Text = "nome";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(198, 68);
            label2.Name = "label2";
            label2.Size = new Size(155, 25);
            label2.TabIndex = 2;
            label2.Text = "Inserisci cognome";
            // 
            // textBoxCognome
            // 
            textBoxCognome.Location = new Point(382, 65);
            textBoxCognome.Name = "textBoxCognome";
            textBoxCognome.Size = new Size(150, 31);
            textBoxCognome.TabIndex = 3;
            textBoxCognome.Text = "cognome";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(228, 123);
            label3.Name = "label3";
            label3.Size = new Size(121, 25);
            label3.TabIndex = 4;
            label3.Text = "Inserisci email";
            // 
            // textBoxEmail
            // 
            textBoxEmail.Location = new Point(382, 117);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(150, 31);
            textBoxEmail.TabIndex = 5;
            textBoxEmail.Text = "email";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(193, 180);
            label4.Name = "label4";
            label4.Size = new Size(156, 25);
            label4.TabIndex = 6;
            label4.Text = "Inserisci password";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(382, 174);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(150, 31);
            textBoxPassword.TabIndex = 7;
            textBoxPassword.Text = "password";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(179, 234);
            label5.Name = "label5";
            label5.Size = new Size(170, 25);
            label5.TabIndex = 8;
            label5.Text = "Conferma Password";
            // 
            // textBoxConfermaPassword
            // 
            textBoxConfermaPassword.Location = new Point(382, 228);
            textBoxConfermaPassword.Name = "textBoxConfermaPassword";
            textBoxConfermaPassword.Size = new Size(150, 31);
            textBoxConfermaPassword.TabIndex = 9;
            textBoxConfermaPassword.Text = "password";
            textBoxConfermaPassword.TextChanged += textBoxConfermaPassword_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(246, 294);
            label6.Name = "label6";
            label6.Size = new Size(103, 25);
            label6.TabIndex = 10;
            label6.Text = "Inserisci età";
            // 
            // textBoxEta
            // 
            textBoxEta.Location = new Point(382, 291);
            textBoxEta.Name = "textBoxEta";
            textBoxEta.Size = new Size(150, 31);
            textBoxEta.TabIndex = 11;
            textBoxEta.Text = "età";
            textBoxEta.TextChanged += textBoxEta_TextChanged;
            // 
            // checkBoxMuscoli
            // 
            checkBoxMuscoli.AutoSize = true;
            checkBoxMuscoli.Location = new Point(165, 344);
            checkBoxMuscoli.Name = "checkBoxMuscoli";
            checkBoxMuscoli.Size = new Size(409, 29);
            checkBoxMuscoli.TabIndex = 12;
            checkBoxMuscoli.Text = "sei interessato ad allenare determinati muscoli?";
            checkBoxMuscoli.UseVisualStyleBackColor = true;
            checkBoxMuscoli.CheckedChanged += checkBoxMuscoli_CheckedChanged;
            // 
            // buttonRegistrazione
            // 
            buttonRegistrazione.Location = new Point(505, 398);
            buttonRegistrazione.Name = "buttonRegistrazione";
            buttonRegistrazione.Size = new Size(112, 34);
            buttonRegistrazione.TabIndex = 13;
            buttonRegistrazione.Text = "Registrati";
            buttonRegistrazione.UseVisualStyleBackColor = true;
            buttonRegistrazione.Click += buttonRegistrazione_Click;
            // 
            // ListBoxMuscoli
            // 
            ListBoxMuscoli.FormattingEnabled = true;
            ListBoxMuscoli.Items.AddRange(new object[] { "Braccia", "Gambe", "Petto", "Addominali", "Spalle", "Schiena", "Glutei" });
            ListBoxMuscoli.Location = new Point(164, 406);
            ListBoxMuscoli.Name = "ListBoxMuscoli";
            ListBoxMuscoli.Size = new Size(180, 144);
            ListBoxMuscoli.TabIndex = 14;
            // 
            // labelMuscoli
            // 
            labelMuscoli.AutoSize = true;
            labelMuscoli.Location = new Point(164, 378);
            labelMuscoli.Name = "labelMuscoli";
            labelMuscoli.Size = new Size(124, 25);
            labelMuscoli.TabIndex = 15;
            labelMuscoli.Text = "Selezionali qui";
            // 
            // Registrazione
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 584);
            Controls.Add(labelMuscoli);
            Controls.Add(ListBoxMuscoli);
            Controls.Add(buttonRegistrazione);
            Controls.Add(checkBoxMuscoli);
            Controls.Add(textBoxEta);
            Controls.Add(label6);
            Controls.Add(textBoxConfermaPassword);
            Controls.Add(label5);
            Controls.Add(textBoxPassword);
            Controls.Add(label4);
            Controls.Add(textBoxEmail);
            Controls.Add(label3);
            Controls.Add(textBoxCognome);
            Controls.Add(label2);
            Controls.Add(textBoxNome);
            Controls.Add(label1);
            Name = "Registrazione";
            Text = "Registrazione";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBoxNome;
        private Label label2;
        private TextBox textBoxCognome;
        private Label label3;
        private TextBox textBoxEmail;
        private Label label4;
        private TextBox textBoxPassword;
        private Label label5;
        private TextBox textBoxConfermaPassword;
        private Label label6;
        private TextBox textBoxEta;
        private CheckBox checkBoxMuscoli;
        private Button buttonRegistrazione;
        private CheckedListBox ListBoxMuscoli;
        private Label labelMuscoli;
    }
}