﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Volpe_Ragusa.csharp
{
    public partial class Impostazioni : Form
    {
        //forse dovrei aggiungere dei modificatori
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
            //fai un eccezione su questo
            string eta= textBoxNewEta.Text;
            List<string> newmuscoli= getMuscoliSelezionati();
            //devi vedere se la vecchia password corrisponde

            using (WebClient client = new WebClient())
            {
                try
                {
                    // URL del server Python
                    string pythonServerUrl = "http://localhost:5000/modifica_profilo";
                    //lato server se si vuole cambiare una nuova password, devi verificare che
                    //la vecchia password è giusta
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
                    // Creare il contenuto della richiesta POST
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    // Impostare l'intestazione Content-Type
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    // Invio di una richiesta POST
                    string response = client.UploadString($"{pythonServerUrl}", "POST", jsonData);
                    // Leggi la risposta
                    Console.WriteLine($"Risposta dal server Python: {response}");
                    if(response=="ok"){
                        //se va bene vado ad account
                        Utente utente=Utente.Istanza;
                        utente.setAddress(email);
                        Account account1 = new Account();
                        this.Close();
                        account1.Show();
                    }else{
                        labelErrore.Text = response;
                        labelErrore.Visible = true;
                        Console.WriteLine(response);
                    }
                }
                catch (WebException ex)
                {
                    // Gestisci eventuali errori durante la richiesta HTTP
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }

            Account account= new Account();
            this.Close();
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
