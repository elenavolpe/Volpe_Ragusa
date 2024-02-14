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
            PanelPreferred = new FlowLayoutPanel();
            PanelNovità = new FlowLayoutPanel();
            PanelExercises = new FlowLayoutPanel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(175, 23);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(163, 15);
            label1.TabIndex = 0;
            label1.Text = "Ciao, benvenuto in MyFitPlan";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label2.Location = new Point(36, 51);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(263, 15);
            label2.TabIndex = 1;
            label2.Text = "Ecco gli esercizi più gettonati dai nostri iscritti:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(36, 143);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(86, 15);
            label3.TabIndex = 2;
            label3.Text = "Ecco le novità:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(36, 222);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(199, 15);
            label4.TabIndex = 3;
            label4.Text = "Ecco la lista di tutti i nostri esercizi:";
            // 
            // button1
            // 
            button1.Location = new Point(630, 72);
            button1.Margin = new Padding(2, 2, 2, 2);
            button1.Name = "button1";
            button1.Size = new Size(93, 20);
            button1.TabIndex = 4;
            button1.Text = "Account";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(629, 105);
            button2.Margin = new Padding(2, 2, 2, 2);
            button2.Name = "button2";
            button2.Size = new Size(96, 20);
            button2.TabIndex = 5;
            button2.Text = "La tua scheda";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(629, 143);
            button3.Margin = new Padding(2, 2, 2, 2);
            button3.Name = "button3";
            button3.Size = new Size(96, 20);
            button3.TabIndex = 6;
            button3.Text = "Logout";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // PanelPreferred
            // 
            PanelPreferred.Location = new Point(40, 72);
            PanelPreferred.Margin = new Padding(2, 2, 2, 2);
            PanelPreferred.Name = "PanelPreferred";
            PanelPreferred.Size = new Size(304, 69);
            PanelPreferred.TabIndex = 10;
            // 
            // PanelNovità
            // 
            PanelNovità.Location = new Point(38, 162);
            PanelNovità.Margin = new Padding(2, 2, 2, 2);
            PanelNovità.Name = "PanelNovità";
            PanelNovità.Size = new Size(306, 58);
            PanelNovità.TabIndex = 11;
            // 
            // PanelExercises
            // 
            PanelExercises.Location = new Point(38, 242);
            PanelExercises.Margin = new Padding(2, 2, 2, 2);
            PanelExercises.Name = "PanelExercises";
            PanelExercises.Size = new Size(307, 65);
            PanelExercises.TabIndex = 12;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Location = new Point(14, 45);
            flowLayoutPanel1.Margin = new Padding(2, 2, 2, 2);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(426, 262);
            flowLayoutPanel1.TabIndex = 13;
            // 
            // Home
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(871, 314);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(PanelExercises);
            Controls.Add(PanelNovità);
            Controls.Add(PanelPreferred);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new Padding(2, 2, 2, 2);
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
        private FlowLayoutPanel PanelPreferred;
        private FlowLayoutPanel PanelNovità;
        private FlowLayoutPanel PanelExercises;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}