using Newtonsoft.Json;
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
    public partial class Home : Form
    {
        string email;
        public Home(string email)
        {
            InitializeComponent();
            this.email = email;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Account account = new Account(email);
            this.Close();
            account.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Scheda scheda = new Scheda(email);
            this.Close();
            scheda.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 login=new Form1();
            this.Close();
            login.Show();
        }

        //vedi come chiamare questa funzione
        public void carica_esercizi()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    // URL del server Python
                    string url = "http://localhost:5000/get_esercizi";
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

        //vedi come chiamare questa funzione
        public void carica_esercizi_preferiti()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    // URL del server Python
                    string url = "http://localhost:5000/get_esercizi_preferiti";
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

        //vedi come chiamare questa funzione
        public void carica_esercizi_novità()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    // URL del server Python
                    string url = "http://localhost:5000/get_esercizi_recenti";
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
    }
}
