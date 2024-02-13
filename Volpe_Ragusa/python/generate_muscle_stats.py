import pandas as pd
import matplotlib.pyplot as plt
from io import BytesIO

def generate_muscle_stats(data):
    
    # Creazione Series per permettere di usare i metodi di Pandas
    muscle_series = pd.Series(data)

    # Calcolo del conteggio dei muscoli allenati
    muscle_count = muscle_series.value_counts()

    # Calcolo delle percentuali dei muscoli allenati rispetto al totale degli esercizi (len(df))
    muscle_percentage = (muscle_count / len(muscle_series)) * 100

    # Creazione di un grafico a torta per visualizzare le percentuali dei muscoli allenati
    plt.figure(figsize=(8, 6))
    muscle_percentage.plot(kind='pie', autopct='%1.1f%%', startangle=140)
    plt.title('Percentuali dei muscoli allenati')
    plt.ylabel('')
    plt.axis('equal')  # Garantisce un grafico dall'aspetto circolare, non ellittico
    plt.tight_layout()

    # Se invece vogliamo che il file lo trasmetta tramite Restful APIs, python è un server e usiamo Flask con la Route /get_muscle_stats
    image_stream = BytesIO()
    plt.savefig(image_stream, format='png')
    image_stream.seek(0)
    
    # Stampa dei risultati
    print("Percentuali dei muscoli allenati:\n", muscle_percentage)
    
    return image_stream