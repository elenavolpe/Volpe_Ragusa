package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"strconv"
)

func main() {
	// Instanzio un nuovo mux per non usare quello di default ("DefaultServeMux") come best-practice del pacchetto go.

	// It took me a long time to realise it isn't anything special. The default servemux is just a plain ol' servemux like we've already been using, which gets instantiated by default when the net/http package is used and is stored in a global variable. Here's the relevant line from the Go source:

	// 	Generally speaking, I recommended against using the default servemux because it makes your code less clear and explicit and it poses a security risk. Because it's stored in a global variable, any package is able to access it and register a route — including any third-party packages that your application imports. If one of those third-party packages is compromised, they could use the default servemux to expose a malicious handler to the web.
	// Instead it's better to use your own locally-scoped servemux, like we have been so far. But if you do decide to use the default servemux...
	// The net/http package provides a couple of shortcuts for registering routes with the default servemux: http.Handle() and http.HandleFunc(). These do exactly the same as their namesake functions we've already looked at, with the difference that they add handlers to the default servemux instead of one that you've created.
	// Additionally, http.ListenAndServe() will fall back to using the default servemux if no other handler is provided (that is, the second argument is set to nil).
	mux := http.NewServeMux()

	// Definizione handlers richieste per le diverse routes gestite dal multiplexer/router mux

	// Endpoints per la gestione degli utenti
	mux.HandleFunc("/signup", func(w http.ResponseWriter, r *http.Request) {
		name := r.FormValue("name")
		surname := r.FormValue("surname")
		email := r.FormValue("email")
		password := r.FormValue("password")
		string_age := r.FormValue("age")
		age, err := strconv.Atoi(string_age)
		if err != nil {
			http.Error(w, "Invalid parameter", http.StatusBadRequest)
			return
		}
		usr := make(chan string) // Sarà la mail dell'utente se la registrazione è andata a buon fine, altrimenti "failure"
		var s string
		go signup(name, surname, email, password, age, usr)
		s = <-usr
		w.Write([]byte(s))
	})

	mux.HandleFunc("/deleteAccount", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		done := make(chan bool)
		var s string
		go deleteAccount(email, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	// mux.HandleFunc("/modifyprofile", func(w http.ResponseWriter, r *http.Request) {
	// 	//TO_DO, riceve in input un dizionario con key e dato da modificare
	// 	//TO_DO hai idea se ti posso passare nel dizionario la stringa di muscoli?
	// 	//se va tutto bene ritornami la mail del cliente, sennò ritornami "failure"
	// 	// Qual' è la struttura delle informazioni che ricevo input?
	// })

	// mux.HandleFunc("/verifypassword", func(w http.ResponseWriter, r *http.Request) {
	//TO_DO, riceve in input l'email e la password, deve verificare che la password è corretta
	// questa è la login, proprio dopo, cambio l'endpoint successivo in "ok" e ritorno una stringa "success" in caso di successo, "failure" in caso di fallimento.
	// })

	mux.HandleFunc("/login", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		password := r.FormValue("password")
		done := make(chan bool) // Sarà il messaggio "ok" se il login è andato a buon fine, altrimenti "failure"
		var s string
		go login(email, password, done)
		if <-done {
			s = "ok"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/verifypassword", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		password := r.FormValue("oldPassword")
		done := make(chan bool)
		var s string
		go login(email, password, done)
		if <-done {
			s = "ok"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	// Queste 5 modify restiutiscono l'email utente in caso di successo, altrimenti "failure"
	mux.HandleFunc("/modifypassword", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		new_pw := r.FormValue("newpassword")
		usr := make(chan string)
		var s string
		go modifyPassword(email, new_pw, usr)
		s = <-usr
		w.Write([]byte(s))
	})

	mux.HandleFunc("/modifyemail", func(w http.ResponseWriter, r *http.Request) {
		old_email := r.FormValue("email")
		new_email := r.FormValue("newemail")
		usr := make(chan string)
		var s string
		go modifyEmail(old_email, new_email, usr)
		s = <-usr
		w.Write([]byte(s))
	})

	mux.HandleFunc("/modifyname", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		new_name := r.FormValue("newname")
		usr := make(chan string)
		var s string
		go modifyName(email, new_name, usr)
		s = <-usr
		w.Write([]byte(s))
	})

	mux.HandleFunc("/modifysurname", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		new_surname := r.FormValue("newsurname")
		usr := make(chan string)
		var s string
		go modifySurname(email, new_surname, usr)
		s = <-usr
		w.Write([]byte(s))
	})

	mux.HandleFunc("/modifyage", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		new_age := r.FormValue("newage")
		usr := make(chan string)
		var s string
		go modifyAge(email, new_age, usr)
		s = <-usr
		w.Write([]byte(s))
	})

	mux.HandleFunc("/getInfo", func(w http.ResponseWriter, r *http.Request) {
		// Riceve l'email da python, ritorna, se non ci sono errori, tutti i dati dell'utente. Altrimenti ritorna un utente con il campo id settato a -1 e i campi stringa vuoti
		email := r.FormValue("email")
		usr := make(chan User)
		go getUserInfo(email, usr)
		w.Header().Set("Content-Type", "application/json")
		err := json.NewEncoder(w).Encode(<-usr) // I nomi dei campi del json di User puoi vederli in types.go
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	// Endpoints per la gestione degli esercizi
	mux.HandleFunc("/addExercise", func(w http.ResponseWriter, r *http.Request) {
		name := r.FormValue("name")
		description := r.FormValue("description")
		done := make(chan bool)
		var s string
		go addExercise(name, description, done)
		if <-done {
			s = "Success"
		} else {
			s = "Failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/deleteExercise", func(w http.ResponseWriter, r *http.Request) {
		name := r.FormValue("name")
		done := make(chan bool)
		var s string
		go deleteExercise(name, done)
		if <-done {
			s = "Success"
		} else {
			s = "Failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/editExerciseName", func(w http.ResponseWriter, r *http.Request) {
		oldName := r.FormValue("oldName")
		newName := r.FormValue("newName")
		done := make(chan bool)
		var s string
		go editExerciseName(oldName, newName, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/editExerciseDesc", func(w http.ResponseWriter, r *http.Request) {
		oldName := r.FormValue("oldName")
		newDescription := r.FormValue("newDescription")
		done := make(chan bool)
		var s string
		go editExerciseDescription(oldName, newDescription, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/getExercises", func(w http.ResponseWriter, r *http.Request) {
		exercises := make(chan []Exercise)
		go getExercises(exercises)
		w.Header().Set("Content-Type", "application/json")
		err := json.NewEncoder(w).Encode(<-exercises) // Probablimente bisognerà spiegare come funziona json.NewEnconder(w).Encode()
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	mux.HandleFunc("/getMostPopularExercises", func(w http.ResponseWriter, r *http.Request) {
		limitParam := r.FormValue("limit")
		limitValue := 3
		var err error
		if limitParam != "" {
			limitValue, err = strconv.Atoi(limitParam)
			if err != nil {
				http.Error(w, "Invalid limit parameter", http.StatusBadRequest)
				return
			}
		}
		exercises := make(chan []Exercise)
		go getMostPopularExercises(limitValue, exercises)
		w.Header().Set("Content-Type", "application/json")
		err = json.NewEncoder(w).Encode(<-exercises)
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	mux.HandleFunc("/getMostRecentExercises", func(w http.ResponseWriter, r *http.Request) {
		limitParam := r.FormValue("limit")
		limitValue := 3
		var err error
		if limitParam != "" {
			limitValue, err = strconv.Atoi(limitParam)
			if err != nil {
				http.Error(w, "Invalid limit parameter", http.StatusBadRequest)
				return
			}
		}
		exercises := make(chan []Exercise)
		go getMostRecentExercises(limitValue, exercises)
		w.Header().Set("Content-Type", "application/json")
		err = json.NewEncoder(w).Encode(<-exercises)
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	// Endpoints per le funzionalità di gestione dei dati relativi ai muscoli

	mux.HandleFunc("/addMuscle", func(w http.ResponseWriter, r *http.Request) {
		muscle := r.FormValue("muscle")
		done := make(chan bool)
		var s string
		go addMuscle(muscle, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/editMuscleName", func(w http.ResponseWriter, r *http.Request) {
		old_muscle := r.FormValue("old_muscle")
		new_muscle := r.FormValue("new_muscle")
		done := make(chan bool)
		var s string
		go editMuscleName(old_muscle, new_muscle, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/deleteMuscle", func(w http.ResponseWriter, r *http.Request) {
		muscle := r.FormValue("muscle")
		done := make(chan bool)
		var s string
		go deleteMuscle(muscle, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	// Endpoints per le funzionalità di gestione dei muscoli relativi agli esercizi associati
	mux.HandleFunc("/addMuscleExercise", func(w http.ResponseWriter, r *http.Request) {
		ex_name := r.FormValue("ex_name")
		muscle_name := r.FormValue("muscle_name")
		done := make(chan bool)
		var s string
		go addMuscleExercise(ex_name, muscle_name, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/deleteMuscleExercise", func(w http.ResponseWriter, r *http.Request) {
		ex_name := r.FormValue("ex_name")
		muscle_name := r.FormValue("muscle_name")
		done := make(chan bool)
		var s string
		go deleteMuscleExercise(ex_name, muscle_name, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	// Endpoints per le funzionalità di gestione dei muscoli preferiti dell'utente
	mux.HandleFunc("/addPreferredMuscle", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		muscle_name := r.FormValue("muscle_name")
		done := make(chan bool)
		var s string
		go addPreferredMuscle(email, muscle_name, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/deletePreferredMuscle", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		muscle_name := r.FormValue("muscle_name")
		done := make(chan bool)
		var s string
		go deletePreferredMuscle(email, muscle_name, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/modifypreferredmuscles", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		new_surname := r.FormValue("newpreferredmuscles")
		usr := make(chan string)
		var s string
		go modifyPreferredMuscles(usr)
		s = <-usr
		w.Write([]byte(s))
	})

	// Endpoints per le funzionalità di gestione delle schede degli utenti
	mux.HandleFunc("/updateWorkoutName", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		wp_name := r.FormValue("workout_name")
		done := make(chan bool)
		var s string
		go updateUserWorkoutName(email, wp_name, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/updateWorkoutDesc", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		wp_desc := r.FormValue("workout_desc")
		done := make(chan bool)
		var s string
		go updateUserWorkoutDescription(email, wp_desc, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/deleteUserWorkout", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		done := make(chan bool)
		var s string
		go deleteUserWorkout(email, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	// Endpoints per le funzionalità di gestione degli esercizi nelle schede di allenamento
	mux.HandleFunc("/addExerciseWorkout", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		ex_name := r.FormValue("ex_name")
		sets := r.FormValue("sets")
		reps := r.FormValue("reps")
		ex_sets, err := strconv.Atoi(sets)
		if err != nil {
			http.Error(w, "Invalid parameter", http.StatusBadRequest)
			return
		}
		ex_reps, err := strconv.Atoi(reps)
		if err != nil {
			http.Error(w, "Invalid parameter", http.StatusBadRequest)
			return
		}
		done := make(chan bool)
		var s string
		go addExerciseWorkoutplan(email, ex_name, ex_sets, ex_reps, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/editExerciseSetsWorkout", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		ex_name := r.FormValue("ex_name")
		sets := r.FormValue("sets")
		ex_sets, err := strconv.Atoi(sets)
		if err != nil {
			http.Error(w, "Invalid parameter", http.StatusBadRequest)
			return
		}
		done := make(chan bool)
		var s string
		go editExerciseSetsWorkoutplan(email, ex_name, ex_sets, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/editExerciseRepsWorkout", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		ex_name := r.FormValue("ex_name")
		reps := r.FormValue("reps")
		ex_reps, err := strconv.Atoi(reps)
		if err != nil {
			http.Error(w, "Invalid parameter", http.StatusBadRequest)
			return
		}
		done := make(chan bool)
		var s string
		go editExerciseRepsWorkoutplan(email, ex_name, ex_reps, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/deleteExerciseWorkout", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		ex_name := r.FormValue("ex_name")
		done := make(chan bool)
		var s string
		go deleteExerciseWorkoutplan(email, ex_name, done)
		if <-done {
			s = "success"
		} else {
			s = "failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/getName", func(w http.ResponseWriter, r *http.Request) {
		//TO_DO, riceve in input l'email e deve ritornare il nome corrispondente
		email := r.FormValue("email")
		name := make(chan string)
		var s string
		go getUserName(email, name)
		s = <-name
		w.Write([]byte(s))
	})

	mux.HandleFunc("/getWorkoutPlan", func(w http.ResponseWriter, r *http.Request) {
		//TO_DO, riceve in input l'email e ritorna il workoutPlan
		email := r.FormValue("email")
		w_plan := make(chan []ExerciseWorkout)
		go getWorkoutPlan(email, w_plan)
		w.Header().Set("Content-Type", "application/json")
		err := json.NewEncoder(w).Encode(<-w_plan)
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	mux.HandleFunc("/getPreferredMuscles", func(w http.ResponseWriter, r *http.Request) {
		//TO_DO, riceve in input l'email e ritorna la lista dei muscoli preferiti
		email := r.FormValue("email")
		muscles := make(chan []string)
		go getPreferredMuscles(email, muscles)
		w.Header().Set("Content-Type", "application/json")
		err := json.NewEncoder(w).Encode(<-muscles)
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	// mux.HandleFunc("/getProposedExercises", func(w http.ResponseWriter, r *http.Request) {
	// 	//TO_DO, ritorna gli esercizi proposti in base ai muscoli preferiti(passati in input)
	// })

	// Porta del server
	port := 8080

	// Definizione server personalizzato che fa uso come handler del multiplexer/router mux
	server := &http.Server{
		Addr:    fmt.Sprintf(":%d", port),
		Handler: mux,
	}

	// Avvio del server
	fmt.Printf("Server is listening on port %d...\n", port)
	log.Fatal(server.ListenAndServe())
}
