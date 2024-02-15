using Newtonsoft.Json;
using System.Net;
using System.Text;
public class Utente
{
    private static Utente istanza;

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
            if (istanza == null)
            {
                istanza = new Utente();
            }

            return istanza;
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
                    // Imposto l'intestazione Content-Type
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    // Invio di una richiesta POST
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    if(responseBody!="errore"){
                        Console.WriteLine(responseBody);
                        InfoUtente datiUtente = JsonConvert.DeserializeObject<InfoUtente>(responseBody);
                        Console.WriteLine(datiUtente);
                        this.email=datiUtente.email;
                        this.name=datiUtente.name;
                        this.cognome=datiUtente.surname;
                        this.eta=datiUtente.age;
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
            Console.WriteLine("sto recuperando i muscoli");
            string url = "http://localhost:5000/get_muscles";
            using (WebClient client = new WebClient()){
                try{
                    var dataToSend = new
                    {
                        email = this.email
                    };
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    // Invio di una richiesta POST
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    Console.WriteLine(responseBody);
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
            //Console.WriteLine("vediamo");
            muscoli=getMuscles(email);
        }
    }
}
