using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
public class Caricamenti{
    string email;
    List<string> listaEsercizi;
    public Caricamenti(string email){
        this.email=email;
        listaEsercizi=getListExercises(email);
    }
    public void carica_esercizi_consigliati(string email, FlowLayoutPanel contenitore)
        {
            Label label=new Label();
            label.Text="Ecco gli esercizi che ti consigliamo in base ai tuoi muscoli preferiti ";
            contenitore.Controls.Add(label);
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_consigliati";
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", email }
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
                        bool esiste=checkExist(exerciseData.Exercise.Name,listaEsercizi);
                        if(esiste==false){
                            button.Text="aggiungi";
                            button.Click += aggiungiEsercizio;
                        }else{
                            button.Text="elimina";
                            button.Click += eliminaEsercizio;
                        }
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
        }

        public void get_grafico(string email,FlowLayoutPanel contenitore)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_muscle_stats";
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", email }
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
                    Label label=new Label();
                    label.Text="ecco il grafico che rappresenta la % di muscoli che stai allenando in base alla tua scheda ";
                    contenitore.Controls.Add(label);
                    //flowLayoutPanel1.Controls.Add(grafico); TO_DO
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        public void carica_muscoli_trascurati(string email, FlowLayoutPanel contenitore)
        {
            Label label=new Label();
            label.Text="Perchè non aggiungi uno di questi esercizi? Sembrerebbe che stai trascurando qualche gruppo muscolare ";
            contenitore.Controls.Add(label);
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_trascurati";
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", email }
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
                        bool esiste=checkExist(exerciseData.Exercise.Name,listaEsercizi);
                        if(esiste==false){
                            button.Text="aggiungi";
                            button.Click += aggiungiEsercizio;
                        }else{
                            button.Text="elimina";
                            button.Click += eliminaEsercizio;
                        }
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
        }

        public void carica_esercizi(string email, FlowLayoutPanel contenitore)
        {
            if(this.email=="admin@mail.it"){ //TO_DO sistemare l'email
                Label label=new Label();
                label.Text="Ciao Admin, qui puoi eliminare o aggiungere esercizi dalla lista ";
                contenitore.Controls.Add(label);
            }
            using (WebClient client = new WebClient())
            {
                try
                {
                    // URL del server Python
                    string url = "http://localhost:5000/get_esercizi";
                    // Creazione dei dati da inviare come parte della richiesta POST
                    NameValueCollection postData = new NameValueCollection
                    {
                        { "email", email }
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
                        if(this.email=="admin@mail.it"){
                            button.Text="elimina";
                            button.Click += deleteEsercizio;
                        }
                        else{
                            bool esiste=checkExist(exerciseData.Exercise.Name,listaEsercizi);
                            if(esiste==false){
                                button.Text="aggiungi";
                                button.Click += aggiungiEsercizio;
                            }else{
                                button.Text="elimina";
                                button.Click += eliminaEsercizio;
                            }
                        }
                        panel.Controls.Add(button);
                        //TO_DO sistemare grandezza di questo panel
                        contenitore.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
                    // Gestisci eventuali errori durante la richiesta HTTP
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        public void carica_esercizi_preferiti(string email, FlowLayoutPanel contenitore)
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
                        bool esiste=checkExist(exerciseData.Exercise.Name,listaEsercizi);
                        if(esiste==false){
                            button.Text="aggiungi";
                            button.Click += aggiungiEsercizio;
                        }else{
                            button.Text="elimina";
                            button.Click += eliminaEsercizio;
                        }
                        panel.Controls.Add(button);
                        //TO_DO sistemare grandezza di questo panel
                        contenitore.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        public void carica_esercizi_novità(string email,FlowLayoutPanel contenitore)
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
                        bool esiste=checkExist(exerciseData.Exercise.Name,listaEsercizi);
                        if(esiste==false){
                            button.Text="aggiungi";
                            button.Click += aggiungiEsercizio;
                        }else{
                            button.Text="elimina";
                            button.Click += eliminaEsercizio;
                        }
                        panel.Controls.Add(button);
                        //TO_DO sistemare grandezza di questo panel
                        contenitore.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        public void get_scheda(string email, FlowLayoutPanel contenitore)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_scheda";
                    // Creazione dei dati da inviare come parte della richiesta POST
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
                        panel.FlowDirection=FlowDirection.LeftToRight;
                        panel.AutoSize=true;

                        Label labelName = new Label();
                        labelName.Text=exerciseData.Exercise.Name;
                        labelName.Name="nome";
                        panel.Controls.Add(labelName);

                        Label labelDescription = new Label();
                        labelDescription.Text=exerciseData.Exercise.Description;
                        panel.Controls.Add(labelDescription);

                        Button button = new Button();
                        button.Size= new System.Drawing.Size(90,30);
                        button.Text="elimina";
                        button.Click += eliminaEsercizio;
                        panel.Controls.Add(button);
                        //TO_DO sistemare grandezza di questo panel
                        contenitore.Controls.Add(panel);
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        private void aggiungiEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
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
                    if(responseBody=="ok"){
                        button.Text="elimina";
                        button.Click -= aggiungiEsercizio;
                        button.Click += eliminaEsercizio;
                        Console.WriteLine("inserimento andato a buon fine");
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        private void eliminaEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Control contenitore = button.Parent;
            Control contenitore1=contenitore.Parent;
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
                    if(responseBody=="ok"){
                        button.Text="aggiungi";
                        button.Click -= eliminaEsercizio;
                        button.Click += aggiungiEsercizio;
                        Console.WriteLine("eliminazione andata a buon fine");
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
            if(contenitore1.Name=="Scheda"){ //TO_DO vedere se è giusto
                //svuota il panel e lo ricarica così che si aggiornino gli esercizi
                contenitore.Controls.Clear();
                get_scheda(email,(FlowLayoutPanel)contenitore1);
            }
        }

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

        private void deleteEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
            //button.Text="aggiungi";
            button.Click -= deleteEsercizio;
            //button.Click += addEsercizio;
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
                    if(responseBody=="success"){
                        Console.WriteLine("eliminazione avvenuta con successo");
                        //TO_DO nel caso sistemare il colore del bordo, o renderla grigia
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        private List<string> getListExercises(string email){
            List<string> esercizi= new List<string>();
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_scheda";
                    // Creazione dei dati da inviare come parte della richiesta POST
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
                        esercizi.Add(exerciseData.Exercise.Name);
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
            return esercizi;
        }

        private bool checkExist(string nome, List<string> lista){
            foreach(string esercizio in lista){
                if(nome==esercizio){
                    return true;
                }
            }
            return false;
        }
}