namespace Volpe_Ragusa.csharp
{
    partial class Home
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
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(250, 38);
            label1.Name = "label1";
            label1.Size = new Size(242, 25);
            label1.TabIndex = 0;
            label1.Text = "Ciao, benvenuto in MyFitPlan";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(52, 85);
            label2.Name = "label2";
            label2.Size = new Size(374, 25);
            label2.TabIndex = 1;
            label2.Text = "Ecco gli esercizi più gettonati dai nostri iscritti:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(52, 193);
            label3.Name = "label3";
            label3.Size = new Size(124, 25);
            label3.TabIndex = 2;
            label3.Text = "Ecco le novità:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(52, 296);
            label4.Name = "label4";
            label4.Size = new Size(283, 25);
            label4.TabIndex = 3;
            label4.Text = "Ecco la lista di tutti i nostri esercizi:";
            // 
            // button1
            // 
            button1.Location = new Point(656, 115);
            button1.Name = "button1";
            button1.Size = new Size(133, 34);
            button1.TabIndex = 4;
            button1.Text = "Account";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(656, 175);
            button2.Name = "button2";
            button2.Size = new Size(137, 34);
            button2.TabIndex = 5;
            button2.Text = "La tua scheda";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(656, 238);
            button3.Name = "button3";
            button3.Size = new Size(137, 34);
            button3.TabIndex = 6;
            button3.Text = "Logout";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "Home";
            Text = "Home";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button button1;
        private Button button2;
        private Button button3;
    }
}