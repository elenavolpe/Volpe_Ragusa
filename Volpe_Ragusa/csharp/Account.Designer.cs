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
            buttonHome = new Button();
            buttonScheda = new Button();
            buttonLogout = new Button();
            buttonImpostazioni = new Button();
            labelAggiungi = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            label3 = new Label();
            textBox2 = new TextBox();
            label4 = new Label();
            checkedListBox1 = new CheckedListBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // labelBenvenuto
            // 
            labelBenvenuto.AutoSize = true;
            labelBenvenuto.Location = new Point(28, 13);
            labelBenvenuto.Margin = new Padding(2, 0, 2, 0);
            labelBenvenuto.Name = "labelBenvenuto";
            labelBenvenuto.Size = new Size(176, 15);
            labelBenvenuto.TabIndex = 0;
            labelBenvenuto.Text = "Benvenuto nel tuo profilo nome";
            // 
            // buttonHome
            // 
            buttonHome.Location = new Point(650, 71);
            buttonHome.Margin = new Padding(2);
            buttonHome.Name = "buttonHome";
            buttonHome.Size = new Size(101, 20);
            buttonHome.TabIndex = 6;
            buttonHome.Text = "Home";
            buttonHome.UseVisualStyleBackColor = true;
            buttonHome.Click += buttonHome_Click;
            // 
            // buttonScheda
            // 
            buttonScheda.Location = new Point(650, 105);
            buttonScheda.Margin = new Padding(2);
            buttonScheda.Name = "buttonScheda";
            buttonScheda.Size = new Size(101, 20);
            buttonScheda.TabIndex = 7;
            buttonScheda.Text = "La tua scheda";
            buttonScheda.UseVisualStyleBackColor = true;
            buttonScheda.Click += buttonScheda_Click;
            // 
            // buttonLogout
            // 
            buttonLogout.Location = new Point(650, 171);
            buttonLogout.Margin = new Padding(2);
            buttonLogout.Name = "buttonLogout";
            buttonLogout.Size = new Size(101, 20);
            buttonLogout.TabIndex = 8;
            buttonLogout.Text = "Logout";
            buttonLogout.UseVisualStyleBackColor = true;
            buttonLogout.Click += buttonLogout_Click;
            // 
            // buttonImpostazioni
            // 
            buttonImpostazioni.Location = new Point(650, 139);
            buttonImpostazioni.Margin = new Padding(2);
            buttonImpostazioni.Name = "buttonImpostazioni";
            buttonImpostazioni.Size = new Size(101, 20);
            buttonImpostazioni.TabIndex = 9;
            buttonImpostazioni.Text = "Impostazioni";
            buttonImpostazioni.UseVisualStyleBackColor = true;
            buttonImpostazioni.Click += buttonImpostazioni_Click;
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
            textBox1.Size = new Size(241, 23);
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
            textBox2.Size = new Size(529, 23);
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
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(16, 37);
            flowLayoutPanel1.Margin = new Padding(2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(430, 254);
            flowLayoutPanel1.TabIndex = 10;
            // 
            // Account
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(872, 297);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(buttonImpostazioni);
            Controls.Add(buttonLogout);
            Controls.Add(buttonScheda);
            Controls.Add(buttonHome);
            Controls.Add(labelBenvenuto);
            Margin = new Padding(2);
            Name = "Account";
            Text = "Account";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelBenvenuto;
        /*private Label labelNome;
        private Label labelEmail;
        private Label labelCognome;
        private Label labelEta;
        private Label labelMuscoli;*/
        private Button buttonHome;
        private Button buttonScheda;
        private Button buttonLogout;
        private Button buttonImpostazioni;
        private Label labelAggiungi;
        private Label label2;
        private TextBox textBox1;
        private Label label3;
        private TextBox textBox2;
        private Label label4;
        private CheckedListBox checkedListBox1;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}