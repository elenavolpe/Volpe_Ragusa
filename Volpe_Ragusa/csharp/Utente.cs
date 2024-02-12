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
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", this.email }
                    };
                    byte[] responseBytes = client.UploadValues(url, "POST", postData);
                    string responseBody = Encoding.UTF8.GetString(responseBytes);
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
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", this.email }
                    };
                    byte[] responseBytes = client.UploadValues(url, "POST", postData);
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
