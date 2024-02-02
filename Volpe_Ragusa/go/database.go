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

	deleteUserQuery := "DELETE FROM users WHERE email = ?"
	_, err = db.Exec(deleteUserQuery, email)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Data deleted successfully!")

	// Aggiungere poi queries per cancellare i dati relativi all'utente dalle altre tabelle

	done <- true
}

func signup(name, surname, email, password string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	insertDataQuery := "INSERT INTO users (name, surname, email, pass) VALUES (?, ?, ?, ?)"
	_, err = db.Exec(insertDataQuery, name, surname, email, password)
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

	getUIDQuery := "SELECT id from users WHERE email = ?"
	uid = -1
	err = db.QueryRow(getUIDQuery, email).Scan(&uid)
	if err != nil {
		return // -1 è per segnalare il fallimento della query
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

	addExerciseQuery := "INSERT INTO exercises (name, description) VALUES (?, ?)"
	_, err = db.Exec(addExerciseQuery, name, description)
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

	deleteExerciseQuery := "DELETE FROM exercises WHERE name = ?"
	_, err = db.Exec(deleteExerciseQuery, name)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Exercise deleted successfully!")

	done <- true
}

func editExercise(old_name, new_name, new_description string, done chan<- bool) {
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
		editExerciseQuery := "UPDATE exercises SET description = ? WHERE name = ?"
		_, err = tx.Exec(editExerciseQuery, new_description, old_name)
		if err != nil {
			done <- false
			tx.Rollback()
			return
		}
		fmt.Println("Exercise description edited successfully!")
	}
	if new_name != "" {
		editExerciseQuery := "UPDATE exercises SET name = ? WHERE name = ?"
		_, err = tx.Exec(editExerciseQuery, new_name, old_name)
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

	getExercisesQuery := "SELECT name, description FROM exercises"
	rows, err := db.Query(getExercisesQuery)
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

	getExerciseIDQuery := "SELECT id from exercises WHERE name = ?"
	_, err = db.Exec(getExerciseIDQuery, name)
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

	getExercisesQuery := fmt.Sprintf("SELECT name, description FROM exercises ORDER BY popularity_score DESC LIMIT %d", limit_value)
	rows, err := db.Query(getExercisesQuery)
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

	getExercisesQuery := fmt.Sprintf("SELECT name, description FROM exercises ORDER BY created_at DESC LIMIT %d", limit_value)
	rows, err := db.Query(getExercisesQuery)
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
func addWorkoutplan(name, description, user_email string, done chan<- bool) {
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

	addWorkoutplanQuery := "INSERT INTO workoutplans (name, description, userid) VALUES (?, ?, ?)"
	_, err = db.Exec(addWorkoutplanQuery, name, description, uid)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Workout Plan added successfully!")

	done <- true
}

func editWorkoutplan(old_name, new_name, new_description, user_email string, done chan<- bool) {
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

	// Inizio transazione db
	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	if new_description != "" {
		editWorkoutplanQuery := "UPDATE workoutplans SET description = ? WHERE name = ?"
		_, err = tx.Exec(editWorkoutplanQuery, new_description, old_name)
		if err != nil {
			done <- false
			tx.Rollback()
			return
		}
		fmt.Println("Workout plan description edited successfully!")
	}
	if new_name != "" {
		editWorkoutplanQuery := "UPDATE workoutplans SET name = ? WHERE name = ?"
		_, err = tx.Exec(editWorkoutplanQuery, new_name, old_name)
		if err != nil {
			done <- false
			tx.Rollback()
			return
		}
		fmt.Println("Workout plan name edited successfully!")
	}

	err = tx.Commit()
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Transaction completed successfully!")

	done <- true
}

func getWorkoutplanID(name, description, user_email string) (wp_id int) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		return -1
	}

	getQuery := "SELECT id from workoutplans WHERE name = ? AND description = ? AND userid = ?"
	_, err = db.Exec(getQuery, name, description, uid)
	if err != nil {
		return -1
	}
	fmt.Println("Workout plan ID found successfully!")

	return
}

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

func deleteWorkoutplan(name, description, user_email string, done chan<- bool) {
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

	wp_id := getWorkoutplanID(name, description, user_email)
	if wp_id == -1 {
		done <- false
		return
	}

	if deleteWorkoutplanExercises(wp_id, -1) { // Prima di eliminare la scheda di allenamento, eliminare tutti gli esercizi associati
		deleteQuery := "DELETE FROM workoutplans WHERE id = ?"
		_, err = db.Exec(deleteQuery, wp_id)
		if err != nil {
			done <- false
			return
		}
		fmt.Println("Workout plan deleted successfully!")
	}

	done <- true
}

// ANCORA DA FARE!!!
// Funzioni per la gestione dei dati relativi alle schede di allenamento già esistenti (Non sono per nulla corrette, era un vecchio abbozzo...)
// func addExerciseWorkoutplan(workoutplanName, exerciseName, sets, reps string, done chan<- bool) {
// 	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
// 	if err != nil {
// 		done <- false
//		return
// 	}
// 	defer db.Close()

// 	addExerciseWorkoutplanQuery := "INSERT INTO workoutplan_exercise (workout_plan_name, exercise_name, sets, reps) VALUES (?, ?, ?, ?)"
// 	_, err = db.Exec(addExerciseWorkoutplanQuery, workoutplanName, exerciseName, sets, reps)
// 	if err != nil {
// 		done <- false
//		return
// 	}
// 	fmt.Println("Exercise added successfully!")

// 	done <- true
// }

// func deleteExerciseWorkoutplan(workoutplanName, exerciseName string, done chan<- bool) {
// 	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
// 	if err != nil {
// 		done <- false
//		return
// 	}
// 	defer db.Close()

// 	deleteExerciseWorkoutplanQuery := "DELETE FROM workout_plan_exercise WHERE workout_plan_name = ? AND exercise_name = ?"
// 	_, err = db.Exec(deleteExerciseWorkoutplanQuery, workoutplanName, exerciseName)
// 	if err != nil {
// 		done <- false
//		return
// 	}
// 	fmt.Println("Exercise deleted successfully!")

//		done <- true
//	}
