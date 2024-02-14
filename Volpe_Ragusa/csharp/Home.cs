using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public Home()
        {
            InitializeComponent();
            Utente utente=Utente.Istanza;
            this.email = utente.email;
            Caricamenti caricamenti = new Caricamenti(this.email);
            if(this.email=="admin@mail.it"){
                button2.Enabled=false;
            }
            label1.Text="Ciao "+utente.name+", benvenuto in MyFitPlan";

            this.AutoScroll=true;

            flowLayoutPanel1.AutoSize=true;
            flowLayoutPanel1.FlowDirection=FlowDirection.TopDown;

            flowLayoutPanel1.Controls.Add(label2);
            flowLayoutPanel1.Controls.Add(PanelPreferred);
            PanelPreferred.FlowDirection=FlowDirection.TopDown;
            PanelPreferred.AutoSize=true;

            flowLayoutPanel1.Controls.Add(label3);
            flowLayoutPanel1.Controls.Add(PanelNovità);
            PanelNovità.FlowDirection=FlowDirection.TopDown;
            PanelNovità.AutoSize=true;

            flowLayoutPanel1.Controls.Add(label4);
            flowLayoutPanel1.Controls.Add(PanelExercises);
            PanelExercises.FlowDirection=FlowDirection.TopDown;
            PanelExercises.AutoSize=true;

            //carica_esercizi();
            caricamenti.carica_esercizi(this.email,PanelExercises);
            //carica_esercizi_novità();
            caricamenti.carica_esercizi_novità(this.email,PanelNovità);
            //carica_esercizi_preferiti();
            caricamenti.carica_esercizi_preferiti(this.email,PanelPreferred);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            this.Close();
            account.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Scheda scheda = new Scheda();
            this.Close();
            scheda.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 login=new Form1();
            this.Close();
            login.Show();
        }

        /*public void carica_esercizi()
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
                        PanelExercises.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
                    // Gestisci eventuali errori durante la richiesta HTTP
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }*/

        /*public void carica_esercizi_preferiti()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_esercizi_preferiti";
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
                        PanelPreferred.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }*/

        /*public void carica_esercizi_novità()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_esercizi_recenti";
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
                        PanelNovità.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }*/

        /*private void aggiungiEsercizio(object sender, EventArgs e)
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
        }*/

        /*private void eliminaEsercizio(object sender, EventArgs e)
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
        }*/
    }
}
