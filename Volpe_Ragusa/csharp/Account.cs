using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Volpe_Ragusa.csharp
{
    public partial class Account : Form
    {
        string email;
        public Account(string email)
        {
            InitializeComponent();
            this.email = email;
            //dovrei chiamare uno script python per farmi tornare il nome data l'email
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            Home home = new Home(email);
            this.Close();
            home.Show();
        }

        private void buttonScheda_Click(object sender, EventArgs e)
        {
            Scheda scheda= new Scheda(email);
            this.Close();
            scheda.Show();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            //esci dalla sessione e vai al login
            Form1 login = new Form1();
            this.Close();
            login.Show();
        }

        private void buttonImpostazioni_Click(object sender, EventArgs e)
        {
            Impostazioni impostazioni = new Impostazioni(email);
            this.Close();
            impostazioni.Show();
        }
    }
}
