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
	mux.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
		fmt.Fprintf(w, "Home page")
	})

	mux.HandleFunc("/about", func(w http.ResponseWriter, r *http.Request) {
		fmt.Fprintf(w, "About us page")
	})

	mux.HandleFunc("/contact", func(w http.ResponseWriter, r *http.Request) {
		fmt.Fprintf(w, "Contact us")
	})

	mux.HandleFunc("/signup", func(w http.ResponseWriter, r *http.Request) {
		name := r.FormValue("name")
		surname := r.FormValue("surname")
		email := r.FormValue("email")
		password := r.FormValue("password")
		usr := make(chan string) // Sarà la mail dell'utente se la registrazione è andata a buon fine, altrimenti "failure"
		var s string
		go signup(name, surname, email, password, usr)
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

	mux.HandleFunc("/login", func(w http.ResponseWriter, r *http.Request) {
		email := r.FormValue("email")
		password := r.FormValue("password")
		usr := make(chan string) // Sarà la mail dell'utente se la registrazione è andata a buon fine, altrimenti "failure"
		var s string
		go login(email, password, usr)
		s = <-usr
		w.Write([]byte(s))
	})

	mux.HandleFunc("/addExercise", func(w http.ResponseWriter, r *http.Request) {
		name := r.FormValue("name")
		description := r.FormValue("description")
		muscle := r.FormValue("muscle")
		done := make(chan bool)
		var s string
		go addExercise(name, description, muscle, done)
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

	mux.HandleFunc("/editExerciseMuscle", func(w http.ResponseWriter, r *http.Request) {
		oldMuscle := r.FormValue("oldMuscle")
		newMuscle := r.FormValue("newMuscle")
		done := make(chan bool)
		var s string
		go editExerciseMuscle(oldMuscle, newMuscle, done)
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
		err := json.NewEncoder(w).Encode(<-exercises) // Vedere come funziona json.NewEnconder(w).Encode()
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	mux.HandleFunc("/getMostPopularExercises", func(w http.ResponseWriter, r *http.Request) {
		limitParam := r.FormValue("limit")
		limitValue := 1
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
		limitValue := 1
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
			http.Error(w, "Invalid limit parameter", http.StatusBadRequest)
			return
		}
		ex_reps, err := strconv.Atoi(reps)
		if err != nil {
			http.Error(w, "Invalid limit parameter", http.StatusBadRequest)
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
			http.Error(w, "Invalid limit parameter", http.StatusBadRequest)
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
			http.Error(w, "Invalid limit parameter", http.StatusBadRequest)
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
