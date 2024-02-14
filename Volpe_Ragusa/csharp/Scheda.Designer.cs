namespace Volpe_Ragusa.csharp
{
    partial class Scheda
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
            labelHeader = new Label();
            buttonAccount = new Button();
            buttonHome = new Button();
            PanelEsercizi = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            labelHeader.Location = new Point(62, 13);
            labelHeader.Margin = new Padding(2, 0, 2, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new Size(138, 15);
            labelHeader.TabIndex = 0;
            labelHeader.Text = "Ecco la tua scheda nome";
            labelHeader.Click += labelHeader_Click;
            // 
            // buttonAccount
            // 
            buttonAccount.Location = new Point(628, 85);
            buttonAccount.Margin = new Padding(2, 2, 2, 2);
            buttonAccount.Name = "buttonAccount";
            buttonAccount.Size = new Size(78, 20);
            buttonAccount.TabIndex = 1;
            buttonAccount.Text = "Account";
            buttonAccount.UseVisualStyleBackColor = true;
            buttonAccount.Click += buttonAccount_Click;
            // 
            // buttonHome
            // 
            buttonHome.Location = new Point(628, 116);
            buttonHome.Margin = new Padding(2, 2, 2, 2);
            buttonHome.Name = "buttonHome";
            buttonHome.Size = new Size(78, 20);
            buttonHome.TabIndex = 2;
            buttonHome.Text = "Home";
            buttonHome.UseVisualStyleBackColor = true;
            buttonHome.Click += buttonHome_Click;
            // 
            // PanelEsercizi
            // 
            PanelEsercizi.Location = new Point(33, 42);
            PanelEsercizi.Margin = new Padding(2, 2, 2, 2);
            PanelEsercizi.Name = "PanelEsercizi";
            PanelEsercizi.Size = new Size(407, 221);
            PanelEsercizi.TabIndex = 3;
            // 
            // Scheda
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(871, 270);
            Controls.Add(PanelEsercizi);
            Controls.Add(buttonHome);
            Controls.Add(buttonAccount);
            Controls.Add(labelHeader);
            Margin = new Padding(2, 2, 2, 2);
            Name = "Scheda";
            Text = "Scheda";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelHeader;
        private Button buttonAccount;
        private Button buttonHome;
        private FlowLayoutPanel PanelEsercizi;
    }
}