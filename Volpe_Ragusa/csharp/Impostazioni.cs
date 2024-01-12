using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Volpe_Ragusa.csharp
{
    public partial class Impostazioni : Form
    {
        //forse dovrei aggiungere dei modificatori
        string email;
        public Impostazioni(string email)
        {
            InitializeComponent();
            this.email = email;
        }

        private void labelIntro_Click(object sender, EventArgs e)
        {

        }

        private void buttonAccount_Click(object sender, EventArgs e)
        {
            Account account = new Account(email);
            account.Show();
        }

        private void buttonModifica_Click(object sender, EventArgs e)
        {
            string newName=textBoxNewNome.Text;
            string newPassword=textBoxNewPassword.Text;
            string password=textBoxPassword.Text;
            //fai un eccezione su questo
            int eta=int.Parse(textBoxNewEta.Text);
            List<string> newmuscoli= getMuscoliSelezionati();
            //devi vedere se la vecchia password corrisponde

            //invia nuove modifiche a python

            Account account= new Account(email);
            account.Show();
        }

        private List<string> getMuscoliSelezionati()
        {
            List<string> muscoli = new List<string>();
            for (int i = 0; i < ListBoxNewMuscoli.Items.Count; i++)
            {
                // Verifica se l'elemento è selezionato
                if (ListBoxNewMuscoli.GetItemChecked(i))
                {
                    // Aggiungi l'elemento alla lista degli elementi selezionati
                    muscoli.Add(ListBoxNewMuscoli.Items[i].ToString());
                }
            }
            return muscoli;
        }
    }
}
