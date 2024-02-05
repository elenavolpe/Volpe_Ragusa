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
    try:
         r = connect_go_server('getProposedExercises',muscoli)
    except TypeError as e:
        return f"Errore: {e}"
    return