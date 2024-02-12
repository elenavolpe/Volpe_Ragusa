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
            buttonImpostazioni = new Button();
            label1 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            flowLayoutPanelAggiungi = new FlowLayoutPanel();
            labelAggiungi = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            label3 = new Label();
            textBox2 = new TextBox();
            label4 = new Label();
            checkedListBox1 = new CheckedListBox();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanelAggiungi.SuspendLayout();
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
            labelNome.Location = new Point(272, 143);
            labelNome.Name = "labelNome";
            labelNome.Size = new Size(113, 25);
            labelNome.TabIndex = 1;
            labelNome.Text = "nome: nome";
            labelNome.Visible=false;
            // 
            // labelEmail
            // 
            labelEmail.AutoSize = true;
            labelEmail.Location = new Point(272, 106);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(105, 25);
            labelEmail.TabIndex = 2;
            labelEmail.Text = "email: email";
            labelEmail.Visible=false;
            // 
            // labelCognome
            // 
            labelCognome.AutoSize = true;
            labelCognome.Location = new Point(272, 186);
            labelCognome.Name = "labelCognome";
            labelCognome.Size = new Size(173, 25);
            labelCognome.TabIndex = 3;
            labelCognome.Text = "cognome: cognome";
            labelCognome.Visible=false;
            // 
            // labelEta
            // 
            labelEta.AutoSize = true;
            labelEta.Location = new Point(272, 231);
            labelEta.Name = "labelEta";
            labelEta.Size = new Size(69, 25);
            labelEta.TabIndex = 4;
            labelEta.Text = "età: età";
            labelEta.Visible=false;
            // 
            // labelMuscoli
            // 
            labelMuscoli.AutoSize = true;
            labelMuscoli.Location = new Point(272, 276);
            labelMuscoli.Name = "labelMuscoli";
            labelMuscoli.Size = new Size(209, 25);
            labelMuscoli.TabIndex = 5;
            labelMuscoli.Text = "muscoli preferiti: muscoli";
            labelMuscoli.Visible=false;
            // 
            // buttonHome
            // 
            buttonHome.Location = new Point(582, 110);
            buttonHome.Name = "buttonHome";
            buttonHome.Size = new Size(144, 34);
            buttonHome.TabIndex = 6;
            buttonHome.Text = "Home";
            buttonHome.UseVisualStyleBackColor = true;
            buttonHome.Click += buttonHome_Click;
            // 
            // buttonScheda
            // 
            buttonScheda.Location = new Point(582, 166);
            buttonScheda.Name = "buttonScheda";
            buttonScheda.Size = new Size(144, 34);
            buttonScheda.TabIndex = 7;
            buttonScheda.Text = "La tua scheda";
            buttonScheda.UseVisualStyleBackColor = true;
            buttonScheda.Click += buttonScheda_Click;
            // 
            // buttonLogout
            // 
            buttonLogout.Location = new Point(582, 276);
            buttonLogout.Name = "buttonLogout";
            buttonLogout.Size = new Size(144, 34);
            buttonLogout.TabIndex = 8;
            buttonLogout.Text = "Logout";
            buttonLogout.UseVisualStyleBackColor = true;
            buttonLogout.Click += buttonLogout_Click;
            // 
            // buttonImpostazioni
            // 
            buttonImpostazioni.Location = new Point(582, 222);
            buttonImpostazioni.Name = "buttonImpostazioni";
            buttonImpostazioni.Size = new Size(144, 34);
            buttonImpostazioni.TabIndex = 9;
            buttonImpostazioni.Text = "Impostazioni";
            buttonImpostazioni.UseVisualStyleBackColor = true;
            buttonImpostazioni.Click += buttonImpostazioni_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(0, 25);
            label1.TabIndex = 10;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Location = new Point(18, 393);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(548, 102);
            flowLayoutPanel1.TabIndex = 11;
            // 
            // flowLayoutPanelAggiungi
            // 
            flowLayoutPanelAggiungi.Location = new Point(21, 78);
            flowLayoutPanelAggiungi.Name = "flowLayoutPanelAggiungi";
            flowLayoutPanelAggiungi.Size = new Size(545, 312);
            flowLayoutPanelAggiungi.TabIndex = 12;
            // 
            // labelAggiungi
            // 
            labelAggiungi.AutoSize = true;
            labelAggiungi.Location = new Point(3, 0);
            labelAggiungi.Name = "labelAggiungi";
            labelAggiungi.Size = new Size(59, 25);
            labelAggiungi.TabIndex = 0;
            labelAggiungi.Text = "Aggiungi esercizio";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 42);
            label2.Name = "label2";
            label2.Size = new Size(88, 38);
            label2.TabIndex = 1;
            label2.Text = "nome:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(4, 78);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(241, 31);
            textBox1.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 121);
            label3.Name = "label3";
            label3.Size = new Size(100, 38);
            label3.TabIndex = 3;
            label3.Text = "descrizione:";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(-1, 148);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(529, 31);
            textBox2.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 189);
            label4.Name = "label4";
            label4.Size = new Size(88, 25);
            label4.TabIndex = 5;
            label4.Text = "muscoli allenati";
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(new object[] { "Quadricipiti", "Glutei", "Pettorali", "Addominali", "Deltoidi", "Schiena", "Tricipiti", "Bicipiti", "Trapezi", "Muscoli cardiovascolari", "Polpacci" });
            checkedListBox1.Location = new Point(16, 231);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(229, 60);
            checkedListBox1.TabIndex = 6;
            // 
            // Account
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 495);
            Controls.Add(flowLayoutPanelAggiungi);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(buttonImpostazioni);
            Controls.Add(buttonLogout);
            Controls.Add(buttonScheda);
            Controls.Add(buttonHome);
            /*
            Controls.Add(labelMuscoli);
            Controls.Add(labelEta);
            Controls.Add(labelCognome);
            Controls.Add(labelEmail);
            Controls.Add(labelNome);
            */
            Controls.Add(labelBenvenuto);
            Name = "Account";
            Text = "Account";
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanelAggiungi.ResumeLayout(false);
            flowLayoutPanelAggiungi.PerformLayout();
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
        private Button buttonImpostazioni;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanelAggiungi;
        private Label labelAggiungi;
        private Label label2;
        private TextBox textBox1;
        private Label label3;
        private TextBox textBox2;
        private Label label4;
        private CheckedListBox checkedListBox1;
    }
}