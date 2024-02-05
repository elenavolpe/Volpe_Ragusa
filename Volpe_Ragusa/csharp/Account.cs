using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Volpe_Ragusa.csharp
{
    public partial class Account : Form
    {
        string email;
        /*public Account(string email)
        {
            InitializeComponent();
            this.email = email;
            string nome=get_name(email);
            labelBenvenuto.Text="Benvenuto nel tuo profilo "+nome;
            //TO_DO inserire qui tutti i caricamenti
        }*/

        public Account()
        {
            InitializeComponent();
            Utente utente=Utente.Istanza;
            this.email=utente.email;
            //da cambiare in base all'email che decidiamo per l'admin
            if(this.email=="admin@mail.it"){
                //TO_DO sistemare tutti i label
                buttonScheda.Hide();
                buttonImpostazioni.Hide();
            }
            else{
                carica_esercizi_consigliati();
                get_grafico();
            }
            labelBenvenuto.Text="Benvenuto nel tuo profilo "+utente.name;
            //TO_DO inserire qui tutti i caricamenti
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            this.Close();
            home.Show();
        }

        private void buttonScheda_Click(object sender, EventArgs e)
        {
            Scheda scheda= new Scheda();
            this.Close();
            scheda.Show();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            //esci dalla sessione e vai al login
            Utente utente = Utente.Istanza;
            utente.setAddress("");
            Form1 login = new Form1();
            this.Close();
            login.Show();
        }

        private void buttonImpostazioni_Click(object sender, EventArgs e)
        {
            Impostazioni impostazioni = new Impostazioni();
            this.Close();
            impostazioni.Show();
        }

        public void get_grafico()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_muscle_stats";
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", this.email }
                    };
                    byte[] responseBytes = client.UploadValues(url, "POST", postData);
                    //TO_DO dovrebbe tornare un immagine, quindi vediamo
                    /*string responseBody = System.Text.Encoding.UTF8.GetString(responseBytes);
                    string[] exercises = JsonConvert.DeserializeObject<string[]>(responseBody);
                    for (int i = 0; i < exercises.Length; i++)
                    {
                        Label label = new Label();
                        //vediamo cosa mi torna il json, creo label nome e label descrizione
                        //label.Text = $"Esercizio {i + 1}: {exercises[i]}";
                        label.Location = new System.Drawing.Point(20, 50 + 30 * i);
                        label.Size = new System.Drawing.Size(200, 20);
                        Controls.Add(label);
                    }*/
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        public void carica_esercizi_consigliati()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_consigliati";
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", this.email }
                    };
                    byte[] responseBytes = client.UploadValues(url, "POST", postData);
                    string responseBody = System.Text.Encoding.UTF8.GetString(responseBytes);
                    // Deserializza il JSON ricevuto
                    List<ExerciseData> exerciseList = JsonConvert.DeserializeObject<List<ExerciseData>>(responseBody);
                    foreach (ExerciseData exerciseData in exerciseList)
                    {   
                        FlowLayoutPanel panel= new FlowLayoutPanel();
                        panel.FlowDirection=FlowDirection.TopDown;
                        panel.AutoSize=true;

                        Label labelName = new Label();
                        labelName.Text=exerciseData.Exercise.Name;
                        labelName.Name="nome";
                        panel.Controls.Add(labelName);

                        Label labelDescription = new Label();
                        labelDescription.Text=exerciseData.Exercise.Description;
                        panel.Controls.Add(labelDescription);

                        Label labelMuscles = new Label();
                        foreach(string muscle in exerciseData.Muscles){
                            labelMuscles.Text=muscle +", ";
                        }
                        panel.Controls.Add(labelMuscles);
                        
                        Button button = new Button();
                        button.Size= new System.Drawing.Size(95,32);
                        //TO_DO si dovrebbe fare un controllo se è già aggiunto o meno
                        button.Text="aggiungi";
                        button.Click += aggiungiEsercizio;
                        panel.Controls.Add(button);
                        //TO_DO sistemare grandezza di questo panel
                        //PanelNovità.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        public void carica_muscoli_trascurati()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_trascurati";
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", this.email }
                    };
                    byte[] responseBytes = client.UploadValues(url, "POST", postData);
                    string responseBody = System.Text.Encoding.UTF8.GetString(responseBytes);
                    // Deserializza il JSON ricevuto
                    List<ExerciseData> exerciseList = JsonConvert.DeserializeObject<List<ExerciseData>>(responseBody);
                    foreach (ExerciseData exerciseData in exerciseList)
                    {   
                        FlowLayoutPanel panel= new FlowLayoutPanel();
                        panel.FlowDirection=FlowDirection.TopDown;
                        panel.AutoSize=true;

                        Label labelName = new Label();
                        labelName.Text=exerciseData.Exercise.Name;
                        labelName.Name="nome";
                        panel.Controls.Add(labelName);

                        Label labelDescription = new Label();
                        labelDescription.Text=exerciseData.Exercise.Description;
                        panel.Controls.Add(labelDescription);

                        Label labelMuscles = new Label();
                        foreach(string muscle in exerciseData.Muscles){
                            labelMuscles.Text=muscle +", ";
                        }
                        panel.Controls.Add(labelMuscles);
                        
                        Button button = new Button();
                        button.Size= new System.Drawing.Size(95,32);
                        //TO_DO si dovrebbe fare un controllo se è già aggiunto o meno
                        button.Text="aggiungi";
                        button.Click += aggiungiEsercizio;
                        panel.Controls.Add(button);
                        //TO_DO sistemare grandezza di questo panel
                        //PanelNovità.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        private void aggiungiEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Text="elimina";
            button.Click -= aggiungiEsercizio;
            button.Click += eliminaEsercizio;
            Control contenitore = button.Parent;
            Label label = contenitore.Controls.Find("nome", true).FirstOrDefault() as Label;
            string nomeEsercizio=label.Text;
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/aggiungi_esercizio";
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
        }

        private void eliminaEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Text="aggiungi";
            button.Click -= eliminaEsercizio;
            button.Click += aggiungiEsercizio;
            Control contenitore = button.Parent;
            Label label = contenitore.Controls.Find("nome", true).FirstOrDefault() as Label;
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
        }
    }
}
