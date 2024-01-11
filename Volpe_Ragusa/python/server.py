from flask import Flask, request
import user_manager

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
    image_path = generate_muscle_stats(data)
    return send_file(image_path, mimetype='image/png')


if __name__ == '__main__':
     app.run(host='0.0.0.0', port=5000, threaded=True) # Avvio il server Flask