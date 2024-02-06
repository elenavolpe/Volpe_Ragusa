from utils import connect_go_server, is_valid_email, is_valid_password

# (account) deve essere dizionario con i campi 'email' e 'password'
def login(account):
    if type(account) is not dict:
        return f"Errore login: account deve essere un dizionario, invece è di tipo {type(account)}"
    
    if is_valid_email(account['email']) and is_valid_password(account['password']):
        try:
            r = connect_go_server('verifypassword', account)
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
            #TO_DO go deve verificare che l'email non sia già stata usata,
            #nel caso ritorna "email già in uso"
            r = connect_go_server('signup', account)
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
        if account[key]!="":
            #prendo solo i valori non nulli
            modify[key]=account[key]

    if account['newpassword']!="":
        if account['newpassword']==account['password']:
            return "la nuova password non può essere uguale a quella vecchia"
        else:
            dict={}
            dict['oldPassword']=account['password']
            dict['email']=account['email']
            try:
                #TO_DO go deve ritornare ok se la vecchia password è uguale a password
                #TO_DO implementare questa funzione su go, sistemare come passare parametri
                r = connect_go_server('verifypassword',dict)
                #non ho ancora studiato go, non ho idea di cosa ritorna, per ora commento
                #se la verifica password è andata a buon fine
                try:
                    #TO_DO implementare questa funzione su go
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
            nome = connect_go_server('getName', email)
            print(nome)
            return nome
        except TypeError as e:
            return f"Errore: {e}"
        
#TO_DO federico mi dovresti ritornare le info dell'utente data l'email
def getInfo(email):
    if email['email']!="":
        try:
            utente = connect_go_server('getInfo', email)
            print(utente)
            return utente
        except TypeError as e:
            return f"Errore: {e}"
        
def get_exercise(account):
    if is_valid_email(account):
        try:
            r = connect_go_server('getWorkoutPlan', account)
            return r
        except TypeError as e:
            return f"Errore: {e}"
    else:
        return "email utente non valida"
    
def get_muscoli_preferiti(email):
    if is_valid_email(email):
        try:
            #TO_DO implementare questa funzione in go
            r = connect_go_server('getPreferredMuscles', email)
            #devo ritornarli
        except TypeError as e:
            return f"Errore: {e}"
        return r
    else:
        return "email utente non valida"

#aggiunge alla scheda di email l'esercizio
def aggiungi_esercizio_scheda(email,esercizio):
    dict={}
    dict['email']=email
    dict['exercise']=esercizio
    try:
        #TO_DO da sistemare, ho visto che hai aggiunto ripetute e cose del genere
        r = connect_go_server('addExerciseWorkout', dict)
        #devo ritornare ok
    except TypeError as e:
        return f"Errore: {e}"

#elimina dalla scheda di email l'esercizio
def elimina_esercizio_scheda(email,esercizio):
    dict={}
    dict['email']=email
    dict['exercise']=esercizio
    try:
        #TO_DO da sistemare anche su go
        r = connect_go_server('deleteExerciseWorkout', dict)
        #devo ritornare ok
    except TypeError as e:
        return f"Errore: {e}"