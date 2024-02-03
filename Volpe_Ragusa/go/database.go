package main

import (
	"database/sql"
	"fmt"
	"log"

	_ "github.com/go-sql-driver/mysql" // Driver per la gestione del database MySQL (l'underscore indica che il package viene importato ma non utilizzato direttamente nel codice)
)

func ConnectDB(username, password, host, port, dbName string) (*sql.DB, error) {
	// Inizializzazione della gestione del database per il driver go-sql-driver/mysql
	dataSourceName := fmt.Sprintf("%s:%s@tcp(%s:%s)/%s", username, password, host, port, dbName)
	db, err := sql.Open("mysql", dataSourceName)
	if err != nil {
		return nil, err
	}

	// Poiché open non ci dice se la connessione con il db è avvenuta effettivamente, testiamo la connessione con il metodo Ping() della struct db:
	err = db.Ping()
	if err != nil {
		return nil, err
	}

	fmt.Println("Connected to MySQL!")

	return db, nil
}

func deleteAccount(email string, done chan<- bool) {
	//admin per collegarsi a mysql
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(email)
	if uid == -1 {
		done <- false
		return
	}

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	// Prima di cancellare l'utente, bisogna cancellare i dati relativi alla sua scheda
	deleteQuery := "DELETE FROM workoutplan_exercises WHERE userid = ?"
	_, err = tx.Exec(deleteQuery, uid)
	if err != nil {
		done <- false
		tx.Rollback()
		return
	}

	deleteQuery = "DELETE FROM users WHERE email = ?"
	_, err = tx.Exec(deleteQuery, email)
	if err != nil {
		done <- false
		tx.Rollback()
		return
	}

	err = tx.Commit()
	if err != nil {
		done <- false
		return
	}
	fmt.Println("All user data deleted successfully!")

	done <- true
}

func signup(name, surname, email, password, wp_name, wp_desc string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	signupQuery := "INSERT INTO users (name, surname, email, pass, workout_name, workout_description) VALUES (?, ?, ?, ?, ?, ?)"
	_, err = db.Exec(signupQuery, name, surname, email, password, wp_name, wp_desc)
	if err != nil {
		usr <- "failure"
		return
	}
	fmt.Println("Data inserted successfully!")

	usr <- email
}

func login(email, password string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	loginQuery := "SELECT name FROM users WHERE email = ? AND pass = ?"
	var name string
	err = db.QueryRow(loginQuery, email, password).Scan(&name)
	if err != nil {
		usr <- "failure"
		return
	}
	fmt.Println("User found!")

	usr <- email
}

func getUID(email string) (uid int) { // Funzione per ottenere l'User ID dell'account loggato richiedente
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := "SELECT id from users WHERE email = ?"
	err = db.QueryRow(getQuery, email).Scan(&uid)
	if err != nil {
		return -1 // -1 è per segnalare il fallimento della query
	}
	fmt.Println("UID found!")

	return
}

// Funzioni per la gestione dei dati relativi agli esercizi

func addExercise(name, description string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	addQuery := "INSERT INTO exercises (name, description) VALUES (?, ?)"
	_, err = db.Exec(addQuery, name, description)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Exercise added successfully!")

	done <- true
}

func deleteExercise(name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	deleteQuery := "DELETE FROM exercises WHERE name = ?"
	_, err = db.Exec(deleteQuery, name)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Exercise deleted successfully!")

	done <- true
}

func editExercise(old_name, new_name, new_description string, done chan<- bool) {
	if new_name == "" && new_description == "" {
		done <- false
		return
	}

	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	if new_description != "" {
		editQuery := "UPDATE exercises SET description = ? WHERE name = ?"
		_, err = tx.Exec(editQuery, new_description, old_name)
		if err != nil {
			done <- false
			tx.Rollback()
			return
		}
		fmt.Println("Exercise description edited successfully!")
	}
	if new_name != "" {
		editQuery := "UPDATE exercises SET name = ? WHERE name = ?"
		_, err = tx.Exec(editQuery, new_name, old_name)
		if err != nil {
			done <- false
			tx.Rollback()
			return
		}
		fmt.Println("Exercise name edited successfully!")
	}

	err = tx.Commit()
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Transaction completed successfully!")

	done <- true
}

func getExercises(exercises chan<- []Exercise) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := "SELECT name, description FROM exercises"
	rows, err := db.Query(getQuery)
	if err != nil {
		log.Fatal(err)
	}
	defer rows.Close()

	var exs []Exercise
	for rows.Next() {
		var ex Exercise
		err := rows.Scan(&ex.Name, &ex.Description)
		if err != nil {
			log.Fatal(err)
		}
		exs = append(exs, ex)
	}
	err = rows.Err()
	if err != nil {
		log.Fatal(err)
	}

	exercises <- exs
}

func getExerciseID(name string) (ex_id int) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := "SELECT id from exercises WHERE name = ?"
	err = db.QueryRow(getQuery, name).Scan(&ex_id)
	if err != nil {
		return -1
	}
	fmt.Println("Exercise ID found successfully!")

	return
}

func getMostPopularExercises(limit_value int, exercises chan<- []Exercise) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := fmt.Sprintf("SELECT name, description FROM exercises ORDER BY popularity_score DESC LIMIT %d", limit_value)
	rows, err := db.Query(getQuery)
	if err != nil {
		log.Fatal(err)
	}
	defer rows.Close()

	var exs []Exercise
	for rows.Next() {
		var ex Exercise
		err := rows.Scan(&ex.Name, &ex.Description)
		if err != nil {
			log.Fatal(err)
		}
		exs = append(exs, ex)
	}
	err = rows.Err()
	if err != nil {
		log.Fatal(err)
	}

	exercises <- exs
}

func getMostRecentExercises(limit_value int, exercises chan<- []Exercise) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := fmt.Sprintf("SELECT name, description FROM exercises ORDER BY created_at DESC LIMIT %d", limit_value)
	rows, err := db.Query(getQuery)
	if err != nil {
		log.Fatal(err)
	}
	defer rows.Close()

	var exs []Exercise
	for rows.Next() {
		var ex Exercise
		err := rows.Scan(&ex.Name, &ex.Description)
		if err != nil {
			log.Fatal(err)
		}
		exs = append(exs, ex)
	}
	err = rows.Err()
	if err != nil {
		log.Fatal(err)
	}

	exercises <- exs
}

// Funzioni per la gestione delle schede di allenamento (creazione, modifica, eliminazione)
func deleteWorkoutplanExercises(wp_id, ex_id int) (done bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	if ex_id == -1 { // Elimino tutti gli esercizi associati alla scheda di allenamento con id "wp_id"
		deleteQuery := "DELETE FROM workoutplan_exercises WHERE workoutplanid = ?"
		_, err = db.Exec(deleteQuery, wp_id)

		return err == nil
	}

	deleteQuery := "DELETE FROM workoutplan_exercises WHERE workoutplanid = ? AND exerciseid = ?"
	_, err = db.Exec(deleteQuery, wp_id, ex_id)

	return err == nil
}

// Funzioni per la gestione dei dati relativi alle schede di allenamento
func addExerciseWorkoutplan(user_email, ex_name string, ex_sets, ex_reps int, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		done <- false
		return
	}

	ex_id := getExerciseID(ex_name)
	if ex_id == -1 {
		done <- false
		return
	}

	addQuery := "INSERT INTO workoutplan_exercises (userid, exerciseid, sets, reps) VALUES (?, ?, ?, ?)"
	_, err = db.Exec(addQuery, uid, ex_id, ex_sets, ex_reps)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Exercise added to workout plan successfully!")

	done <- true
}

func editExerciseWorkoutplan(user_email, ex_name string, new_ex_sets, new_ex_reps int, done chan<- bool) {
	if new_ex_sets == -1 && new_ex_reps == -1 {
		done <- false
		return
	}

	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		done <- false
		return
	}

	ex_id := getExerciseID(ex_name)
	if ex_id == -1 {
		done <- false
		return
	}

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	if new_ex_sets != -1 {
		editQuery := "UPDATE workoutplan_exercises SET sets = ?  WHERE userid = ? AND exerciseid = ?"
		_, err = tx.Exec(editQuery, uid, ex_id, new_ex_sets)
		if err != nil {
			done <- false
			tx.Rollback()
			return
		}
		fmt.Println("Sets of the workout plan exercise edited successfully!")
	}

	if new_ex_reps != -1 {
		editQuery := "UPDATE workoutplan_exercises SET reps = ?  WHERE userid = ? AND exerciseid = ?"
		_, err = tx.Exec(editQuery, uid, ex_id, new_ex_reps)
		if err != nil {
			done <- false
			tx.Rollback()
			return
		}
		fmt.Println("Reps of the workout plan exercise edited successfully!")
	}

	err = tx.Commit()
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Transaction completed successfully!")

	done <- true
}

func deleteExerciseWorkoutplan(user_email, ex_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		done <- false
		return
	}

	ex_id := getExerciseID(ex_name)
	if ex_id == -1 {
		done <- false
		return
	}

	deleteQuery := "DELETE FROM workoutplan_exercises WHERE uid = ? AND ex_id = ?"
	_, err = db.Exec(deleteQuery, uid, ex_id)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Exercise deleted from workout plan successfully!")

	done <- true
}
