from utils import connect_go_server
import user_manager

#ritorna la lista degli esercizi (tutti)
def get_exercises():
    try:
         r = connect_go_server('getExercises')
    except TypeError as e:
        return f"Errore: {e}"
    return

#ritorna i primi 3 esercizi più selezionati
def get_preferred():
    try:
         r = connect_go_server('getMostPopularExercises')
    except TypeError as e:
        return f"Errore: {e}"
    return

#ritorna gli esercizi aggiunti più di recente (i primi 3?)
def get_recent():
    try:
         r = connect_go_server('getMostRecentExercises')
    except TypeError as e:
        return f"Errore: {e}"
    return

#ritorna gli esercizi consigliati in base agli esercizi preferiti
def get_consigliati(email):
    muscoli=user_manager.get_muscoli_preferiti(email['email'])
    #TO_DO in realtà invece di chiedere di nuovo a go, potremmo semplicemente usare
    #la get_exercises e filtrare gli esercizi corrispondenti
    try:
         r = connect_go_server('getProposedExercises',muscoli)
    except TypeError as e:
        return f"Errore: {e}"
    return

#ritorna gli esercizi consigliati in base ai muscoli che stanno venendo trascurati
def get_trascurati(email):
    #TO_DO in realtà potremmo, usando get_scheda o get_muscle_stats, ottenere
    #una lista dei muscoli trascurati, e quindi usando get_exercises(già implementata),
    #ritornare gli esercizi adatti, così da evitare di chiedere di nuovo a go
    try:
         r = connect_go_server('getEserciziTrascurati',email)
    except TypeError as e:
        return f"Errore: {e}"
    return