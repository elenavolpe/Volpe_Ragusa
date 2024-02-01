from utils import connect_go_server
import user_manager

#forse potremmo farli chiedere da c# direttamente a go?
def get_exercises():
    try:
         r = connect_go_server('getExercises')
    except TypeError as e:
        return f"Errore: {e}"
    return

def get_preferred():
    try:
         r = connect_go_server('getMostPopularExercises')
    except TypeError as e:
        return f"Errore: {e}"
    return

def get_recent():
    try:
         r = connect_go_server('getMostRecentExercises')
    except TypeError as e:
        return f"Errore: {e}"
    return

def get_consigliati(email):
    try:
         r = connect_go_server('getProposedExercises',email['email'])
    except TypeError as e:
        return f"Errore: {e}"
    return

def get_grafico_muscoli(email):
    try:
        exercises=user_manager.get_exercise(email)
        #TO_DO chiamare modulo python che lo crea
        print("mi dava fastidio l'errore su except")
    except TypeError as e:
        return f"Errore: {e}"
    return