from utils import connect_go_server, is_valid_email, is_valid_password

# (account) deve essere dizionario con i campi 'email' e 'password'
def login(account):
    if type(account) is not dict:
        return f"Errore login: account deve essere un dizionario, invece è di tipo {type(account)}"
    
    if is_valid_email(account['email']) and is_valid_password(account['password']):
        try:
            r = connect_go_server('verifypassword', account)
            return r
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
    
    modify={}
    for key in account: #vedi se i muscoli te li passo bene
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
                r = connect_go_server('verifypassword',dict)
                if r=="ok":
                    try:
                        #TO_DO implementare questa funzione su go
                        # potremmo invece fare una funzione su go che modifica in questo caso la password e, negli altri casi, usare funzioni che modificano i campi corrispondenti?
                        # perché verrebbe più comodo, per esempio, fare una funzione che modifica la password in go, e una che modifica il nome in go, ecc. e python gestisce la chiamata a queste funzioni in base a quali campi sono stati modificati
                        # per la lista dei muscoli preferiti, invece, potremmo fare una funzione che aggiunge o toglie un muscolo preferito in go, o in un altro modo dipende da come l'hai pensato tu da C#
                        r = connect_go_server('modifypassword', modify)
                    except TypeError as e:
                        return f"Errore: {e}"
                else:
                    return "verifica password non andata a buon fine "
            except TypeError as e:
                return f"Errore: {e}"
    else: #in questo caso non c'è la necessità di verificare la password
        if account['newemail']!="":
            if account['newemail']==account['email']:
                return "la nuova email non può essere uguale a quella vecchia"
            else:
                if is_valid_email(account['newemail']):
                    try:
                        r = connect_go_server('modifyemail', modify)
                    except TypeError as e:
                        return f"Errore: {e}"
                else:
                    return "email non valida"
        if account['newname']!="":
            if account['newname']==account['name']:
                return "il nuovo nome non può essere uguale a quello vecchio"
            try:
                r = connect_go_server('modifyname', modify)
            except TypeError as e:
                return f"Errore: {e}"
        if account['newsurname']!="":
            if account['newsurname']==account['surname']:
                return "il nuovo cognome non può essere uguale a quello vecchio"
            try:
                r = connect_go_server('modifysurname', modify)
            except TypeError as e:
                return f"Errore: {e}"
        # Gli altri campi modifica la chiave se non è giusta, se ne mancano altri aggiungili
        if account['newage']!="":
            if account['newage']==account['age']:
                return "la nuova età non può essere uguale a quella vecchia"
            try:
                r = connect_go_server('modifyage', modify)
            except TypeError as e:
                return f"Errore: {e}"
        if account['newpreferredmuscles']!="":
            if account['newpreferredmuscles']==account['preferredmuscles']:
                return "i nuovi muscoli preferiti non possono essere uguali a quelli vecchi"
            try:
                r = connect_go_server('modifypreferredmuscles', modify)
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
        
def getInfo(email):
    if email['email']!="":
        try:
            utente = connect_go_server('getInfo', email)
            print(utente)
            if utente['id']==-1:
                return "errore"
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
            #TO_DO ritornami un json dei nomi in caso
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
    except TypeError as e:
        return f"Errore: {e}"

#elimina dalla scheda di email l'esercizio
def elimina_esercizio_scheda(email,esercizio):
    dict={}
    dict['email']=email
    dict['exercise']=esercizio
    try:
        r = connect_go_server('deleteExerciseWorkout', dict)
    except TypeError as e:
        return f"Errore: {e}"