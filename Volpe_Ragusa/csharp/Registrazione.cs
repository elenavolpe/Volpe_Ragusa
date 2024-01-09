using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Volpe_Ragusa.csharp
{
    public partial class Registrazione : Form
    {
        public Registrazione()
        {
            InitializeComponent();
            // Registra l'evento CheckedChanged per la CheckBox
            checkBoxMuscoli.CheckedChanged += checkBoxMuscoli_CheckedChanged;
        }

        private void buttonRegistrazione_Click(object sender, EventArgs e)
        {
            string nome = textBoxNome.Text;
            string cognome = textBoxCognome.Text;
            string email = textBoxEmail.Text;
            string password = textBoxPassword.Text;
            string conferma_password = textBoxConfermaPassword.Text;

            List<string> muscoli = getMuscoliSelezionati();
            //fai un eccezione su questo
            int eta = int.Parse(textBoxEta.Text);

            if (IsOnlyCharacters(nome) && nome != "" && IsOnlyCharacters(cognome) && cognome != "" && IsValidEmail(email) && password != "" && conferma_password == password && eta > 0)
            {
                //invia le cose a python per la registrazione
            }
        }

        private void checkBoxMuscoli_CheckedChanged(object sender, EventArgs e)
        {
            // Controlla se la CheckBox è selezionata
            if (checkBoxMuscoli.Checked)
            {
                // Seleziona l'elemento predefinito nella ListBox
                ListBoxMuscoli.SelectedIndex = 0;

                // Mostra la ListBox
                ListBoxMuscoli.Visible = true;
                labelMuscoli.Visible = true;
            }
            else
            {
                // Nasconde la ListBox
                ListBoxMuscoli.Visible = false;
                labelMuscoli.Visible = false;
            }
        }

        private void textBoxEta_TextChanged(object sender, EventArgs e)
        {
            //vedi se è un intero, in caso dai errore
        }

        private void textBoxConfermaPassword_TextChanged(object sender, EventArgs e)
        {
            //vedi se è uguale a password, in caso dai errore
        }

        //studiarlo perchè probabilmente ce lo chiederà
        static bool IsValidEmail(string email)
        {
            // Pattern regex per validare un indirizzo email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Creare un oggetto Regex sulla base del pattern
            Regex regex = new Regex(pattern);

            // Verificare se la stringa dell'email corrisponde al pattern
            return regex.IsMatch(email);
        }

        //studiarlo perchè probabilmente lo chiede
        static bool IsOnlyCharacters(string inputString)
        {
            // Verifica se ogni carattere nella stringa è una lettera
            return inputString.All(char.IsLetter);
        }

        private List<string> getMuscoliSelezionati()
        {
            List<string> muscoli = new List<string>();
            for (int i = 0; i < ListBoxMuscoli.Items.Count; i++)
            {
                // Verifica se l'elemento è selezionato
                if (ListBoxMuscoli.GetItemChecked(i))
                {
                    // Aggiungi l'elemento alla lista degli elementi selezionati
                    muscoli.Add(ListBoxMuscoli.Items[i].ToString());
                }
            }
            return muscoli;
        }
    }
}
