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

// Funzioni di gestione degli utenti

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
	deleteQuery := "DELETE FROM user_exercises WHERE userid = ?"
	_, err = tx.Exec(deleteQuery, uid)
	if err != nil {
		done <- false
		tx.Rollback()
		return
	}

	deleteQuery = "DELETE FROM users WHERE id = ?"
	_, err = tx.Exec(deleteQuery, uid)
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
	fmt.Println("All queries run successfully!")

	done <- true
}

func signup(name, surname, email, password string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	signupQuery := "INSERT INTO users (name, surname, email, pass) VALUES (?, ?, ?, ?)"
	_, err = db.Exec(signupQuery, name, surname, email, password)
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
		if err == sql.ErrNoRows {
			fmt.Println("No User found!")
		}
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
		if err == sql.ErrNoRows {
			fmt.Println("No User found!")
		}
		return -1 // -1 è per segnalare il fallimento della query
	}
	fmt.Println("UID found!")

	return
}

func getUserName(email string, name chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	user_name := ""

	getQuery := "SELECT name from users WHERE email = ?"
	err = db.QueryRow(getQuery, email).Scan(&user_name)
	if err != nil {
		if err == sql.ErrNoRows {
			fmt.Println("User not found!")
		}
		name <- "failure"
		return
	}
	fmt.Println("User found!")

	name <- user_name
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
	fmt.Println("Query run successfully!")

	done <- true
}

func editExerciseName(old_name, new_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE exercises SET name = ? WHERE name = ?"
	_, err = db.Exec(editQuery, new_name, old_name)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Exercise name edited successfully!")

	done <- true
}

func editExerciseDescription(old_name, new_description string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE exercises SET description = ? WHERE name = ?"
	_, err = db.Exec(editQuery, new_description, old_name)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Exercise description edited successfully!")

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
		log.Println(err)
		return
	}
	defer rows.Close()

	var exs []Exercise
	for rows.Next() {
		var ex Exercise
		err := rows.Scan(&ex.Name, &ex.Description)
		if err != nil {
			log.Println(err)
			return
		}
		exs = append(exs, ex)
	}
	err = rows.Err()
	if err != nil {
		log.Println(err)
		return
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
		if err == sql.ErrNoRows {
			fmt.Println("No Exercise found!")
		}
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
		log.Println(err)
		return
	}
	defer rows.Close()

	var exs []Exercise
	for rows.Next() {
		var ex Exercise
		err := rows.Scan(&ex.Name, &ex.Description)
		if err != nil {
			log.Println(err)
			return
		}
		exs = append(exs, ex)
	}
	err = rows.Err()
	if err != nil {
		log.Println(err)
		return
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
		log.Println(err)
		return
	}
	defer rows.Close()

	var exs []Exercise
	for rows.Next() {
		var ex Exercise
		err := rows.Scan(&ex.Name, &ex.Description)
		if err != nil {
			log.Println(err)
			return
		}
		exs = append(exs, ex)
	}
	err = rows.Err()
	if err != nil {
		log.Println(err)
		return
	}

	exercises <- exs
}

// Funzioni per la gestione dei dati relativi ai muscoli

func addMuscle(name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	addQuery := "INSERT INTO muscles (name) VALUES (?)"
	_, err = db.Exec(addQuery, name)
	if err != nil {
		done <- false
		return
	}
	fmt.Printf("Muscle added successfully!")

	done <- true
}

func editMuscleName(old_name, new_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE muscles SET name = ? WHERE name = ?"
	_, err = db.Exec(editQuery, new_name, old_name)
	if err != nil {
		done <- false
		return
	}
	fmt.Printf("Muscle added successfully!")

	done <- true
}

func getMuscleID(muscle string) (muscle_id int) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := "SELECT id from muscles WHERE name = ?"
	err = db.QueryRow(getQuery, muscle).Scan(&muscle_id)
	if err != nil {
		if err == sql.ErrNoRows {
			fmt.Println("No Muscle found!")
		}
		return -1 // -1 è per segnalare il fallimento della query
	}
	fmt.Println("Muscle's ID found!")

	return
}

func deleteMuscle(name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	muscle_id := getMuscleID(name)
	if muscle_id == -1 {
		done <- false
		return
	}

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	// Prima di cancellare il muscolo scelto, bisogna cancellare gli accompiamenti con i vari esercizi in cui è presente
	deleteQuery := "DELETE FROM exercise_muscles WHERE muscle = ?"
	_, err = tx.Exec(deleteQuery, muscle_id)
	if err != nil {
		done <- false
		tx.Rollback()
		return
	}

	deleteQuery = "DELETE FROM muscles WHERE id = ?"
	_, err = tx.Exec(deleteQuery, muscle_id)
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
	fmt.Println("All queries run successfully!")

	done <- true
}

// Funzioni per la gestione dei dati dei muscoli relativamente agli esercizi

// func addMuscleExercises(muscle_name, string, done chan<- bool) {
// 	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
// 	if err != nil {
// 		log.Fatal(err)
// 	}
// 	defer db.Close()

// 	addQuery := "INSERT INTO muscles (name) VALUES (?)"
// 	_, err = db.Exec(addQuery, name)
// 	if err != nil {
// 		done <- false
// 		return
// 	}
// 	fmt.Printf("Muscle added successfully!")

// 	done <- true
// }

// Funzioni per la gestione delle schede di allenamento (tabella users - campi workout_name e workout_description)
func updateUserWorkoutName(user_email, wp_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	addQuery := "UPDATE users SET workout_name = ? WHERE email = ?"
	_, err = db.Exec(addQuery, wp_name, user_email)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Workout plan for user added successfully!")

	done <- true
}

func updateUserWorkoutDescription(user_email, wp_desc string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	addQuery := "UPDATE users SET workout_description = ? WHERE email = ?"
	_, err = db.Exec(addQuery, wp_desc, user_email)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Workout plan for user added successfully!")

	done <- true
}

func deleteUserWorkout(user_email string, done chan<- bool) {
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

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	deleteQuery := "DELETE FROM user_exercises WHERE userid = ?"
	_, err = tx.Exec(deleteQuery, uid)
	if err != nil {
		done <- false
		tx.Rollback()
		return
	}
	fmt.Println("All queries run successfully")

	// Reset delle informazioni della scheda dell'utente (nome e descrizione a NULL)
	updateQuery := "UPDATE users SET workout_name = NULL, workout_description = NULL WHERE id = ?"
	_, err = tx.Exec(updateQuery, uid)
	if err != nil {
		done <- false
		tx.Rollback()
		return
	}
	fmt.Println("Reset User workout info done!")

	err = tx.Commit()
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Transaction completed successfully!")

	done <- true
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

	addQuery := "INSERT INTO user_exercises (userid, exerciseid, sets, reps) VALUES (?, ?, ?, ?)"
	_, err = db.Exec(addQuery, uid, ex_id, ex_sets, ex_reps)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Exercise added to workout plan successfully!")

	done <- true
}

func editExerciseSetsWorkoutplan(user_email, ex_name string, new_ex_sets int, done chan<- bool) {
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

	editQuery := "UPDATE user_exercises SET sets = ?  WHERE userid = ? AND exerciseid = ?"
	_, err = db.Exec(editQuery, new_ex_sets, uid, ex_id)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Sets of the workout plan exercise edited successfully!")

	done <- true
}

func editExerciseRepsWorkoutplan(user_email, ex_name string, new_ex_reps int, done chan<- bool) {
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

	editQuery := "UPDATE user_exercises SET reps = ?  WHERE userid = ? AND exerciseid = ?"
	_, err = db.Exec(editQuery, new_ex_reps, uid, ex_id)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Reps of the workout plan exercise edited successfully!")

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

	deleteQuery := "DELETE FROM user_exercises WHERE uid = ? AND ex_id = ?"
	_, err = db.Exec(deleteQuery, uid, ex_id)
	if err != nil {
		done <- false
		return
	}
	fmt.Println("Query run successfully!")

	done <- true
}
