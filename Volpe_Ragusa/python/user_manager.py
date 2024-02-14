from utils import connect_go_server, is_valid_email, is_valid_password
import json

# (account) deve essere dizionario con i campi 'email' e 'password'
def login(account):
    if not isinstance(account, dict):
        return f"Errore login: account deve essere un dizionario, invece è di tipo {type(account)}"
    
    if is_valid_email(account['email']) and is_valid_password(account['password']):
        try:
            r = connect_go_server('login', account)
            return r
        except Exception as e:
            return f"Errore: {e}"
    else:
        return "Errore formato email e/o password non validi"

# In questo caso account deve essere un dizionario con i campi 'email', 'password', 'name', 'surname'
def signin(account):
    if not isinstance(account, dict):
        return f"Errore signin: account deve essere un dizionario, invece è di tipo {type(account)}"
    
    if is_valid_email(account['email']) and is_valid_password(account['password']) and account['name'] != '' and account['surname'] != '':
        try:
            r = connect_go_server('signup', account)
            return r
        except Exception as e:
            return f"Errore: {e}"
    else:
        return "Errore: formato email e/o password non validi"
    
def modifica_profilo(account):
    if not isinstance(account, dict):
        return f"Errore signin: account deve essere un dizionario, invece è di tipo {type(account)}"    
    flag=False
    #nel caso in cui si vuole cambiare la password
    if account['newpassword']!='':
        if 'password' not in account or account['password']=='':
            return "inserire la vecchia password"
        #prima si verifica che non sia uguale a quella vecchia
        if account['newpassword']==account['password']:
            return "la nuova password non può essere uguale a quella vecchia"
        else:
            #poi si verifica che quella vecchia sia quella giusta
            dictionary={}
            dictionary['oldPassword']=account['password']
            dictionary['email']=account['email']
            try:
                r = connect_go_server('verifypassword',dictionary)
                if r=="ok":
                    try:
                        modify={}
                        modify['email']=account['email']
                        modify['newpassword']=account['newpassword']
                        r = connect_go_server('modifypassword', modify)
                        if r=="failure":
                            flag=True
                    except Exception as e:
                        return f"Errore: {e}"
                else:
                    return "verifica password non andata a buon fine "
            except Exception as e:
                return f"Errore: {e}"
    #in questo caso non c'è la necessità di verificare la password
    if account['nome']!="":
        try:
            modify={}
            modify['email']=account['email']
            modify['newname']=account['nome']
            r = connect_go_server('modifyname', modify)
            if r=="failure":
                flag=True
        except Exception as e:
            return f"Errore: {e}"
    if account['eta']!=0:
        try:
            modify={}
            modify['email']=account['email']
            modify['newage']=account['eta']
            r = connect_go_server('modifyage', modify)
            if r=="failure":
                flag=True
        except Exception as e:
            return f"Errore: {e}"
    if len(account['muscoli'])!=0:
        try:
            modify={}
            modify['email']=account['email']
            modify['newpreferredmuscles']=account['muscoli']
            r = connect_go_server('modifypreferredmuscles', modify)
            if r=="failure":
                flag=True
        except Exception as e:
            return f"Errore: {e}"
    if flag==True:
        return "qualcosa è andato storto"
    return "ok"
        
def get_name(email):
    if 'email' in email and email['email']!="":
        try:
            nome = connect_go_server('getName', email)
            print(nome)
            return nome
        except Exception as e:
            return f"Errore: {e}"
    return "email utente mancante"

#ritorna un json con le info dell'utente        
def getInfo(email):
    if 'email' in email and email['email']!="":
        try:
            print(email)
            utente = connect_go_server('getInfo', email)
            print(utente)
            if utente["id"]==-1:
                return "errore"
            return json.dumps(utente)
        except Exception as e:
            return f"Errore: {e}"
    return "email utente mancante"

#ritorna un json con la scheda dell'utente        
def get_exercise(account):
    #if is_valid_email(account):
        try:
            r = connect_go_server('getWorkoutPlan', account)
            return json.dumps(r)
        except Exception as e:
            return f"Errore: {e}"
    #else:
    #    return "email utente non valida"

#ritorna un json con i muscoli preferiti dell'utente    
def get_muscoli_preferiti(email):
    #if is_valid_email(email):
        try:
            r = connect_go_server('getPreferredMuscles', email)
        except Exception as e:
            return f"Errore: {e}"
        return json.dumps(r)
    #else:
    #    return "email utente non valida"

#aggiunge alla scheda di email l'esercizio
def aggiungi_esercizio_scheda(data):
    #dict={}
    #dict['email']=email
    #dict['exercise']=esercizio
    try:
        r = connect_go_server('addExerciseWorkout', data)
        return r
    except Exception as e:
        return f"Errore: {e}"

#elimina dalla scheda di email l'esercizio
def elimina_esercizio_scheda(data):
    #dict={}
    #dict['email']=email
    #dict['exercise']=esercizio
    try:
        r = connect_go_server('deleteExerciseWorkout', data)
        return r
    except Exception as e:
        return f"Errore: {e}"

#Verifica admin
def authenticate_admin(email):
    data={}
    data['email']=email
    try:
         r = connect_go_server('verifyadmin',data)
    except Exception as e:
        return f"Errore: {e}"
    return r