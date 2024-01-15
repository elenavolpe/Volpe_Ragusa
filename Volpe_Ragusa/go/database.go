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
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	deleteUserQuery := "DELETE FROM users WHERE email = ?"
	_, err = db.Exec(deleteUserQuery, email)
	if err != nil {
		done <- false
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
	}
	fmt.Println("User found!")

	usr <- email
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
	}
	fmt.Println("Exercise deleted successfully!")

	done <- true
}

func editExercise(oldName, newName, newDescription string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	if newDescription != "" {
		editExerciseQuery := "UPDATE exercises SET description = ? WHERE name = ?"
		_, err = db.Exec(editExerciseQuery, newDescription, oldName)
		if err != nil {
			done <- false
		}
		fmt.Println("Exercise description edited successfully!")
	}
	if newName != "" {
		editExerciseQuery := "UPDATE exercises SET name = ? WHERE name = ?"
		_, err = db.Exec(editExerciseQuery, newName, oldName)
		if err != nil {
			done <- false
		}
		fmt.Println("Exercise name edited successfully!")
	}

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

func getMostPopularExercises(limitValue int, exercises chan<- []Exercise) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getExercisesQuery := fmt.Sprintf("SELECT name, description FROM exercises ORDER BY popularity_score DESC LIMIT %d", limitValue)
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

func getMostRecentExercises(limitValue int, exercises chan<- []Exercise) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getExercisesQuery := fmt.Sprintf("SELECT name, description FROM exercises ORDER BY created_at DESC LIMIT %d", limitValue)
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

// Funzioni per la gestione dei dati relativi alle schede di allenamento già esistenti
// func addExerciseWorkoutPlan(workoutPlanName, exerciseName, sets, reps string, done chan<- bool) {
// 	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
// 	if err != nil {
// 		done <- false
// 	}
// 	defer db.Close()

// 	addExerciseWorkoutPlanQuery := "INSERT INTO workoutplan_exercise (workout_plan_name, exercise_name, sets, reps) VALUES (?, ?, ?, ?)"
// 	_, err = db.Exec(addExerciseWorkoutPlanQuery, workoutPlanName, exerciseName, sets, reps)
// 	if err != nil {
// 		done <- false
// 	}
// 	fmt.Println("Exercise added successfully!")

// 	done <- true
// }

// func deleteExerciseWorkoutPlan(workoutPlanName, exerciseName string, done chan<- bool) {
// 	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
// 	if err != nil {
// 		done <- false
// 	}
// 	defer db.Close()

// 	deleteExerciseWorkoutPlanQuery := "DELETE FROM workout_plan_exercise WHERE workout_plan_name = ? AND exercise_name = ?"
// 	_, err = db.Exec(deleteExerciseWorkoutPlanQuery, workoutPlanName, exerciseName)
// 	if err != nil {
// 		done <- false
// 	}
// 	fmt.Println("Exercise deleted successfully!")

// 	done <- true
// }
