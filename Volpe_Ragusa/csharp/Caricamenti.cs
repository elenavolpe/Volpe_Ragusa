using System.Net;
using Newtonsoft.Json;
using System.Text;
public class Caricamenti{
    string email;
    List<string> listaEsercizi;
    public Caricamenti(string email){
        this.email=email;
        listaEsercizi=getListExercises(email);
    }
    public void carica_esercizi_consigliati(string email, FlowLayoutPanel contenitore)
        {
            TextBox textBox = new TextBox();
            textBox.Multiline = true;

            textBox.ReadOnly = true;
            textBox.Width = 500;
            textBox.Height = 100;
            textBox.Font = new Font(textBox.Font, FontStyle.Bold);
            textBox.Text = "Ecco gli esercizi che ti consigliamo in base ai tuoi muscoli preferiti ";

            contenitore.Controls.Add(textBox);
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_consigliati";
                    var dataToSend = new
                    {
                        email = this.email
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Impostare l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuare la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    // Deserializzo il JSON ricevuto
                    List<ExerciseData> exerciseList = JsonConvert.DeserializeObject<List<ExerciseData>>(responseBody);
                    if(exerciseList!=null){
                        foreach (ExerciseData exerciseData in exerciseList)
                        {   
                            FlowLayoutPanel panel= new FlowLayoutPanel();
                            panel.FlowDirection=FlowDirection.TopDown;
                            panel.AutoSize=true;
                            panel.Margin = new Padding(0, 20, 0, 20);
                            panel.BorderStyle = BorderStyle.FixedSingle;

                            Label labelName = new Label();
                            labelName.AutoSize=true;
                            labelName.Text=exerciseData.Exercise.Name;
                            labelName.Name="nome";
                            panel.Controls.Add(labelName);

                            TextBox descrizione = new TextBox();
                            descrizione.Multiline = true;
                            descrizione.ReadOnly = true;
                            descrizione.Width = 500;
                            descrizione.Height = 100;
                            descrizione.Text = exerciseData.Exercise.Description;
                            panel.Controls.Add(descrizione);

                            Label labelMuscles = new Label();
                            labelMuscles.AutoSize=true;
                            string stringaConVirgole = string.Join(", ", exerciseData.Muscles);
                            labelMuscles.Text = stringaConVirgole;
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

                            contenitore.Controls.Add(panel);
                        }
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
                    var dataToSend = new
                    {
                        email = this.email
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Converto il JSON in un array di byte
                    byte[] requestData = Encoding.UTF8.GetBytes(jsonData);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    byte[] responseBytes = client.UploadData(url, "POST", requestData);
                    //Converte i byte ricevuti in un'immagine
                    Image image = Image.FromStream(new MemoryStream(responseBytes));
                    
                    FlowLayoutPanel panel = new FlowLayoutPanel();
                    panel.AutoSize=true;
                    panel.FlowDirection=FlowDirection.TopDown;
                    panel.BorderStyle=BorderStyle.FixedSingle;
                    panel.Margin = new Padding(0, 20, 0, 20);

                    TextBox textBox = new TextBox();
                    textBox.Multiline = true;
                    textBox.ReadOnly = true;
                    textBox.Width = 500;
                    textBox.Height = 100;
                    textBox.Font = new Font(textBox.Font, FontStyle.Bold);
                    textBox.Text = "ecco il grafico che rappresenta la % di muscoli che stai allenando in base alla tua scheda ";
                    panel.Controls.Add(textBox);

                    PictureBox pictureBox= new PictureBox();
                    pictureBox.Size = new Size(500, 400);
                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox.Image=image;
                    panel.Controls.Add(pictureBox);
                    contenitore.Controls.Add(panel);
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        public void carica_muscoli_trascurati(string email, FlowLayoutPanel contenitore)
        {   
            TextBox textBox = new TextBox();
            textBox.Multiline = true;

            textBox.ReadOnly = true;
            textBox.Width = 500;
            textBox.Height = 100;
            textBox.Font = new Font(textBox.Font, FontStyle.Bold);
            textBox.Text = "Perchè non aggiungi uno di questi esercizi? Sembrerebbe che stai trascurando qualche gruppo muscolare ";
            contenitore.Controls.Add(textBox);
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_trascurati";
                    var dataToSend = new
                    {
                        email = this.email
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    // Deserializzo il JSON ricevuto
                    List<ExerciseData> exerciseList = JsonConvert.DeserializeObject<List<ExerciseData>>(responseBody);
                    if(exerciseList!=null){
                        foreach (ExerciseData exerciseData in exerciseList)
                        {   
                            FlowLayoutPanel panel= new FlowLayoutPanel();
                            panel.FlowDirection=FlowDirection.TopDown;
                            panel.AutoSize=true;
                            panel.Margin = new Padding(0, 20, 0, 20);
                            panel.BorderStyle = BorderStyle.FixedSingle; 

                            Label labelName = new Label();
                            labelName.AutoSize=true;
                            labelName.Text=exerciseData.Exercise.Name;
                            labelName.Name="nome";
                            panel.Controls.Add(labelName);

                            TextBox descrizione = new TextBox();
                            descrizione.Multiline = true;
                            descrizione.ReadOnly = true;
                            descrizione.Width = 500;
                            descrizione.Height = 100;
                            descrizione.Text = exerciseData.Exercise.Description;
                            panel.Controls.Add(descrizione);

                            Label labelMuscles = new Label();
                            labelMuscles.AutoSize=true;
                            string stringaConVirgole = string.Join(", ", exerciseData.Muscles);
                            labelMuscles.Text = stringaConVirgole;

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
                            contenitore.Controls.Add(panel);
                        }
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
            if(this.email=="admin@mail.it"){
                Label label=new Label();
                label.AutoSize=true;
                label.Font = new Font(label.Font, FontStyle.Bold);
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
                    var dataToSend = new
                    {
                        email = this.email
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    // Deserializza il JSON ricevuto
                    List<ExerciseData> exerciseList = JsonConvert.DeserializeObject<List<ExerciseData>>(responseBody);
                    if(exerciseList!=null){
                        foreach (ExerciseData exerciseData in exerciseList)
                        {   
                            FlowLayoutPanel panel= new FlowLayoutPanel();
                            panel.FlowDirection=FlowDirection.TopDown;
                            panel.AutoSize=true;
                            panel.Margin = new Padding(0, 20, 0, 20);
                            panel.BorderStyle = BorderStyle.FixedSingle; 

                            Label labelName = new Label();
                            labelName.AutoSize=true;
                            labelName.Text=exerciseData.Exercise.Name;
                            labelName.Name="nome";
                            panel.Controls.Add(labelName);

                            TextBox descrizione = new TextBox();
                            descrizione.Name="descrizione";
                            descrizione.Multiline = true;
                            descrizione.ReadOnly = true;
                            descrizione.Width = 500;
                            descrizione.Height = 100;
                            descrizione.Text = exerciseData.Exercise.Description;
                            panel.Controls.Add(descrizione);

                            Label labelMuscles = new Label();
                            labelMuscles.Name="muscoli";
                            labelMuscles.AutoSize=true;
                            string stringaConVirgole = string.Join(", ", exerciseData.Muscles);
                            labelMuscles.Text = stringaConVirgole;

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

                            contenitore.Controls.Add(panel);
                        }
                    }
                }
                catch (WebException ex)
                {
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
                    var dataToSend = new
                    {
                        email = this.email
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    // Deserializza il JSON ricevuto
                    List<Exercise> exerciseList = JsonConvert.DeserializeObject<List<Exercise>>(responseBody);
                    if(exerciseList!=null){
                        foreach (Exercise exerciseData in exerciseList)
                        {   
                            FlowLayoutPanel panel= new FlowLayoutPanel();
                            panel.FlowDirection=FlowDirection.TopDown;
                            panel.AutoSize=true;
                            panel.Margin = new Padding(0, 20, 0, 20);
                            panel.BorderStyle = BorderStyle.FixedSingle; 

                            Label labelName = new Label();
                            labelName.AutoSize=true;
                            labelName.Text=exerciseData.Name;
                            labelName.Name="nome";
                            panel.Controls.Add(labelName);

                            TextBox descrizione = new TextBox();
                            descrizione.Multiline = true;
                            descrizione.ReadOnly = true;
                            descrizione.Width = 500;
                            descrizione.Height = 100;
                            descrizione.Text = exerciseData.Description;
                            panel.Controls.Add(descrizione);
                            
                            Button button = new Button();
                            button.Size= new System.Drawing.Size(95,32);
                            if(email=="admin@mail.it"){
                                
                            }else{
                                bool esiste=checkExist(exerciseData.Name,listaEsercizi);
                                if(esiste==false){
                                    button.Text="aggiungi";
                                    button.Click += aggiungiEsercizio;
                                }else{
                                    button.Text="elimina";
                                    button.Click += eliminaEsercizio;
                                }
                                panel.Controls.Add(button);
                            }
                            contenitore.Controls.Add(panel);
                        }
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
                    var dataToSend = new
                    {
                        email = this.email
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    // Deserializzo il JSON ricevuto
                    List<Exercise> exerciseList = JsonConvert.DeserializeObject<List<Exercise>>(responseBody);
                    if(exerciseList!=null){
                        foreach (Exercise exerciseData in exerciseList)
                        {   
                            FlowLayoutPanel panel= new FlowLayoutPanel();
                            panel.FlowDirection=FlowDirection.TopDown;
                            panel.AutoSize=true;
                            panel.Margin = new Padding(0, 20, 0, 20);
                            panel.BorderStyle = BorderStyle.FixedSingle; 

                            Label labelName = new Label();
                            labelName.AutoSize=true;
                            labelName.Text=exerciseData.Name;
                            labelName.Name="nome";
                            panel.Controls.Add(labelName);

                            TextBox descrizione = new TextBox();
                            descrizione.Multiline = true;
                            descrizione.ReadOnly = true;
                            descrizione.Width = 500;
                            descrizione.Height = 100;
                            descrizione.Text = exerciseData.Description;
                            panel.Controls.Add(descrizione);
                            
                            Button button = new Button();
                            button.Size= new System.Drawing.Size(95,32);
                            if(email=="admin@mail.it"){
                                
                            }else{
                                bool esiste=checkExist(exerciseData.Name,listaEsercizi);
                                if(esiste==false){
                                    button.Text="aggiungi";
                                    button.Click += aggiungiEsercizio;
                                }else{
                                    button.Text="elimina";
                                    button.Click += eliminaEsercizio;
                                }
                                panel.Controls.Add(button);
                            }

                            contenitore.Controls.Add(panel);
                        }
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
            contenitore.Controls.Clear();
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/get_scheda";
                    // Creazione dei dati da inviare come parte della richiesta POST
                    var dataToSend = new
                    {
                        email = this.email
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    // Deserializzo il JSON ricevuto
                    List<ExerciseData> exerciseList = JsonConvert.DeserializeObject<List<ExerciseData>>(responseBody);
                    if(exerciseList!=null){
                        foreach (ExerciseData exerciseData in exerciseList)
                        {   
                            FlowLayoutPanel panel= new FlowLayoutPanel();
                            panel.FlowDirection=FlowDirection.TopDown;
                            panel.AutoSize=true;
                            panel.Margin = new Padding(0, 20, 0, 20);
                            panel.BorderStyle = BorderStyle.FixedSingle; 

                            Label labelName = new Label();
                            labelName.AutoSize=true;
                            labelName.Text=exerciseData.Exercise.Name;
                            labelName.Name="nome";
                            panel.Controls.Add(labelName);

                            TextBox descrizione = new TextBox();
                            descrizione.Multiline = true;
                            descrizione.ReadOnly = true;
                            descrizione.Width = 500;
                            descrizione.Height = 100;
                            descrizione.Text = exerciseData.Exercise.Description;
                            panel.Controls.Add(descrizione);

                            Button button = new Button();
                            button.Size= new System.Drawing.Size(90,30);
                            button.Text="elimina";
                            button.Click += eliminaEsercizio;
                            panel.Controls.Add(button);

                            contenitore.Controls.Add(panel);
                        }
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        //permette all'utente di aggiungere l'esercizio alla scheda
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
                    var dataToSend = new
                    {
                        email = this.email,
                        exercise=nomeEsercizio
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
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

        //permette all'utente di eliminare l'esercizio dalla scheda
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
                    var dataToSend = new
                    {
                        email = this.email,
                        exercise=nomeEsercizio
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
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
            if(contenitore1.Name=="Scheda"){
                //svuota il panel e lo ricarica così che si aggiornino gli esercizi
                contenitore.Controls.Clear();
                get_scheda(email,(FlowLayoutPanel)contenitore1);
            }
        }

        //permette all'admin di inserire l'esercizio alla lista di esercizi
        private void addEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Control contenitore = button.Parent;
            Label label = contenitore.Controls.Find("nome", true).FirstOrDefault() as Label;
            string nomeEsercizio=label.Text;
            TextBox box = contenitore.Controls.Find("descrizione", true).FirstOrDefault() as TextBox;
            string descrizioneEsercizio=box.Text;
            Label label2 = contenitore.Controls.Find("muscoli", true).FirstOrDefault() as Label;
            string stringamuscoli=label2.Text;
            List<string> muscoliSelezionati = new List<string>(stringamuscoli.Split(new string[] { ", " }, StringSplitOptions.None));
            Console.WriteLine(string.Join(", ", muscoliSelezionati));
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
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    if(responseBody=="ok"){
                        button.Text="elimina";
                        button.Click -= addEsercizio;
                        button.Click += deleteEsercizio;
                        Console.WriteLine("aggiunta avvenuta con successo");
                        contenitore.BackColor=Color.White;
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine($"Errore durante la richiesta HTTP: {ex.Message}");
                }
            }
        }

        //permette all'admin di eliminare un esercizio dalla lista di esercizi
        private void deleteEsercizio(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Control contenitore = button.Parent;
            Label label = contenitore.Controls.Find("nome", true).FirstOrDefault() as Label;
            string nomeEsercizio=label.Text;
            using (WebClient client = new WebClient())
            {
                try
                {
                    string url = "http://localhost:5000/delete_exercise";
                    var dataToSend = new
                    {
                        email = this.email,
                        name=nomeEsercizio
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    if(responseBody=="success"){
                        button.Text="aggiungi";
                        button.Click -= deleteEsercizio;
                        button.Click += addEsercizio;
                        Console.WriteLine("eliminazione avvenuta con successo");
                        contenitore.BackColor=Color.OrangeRed;
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
                    var dataToSend = new
                    {
                        email = this.email
                    };
                    // Serializzo l'oggetto in formato JSON
                    string jsonData = JsonConvert.SerializeObject(dataToSend);
                    // Imposto l'intestazione Content-Type sulla richiesta HTTP
                    client.Headers.Add("Content-Type", "application/json");
                    // Effettuo la richiesta POST con i dati JSON
                    string responseBody = client.UploadString(url, "POST", jsonData);
                    // Deserializzo il JSON ricevuto
                    List<ExerciseData> exerciseList = JsonConvert.DeserializeObject<List<ExerciseData>>(responseBody);
                    if(exerciseList!=null){
                    foreach (ExerciseData exerciseData in exerciseList)
                        {   
                            esercizi.Add(exerciseData.Exercise.Name);
                        }
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