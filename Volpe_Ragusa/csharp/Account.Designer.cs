namespace Volpe_Ragusa.csharp
{
    partial class Account
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
            labelBenvenuto = new Label();
            labelNome = new Label();
            labelEmail = new Label();
            labelCognome = new Label();
            labelEta = new Label();
            labelMuscoli = new Label();
            buttonHome = new Button();
            buttonScheda = new Button();
            buttonLogout = new Button();
            SuspendLayout();
            // 
            // labelBenvenuto
            // 
            labelBenvenuto.AutoSize = true;
            labelBenvenuto.Location = new Point(40, 21);
            labelBenvenuto.Name = "labelBenvenuto";
            labelBenvenuto.Size = new Size(265, 25);
            labelBenvenuto.TabIndex = 0;
            labelBenvenuto.Text = "Benvenuto nel tuo profilo nome";
            // 
            // labelNome
            // 
            labelNome.AutoSize = true;
            labelNome.Location = new Point(288, 148);
            labelNome.Name = "labelNome";
            labelNome.Size = new Size(113, 25);
            labelNome.TabIndex = 1;
            labelNome.Text = "nome: nome";
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(288, 111);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(105, 25);
            labelEmail.TabIndex = 2;
            labelEmail.Text = "email: email";
            // 
            // labelCognome
            // 
            labelCognome.AutoSize = true;
            labelCognome.Location = new Point(288, 191);
            labelCognome.Name = "labelCognome";
            labelCognome.Size = new Size(173, 25);
            labelCognome.TabIndex = 3;
            labelCognome.Text = "cognome: cognome";
            // 
            // labelEta
            // 
            labelEta.AutoSize = true;
            labelEta.Location = new Point(288, 236);
            labelEta.Name = "labelEta";
            labelEta.Size = new Size(69, 25);
            labelEta.TabIndex = 4;
            labelEta.Text = "età: età";
            // 
            // labelMuscoli
            // 
            labelMuscoli.AutoSize = true;
            labelMuscoli.Location = new Point(288, 281);
            labelMuscoli.Name = "labelMuscoli";
            labelMuscoli.Size = new Size(209, 25);
            labelMuscoli.TabIndex = 5;
            labelMuscoli.Text = "muscoli preferiti: muscoli";
            // 
            // buttonHome
            // 
            buttonHome.Location = new Point(644, 115);
            buttonHome.Name = "buttonHome";
            buttonHome.Size = new Size(144, 34);
            buttonHome.TabIndex = 6;
            buttonHome.Text = "Home";
            buttonHome.UseVisualStyleBackColor = true;
            buttonHome.Click += buttonHome_Click;
            // 
            // buttonScheda
            // 
            buttonScheda.Location = new Point(644, 171);
            buttonScheda.Name = "buttonScheda";
            buttonScheda.Size = new Size(144, 34);
            buttonScheda.TabIndex = 7;
            buttonScheda.Text = "La tua scheda";
            buttonScheda.UseVisualStyleBackColor = true;
            buttonScheda.Click += buttonScheda_Click;
            // 
            // buttonLogout
            // 
            buttonLogout.Location = new Point(644, 226);
            buttonLogout.Name = "buttonLogout";
            buttonLogout.Size = new Size(144, 34);
            buttonLogout.TabIndex = 8;
            buttonLogout.Text = "Logout";
            buttonLogout.UseVisualStyleBackColor = true;
            buttonLogout.Click += buttonLogout_Click;
            // 
            // Account
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonLogout);
            Controls.Add(buttonScheda);
            Controls.Add(buttonHome);
            Controls.Add(labelMuscoli);
            Controls.Add(labelEta);
            Controls.Add(labelCognome);
            Controls.Add(labelEmail);
            Controls.Add(labelNome);
            Controls.Add(labelBenvenuto);
            Name = "Account";
            Text = "Account";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelBenvenuto;
        private Label labelNome;
        private Label labelEmail;
        private Label labelCognome;
        private Label labelEta;
        private Label labelMuscoli;
        private Button buttonHome;
        private Button buttonScheda;
        private Button buttonLogout;
    }
}