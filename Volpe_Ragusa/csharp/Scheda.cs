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
using System.Threading.Tasks;
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
            //string nome=get_name(email);
            labelHeader.Text="Ecco la tua scheda "+utente.name;
            //TO_DO inserire qui tutti i caricamenti
        }

        //è sbagliato, non è al click
        private void labelHeader_Click(object sender, EventArgs e)
        {
            //fare in modo che aperta la sessione al posto di nome si veda il nome dell'utente
        }

        //vedi come chiamare questa funzione
        public void carica_esercizi()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    // URL del server Python
                    string url = "http://localhost:5000/get_scheda";
                    // Creazione dei dati da inviare come parte della richiesta POST
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", this.email }
                        // Aggiungi altri parametri se necessario
                    };
                    // Invio della richiesta POST sincrona
                    byte[] responseBytes = client.UploadValues(url, "POST", postData);
                    // Converti la risposta in una stringa
                    string responseBody = System.Text.Encoding.UTF8.GetString(responseBytes);
                    // metto quello che torna nel vettore di esercizi
                    string[] exercises = JsonConvert.DeserializeObject<string[]>(responseBody);
                    // Aggiungi dinamicamente i controlli al form
                    for (int i = 0; i < exercises.Length; i++)
                    {
                        Label label = new Label();
                        //vediamo cosa mi torna il json, creo label nome e label descrizione
                        //label.Text = $"Esercizio {i + 1}: {exercises[i]}";
                        label.Location = new System.Drawing.Point(20, 50 + 30 * i);
                        label.Size = new System.Drawing.Size(200, 20);
                        Controls.Add(label);
                    }
                }
                catch (WebException ex)
                {
                    // Gestisci eventuali errori durante la richiesta HTTP
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
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
