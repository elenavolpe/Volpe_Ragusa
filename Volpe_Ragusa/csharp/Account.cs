using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
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
            flowLayoutPanel1.AutoSize=true;
            flowLayoutPanel1.Size = new Size(615, 424);
            flowLayoutPanel1.WrapContents = true;
            flowLayoutPanel1.FlowDirection=FlowDirection.TopDown;
            flowLayoutPanel1.AutoScroll=true;
            if(this.email=="admin@mail.it"){
                labelBenvenuto.Text="Benvenuto nel tuo profilo "+utente.name + ", qui puoi aggiungere o eliminare esercizi dalla lista ";
                buttonScheda.Enabled=false;
                buttonImpostazioni.Enabled=false;
                FlowLayoutPanel panel= new FlowLayoutPanel();
                panel.FlowDirection=FlowDirection.TopDown;
                panel.AutoSize=true;
                panel.BorderStyle=BorderStyle.FixedSingle;
                panel.Margin = new Padding(0, 20, 0, 20);
                labelAggiungi.Font = new Font(labelAggiungi.Font, FontStyle.Bold);
                flowLayoutPanel1.Controls.Add(labelAggiungi);
                panel.Controls.Add(label2);
                textBox1.Name="nome";
                panel.Controls.Add(textBox1);
                panel.Controls.Add(label3);
                textBox2.Name="descrizione";
                textBox2.Size = new Size(529, 200);
                panel.Controls.Add(textBox2);
                panel.Controls.Add(label4);
                panel.Controls.Add(checkedListBox1);
                Button buttonaggiungi= new Button();
                buttonaggiungi.AutoSize=true;
                buttonaggiungi.Text="aggiungi";
                buttonaggiungi.Click+=addEsercizio;
                panel.Controls.Add(buttonaggiungi);
                flowLayoutPanel1.Controls.Add(panel);
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
                if(utente.muscoli!=null){
                    string stringaConVirgole = string.Join(", ", utente.muscoli);
                    labelmuscoli.Text="muscoli preferiti: " + stringaConVirgole;
                }else{
                    labelmuscoli.Text="muscoli preferiti: nessuno";
                }
                panel.Controls.Add(labelmuscoli);
                flowLayoutPanel1.Controls.Add(panel);
                caricamenti.get_grafico(this.email,flowLayoutPanel1);
                if(utente.muscoli!=null){
                    caricamenti.carica_esercizi_consigliati(this.email,flowLayoutPanel1);
                }
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
                    //converto l'oggetto in json
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    // Invio di una richiesta POST
                    string response = client.UploadString(url, "POST", jsonData);
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
    }
}
