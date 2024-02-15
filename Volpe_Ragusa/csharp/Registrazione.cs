using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;

namespace Volpe_Ragusa.csharp
{
    public partial class Registrazione : Form
    {
        public Registrazione()
        {
            InitializeComponent();
            ListBoxMuscoli.Visible = false;
            labelMuscoli.Visible = false;
        }

        private void buttonRegistrazione_Click(object sender, EventArgs e)
        {
            string nome = textBoxNome.Text;
            string cognome = textBoxCognome.Text;
            string email = textBoxEmail.Text;
            string password = textBoxPassword.Text;
            string conferma_password = textBoxConfermaPassword.Text;
            int età;

            List<string> muscoli = new List<string>();
            if(checkBoxMuscoli.Checked){
                muscoli = getMuscoliSelezionati();
            }

            if ( nome != "" && cognome != "" && password != "" && conferma_password != "" && textBoxEta.Text != "")
            {
                if(IsOnlyCharacters(nome) && IsOnlyCharacters(cognome))
                {
                    if(int.TryParse(textBoxEta.Text,out età)){
                        if(IsValidEmail(email))
                        {
                            if(conferma_password == password)
                            {
                                using (WebClient client = new WebClient())
                                {
                                    try
                                    {
                                        // URL del server Python
                                        string pythonServerUrl = "http://localhost:5000/signin";
                                        // Creo un oggetto con i dati da inviare come JSON
                                        var dataToSend = new
                                        {
                                            name= nome,
                                            surname= cognome,
                                            email = email,
                                            password = password,
                                            eta= età,
                                            muscoli= muscoli
                                        };
                                        string jsonData = JsonConvert.SerializeObject(dataToSend);
                                        // Imposto l'intestazione Content-Type
                                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                                        // Invio di una richiesta POST
                                        string response = client.UploadString(pythonServerUrl, "POST", jsonData);
                                        if(response==email){
                                            Form1 login= new Form1();
                                            this.Close();
                                            login.Show();
                                        }else{
                                            labelErrore.Text=response;
                                            labelErrore.Visible=true;
                                        }
                                    }
                                    catch (WebException ex)
                                    {
                                        Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                                    }
                                }
                            }
                            else
                            {
                                labelErrore.Text="Attenzione, le password non corrispondono";
                                labelErrore.Visible=true;
                            }
                        }
                        else
                        {
                            labelErrore.Text="Attenzione, email non valida";
                            labelErrore.Visible=true;
                        }
                    }
                    else
                    {
                        labelErrore.Text="Attenzione, età non valida";
                        labelErrore.Visible=true;
                    }
                }
                else
                {
                    labelErrore.Text="Attenzione, nome e cognome devono essere letterali";
                    labelErrore.Visible=true;
                }
            }
            else
            {
                labelErrore.Text="Attenzione, non possono esserci campi vuoti";
                labelErrore.Visible=true;
            }
        }

        private void checkBoxMuscoli_CheckedChanged(object sender, EventArgs e)
        {
            // Controlla se la CheckBox è selezionata
            if (checkBoxMuscoli.Checked)
            {
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
        }

        private void textBoxConfermaPassword_TextChanged(object sender, EventArgs e)
        {
        }

        static bool IsValidEmail(string email)
        {
            // Pattern regex per validare un indirizzo email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            // Crea un oggetto Regex sulla base del pattern
            Regex regex = new Regex(pattern);
            // Verifica se la stringa dell'email corrisponde al pattern
            return regex.IsMatch(email);
        }

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

        private void linkLabelLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 login = new Form1();
            this.Close();
            login.Show();
        }
    }
}
