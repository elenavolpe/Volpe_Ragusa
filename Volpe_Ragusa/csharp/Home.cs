
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Volpe_Ragusa.csharp
{
    public partial class Home : Form
    {
        string email;

        public Home()
        {
            InitializeComponent();
            Utente utente=Utente.Istanza;
            this.email = utente.email;
            Caricamenti caricamenti = new Caricamenti(this.email);
            if(this.email=="admin@mail.it"){
                button2.Enabled=false;
            }
            label1.Text="Ciao "+utente.name+", benvenuto in MyFitPlan";

            this.AutoScroll=true;

            flowLayoutPanel1.AutoSize=true;
            flowLayoutPanel1.FlowDirection=FlowDirection.TopDown;

            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(PanelPreferred);
            PanelPreferred.FlowDirection=FlowDirection.TopDown;
            PanelPreferred.AutoSize=true;

            flowLayoutPanel1.Controls.Add(label3);
            flowLayoutPanel1.Controls.Add(PanelNovità);
            PanelNovità.FlowDirection=FlowDirection.TopDown;
            PanelNovità.AutoSize=true;

            flowLayoutPanel1.Controls.Add(label4);
            flowLayoutPanel1.Controls.Add(PanelExercises);
            PanelExercises.FlowDirection=FlowDirection.TopDown;
            PanelExercises.AutoSize=true;

            caricamenti.carica_esercizi(this.email,PanelExercises);
            caricamenti.carica_esercizi_novità(this.email,PanelNovità);
            caricamenti.carica_esercizi_preferiti(this.email,PanelPreferred);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            this.Close();
            account.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Scheda scheda = new Scheda();
            this.Close();
            scheda.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 login=new Form1();
            this.Close();
            login.Show();
        }
    }
}
