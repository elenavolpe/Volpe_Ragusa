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

        public Account()
        {
            InitializeComponent();
            Utente utente=Utente.Istanza;
            this.email=utente.email;
            Caricamenti caricamenti= new Caricamenti(this.email);
            //da cambiare in base all'email che decidiamo per l'admin
            if(this.email=="admin@mail.it"){
                //TO_DO sistemare tutti i label
                labelBenvenuto.Text="Benvenuto nel tuo profilo "+utente.name + ", qui puoi aggiungere o eliminare esercizi dalla lista ";
                buttonScheda.Hide();
                buttonImpostazioni.Hide();
                //carica_esercizi();
                caricamenti.carica_esercizi(this.email,flowLayoutPanel1);
            }
            else{
                labelBenvenuto.Text="Benvenuto nel tuo profilo "+utente.name;
                labelEmail.Text=utente.email;
                labelNome.Text=utente.name;
                labelCognome.Text="cognome";
                labelEta.Text="età";
                labelMuscoli.Text="muscoli";
                //get_grafico();
                caricamenti.get_grafico(this.email,flowLayoutPanel1);
                //carica_esercizi_consigliati();
                caricamenti.carica_esercizi_consigliati(this.email,flowLayoutPanel1);
                //carica_muscoli_trascurati();
                caricamenti.carica_muscoli_trascurati(this.email,flowLayoutPanel1);
            }
            //TO_DO inserire qui tutti i caricamenti
            this.AutoScroll=true;
            flowLayoutPanel1.AutoSize=true;
            flowLayoutPanel1.FlowDirection=FlowDirection.TopDown;
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

        /*public void get_grafico()
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
                    /*Label label=new Label();
                    label.Text="ecco il grafico che rappresenta la % di muscoli che stai allenando in base alla tua scheda ";
                    flowLayoutPanel1.Controls.Add(label);
                    //flowLayoutPanel1.Controls.Add(grafico); TO_DO
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }*/

        /*public void carica_esercizi_consigliati()
        {
            Label label=new Label();
            label.Text="Ecco gli esercizi che ti consigliamo in base ai tuoi muscoli preferiti ";
            flowLayoutPanel1.Controls.Add(label);
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
        }*/

        /*public void carica_muscoli_trascurati()
        {
            Label label=new Label();
            label.Text="Perchè non aggiungi uno di questi esercizi? Sembrerebbe che stai trascurando qualche gruppo muscolare ";
            flowLayoutPanel1.Controls.Add(label);
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
        }*/

        /*public void carica_esercizi()
        {
            Label label=new Label();
            label.Text="Ciao Admin, qui puoi eliminare o aggiungere esercizi dalla lista ";
            flowLayoutPanel1.Controls.Add(label);
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
                        button.Text="elimina";
                        button.Click += deleteEsercizio;
                        panel.Controls.Add(button);
                        //TO_DO sistemare grandezza di questo panel
                        flowLayoutPanel1.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
                    // Gestisci eventuali errori durante la richiesta HTTP
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }*/

        /*private List<string> get_scheda_utente(string email){

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

        /*private void addEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Text="elimina";
            button.Click -= addEsercizio;
            button.Click += deleteEsercizio;
            Control contenitore = button.Parent;
            Label label = contenitore.Controls.Find("nome", true).FirstOrDefault() as Label;
            string nomeEsercizio=label.Text;
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/add_exercise";
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

        /*private void deleteEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.Text="aggiungi";
            button.Click -= deleteEsercizio;
            button.Click += addEsercizio;
            Control contenitore = button.Parent;
            Label label = contenitore.Controls.Find("nome", true).FirstOrDefault() as Label;
            string nomeEsercizio=label.Text;
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/delete_exercise";
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
