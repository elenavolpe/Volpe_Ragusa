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
        Caricamenti caricamenti;
        public Account()
        {
            InitializeComponent();
            Utente utente=Utente.Istanza;
            this.email=utente.email;
            caricamenti= new Caricamenti(this.email);
            flowLayoutPanel1.Controls.Clear(); 
            flowLayoutPanel1.AutoSize=false;
            flowLayoutPanel1.Size = new Size(615, 424);
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.FlowDirection=FlowDirection.TopDown;
            flowLayoutPanel1.AutoScroll=true;
            //da cambiare in base all'email che decidiamo per l'admin
            if(this.email=="Admin@mail.it"){
                labelBenvenuto.Text="Benvenuto nel tuo profilo "+utente.name + ", qui puoi aggiungere o eliminare esercizi dalla lista ";
                buttonScheda.Hide();
                buttonImpostazioni.Hide();
                /*Label label2=new Label();
                label2.Text = "nome:";
                label2.AutoSize=true;
                Label labelAggiungi= new Label();
                labelAggiungi.Text = "Aggiungi esercizio";
                labelAggiungi.AutoSize=true;*/
                flowLayoutPanel1.Controls.Add(label2);
                flowLayoutPanel1.Controls.Add(labelAggiungi);
                flowLayoutPanel1.Controls.Add(textBox1);
                textBox1.Name="nome"; //vedi
                flowLayoutPanel1.Controls.Add(label3);
                flowLayoutPanel1.Controls.Add(textBox2);
                textBox2.Name="descrizione"; //vedi
                flowLayoutPanel1.Controls.Add(label4);
                flowLayoutPanel1.Controls.Add(checkedListBox1);
                Button buttonaggiungi= new Button();
                buttonaggiungi.Text="aggiungi";
                buttonaggiungi.Click+=addEsercizio;
                //carica_esercizi();
                caricamenti.carica_esercizi(this.email,flowLayoutPanel1);
            }
            else{
                labelBenvenuto.Text="Benvenuto nel tuo profilo "+utente.name;
                FlowLayoutPanel panel= new FlowLayoutPanel();
                panel.AutoSize=true;
                panel.FlowDirection=FlowDirection.TopDown;
                panel.BorderStyle=BorderStyle.FixedSingle;
                panel.Margin = new Padding(0, 20, 0, 0);
                Label labelemail= new Label();
                labelemail.AutoSize=true;
                labelemail.Text="email: " + utente.email;
                panel.Controls.Add(labelemail);
                Label labelnome= new Label();
                labelnome.AutoSize=true;
                labelnome.Text="nome: " + utente.name;
                panel.Controls.Add(labelnome);
                Label labelcognome= new Label();
                labelcognome.AutoSize=true;
                labelcognome.Text="cognome: " + utente.cognome;
                panel.Controls.Add(labelcognome);
                Label labeleta= new Label();
                labeleta.AutoSize=true;
                labeleta.Text="età: " + utente.eta.ToString();
                panel.Controls.Add(labeleta);
                Label labelmuscoli= new Label();
                labelmuscoli.AutoSize=true;
                string stringaConVirgole = string.Join(", ", utente.muscoli);
                labelmuscoli.Text="muscoli preferiti: " + stringaConVirgole;
                panel.Controls.Add(labelmuscoli);
                flowLayoutPanel1.Controls.Add(panel);
                //get_grafico();
                caricamenti.get_grafico(this.email,flowLayoutPanel1);
                //carica_esercizi_consigliati();
                caricamenti.carica_esercizi_consigliati(this.email,flowLayoutPanel1);
                //carica_muscoli_trascurati();
                caricamenti.carica_muscoli_trascurati(this.email,flowLayoutPanel1);
            }
            this.AutoScroll=true;
            flowLayoutPanel1.AutoSize=true;
            flowLayoutPanel1.FlowDirection=FlowDirection.TopDown;
        }

        private void addEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Control contenitore = button.Parent;
            TextBox box1 = contenitore.Controls.Find("nome", true).FirstOrDefault() as TextBox;
            string nomeEsercizio=box1.Text;
            TextBox box2 = contenitore.Controls.Find("descrizione", true).FirstOrDefault() as TextBox;
            string descrizioneEsercizio=box2.Text;
            List<string> muscoliSelezionati = new List<string>();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    muscoliSelezionati.Add(checkedListBox1.Items[i].ToString());
                }
            }
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/add_exercise";
                    var dataToSend = new
                        {
                            email= this.email,
                            nome= nomeEsercizio,
                            descrizione = descrizioneEsercizio,
                            muscoli = muscoliSelezionati
                        };
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    // Impostare l'intestazione Content-Type
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    // Invio di una richiesta POST
                    string response = client.UploadString($"{url}", "POST", jsonData);
                    // Leggi la risposta
                    Console.WriteLine($"Risposta dal server Python: {response}");
                    if(response=="ok"){
                        box1.Text="";
                        box2.Text="";
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
            caricamenti.carica_esercizi(this.email,flowLayoutPanel1);
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
