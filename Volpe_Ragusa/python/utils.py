import re
import requests
import json

def is_valid_email(email):
    # Pattern regex per validare l'email
    pattern = r'^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{2,}$'
    
    # Cerco corrispondenza tra l'email e il pattern
    match = re.search(pattern, email)
    
    return bool(match)

def is_valid_password(password):
    # Pattern regex per validare la password
    pattern = r'^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])[a-zA-Z\d\W_]{8,}$'
    
    # Cerco corrispondenza tra la password e il pattern
    match = re.search(pattern, password)
    
    return bool(match)

def connect_go_server(endpoint, pl={}):
    if not isinstance(pl, dict):
        raise TypeError("Il payload deve essere un dizionario")
    
    url = f"http://go:8080/{endpoint}"
    payload = pl
    headers = {'Content-Type': 'application/json'}

    try:
        response = requests.post(url, json=payload, headers=headers)
        response.raise_for_status()  # Solleverà un'eccezione se la risposta ha uno stato di errore HTTP
        try:
            # Provo a interpretare la risposta come JSON
            json_response = response.json()
            print("Richiesta HTTP riuscita!")
            print("Contenuto della risposta (JSON):", json_response)
            return json_response
        except json.JSONDecodeError:
            # Se la risposta non è un json, restituisco il contenuto come stringa
            print("Richiesta HTTP riuscita!")
            print("Contenuto della risposta (String):", response.text)
            return response.text
    except requests.exceptions.RequestException as e:
        # Gestione delle eccezioni relative alle richieste HTTP
        return f"Errore durante la richiesta HTTP: {e}"
    except Exception as e:
        return f"Errore generico: {e}"