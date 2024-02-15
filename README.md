# Istruzioni per il deploy:

1.Scaricare [Docker Desktop](https://www.docker.com/products/docker-desktop) dal sito ufficiale.

2.Una volta scaricato il progetto, spostarsi nella cartella del progetto da shell e utilizzare il comando:
   ```bash
   docker-compose -f "docker-compose.yml" up -d --build
   ```
oppure, se si utilizza Linux e non è stato attivato l'alias docker-compose, utilizzare il comando:
   ```bash
   docker compose -f "docker-compose.yml" up -d --build 
   ```
3.Tramite shell raggiungere dalla directory di progetto la subdirectory Volpe_Ragusa e utilizzare il comando:
   ```bash
   dotnet run program.cs
   ```
## Descrizione dell'applicazione
L’applicazione da noi presentata ha lo scopo di fornire ai clienti una piattaforma sulla quale potersi iscrivere per poter creare, in base alle proprie preferenze, una propria scheda da palestra personalizzata. L’utente può infatti visualizzare nella Home una serie di esercizi con relativa descrizione e muscoli allenati e tramite interfaccia grafica, aggiungerli alla propria scheda. Il sistema nel suo Account, oltre a mostrargli le sue generalità, gli proporrà in base ai muscoli che l’utente ha scelto in fase di iscrizione, esercizi adatti alle sue esigenze, esercizi in base a gruppi muscolari che esso sta trascurando ed inoltre gli mostrerà un grafico a torta rappresentante le percentuali con cui sta allenando i suoi muscoli. Se l’accesso viene effettuato con l’account admin, esso avrà la possibilità di inserire o eliminare a suo piacimento gli esercizi dalla lista degli esercizi proposti.
