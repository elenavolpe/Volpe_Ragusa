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

#ritorna gli esercizi aggiunti più di recente (i primi 3?) Si ho messo limit 3 nella query di default, stessa cosa per getMostPopularExercises
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

#ritorna la lista di muscoli allenati da un determinato utente
def get_muscoli_allenati(email):
    muscoliAllenati=[]
    eserciziScheda=json.loads(user_manager.get_exercise(email))
    for esercizio in eserciziScheda:
        for muscolo in esercizio['muscles']:
            if muscolo not in muscoliAllenati:
                muscoliAllenati.append(muscolo)
    return muscoliAllenati

#recupera una lista di muscoli allenati da un determinato utente, ho fatto questa funzione e non ho modificato quella sopra perché la usiamo in get_trascurati e avevo paura a cambiarla senza che tu vedessi questa mia aggiunta.
# get_muscle_stats necessita in input una lista di muscoli, con tutti i muscoli ripetuti
def get_muscoli_allenati_con_ripetuti(email):
    muscoli = {}
    muscoliAllenati=[]
    eserciziScheda=json.loads(user_manager.get_exercise(email))
    for esercizio in eserciziScheda:
        for muscolo in esercizio['muscles']:
            muscoliAllenati.append(muscolo)
    muscoli['Muscoli'] = muscoliAllenati
    return muscoli

#ritorna gli esercizi consigliati in base ai muscoli che stanno venendo trascurati
def get_trascurati(email):
    #recupera la lista di muscoli allenati dall'utente
    muscoliAllenati=get_muscoli_allenati(email)
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
    isAdmin = user_manager.authenticate_admin(esercizio['email'], esercizio['password']) #TO_DO fede da dove la prendi la password?
    if isAdmin:
        try:
            r = connect_go_server('addExercise',esercizio)
            if r!="success":
                return "Errore nell'aggiunta dell'esercizio!"
            dict={}
            dict['esercizio']=esercizio['nome']
            for muscolo in esercizio['muscoli']:
                dict['muscolo']=muscolo
                try:
                    r = connect_go_server('addMuscleExercise',dict)
                    if r != "success":
                        return f"Errore nell'aggiunta del muscolo {dict['muscolo']}!"
                except Exception as e:
                    return f"Errore: {e}"
        except Exception as e:
            return f"Errore: {e}"
        return "ok"
    else:
        return "Non sei autorizzato a fare questa operazione!"

#elimina un esercizio dalla lista degli esercizi (admin)
def delete_exercise_admin(data):
    isAdmin = user_manager.authenticate_admin(data['email'])
    if isAdmin:
        #dict={}
        #dict['email']=email
        #dict['name']=nomeEsercizio
        try:
            #TO_DO data è di tipo email,exercise
            r = connect_go_server('deleteExercise',data)
            if r != "success":
                return "Errore nell'eliminazione dell'esercizio!"
        except Exception as e:
            return f"Errore: {e}"
        return r
    else:
        return "Non sei autorizzato a fare questa operazione!"