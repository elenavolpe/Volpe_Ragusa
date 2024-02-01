from utils import connect_go_server, is_valid_email, is_valid_password

# (account) deve essere dizionario con i campi 'email' e 'password'
def login(account):
    if type(account) is not dict:
        return f"Errore login: account deve essere un dizionario, invece è di tipo {type(account)}"
    
    if is_valid_email(account['email']) and is_valid_password(account['password']):
        try:
            r = connect_go_server('login', account)
        except TypeError as e:
            return f"Errore: {e}"
    else:
        return "Errore formato email e/o password non validi"

# In questo caso account deve essere un dizionario con i campi 'email', 'password', 'name', 'surname'
def signin(account):
    if type(account) is not dict:
        return f"Errore signin: account deve essere un dizionario, invece è di tipo {type(account)}"
    
    if is_valid_email(account['email']) and is_valid_password(account['password']) and account['name'] != '' and account['surname'] != '':
        try:
            r = connect_go_server('signin', account)
        except TypeError as e:
            return f"Errore: {e}"
    else:
        return "Errore: formato email e/o password non validi"
    
def modifica_profilo(account):
    if type(account) is not dict:
        return f"Errore signin: account deve essere un dizionario, invece è di tipo {type(account)}"
    
    #TO_DO potremmo prima fare autenticare il cliente?
    modify={}
    for key in account: #TO_DO non saprei come ritorna i muscoli
        if account['key']!="":
            #prendo solo i valori non nulli
            modify['key']=account['key']

    if account['newpassword']!="":
        try:
            #TO_DO go deve ritornare ok se la vecchia password è uguale a password
            #deve dare errore inoltre se la nuova password è uguale a quella vecchia
            r = connect_go_server('verifypassword', account['newpassword'],account['password'])
            #non ho ancora studiato go, non ho idea di cosa ritorna, per ora commento
            #se la verifica password è andata a buon fine
            try:
                r = connect_go_server('modifyprofile', modify)
            except TypeError as e:
                return f"Errore: {e}"
        except TypeError as e:
            return f"Errore: {e}"
    else: #in questo caso non c'è la necessità di verificare la password
        try:
            r = connect_go_server('modifyprofile', modify)
        except TypeError as e:
            return f"Errore: {e}"
        
def get_name(email):
    if email['email']!="":
        try:
            r = connect_go_server('getName', email['email'])
            #devo ritornare il nome
        except TypeError as e:
            return f"Errore: {e}"
        
def get_exercise(account):
    #TO_DO forse invece di fare questo possiamo chiedere a go di autenticarlo?
    if is_valid_email(account['email']):
        try:
            r = connect_go_server('getWorkoutPlan', account['email'])
            #devo ritornarli
        except TypeError as e:
            return f"Errore: {e}"
    else:
        return "email utente non valida"
    
def get_muscoli_preferiti(email):
    if is_valid_email(email):
        try:
            #TO_DO implementare questa funziona in go
            r = connect_go_server('getPreferredMuscles', email)
            #devo ritornarli
        except TypeError as e:
            return f"Errore: {e}"
    else:
        return "email utente non valida"