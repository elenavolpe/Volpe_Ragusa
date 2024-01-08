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

namespace Volpe_Ragusa.csharp
{
    public partial class Registrazione : Form
    {
        public Registrazione()
        {
            InitializeComponent();
        }

        private void buttonRegistrazione_Click(object sender, EventArgs e)
        {
            string nome = textBoxNome.Text;
            string cognome = textBoxCognome.Text;
            string email = textBoxEmail.Text;
            string password = textBoxPassword.Text;
            string conferma_password = textBoxConfermaPassword.Text;
            //fai un eccezione su questo
            int eta = int.Parse(textBoxEta.Text);

            if (IsOnlyCharacters(nome) && nome != "" && IsOnlyCharacters(cognome) && cognome != "" && IsValidEmail(email) && password != "" && conferma_password == password && eta > 0)
            {
                //invia le cose a python per la registrazione
            }
        }

        private void checkBoxMuscoli_CheckedChanged(object sender, EventArgs e)
        {
            //fai uscire la lista dei gruppi muscolari
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
    }
}
