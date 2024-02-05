from flask import Flask, request, send_file
import user_manager
import exercise_manager
from generate_muscle_stats import generate_muscle_stats

# TO-DO: recuperare dati muscoli da C# ,vedi riga 35

# Temporaneo: dati di esempio per test
data = {
    'Muscoli': [['Petto', 'Tricipiti'], ['Spalle'], ['Petto', 'Spalle', 'Tricipiti'], ['Spalle', 'Tricipiti', 'Petto'], 
                ['Quadricipiti', 'Glutei'], ['Glutei', 'Lombari'], ['Quadricipiti'], ['Bicipiti Femorali', 'Glutei'], 
                ['Schiena', 'Bicipiti'], ['Schiena', 'Bicipiti'], ['Schiena', 'Bicipiti', 'Dorsali'], ['Bicipiti', 'Avambracci']] # Muscoli coinvolti accoppiati in sequenza ad ogni esercizio della scheda
    #12 esercizi di esempio
}

app = Flask(__name__)

# Route per la registrazione di un nuovo utente
@app.route('/signin', methods=['POST'])
def signin():
    if request.method == 'POST':
        account = request.get_json()
        return user_manager.signin(account)

# Route per il login di un utente
@app.route('/login', methods=['POST'])
def login():
    if request.method == 'POST':
        account = request.get_json()
        return user_manager.login(account)

# Route per ottenere l'immagine delle statistiche dei muscoli allenati
@app.route('/get_muscle_stats', methods=['GET'])
def get_image():
    #TO_DO qui ti passo l'email, con get_scheda puoi trovare i muscoli allenati
    image = generate_muscle_stats(data)
    return send_file(image, mimetype='image/png')

#modifica profilo utente
@app.route('/modifica_profilo', methods=['POST'])
def modify_profile():
    if request.method == 'POST':
        account = request.get_json()
        return user_manager.modifica_profilo(account)
    
#ritorna la scheda utente
@app.route('/get_scheda', methods=['POST'])
def get_scheda():
    if request.method == 'POST':
        account = request.get_json()
        return user_manager.get_exercise(account)
    
#ritorna tutti gli esercizi, da mettere nella home
@app.route('/get_esercizi', methods=['POST'])
def get_esercizi():
    if request.method == 'POST':
        return exercise_manager.get_exercises()

#ritorna gli esercizi più quotati
@app.route('/get_esercizi_preferiti', methods=['POST'])
def get_preferiti():
    if request.method == 'POST':
        return exercise_manager.get_preferred()
    
#ritorna, se ci sono, esercizi aggiunti di recente (potremmo fare negli ultimi 2 giorni?)
@app.route('/get_esercizi_recenti', methods=['POST'])
def get_recenti():
    if request.method == 'POST':
        return  exercise_manager.get_recent()

#ritorna il nome data l'email    
@app.route('/get_name', methods=['POST'])
def get_nome():
    if request.method == 'POST':
        email = request.get_json()
        return  user_manager.get_name()

#ritorna gli esercizi consigliati in base ai muscoli preferiti    
@app.route('/get_consigliati', methods=['POST'])
def get_esercizi_consigliati():
    if request.method == 'POST':
        email = request.get_json()
        return  exercise_manager.get_consigliati(email)

#aggiunge esercizio alla scheda del cliente
@app.route('/aggiungi_esercizio', methods=['POST'])
def aggiungi_esercizio():
    if request.method == 'POST':
        email = request.form.get('email')
        esercizio = request.form.get('nomeEsercizio')
        return  user_manager.aggiungi_esercizio_scheda(email,esercizio)

#elimina esercizio dalla scheda del cliente
@app.route('/elimina_esercizio', methods=['POST'])
def elimina_esercizio():
    if request.method == 'POST':
        email = request.form.get('email')
        esercizio = request.form.get('nomeEsercizio')
        return  user_manager.elimina_esercizio_scheda(email,esercizio)

if __name__ == '__main__':
     app.run(host='0.0.0.0', port=5000, threaded=True) # Avvio il server Flask