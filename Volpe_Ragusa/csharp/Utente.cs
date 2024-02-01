using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using System.Text;
public class Utente
{
    private static Utente _istanza;
    //private static readonly object _oggettoLock = new object();

    public string name { get; set; }
    //public string cognome { get; set; }

    public string email;

    // Il costruttore è privato per evitare l'istanziazione diretta della classe
    private Utente()
    {
        //this.email=email;
        //this.nome=get_name(email);
    }

    public static Utente Istanza
    {
        get
        {
            // Utilizzo del double-check locking per migliorare le prestazioni e evitare problemi di concorrenza
            if (_istanza == null)
            {
                //lock (_oggettoLock)
                //{
                    //if (_istanza == null)
                    //{
                        _istanza = new Utente();
                    //}
                //}
            }

            return _istanza;
        }
    }

    private string get_name(string email)
        {
            string url = "http://localhost:5000/get_name";
            string name="nome";
            using (WebClient client = new WebClient()){
                try{
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", this.email }
                    };
                    byte[] responseBytes = client.UploadValues(url, "POST", postData);
                    name = Encoding.UTF8.GetString(responseBytes);
                    //TO_DO vedi se ritorna giusto
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
            return name;
        }

    public void setAddress(string email)
    {
        this.email=email;
        if(email==""){
            this.name="";
        }
        else{
            this.name=get_name(email);
        }
    }
}
