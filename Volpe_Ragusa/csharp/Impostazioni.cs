﻿using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
//using System.Text;
using System.Windows.Forms;

namespace Volpe_Ragusa.csharp
{
    public partial class Impostazioni : Form
    {
        string email;
        public Impostazioni()
        {
            InitializeComponent();
            Utente utente=Utente.Istanza;
            this.email = utente.email;
            labelIntro.Text="Ciao "+utente.name+", qui puoi modificare il tuo profilo";
            labelErrore.Visible=false;
        }

        private void labelIntro_Click(object sender, EventArgs e)
        {

        }

        private void buttonAccount_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            this.Close();
            account.Show();
        }

        private void buttonModifica_Click(object sender, EventArgs e)
        {
            string newName=textBoxNewNome.Text;
            string newPassword=textBoxNewPassword.Text;
            string password=textBoxPassword.Text;
            int eta;
            List<string> newmuscoli= getMuscoliSelezionati();
            int.TryParse(textBoxNewEta.Text, out eta); 
            using (WebClient client = new WebClient())
            {
                try
                {
                    string pythonServerUrl = "http://localhost:5000/modifica_profilo";
                    var dataToSend = new
                    {
                        nome = newName,
                        email = email,
                        newpassword = newPassword,
                        password = password,
                        eta = eta,
                        muscoli = newmuscoli
                    };
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    // Invio di una richiesta POST
                    string response = client.UploadString(pythonServerUrl, "POST", jsonData);
                    if(response=="ok"){
                        Utente utente=Utente.Istanza;
                        utente.setAddress(email);
                        this.Close();
                        Account account1 = new Account();
                        account1.Show();
                    }else{
                        labelErrore.Text = response;
                        labelErrore.Visible = true;
                        Console.WriteLine(response);
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        private List<string> getMuscoliSelezionati()
        {
            List<string> muscoli = new List<string>();
            for (int i = 0; i < ListBoxNewMuscoli.Items.Count; i++)
            {
                // Verifica se l'elemento è selezionato
                if (ListBoxNewMuscoli.GetItemChecked(i))
                {
                    // Aggiunge l'elemento alla lista degli elementi selezionati
                    muscoli.Add(ListBoxNewMuscoli.Items[i].ToString());
                }
            }
            return muscoli;
        }
    }
}
