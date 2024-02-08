from utils import connect_go_server
import user_manager
import json

#ritorna la lista degli esercizi (tutti)
def get_exercises():
    try:
         r = connect_go_server('getExercises')
    except Exception as e:
        return f"Errore: {e}"
    return json.loads(r)

#ritorna i primi 3 esercizi più selezionati
def get_preferred():
    try:
         r = connect_go_server('getMostPopularExercises')
    except Exception as e:
        return f"Errore: {e}"
    return json.loads(r)

#ritorna gli esercizi aggiunti più di recente (i primi 3?) Si ho messo limit 3 nella query di default, stessa cosa per getMostPopularExercises
def get_recent():
    try:
         r = connect_go_server('getMostRecentExercises')
    except Exception as e:
        return f"Errore: {e}"
    return json.loads(r)

#ritorna gli esercizi consigliati in base agli esercizi preferiti
def get_consigliati(email):
    muscoli=json.loads(user_manager.get_muscoli_preferiti(email))
    esercizi=json.loads(get_exercises()) # probabilmente non funzionerà
    consigliati=[] #vedi
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

def get_muscoli_allenati(email):
    eserciziScheda=user_manager.get_exercise(email)
    muscoliAllenati=[]
    for esercizio in eserciziScheda:
        for muscolo in esercizio['muscles']:
            if muscolo not in muscoliAllenati:
                #creo una lista di muscoli allenati
                muscoliAllenati.append(muscolo)

#ritorna gli esercizi consigliati in base ai muscoli che stanno venendo trascurati
def get_trascurati(email):
    #TO_DO vedi se si possono ottenere i muscoli trascurati direttamente
    #da get_muscle_stats
    muscoliAllenati=get_muscoli_allenati(email)
    #recupero la lista di tutti i muscoli
    try:
         #TO_DO Federico se mi puoi ritornare la lista di tutti i muscoli
         allMuscles = connect_go_server('getAllMuscles')
    except Exception as e:
        return f"Errore: {e}"
    #creo una lista dei muscoli trascurati
    muscoliTrascurati=list(set(allMuscles)-set(muscoliAllenati))
    #prendo tutti gli esercizi
    eserciziAll=get_exercises()
    consigliati=[] #vedi
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
    '''
        email= this.email,
        nome= nomeEsercizio,
        descrizione = descrizioneEsercizio,
        muscoli = muscoliSelezionati
    '''
    #muscoli è una lista di stringhe
    #prima verifica che l'utente sia l'admin
    isAdmin = user_manager.authenticate_admin(esercizio['email'])
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
        return
    else:
        return "Non sei autorizzato a fare questa operazione!"

#elimina un esercizio dalla lista degli esercizi (admin)
def delete_exercise_admin(email,nomeEsercizio):
    #prima verifica che l'utente sia l'admin
    isAdmin = user_manager.authenticate_admin(email)
    if isAdmin:
        dict={}
        dict['email']=email
        dict['name']=nomeEsercizio
        try:
            r = connect_go_server('deleteExercise',dict)
            if r != "success":
                return "Errore nell'eliminazione dell'esercizio!"
        except Exception as e:
            return f"Errore: {e}"
        return r
    else:
        return "Non sei autorizzato a fare questa operazione!"