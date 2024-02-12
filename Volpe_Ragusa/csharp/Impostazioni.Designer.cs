namespace Volpe_Ragusa.csharp
{
    partial class Impostazioni
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
            labelIntro = new Label();
            label1 = new Label();
            textBoxNewNome = new TextBox();
            textBoxNewEta = new TextBox();
            label2 = new Label();
            label3 = new Label();
            ListBoxNewMuscoli = new CheckedListBox();
            textBoxNewPassword = new TextBox();
            label4 = new Label();
            label5 = new Label();
            textBoxPassword = new TextBox();
            buttonModifica = new Button();
            buttonAccount = new Button();
            labelErrore = new Label();
            SuspendLayout();
            // 
            // labelIntro
            // 
            labelIntro.AutoSize = true;
            labelIntro.Location = new Point(181, 22);
            labelIntro.Name = "labelIntro";
            labelIntro.Size = new Size(417, 25);
            labelIntro.TabIndex = 0;
            labelIntro.Text = "Ciao nome, qui puoi modificare le tue impostazioni";
            labelIntro.Click += labelIntro_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(101, 87);
            label1.Name = "label1";
            label1.Size = new Size(124, 25);
            label1.TabIndex = 1;
            label1.Text = "cambia nome:";
            // 
            // textBoxNewNome
            // 
            textBoxNewNome.Location = new Point(278, 81);
            textBoxNewNome.Name = "textBoxNewNome";
            textBoxNewNome.Size = new Size(150, 31);
            textBoxNewNome.TabIndex = 2;
            // 
            // textBoxNewEta
            // 
            textBoxNewEta.Location = new Point(278, 138);
            textBoxNewEta.Name = "textBoxNewEta";
            textBoxNewEta.Size = new Size(150, 31);
            textBoxNewEta.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(123, 144);
            label2.Name = "label2";
            label2.Size = new Size(102, 25);
            label2.TabIndex = 3;
            label2.Text = "cambia età:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 247);
            label3.Name = "label3";
            label3.Size = new Size(205, 25);
            label3.TabIndex = 5;
            label3.Text = "cambia muscoli preferiti:";
            // 
            // ListBoxNewMuscoli
            // 
            ListBoxNewMuscoli.FormattingEnabled = true;
            ListBoxNewMuscoli.Items.AddRange(new object[] { "Quadricipiti", "Glutei", "Pettorali", "Addominali", "Deltoidi", "Schiena", "Tricipiti", "Bicipiti", "Trapezi", "Muscoli cardiovascolari", "Polpacci" });
            ListBoxNewMuscoli.Location = new Point(268, 247);
            ListBoxNewMuscoli.Name = "ListBoxNewMuscoli";
            ListBoxNewMuscoli.Size = new Size(180, 144);
            ListBoxNewMuscoli.TabIndex = 6;
            // 
            // textBoxNewPassword
            // 
            textBoxNewPassword.Location = new Point(278, 191);
            textBoxNewPassword.Name = "textBoxNewPassword";
            textBoxNewPassword.Size = new Size(150, 31);
            textBoxNewPassword.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(70, 197);
            label4.Name = "label4";
            label4.Size = new Size(155, 25);
            label4.TabIndex = 7;
            label4.Text = "cambia password:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(453, 197);
            label5.Name = "label5";
            label5.Size = new Size(151, 25);
            label5.TabIndex = 9;
            label5.Text = "password attuale:";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(620, 197);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(150, 31);
            textBoxPassword.TabIndex = 10;
            // 
            // buttonModifica
            // 
            buttonModifica.Location = new Point(658, 404);
            buttonModifica.Name = "buttonModifica";
            buttonModifica.Size = new Size(112, 34);
            buttonModifica.TabIndex = 11;
            buttonModifica.Text = "Modifica";
            buttonModifica.UseVisualStyleBackColor = true;
            buttonModifica.Click += buttonModifica_Click;
            // 
            // buttonAccount
            // 
            buttonAccount.Location = new Point(12, 404);
            buttonAccount.Name = "buttonAccount";
            buttonAccount.Size = new Size(175, 34);
            buttonAccount.TabIndex = 12;
            buttonAccount.Text = "Torna all'account";
            buttonAccount.UseVisualStyleBackColor = true;
            buttonAccount.Click += buttonAccount_Click;
            // 
            // labelErrore
            // 
            labelErrore.AutoSize = true;
            labelErrore.ForeColor = Color.Red;
            labelErrore.Location = new Point(619, 366);
            labelErrore.Name = "labelErrore";
            labelErrore.Size = new Size(151, 25);
            labelErrore.TabIndex = 13;
            labelErrore.Text = "messaggio errore";
            // 
            // Impostazioni
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(labelErrore);
            Controls.Add(buttonAccount);
            Controls.Add(buttonModifica);
            Controls.Add(textBoxPassword);
            Controls.Add(label5);
            Controls.Add(textBoxNewPassword);
            Controls.Add(label4);
            Controls.Add(ListBoxNewMuscoli);
            Controls.Add(label3);
            Controls.Add(textBoxNewEta);
            Controls.Add(label2);
            Controls.Add(textBoxNewNome);
            Controls.Add(label1);
            Controls.Add(labelIntro);
            Name = "Impostazioni";
            Text = "Impostazioni";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelIntro;
        private Label label1;
        private TextBox textBoxNewNome;
        private TextBox textBoxNewEta;
        private Label label2;
        private Label label3;
        private CheckedListBox ListBoxNewMuscoli;
        private TextBox textBoxNewPassword;
        private Label label4;
        private Label label5;
        private TextBox textBoxPassword;
        private Button buttonModifica;
        private Button buttonAccount;
        private Label labelErrore;
    }
}