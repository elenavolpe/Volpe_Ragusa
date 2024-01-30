using System.Text;
using System.Xml.Linq;
using Volpe_Ragusa.csharp;
using System.Net.Http;
using System.Threading.Tasks;
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
                        string pythonServerUrl = "http://localhost:5000/login"; //TO_DO da sistemare
                        // Creare un oggetto con i dati da inviare come JSON
                        var dataToSend = new
                        {
                            email = email,
                            password = password
                        };
                        string jsonData = JsonConvert.SerializeObject(dataToSend);
                        // Creare il contenuto della richiesta POST
                        StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        // Impostare l'intestazione Content-Type
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        // Invio di una richiesta POST
                        string response = client.UploadString($"{pythonServerUrl}/endpoint", "POST", jsonData);
                        // Leggi la risposta
                        Console.WriteLine($"Risposta dal server Python: {response}");
                        
                        //se la risposta è positiva posso passare direttamente a home o account come parametro nome e email
                        Account account = new Account(email);
                        this.Close();
                        account.Show();
                    }
                    catch (WebException ex)
                    {
                        // Gestisci eventuali errori durante la richiesta HTTP
                        Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                    }
                }
            }
        }

        private void linkRegistrazione_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registrazione registrazione = new Registrazione();
            this.Close();
            registrazione.Show();
        }
    }
}
