using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace Volpe_Ragusa.csharp
{
    public partial class Scheda : Form
    {
        string email;
        public Scheda()
        {
            InitializeComponent();
            Utente utente=Utente.Istanza;
            this.email = utente.email;
            Caricamenti caricamenti= new Caricamenti(this.email);
            labelHeader.Text="Ecco la tua scheda "+utente.name;

            this.AutoScroll=true;

            PanelEsercizi.FlowDirection=FlowDirection.TopDown;
            PanelEsercizi.AutoSize=true;
            PanelEsercizi.Name="Scheda";
            caricamenti.get_scheda(this.email,PanelEsercizi);
        }

        private void labelHeader_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonAccount_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            this.Close();
            account.Show();
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            this.Close();
            home.Show();
        }
    }
}
