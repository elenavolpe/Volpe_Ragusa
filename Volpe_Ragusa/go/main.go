package main

import (
	"encoding/json"
	"fmt"
	"go_app/database"
	"go_app/types"
	"log"
	"net/http"
)

func main() {
	// Instanzio un nuovo mux per non usare quello di default ("DefaultServeMux") come best-practice del pacchetto go "net/http".
	mux := http.NewServeMux()

	// Definizione handlers richieste per le diverse routes gestite dal multiplexer/router mux

	// Endpoints per la gestione degli utenti
	mux.HandleFunc("/signup", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			var signupReq types.SignupReq
			err := json.NewDecoder(r.Body).Decode(&signupReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			usr := make(chan string) // Sarà la mail dell'utente se la registrazione è andata a buon fine, altrimenti "failure"
			var s string
			go database.Signup(signupReq.Name, signupReq.Surname, signupReq.Email, signupReq.Password, signupReq.Muscles, signupReq.Age, usr)
			s = <-usr
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/verifyadmin", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			var verifyReq types.EmailReq
			err := json.NewDecoder(r.Body).Decode(&verifyReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			isAdmin := make(chan bool)
			go database.AuthAdmin(verifyReq.Email, isAdmin)
			w.Header().Set("Content-Type", "application/json")
			err = json.NewEncoder(w).Encode(<-isAdmin)
			if err != nil {
				http.Error(w, err.Error(), http.StatusInternalServerError)
			}
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/login", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			var loginReq types.LoginReq
			err := json.NewDecoder(r.Body).Decode(&loginReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			done := make(chan bool)
			var s string
			go database.Login(loginReq.Email, loginReq.Password, done)
			if <-done {
				s = "ok"
			} else {
				s = "failure"
			}
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/verifypassword", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type VerifyReq struct {
				Email    string `json:"email"`
				Password string `json:"oldPassword"`
			}
			var loginReq VerifyReq
			err := json.NewDecoder(r.Body).Decode(&loginReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			done := make(chan bool)
			var s string
			go database.Login(loginReq.Email, loginReq.Password, done)
			if <-done {
				s = "ok"
			} else {
				s = "failure"
			}
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	// Queste 5 modify restiutiscono l'email utente in caso di successo, altrimenti "failure"
	mux.HandleFunc("/modifypassword", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type ModifyReq struct {
				Email       string `json:"email"`
				NewPassword string `json:"newpassword"`
			}
			var modifyReq ModifyReq
			err := json.NewDecoder(r.Body).Decode(&modifyReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			usr := make(chan string)
			var s string
			go database.ModifyPassword(modifyReq.Email, modifyReq.NewPassword, usr)
			s = <-usr
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/modifyemail", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type ModifyReq struct {
				Email    string `json:"email"`
				NewEmail string `json:"newemail"`
			}
			var modifyReq ModifyReq
			err := json.NewDecoder(r.Body).Decode(&modifyReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			usr := make(chan string)
			var s string
			go database.ModifyEmail(modifyReq.Email, modifyReq.NewEmail, usr)
			s = <-usr
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/modifyname", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type ModifyReq struct {
				Email   string `json:"email"`
				NewName string `json:"newname"`
			}
			var modifyReq ModifyReq
			err := json.NewDecoder(r.Body).Decode(&modifyReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			usr := make(chan string)
			var s string
			go database.ModifyName(modifyReq.Email, modifyReq.NewName, usr)
			s = <-usr
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/modifysurname", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type ModifyReq struct {
				Email      string `json:"email"`
				NewSurname string `json:"newsurname"`
			}
			var modifyReq ModifyReq
			err := json.NewDecoder(r.Body).Decode(&modifyReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			usr := make(chan string)
			var s string
			go database.ModifySurname(modifyReq.Email, modifyReq.NewSurname, usr)
			s = <-usr
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/modifyage", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type ModifyReq struct {
				Email  string `json:"email"`
				NewAge int    `json:"newage"`
			}
			var modifyReq ModifyReq
			err := json.NewDecoder(r.Body).Decode(&modifyReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			usr := make(chan string)
			var s string
			go database.ModifyAge(modifyReq.Email, modifyReq.NewAge, usr)
			s = <-usr
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/getInfo", func(w http.ResponseWriter, r *http.Request) {
		// Riceve l'email da python, ritorna, se non ci sono errori, tutti i dati dell'utente. Altrimenti ritorna un utente con il campo id settato a -1 e i campi stringa vuoti
		if r.Method == http.MethodPost {
			var emailReq types.EmailReq
			err := json.NewDecoder(r.Body).Decode(&emailReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			usr := make(chan types.User)
			go database.GetUserInfo(emailReq.Email, usr)
			w.Header().Set("Content-Type", "application/json")
			err = json.NewEncoder(w).Encode(<-usr) // I nomi dei campi del json di User si possono vedere in types.go
			if err != nil {
				http.Error(w, err.Error(), http.StatusInternalServerError)
			}
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	// Endpoints per la gestione degli esercizi
	mux.HandleFunc("/addExercise", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type ExerciseAndMuscles struct {
				Name        string   `json:"esercizio"`
				Description string   `json:"descrizione"`
				Muscles     []string `json:"muscoli"`
			}
			var ex ExerciseAndMuscles
			err := json.NewDecoder(r.Body).Decode(&ex)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			done := make(chan bool)
			var s string
			go database.AddExercise(ex.Name, ex.Description, ex.Muscles, done)
			if <-done {
				s = "success"
			} else {
				s = "failure"
			}
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/deleteExercise", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type DeleteReq struct {
				Name string `json:"name"`
			}
			var delReq DeleteReq
			err := json.NewDecoder(r.Body).Decode(&delReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			done := make(chan bool)
			var s string
			go database.DeleteExercise(delReq.Name, done)
			if <-done {
				s = "success"
			} else {
				s = "failure"
			}
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/editExerciseName", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type EditReq struct {
				OldName string `json:"oldName"`
				NewName string `json:"newName"`
			}
			var editReq EditReq
			err := json.NewDecoder(r.Body).Decode(&editReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			done := make(chan bool)
			var s string
			go database.EditExerciseName(editReq.OldName, editReq.NewName, done)
			if <-done {
				s = "success"
			} else {
				s = "failure"
			}
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/editExerciseDesc", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type EditReq struct {
				Name           string `json:"name"`
				NewDescription string `json:"newDescription"`
			}
			var editReq EditReq
			err := json.NewDecoder(r.Body).Decode(&editReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			done := make(chan bool)
			var s string
			go database.EditExerciseDescription(editReq.Name, editReq.NewDescription, done)
			if <-done {
				s = "success"
			} else {
				s = "failure"
			}
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/getExercises", func(w http.ResponseWriter, r *http.Request) {
		exercises := make(chan []types.ExerciseWorkout)
		go database.GetExercises(exercises)
		w.Header().Set("Content-Type", "application/json")
		err := json.NewEncoder(w).Encode(<-exercises)
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	mux.HandleFunc("/getMostPopularExercises", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			var limitReq map[string]int
			err := json.NewDecoder(r.Body).Decode(&limitReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			limit, exists := limitReq["limit"]
			if !exists {
				limit = 3
			}
			exercises := make(chan []types.Exercise)
			go database.GetMostPopularExercises(limit, exercises)
			w.Header().Set("Content-Type", "application/json")
			err = json.NewEncoder(w).Encode(<-exercises)
			if err != nil {
				http.Error(w, err.Error(), http.StatusInternalServerError)
			}
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/getMostRecentExercises", func(w http.ResponseWriter, r *http.Request) {
		exercises := make(chan []types.Exercise)
		go database.GetMostRecentExercises(exercises)
		w.Header().Set("Content-Type", "application/json")
		err := json.NewEncoder(w).Encode(<-exercises)
		if err != nil {
			http.Error(w, err.Error(), http.StatusInternalServerError)
		}
	})

	// Endpoints per le funzionalità di gestione dei dati relativi ai muscoli
	mux.HandleFunc("/getMuscles", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			muscles := make(chan []string)
			go database.GetMuscles(muscles)
			w.Header().Set("Content-Type", "application/json")
			err := json.NewEncoder(w).Encode(<-muscles)
			if err != nil {
				http.Error(w, err.Error(), http.StatusInternalServerError)
			}
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	// Endpoints per le funzionalità di gestione dei muscoli preferiti dell'utente
	mux.HandleFunc("/modifypreferredmuscles", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			type ModifyReq struct {
				Email               string   `json:"email"`
				NewPreferredMuscles []string `json:"newpreferredmuscles"`
			}
			var modifyReq ModifyReq
			err := json.NewDecoder(r.Body).Decode(&modifyReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			usr := make(chan string)
			var s string
			go database.ModifyPreferredMuscles(modifyReq.Email, modifyReq.NewPreferredMuscles, usr)
			s = <-usr // email dell'utente in caso di successo, altrimenti "failure"
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	// Endpoints per le funzionalità di gestione degli esercizi nelle schede di allenamento
	mux.HandleFunc("/addExerciseWorkout", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			var addReq types.UserExerciseReq
			err := json.NewDecoder(r.Body).Decode(&addReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			done := make(chan bool)
			var s string
			go database.AddExerciseWorkoutplan(addReq.Email, addReq.Exercise, done)
			if <-done {
				s = "ok"
			} else {
				s = "failure"
			}
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/deleteExerciseWorkout", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			var deleteReq types.UserExerciseReq
			err := json.NewDecoder(r.Body).Decode(&deleteReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			done := make(chan bool)
			var s string
			go database.DeleteExerciseWorkoutplan(deleteReq.Email, deleteReq.Exercise, done)
			if <-done {
				s = "ok"
			} else {
				s = "failure"
			}
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/getName", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			var emailReq types.EmailReq
			err := json.NewDecoder(r.Body).Decode(&emailReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			name := make(chan string)
			var s string
			go database.GetUserName(emailReq.Email, name)
			s = <-name
			w.Write([]byte(s))
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/getWorkoutPlan", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			var emailReq types.EmailReq
			err := json.NewDecoder(r.Body).Decode(&emailReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			w_plan := make(chan []types.ExerciseWorkout)
			go database.GetWorkoutPlan(emailReq.Email, w_plan)
			w.Header().Set("Content-Type", "application/json")
			err = json.NewEncoder(w).Encode(<-w_plan)
			if err != nil {
				http.Error(w, err.Error(), http.StatusInternalServerError)
			}
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
	})

	mux.HandleFunc("/getPreferredMuscles", func(w http.ResponseWriter, r *http.Request) {
		if r.Method == http.MethodPost {
			var emailReq types.EmailReq
			err := json.NewDecoder(r.Body).Decode(&emailReq)
			if err != nil {
				http.Error(w, "Errore durante la decodifica del JSON: "+err.Error(), http.StatusBadRequest)
				return
			}
			muscles := make(chan []string)
			go database.GetPreferredMuscles(emailReq.Email, muscles)
			w.Header().Set("Content-Type", "application/json")
			err = json.NewEncoder(w).Encode(<-muscles)
			if err != nil {
				http.Error(w, err.Error(), http.StatusInternalServerError)
			}
		} else {
			http.Error(w, "Metodo di richiesta non valido!", http.StatusMethodNotAllowed)
		}
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
