package main

import (
	"fmt"
	"log"
	"net/http"
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
