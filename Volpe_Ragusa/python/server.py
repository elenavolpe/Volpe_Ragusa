from flask import Flask, request, send_file
import user_manager
import exercise_manager
from generate_muscle_stats import generate_muscle_stats

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
@app.route('/get_muscle_stats', methods=['POST'])
def get_image():
    data=request.get_json()
    #prendo i muscoli allenati
    allenati=exercise_manager.get_muscoli_allenati_con_ripetuti(data)
    image = generate_muscle_stats(allenati)
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
    
#ritorna, se ci sono, esercizi aggiunti di recente (negli ultimi 2 giorni)
@app.route('/get_esercizi_recenti', methods=['POST'])
def get_recenti():
    if request.method == 'POST':
        return  exercise_manager.get_recent()

#ritorna il nome data l'email    
@app.route('/get_name', methods=['POST'])
def get_nome():
    if request.method == 'POST':
        email = request.get_json()
        return  user_manager.get_name(email)
    
#ritorna le info dell'utente data l'email    
@app.route('/get_info', methods=['POST'])
def get_info():
    if request.method == 'POST':
        email = request.get_json()
        return  user_manager.getInfo(email)

#ritorna gli esercizi consigliati in base ai muscoli preferiti    
@app.route('/get_consigliati', methods=['POST'])
def get_esercizi_consigliati():
    if request.method == 'POST':
        email = request.get_json()
        return  exercise_manager.get_consigliati(email)
    
#ritorna gli esercizi consigliati in base ai muscoli preferiti    
@app.route('/get_trascurati', methods=['POST'])
def get_esercizi_trascurati():
    if request.method == 'POST':
        email = request.get_json()
        return  exercise_manager.get_trascurati(email)

#aggiunge esercizio alla scheda del cliente
@app.route('/aggiungi_esercizio', methods=['POST'])
def aggiungi_esercizio():
    if request.method == 'POST':
        data=request.get_json()
        return  user_manager.aggiungi_esercizio_scheda(data)

#elimina esercizio dalla scheda del cliente
@app.route('/elimina_esercizio', methods=['POST'])
def elimina_esercizio():
    if request.method == 'POST':
        data=request.get_json()
        return  user_manager.elimina_esercizio_scheda(data)
    
#aggiunge esercizio alla lista di esercizi (admin)
@app.route('/add_exercise', methods=['POST'])
def add_exercise():
    if request.method == 'POST':
        esercizio=request.get_json()
        return  exercise_manager.add_exercise_admin(esercizio)
    
#aggiunge esercizio alla lista di esercizi (admin)
@app.route('/delete_exercise', methods=['POST'])
def delete_exercise():
    if request.method == 'POST':
        data=request.get_json()
        return  exercise_manager.delete_exercise_admin(data)
    
#ritorna i preferred muscles dell'utente
@app.route('/get_muscles', methods=['POST'])
def get_muscles():
    if request.method == 'POST':
        email = request.get_json()
        return  user_manager.get_muscoli_preferiti(email)

if __name__ == '__main__':
     app.run(host='0.0.0.0', port=5000, threaded=True) # Avvio il server Flask