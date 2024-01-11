import pandas as pd
import matplotlib.pyplot as plt
from flask import Flask, send_file
import os

app = Flask(__name__)

def generate_muscle_stats(data):
    # Creazione dataframe per permettere di usare i metodi di Pandas
    df = pd.DataFrame(data)

    # Calcolo del conteggio dei muscoli allenati
    muscle_count = df.explode('Muscoli')['Muscoli'].value_counts()

    # Calcolo delle percentuali dei muscoli allenati rispetto al totale degli esercizi (len(df))
    muscle_percentage = (muscle_count / len(df)) * 100

    # Creazione di un grafico a torta per visualizzare le percentuali dei muscoli allenati
    plt.figure(figsize=(8, 6))
    muscle_percentage.plot(kind='pie', autopct='%1.1f%%', startangle=140)
    plt.title('Percentuali dei muscoli allenati')
    plt.ylabel('')
    plt.axis('equal')  # Garantisce un grafico dall'aspetto circolare, non ellittico
    plt.tight_layout()

    # Invece di mostrare il grafico, lo salva in un file così da poterlo visualizzare nella UI in C#

    # Creo una directory se non esiste per salvare l'immagine
    image_directory = 'downloadable'
    if not os.path.exists(image_directory):
        os.makedirs(image_directory)

    # Se invece vogliamo che il file lo trasmetta tramite Restful APIs, python è un server e usiamo Flask con la Route /get_muscle_stats
    
    # Salva l'immagine in un file nella directory creata
    image_path = os.path.join(image_directory, 'muscle_stats.png')
    plt.savefig(image_path)
    # Stampa dei risultati
    print("Percentuali dei muscoli allenati:\n", muscle_percentage)
    return image_path

    # plt.show()
    
    # Cambiare metoodo da creazione di un file a trasmissione tramite Restful APIs