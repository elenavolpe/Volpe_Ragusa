using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.Json;
public class Utente
{
    private static Utente _istanza;
    //private static readonly object _oggettoLock = new object();

    public string name;
    public string email;
    public string cognome;
    public int eta;
    public List<string> muscoli;

    // Il costruttore è privato per evitare l'istanziazione diretta della classe
    private Utente()
    {
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

    private string getInfo(string email)
        {
            string url = "http://localhost:5000/get_info";
            using (WebClient client = new WebClient()){
                try{
                    var dataToSend = new
                    {
                        email= this.email
                    };
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Creare il contenuto della richiesta POST
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    // Impostare l'intestazione Content-Type
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    // Invio di una richiesta POST
                    string responseBody = client.UploadString($"{url}", "POST", jsonData);
                    /*
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", this.email }
                    };
                    string jsonData = JsonConvert.SerializeObject(postData);
                    byte[] responseBytes = client.UploadValues(url, "POST", jsonData);
                    string responseBody = Encoding.UTF8.GetString(responseBytes);*/
                    if(responseBody!="errore"){
                        InfoUtente datiUtente = JsonConvert.DeserializeObject<InfoUtente>(responseBody);
                        Console.WriteLine(datiUtente);
                        this.email=datiUtente.email;
                        this.name=datiUtente.nome;
                        this.cognome=datiUtente.cognome;
                        this.eta=datiUtente.età;
                    }else{
                        Console.WriteLine("errore nel recupero dati dell'utente ");
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
            return name;
        }

    private List<string> getMuscles(string email)
        {
            string url = "http://localhost:5000/get_muscles";
            using (WebClient client = new WebClient()){
                try{
                    var dataToSend = new
                    {
                        email = this.email
                    };
                    // Serializzare l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Convertire il JSON in un array di byte
                    byte[] requestData = Encoding.UTF8.GetBytes(jsonData);
                    // Impostare l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuare la richiesta POST con i dati JSON
                    byte[] responseBytes = client.UploadData(url, "POST", requestData);
                    // Decodificare la risposta
                    string responseBody = Encoding.UTF8.GetString(responseBytes);
                    if(responseBody!="errore"){
                        Console.WriteLine(responseBody);
                        List<string> muscoli = JsonConvert.DeserializeObject<List<string>>(responseBody);
                        return muscoli;
                    }else{
                        Console.WriteLine("errore nel recupero dei muscoli dell'utente ");
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
            return null;
        }

    public void setAddress(string email)
    {
        this.email=email;
        if(email==""){
            this.name="";
            this.cognome="";
            this.eta=0;
            this.muscoli=null;
        }
        else{
            getInfo(email);
            muscoli=getMuscles(email);
        }
    }
}
