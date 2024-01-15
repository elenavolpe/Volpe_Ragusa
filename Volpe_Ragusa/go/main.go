package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"strconv"
)

func main() {
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

	mux.HandleFunc("/register", func(w http.ResponseWriter, r *http.Request) {
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
		done := make(chan bool) // Sarà la mail dell'utente se la registrazione è andata a buon fine, altrimenti "failure"
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

	mux.HandleFunc("/editExercise", func(w http.ResponseWriter, r *http.Request) {
		oldName := r.FormValue("oldName")
		newName := r.FormValue("newName")
		newDescription := r.FormValue("newDescription")
		done := make(chan bool)
		var s string
		go editExercise(oldName, newName, newDescription, done)
		if <-done {
			s = "Success"
		} else {
			s = "Failure"
		}
		w.Write([]byte(s))
	})

	mux.HandleFunc("/getExercises", func(w http.ResponseWriter, r *http.Request) {
		exercises := make(chan []Exercise)
		go getExercises(exercises)
		w.Header().Set("Content-Type", "application/json")
		err := json.NewEncoder(w).Encode(<-exercises)
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	mux.HandleFunc("/getMostPopularExercises", func(w http.ResponseWriter, r *http.Request) {
		limitParam := r.FormValue("limit")
		limitValue, err := strconv.Atoi(limitParam)
		if err != nil {
			http.Error(w, "Invalid limit parameter", http.StatusBadRequest)
			return
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
		limitValue, err := strconv.Atoi(limitParam)
		if err != nil {
			http.Error(w, "Invalid limit parameter", http.StatusBadRequest)
			return
		}
		exercises := make(chan []Exercise)
		go getMostRecentExercises(limitValue, exercises)
		w.Header().Set("Content-Type", "application/json")
		err = json.NewEncoder(w).Encode(<-exercises)
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	// Endpoints per le funzionalità di gestione degli esercizi nelle schede di allenamento già esistenti
	// mux.HandleFunc("/addExerciseWorkoutPlan", func(w http.ResponseWriter, r *http.Request) {
	// 	workoutPlanName := r.FormValue("workoutPlanName")
	// 	exerciseName := r.FormValue("exerciseName")
	// 	sets := r.FormValue("sets")
	// 	reps := r.FormValue("reps")
	// 	done := make(chan bool)
	// 	var s string
	// 	go addExerciseWorkoutPlan(workoutPlanName, exerciseName, sets, reps, done)
	// 	if <-done {
	// 		s = "Success"
	// 	} else {
	// 		s = "Failure"
	// 	}
	// 	w.Write([]byte(s))
	// })

	// mux.HandleFunc("/deleteExerciseWorkoutPlan", func(w http.ResponseWriter, r *http.Request) {
	// 	workoutPlanName := r.FormValue("workoutPlanName")
	// 	exerciseName := r.FormValue("exerciseName")
	// 	done := make(chan bool)
	// 	var s string
	// 	go deleteExerciseWorkoutPlan(workoutPlanName, exerciseName, done)
	// 	if <-done {
	// 		s = "Success"
	// 	} else {
	// 		s = "Failure"
	// 	}
	// 	w.Write([]byte(s))
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
