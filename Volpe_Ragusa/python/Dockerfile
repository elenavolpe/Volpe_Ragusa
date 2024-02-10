# Usa un'immagine di Python come base
FROM python:3.10-slim

# Install pip requirements
COPY requirements.txt .
RUN python -m pip install -r requirements.txt

# Imposta la directory di lavoro nel container
WORKDIR /app

# Copia i file del tuo progetto nella directory di lavoro
COPY . .

# Installa Flask (se necessario)
#RUN pip install Flask

# Esporta la porta su cui l'applicazione Flask ascolterà
EXPOSE 5000

# Comando per avviare l'applicazione
CMD ["python", "server.py"]