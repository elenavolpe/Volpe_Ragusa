using System.Text;
using System.Xml.Linq;
using Volpe_Ragusa.csharp;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;

namespace Volpe_Ragusa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Visible=true;
        }

        private void textBoxEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string email = textBoxEmail.Text;
            string password = textBoxPassword.Text;
            if (email != "" && password != "")
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        // URL del server Python
                        string pythonServerUrl = "http://localhost:5000/login";
                        // Creare un oggetto con i dati da inviare come JSON
                        var dataToSend = new
                        {
                            email = email,
                            password = password
                        };
                        string jsonData = JsonConvert.SerializeObject(dataToSend);
                        // Imposto l'intestazione Content-Type
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        // Invio di una richiesta POST
                        string response = client.UploadString(pythonServerUrl, "POST", jsonData);
                        
                        //se la risposta è positiva posso passare direttamente a home o account come parametro nome e email
                        if(response=="ok"){
                            Utente utente=Utente.Istanza;
                            utente.setAddress(email);
                            Account account = new Account();
                            this.Visible=false;
                            account.ShowDialog();
                        }else{
                            labelErrore.Text="email e/o password errati";
                            labelErrore.Visible=true;
                        }
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                    }
                }
            }else{
                labelErrore.Text="Attenzione, i campi non possono essere vuoti";
                labelErrore.Visible=true;
            }
        }

        private void linkRegistrazione_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registrazione registrazione = new Registrazione();
            this.Visible=false;
            registrazione.ShowDialog();
        }
    }
}
