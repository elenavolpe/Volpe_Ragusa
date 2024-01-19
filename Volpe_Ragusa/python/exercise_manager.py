from utils import connect_go_server

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