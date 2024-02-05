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

            this.AutoScroll=true;

            PanelEsercizi.FlowDirection=FlowDirection.TopDown;
            PanelEsercizi.AutoSize=true;
            carica_esercizi();
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
                    string url = "http://localhost:5000/get_scheda";
                    // Creazione dei dati da inviare come parte della richiesta POST
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", this.email }
                        // Aggiungi altri parametri se necessario
                    };
                    byte[] responseBytes = client.UploadValues(url, "POST", postData);
                    string responseBody = System.Text.Encoding.UTF8.GetString(responseBytes);
                    // metto quello che torna nel vettore di esercizi
                    string[] exercises = JsonConvert.DeserializeObject<string[]>(responseBody);
                    for (int i = 0; i < exercises.Length; i++)
                    {   
                        FlowLayoutPanel panel= new FlowLayoutPanel();
                        panel.FlowDirection=FlowDirection.LeftToRight;
                        panel.AutoSize=true;

                        Label label = new Label();
                        label.Text=exercises[i];
                        panel.Controls.Add(label);

                        Button button = new Button();
                        button.Size= new System.Drawing.Size(90,30);
                        button.Text="elimina";
                        button.Click += eliminaEsercizio;
                        panel.Controls.Add(button);
                        //vediamo cosa mi torna il json, creo label nome e label descrizione
                        //TO_DO aggiungere evento che elimina l'esercizio al bottone
                        //TO_DO sistemare grandezza di questo panel
                        PanelEsercizi.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
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

        private void eliminaEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Control contenitore = button.Parent;
            Label label = contenitore.Controls.OfType<Label>().FirstOrDefault();
            string nomeEsercizio=label.Text;
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/elimina_esercizio";
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", this.email },
                        {"esercizio", nomeEsercizio}
                    };
                    byte[] responseBytes = client.UploadValues(url, "POST", postData);
                    string responseBody = System.Text.Encoding.UTF8.GetString(responseBytes);
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
            //TO_DO, dovrei ricaricare il panel forse, per aggiornare gli esercizi
        }
    }
}
