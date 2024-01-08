import pandas as pd
import matplotlib.pyplot as plt

# TO-DO: recuperare dati muscoli da Go

# Temporaneo: dati di esempio per test
data = {
    'Muscoli': [['Petto', 'Tricipiti'], ['Spalle'], ['Petto', 'Spalle', 'Tricipiti'], ['Spalle', 'Tricipiti', 'Petto'], ['Quadricipiti', 'Glutei'], ['Glutei', 'Lombari'], ['Quadricipit'], ['Bicipiti Femorali', 'Glutei'], ['Schiena', 'Bicipiti'], ['Schiena', 'Bicipiti'], ['Schiena', 'Bicipiti', 'Dorsali'], ['Bicipiti', 'Avambracci']] # Muscoli coinvolti accoppiati in sequenza ad ogni esercizio della scheda
    #12 esercizi di esempio
}

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
plt.show()

# Stampa dei risultati
print("Percentuali dei muscoli allenati:\n", muscle_percentage)