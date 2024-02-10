import re
import requests

def is_valid_email(email):
    # Pattern regex per validare l'email
    pattern = r'^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{2,}+$'
    
    # Cerco correpondenza tra l'email e il pattern
    match = re.search(pattern, email)
    
    return bool(match)

def is_valid_password(password):
    # Pattern regex per validare la password
    pattern = r'^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$'
    
    # Cerco correpondenza tra la password e il pattern
    match = re.search(pattern, password)
    
    return bool(match)

def connect_go_server(endpoint, pl):
    if type(pl) is not dict:
        raise TypeError("Il payload deve essere un dizionario")
    
    #TO_DO tu avevi messo 8081, ma mi sembra che ascolta su 8080, in caso ricambia
    url = "http://localhost:8080/" + str(endpoint)  # TO-DO: Server Go in ascolto su porta 8081
    payload = pl
    headers = {'Content-Type': 'application/json'}

    try:
        response = requests.post(url, json=payload, headers=headers)
        response.raise_for_status()  # Solleverà un'eccezione se la risposta ha uno stato di errore HTTP
    except requests.exceptions.RequestException as e:
        # Gestione delle eccezioni relative alle richieste HTTP
        return f"Errore durante la richiesta HTTP: {e}"
    except Exception as e:
        # Gestione generale delle eccezioni
        return f"Errore generico: {e}"
    else:
        # Blocco di codice eseguito solo se la richiesta ha avuto successo
        print("Richiesta HTTP riuscita!")
        print("Contenuto della risposta:", response.text)
        return response.text