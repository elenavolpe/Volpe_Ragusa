from utils import connect_go_server
import user_manager
import json

#ritorna la lista degli esercizi (tutti)
def get_exercises():
    try:
         r = connect_go_server('getExercises')
    except Exception as e:
        return f"Errore: {e}"
    return json.dumps(r)

#ritorna i primi 3 esercizi più selezionati
def get_preferred():
    try:
         r = connect_go_server('getMostPopularExercises')
    except Exception as e:
        return f"Errore: {e}"
    return json.dumps(r)

#ritorna gli esercizi aggiunti più di recente (negli ultimi 2 giorni)
def get_recent():
    try:
         r = connect_go_server('getMostRecentExercises')
    except Exception as e:
        return f"Errore: {e}"
    return json.dumps(r)

#ritorna gli esercizi consigliati in base ai muscoli preferiti
def get_consigliati(email):
    muscoli=json.loads(user_manager.get_muscoli_preferiti(email))
    esercizi=json.loads(get_exercises())
    consigliati=[]
    #scorro tutti gli esercizi
    for esercizio in esercizi:
        #muscoli dell'esercizio in questione
        muscoliEsercizio=esercizio['muscles']
        #scorro i muscoli preferiti
        for muscolo in muscoli:
            #se quell'esercizio allena almeno quel determinato muscolo
            if muscolo in muscoliEsercizio:
                consigliati.append(esercizio)
                break
    print(consigliati)
    return json.dumps(consigliati)

#ritorna la lista di tutti i muscoli allenabili
def getAllMuscles():
    try:
        muscoli=connect_go_server('getMuscles')
    except Exception as e:
        return f"Errore: {e}"
    return muscoli

#recupera una lista di muscoli allenati da un determinato utente
# get_muscle_stats necessita in input una lista di muscoli, con tutti i muscoli ripetuti
def get_muscoli_allenati_con_ripetuti(email):
    muscoli=[]
    eserciziScheda=json.loads(user_manager.get_exercise(email))
    for esercizio in eserciziScheda:
        for muscolo in esercizio['muscles']:
            muscoli.append(muscolo)
    return muscoli

#ritorna gli esercizi consigliati in base ai muscoli che stanno venendo trascurati
def get_trascurati(email):
    #recupera la lista di muscoli allenati dall'utente
    muscoliAllenati=get_muscoli_allenati_con_ripetuti(email)
    #recupero la lista di tutti i muscoli
    allMuscles=getAllMuscles()
    #creo una lista dei muscoli trascurati
    muscoliTrascurati=list(set(allMuscles)-set(muscoliAllenati))
    #prendo tutti gli esercizi
    eserciziAll=json.loads(get_exercises())
    consigliati=[]
    #scorro tutti gli esercizi
    for esercizio in eserciziAll:
        #muscoli dell'esercizio in questione
        muscoliEsercizio=esercizio['muscles']
        #scorro i muscoli trascurati
        for muscolo in muscoliTrascurati:
            #se quell'esercizio allena almeno quel determinato muscolo
            if muscolo in muscoliEsercizio:
                consigliati.append(esercizio)
                break
    print(consigliati)
    return json.dumps(consigliati)

#aggiunge un esercizio alla lista degli esercizi (admin)
def add_exercise_admin(esercizio):
    isAdmin = user_manager.authenticate_admin(esercizio['email'])
    if isAdmin:
        try:
            if esercizio['nome']!="" and esercizio['descrizione']!="" and len(esercizio['muscoli'])!=0:
                dict={}
                dict['esercizio']=esercizio['nome']
                dict['muscoli']=esercizio['muscoli']
                dict['descrizione']=esercizio['descrizione']
                r = connect_go_server('addExercise',dict)
                if r != "success":
                    return f"Errore nell'aggiunta dell'esercizio {dict['esercizio']}!"
            else:
                return "non possono esserci campi vuoti"
        except Exception as e:
            return f"Errore: {e}"
        return "ok"
    else:
        return "Non sei autorizzato a fare questa operazione!"

#elimina un esercizio dalla lista degli esercizi (admin)
def delete_exercise_admin(data):
    isAdmin = user_manager.authenticate_admin(data['email'])
    if isAdmin:
        dati={}
        dati['name']=data['name']
        try:
            r = connect_go_server('deleteExercise',dati)
            if r != "success":
                return "Errore nell'eliminazione dell'esercizio!"
        except Exception as e:
            return f"Errore: {e}"
        return r
    else:
        return "Non sei autorizzato a fare questa operazione!"