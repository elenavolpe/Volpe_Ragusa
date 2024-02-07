package main

import (
	"database/sql"
	"fmt"
	"log"

	"github.com/go-sql-driver/mysql" // Driver per la gestione del database MySQL (l'underscore indica che il package viene importato ma non utilizzato direttamente nel codice, rimosso perché utilizzo ora variabile del package (mysql) a riga 95)
)

func ConnectDB(username, password, host, port, dbName string) (*sql.DB, error) {
	// Inizializzazione della gestione del database per il driver go-sql-driver/mysql
	dataSourceName := fmt.Sprintf("%s:%s@tcp(%s:%s)/%s", username, password, host, port, dbName)
	db, err := sql.Open("mysql", dataSourceName)
	if err != nil {
		log.Println(err)
		return nil, err
	}

	// Poiché open non ci dice se la connessione con il db è avvenuta effettivamente, testiamo la connessione con il metodo Ping() della struct db:
	err = db.Ping()
	if err != nil {
		log.Println(err)
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
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	deleteQuery = "DELETE FROM users WHERE id = ?"
	_, err = tx.Exec(deleteQuery, uid)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	err = tx.Commit()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}
	fmt.Println("All queries run successfully!")

	done <- true
}

func signup(name, surname, email, password string, age int, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	signupQuery := "INSERT INTO users (name, surname, email, pass, age) VALUES (?, ?, ?, ?, ?)"
	_, err = db.Exec(signupQuery, name, surname, email, password, age)
	if err != nil {
		log.Println(err)
		// Controllo errore "duplicate key" per MySQL (codice errore: 1062)
		if mysqlErr, ok := err.(*mysql.MySQLError); ok && mysqlErr.Number == 1062 { // Se è vero che c'è errore di mysql (ok impostato a true) e l'errore è 1062 (mysqlErr.number == 1062) ritorno al canale che l'email è gia in uso
			usr <- "email già in uso"
			return
		}
		usr <- "failure"
		return
	}
	fmt.Println("Data inserted successfully!")

	usr <- email
}

func login(email, password string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	loginQuery := "SELECT EXISTS(SELECT 1 FROM users WHERE email = ? AND pass = ?)"
	var exists bool
	err = db.QueryRow(loginQuery, email, password).Scan(&exists)
	if err == nil {
		if exists {
			fmt.Println("User found!")
			done <- true
			return
		} else {
			fmt.Println("User not found!")
			done <- false
			return
		}
	} else {
		log.Println(err)
		done <- false
	}
}

func authAdmin(user_email, password string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	loginQuery := "SELECT admin FROM users WHERE email = ? AND pass = ?"
	var isAdmin bool
	err = db.QueryRow(loginQuery, user_email, password).Scan(&isAdmin)
	if err == nil {
		if isAdmin {
			fmt.Println("User is an admin!")
			done <- true
			return
		} else {
			fmt.Println("User is not an admin!")
			done <- false
			return
		}
	} else {
		log.Println(err)
		done <- false
	}
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
		log.Println(err)
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

func modifyPassword(user_email, new_pw string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET pass = ? WHERE email = ?"
	_, err = db.Exec(editQuery, new_pw, user_email)
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}
	fmt.Println("Password updated successfully!")

	usr <- user_email
}

func modifyEmail(old_email, new_email string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET email = ? WHERE email = ?"
	_, err = db.Exec(editQuery, new_email, old_email)
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}
	fmt.Println("Email updated successfully!")

	usr <- new_email
}

func modifyName(user_email, new_name string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET name = ? WHERE email = ?"
	_, err = db.Exec(editQuery, new_name, user_email)
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}
	fmt.Println("Name updated successfully!")

	usr <- user_email
}

func modifySurname(user_email, new_surname string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET surname = ? WHERE email = ?"
	_, err = db.Exec(editQuery, new_surname, user_email)
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}
	fmt.Println("Surname updated successfully!")

	usr <- user_email
}

func modifyAge(user_email, new_age string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET age = ? WHERE email = ?"
	_, err = db.Exec(editQuery, new_age, user_email)
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}
	fmt.Println("Age updated successfully!")

	usr <- user_email
}

func getUserInfo(user_email string, usr chan<- User) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := "SELECT id, name, surname, email, age, workout_name, workout_description FROM users where email = ?"
	var user User
	err = db.QueryRow(getQuery, user_email).Scan(&user.Id, &user.Name, &user.Surname, &user.Email, &user.Age, &user.WorkoutName, &user.WorkoutDescription)
	if err != nil {
		if err == sql.ErrNoRows {
			fmt.Println("No User found!")
		}
		emptyUsr := User{-1, "", "", "", 0, "", ""}
		usr <- emptyUsr
		return
	}
	fmt.Println("User found!")

	usr <- user
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
		log.Println(err)
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
		log.Println(err)
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
		log.Println(err)
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
		log.Println(err)
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
		log.Println(err)
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
		log.Println(err)
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
		log.Println(err)
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
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	deleteQuery = "DELETE FROM muscles WHERE id = ?"
	_, err = tx.Exec(deleteQuery, muscle_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	err = tx.Commit()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}
	fmt.Println("All queries run successfully!")

	done <- true
}

// Funzioni per la gestione dei dati dei muscoli relativamente agli esercizi
func addMuscleExercise(ex_name, muscle_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	ex_id := getExerciseID(ex_name)
	if ex_id == -1 {
		done <- false
		return
	}

	muscle_id := getMuscleID(muscle_name)
	if muscle_id == -1 {
		done <- false
		return
	}

	addQuery := "INSERT INTO exercise_muscles (exerciseid, muscleid) VALUES (?, ?)"
	_, err = db.Exec(addQuery, ex_id, muscle_id)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}
	fmt.Printf("Muscle added successfully!")

	done <- true
}

func deleteMuscleExercise(ex_name, muscle_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	ex_id := getExerciseID(ex_name)
	if ex_id == -1 {
		done <- false
		return
	}

	muscle_id := getMuscleID(muscle_name)
	if muscle_id == -1 {
		done <- false
		return
	}

	deleteQuery := "DELETE FROM exercise_muscles WHERE exerciseid = ? AND muscleid = ?"
	_, err = db.Exec(deleteQuery, ex_id, muscle_id)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}
	fmt.Println("Query run successfully!")

	done <- true
}

// Funzioni per la gestione dei dati dei muscoli preferiti dell'utente
func addPreferredMuscle(user_email, muscle_name string, done chan<- bool) {
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

	muscle_id := getMuscleID(muscle_name)
	if muscle_id == -1 {
		done <- false
		return
	}

	addQuery := "INSERT INTO preferred_muscles (userid, muscleid) VALUES (?, ?)"
	_, err = db.Exec(addQuery, uid, muscle_id)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}
	fmt.Printf("Muscle added successfully!")

	done <- true
}

func deletePreferredMuscle(user_email, muscle_name string, done chan<- bool) {
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

	muscle_id := getMuscleID(muscle_name)
	if muscle_id == -1 {
		done <- false
		return
	}

	deleteQuery := "DELETE FROM preferred_muscles WHERE userid = ? AND muscleid = ?"
	_, err = db.Exec(deleteQuery, uid, muscle_id)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}
	fmt.Println("Query run successfully!")

	done <- true
}

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
		log.Println(err)
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
		log.Println(err)
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
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}
	fmt.Println("All queries run successfully")

	// Reset delle informazioni della scheda dell'utente (nome e descrizione a NULL)
	updateQuery := "UPDATE users SET workout_name = NULL, workout_description = NULL WHERE id = ?"
	_, err = tx.Exec(updateQuery, uid)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}
	fmt.Println("Reset User workout info done!")

	err = tx.Commit()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}
	fmt.Println("Transaction completed successfully!")

	done <- true
}

// Funzioni per la gestione dei dati relativi alle schede di allenamento
func addExerciseWorkoutplan(user_email, ex_name string, done chan<- bool) {
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

	addQuery := "INSERT INTO user_exercises (userid, exerciseid) VALUES (?, ?)"
	_, err = db.Exec(addQuery, uid, ex_id)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}
	fmt.Println("Exercise added to workout plan successfully!")

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
		log.Println(err)
		done <- false
		return
	}
	fmt.Println("Query run successfully!")

	done <- true
}

// DA TESTARE SE FUNZIONA VERAMENTE xD!
// Dovrebbe essere una struttura che verrà poi convertita nel main in un json così modellato:
// [
//     {
//         "exercise": {
//             "Name": "Exercise1",
//             "Description": "Description1"
//         },
//         "muscles": ["Muscle1", "Muscle2"]
//     },
//     {
//         "exercise": {
//             "Name": "Exercise2",
//             "Description": "Description2"
//         },
//         "muscles": ["Muscle3", "Muscle4"]
//     },
//     // ... more ExerciseWorkout entries if any
// ]

func getWorkoutPlan(user_email string, workout_plan chan<- []ExerciseWorkout) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getExerciseQuery := "SELECT E.name, E.description FROM exercises AS E JOIN user_exercises AS UE ON E.id = UE.exerciseid JOIN users AS U ON UE.userid = U.id WHERE U.email = ?"
	rows, err := db.Query(getExerciseQuery, user_email)
	if err != nil {
		log.Println(err)
		return
	}
	defer rows.Close()

	var workout []ExerciseWorkout
	for rows.Next() {
		var ex ExerciseWorkout
		err := rows.Scan(&ex.Exercise.Name, &ex.Exercise.Description)
		if err != nil {
			log.Println(err)
			return
		}
		getMuscleQuery := "SELECT M.name from muscles AS M JOIN exercise_muscles AS EM ON EM.muscleid = M.id JOIN exercises AS E ON E.id = EM.exerciseid WHERE E.name = ?"
		rows1, err := db.Query(getMuscleQuery, ex.Exercise.Name)
		if err != nil {
			log.Println(err)
			return
		}
		defer rows1.Close()

		for rows.Next() {
			var muscle_name string
			err := rows.Scan(&muscle_name)
			if err != nil {
				log.Println(err)
				return
			}

			ex.Muscles = append(ex.Muscles, muscle_name)
		}

		err = rows1.Err()
		if err != nil {
			log.Println(err)
			return
		}

		workout = append(workout, ex)
	}
	err = rows.Err()
	if err != nil {
		log.Println(err)
		return
	}
	fmt.Println("Queries run successfully!")

	workout_plan <- workout
}

func getPreferredMuscles(user_email string, muscles chan<- []string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		log.Println("User not found!")
		return
	}

	getQuery := "SELECT name FROM muscles AS M JOIN preferred_muscles AS PM ON M.id = PM.muscleid JOIN users AS U ON U.id = PM.userid  WHERE PM.userid = ?"
	rows, err := db.Query(getQuery, uid)
	if err != nil {
		log.Println(err)
		return
	}
	defer rows.Close()

	var muscle_list []string
	for rows.Next() {
		var muscle string
		err := rows.Scan()
		if err != nil {
			log.Println(err)
			return
		}

		muscle_list = append(muscle_list, muscle)
	}
	err = rows.Err()
	if err != nil {
		log.Println(err)
		return
	}
	fmt.Println("Query run successfully!")

	muscles <- muscle_list
}

func modifyPreferredMuscles(user_email string, old_preferred_muscles, new_preferred_muscles []string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "localhost", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		log.Println("User not found!")
		return
	}

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	removedMuscles, addedMuscles := findDifferentStrings(old_preferred_muscles, new_preferred_muscles)

	if len(removedMuscles) > 0 {
		for _, muscle := range removedMuscles {
			mid := getMuscleID(muscle)
			if mid == -1 {
				log.Println("Muscle not found!")
				continue
			}
			deleteQuery := "DELETE FROM preferred_muscles WHERE userid = ? AND muscleid = ?"
			_, err = tx.Exec(deleteQuery, uid, mid)
			if err != nil {
				log.Println(err)
				usr <- "failure"
				tx.Rollback()
				return
			}
		}
	}

	if len(addedMuscles) > 0 {
		for _, muscle := range addedMuscles {
			mid := getMuscleID(muscle)
			if mid == -1 {
				log.Println("Muscle not found!")
				continue
			}
			deleteQuery := "INSERT INTO preferred_muscles (userid, muscleid) VALUES (?, ?)"
			_, err = tx.Exec(deleteQuery, uid, mid)
			if err != nil {
				log.Println(err)
				usr <- "failure"
				tx.Rollback()
				return
			}
		}
	}

	err = tx.Commit()
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}
	fmt.Println("Transaction completed successfully!")

	usr <- user_email
}
