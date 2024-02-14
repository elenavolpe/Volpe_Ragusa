package database

import (
	"database/sql"
	"fmt"
	"go_app/types"
	"go_app/utility"
	"log"
	"time"

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

// Quando elimini un utente -> eliminare prima user_exercises e preferred_muscles
func DeleteAccount(email string, done chan<- bool) {
	//admin per collegarsi a mysql
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(email)
	if uid == -1 {
		log.Println("User not found!")
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

	deleteQuery = "DELETE FROM preferred_muscles WHERE userid = ?"
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

func Signup(name, surname, email, password string, muscles []string, age int, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	signupQuery := "INSERT INTO users (name, surname, email, pass, age) VALUES (?, ?, ?, ?, ?)"
	result, err := tx.Exec(signupQuery, name, surname, email, password, age)
	if err != nil {
		log.Println(err)
		// Controllo errore "duplicate key" per MySQL (codice errore: 1062)
		if mysqlErr, ok := err.(*mysql.MySQLError); ok && mysqlErr.Number == 1062 { // Se è vero che c'è errore di mysql (ok impostato a true) e l'errore è 1062 (mysqlErr.number == 1062) ritorno al canale che l'email è gia in uso
			usr <- "email già in uso"
			tx.Rollback()
			return
		}
		usr <- "failure"
		tx.Rollback()
		return
	}

	if len(muscles) > 0 {
		uid, err := result.LastInsertId()
		if err != nil {
			log.Println(err)
			usr <- "failure"
			tx.Rollback()
			return
		}
		for _, muscle := range muscles {
			muscle_id := getMuscleID(muscle)
			if muscle_id == -1 {
				usr <- "failure"
				tx.Rollback()
				return
			}
			addQuery := "INSERT INTO preferred_muscles (userid, muscleid) VALUES (?, ?)"
			_, err = tx.Exec(addQuery, uid, muscle_id)
			if err != nil {
				log.Println(err)
				usr <- "failure"
				tx.Rollback()
				return
			}
		}
		if err != nil {
			log.Println(err)
			usr <- "failure"
			tx.Rollback()
			return
		}
	}

	err = tx.Commit()
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}
	fmt.Println("Signup run successfully!")

	usr <- email
}

func Login(email, password string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	loginQuery := "SELECT EXISTS(SELECT 1 FROM users WHERE email = ? AND BINARY pass = ?)"
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

func AuthAdmin(user_email string, adm chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	loginQuery := "SELECT admin FROM users WHERE email = ?"
	var isAdmin bool
	err = db.QueryRow(loginQuery, user_email).Scan(&isAdmin)
	if err == nil {
		if isAdmin {
			fmt.Println("User is an admin!")
			adm <- true
			return
		} else {
			fmt.Println("User is not an admin!")
			adm <- false
			return
		}
	} else {
		log.Println(err)
		adm <- false
	}
}

func getUID(email string) (uid int) { // Funzione per ottenere l'User ID dell'account loggato richiedente
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
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

func GetUserName(email string, name chan<- string) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
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

func ModifyPassword(user_email, new_pw string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET pass = ? WHERE email = ?"
	result, err := db.Exec(editQuery, new_pw, user_email)
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		usr <- "failure"
		return
	}
	fmt.Println("Password updated successfully!")

	usr <- user_email
}

func ModifyEmail(old_email, new_email string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET email = ? WHERE email = ?"
	result, err := db.Exec(editQuery, new_email, old_email)
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		usr <- "failure"
		return
	}
	fmt.Println("Email updated successfully!")

	usr <- new_email
}

func ModifyName(user_email, new_name string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET name = ? WHERE email = ?"
	result, err := db.Exec(editQuery, new_name, user_email)
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		usr <- "failure"
		return
	}
	fmt.Println("Name updated successfully!")

	usr <- user_email
}

func ModifySurname(user_email, new_surname string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET surname = ? WHERE email = ?"
	result, err := db.Exec(editQuery, new_surname, user_email)
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		usr <- "failure"
		return
	}
	fmt.Println("Surname updated successfully!")

	usr <- user_email
}

func ModifyAge(user_email string, new_age int, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET age = ? WHERE email = ?"
	result, err := db.Exec(editQuery, new_age, user_email)
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		usr <- "failure"
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		usr <- "failure"
		return
	}
	fmt.Println("Age updated successfully!")

	usr <- user_email
}

func GetUserInfo(user_email string, usr chan<- types.User) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := "SELECT id, name, surname, email, age, workout_name, workout_description FROM users where email = ?"
	var user types.User
	var w_name, w_desc sql.NullString
	err = db.QueryRow(getQuery, user_email).Scan(&user.Id, &user.Name, &user.Surname, &user.Email, &user.Age, &w_name, &w_desc)
	if err != nil {
		if err == sql.ErrNoRows {
			fmt.Println("No User found!")
		}
		log.Println(err)
		failure := types.User{Id: -1, Name: "failure", Surname: "failure", Email: "failure", Age: 0, WorkoutName: "failure", WorkoutDescription: "failure"}
		usr <- failure
		return
	}
	fmt.Println("User found!")

	if w_name.Valid {
		user.WorkoutName = w_name.String
	} else {
		user.WorkoutName = "NULL"
	}
	if w_desc.Valid {
		user.WorkoutDescription = w_desc.String
	} else {
		user.WorkoutDescription = "NULL"
	}

	usr <- user
}

// Funzioni per la gestione dei dati relativi agli esercizi

func AddExercise(name, description string, muscles []string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	addQuery := "INSERT INTO exercises (name, description) VALUES (?, ?)"
	result, err := tx.Exec(addQuery, name, description)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	ex_id, err := result.LastInsertId()
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	for _, muscle := range muscles {
		muscle_id := getMuscleID(muscle)
		if muscle_id == -1 {
			done <- false
			tx.Rollback()
			return
		}
		addQuery = "INSERT INTO exercise_muscles (exerciseid, muscleid) VALUES (?, ?)"
		_, err = tx.Exec(addQuery, ex_id, muscle_id)
		if err != nil {
			log.Println(err)
			done <- false
			tx.Rollback()
			return
		}
	}

	err = tx.Commit()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}
	fmt.Println("Queries run successfully!")

	done <- true
}

// Quando elimini un esercizio -> eliminare prima user_exercises e exercise_muscles
func DeleteExercise(name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	ex_id := getExerciseID(name)
	if ex_id == -1 {
		log.Println("Exercise not found!")
		done <- false
		return
	}

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	deleteQuery := "DELETE FROM user_exercises WHERE exerciseid = ?"
	_, err = tx.Exec(deleteQuery, ex_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	deleteQuery = "DELETE FROM exercise_muscles WHERE exerciseid = ?"
	result, err := tx.Exec(deleteQuery, ex_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + deleteQuery)
		done <- false
		tx.Rollback()
		return
	}

	deleteQuery = "DELETE FROM exercises WHERE id = ?"
	result, err = tx.Exec(deleteQuery, ex_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	rowsAffected, err = result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + deleteQuery)
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
	fmt.Println("Queries run successfully!")

	done <- true
}

func EditExerciseName(old_name, new_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE exercises SET name = ? WHERE BINARY name = ?"
	result, err := db.Exec(editQuery, new_name, old_name)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		done <- false
		return
	}
	fmt.Println("Exercise name edited successfully!")

	done <- true
}

func EditExerciseDescription(name, new_description string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE exercises SET description = ? WHERE BINARY name = ?"
	result, err := db.Exec(editQuery, new_description, name)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		done <- false
		return
	}
	fmt.Println("Exercise description edited successfully!")

	done <- true
}

func GetExercises(exercises chan<- []types.ExerciseWorkout) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := "SELECT name, description FROM exercises"
	rows, err := db.Query(getQuery)
	if err != nil {
		log.Println(err)
		errSlice := make([]types.ExerciseWorkout, 1)
		errSlice[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}, Muscles: []string{"failure"}}
		exercises <- errSlice
		return
	}
	defer rows.Close()

	var exs []types.ExerciseWorkout
	for rows.Next() {
		var ex types.ExerciseWorkout
		err := rows.Scan(&ex.Exercise.Name, &ex.Exercise.Description)
		if err != nil {
			log.Println(err)
			errSlice := make([]types.ExerciseWorkout, 1)
			errSlice[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}, Muscles: []string{"failure"}}
			exercises <- errSlice
			return
		}
		getMuscleQuery := "SELECT M.name from muscles AS M JOIN exercise_muscles AS EM ON EM.muscleid = M.id JOIN exercises AS E ON E.id = EM.exerciseid WHERE E.name = ?"
		rows1, err := db.Query(getMuscleQuery, ex.Exercise.Name)
		if err != nil {
			log.Println(err)
			errSlice := make([]types.ExerciseWorkout, 1)
			errSlice[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}, Muscles: []string{"failure"}}
			exercises <- errSlice
			return
		}
		defer rows1.Close()

		for rows1.Next() {
			var muscle_name string
			err := rows1.Scan(&muscle_name)
			if err != nil {
				log.Println(err)
				errSlice := make([]types.ExerciseWorkout, 1)
				errSlice[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}}
				exercises <- errSlice
				return
			}

			ex.Muscles = append(ex.Muscles, muscle_name)
		}

		err = rows1.Err()
		if err != nil {
			log.Println(err)
			errSlice := make([]types.ExerciseWorkout, 1)
			errSlice[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}}
			exercises <- errSlice
			return
		}

		exs = append(exs, ex)
	}
	err = rows.Err()
	if err != nil {
		log.Println(err)
		errSlice := make([]types.ExerciseWorkout, 1)
		errSlice[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}}
		exercises <- errSlice
		return
	}
	fmt.Println("Queries run successfully!")

	exercises <- exs
}

func getExerciseID(name string) (ex_id int) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
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

func GetMostPopularExercises(limit_value int, exercises chan<- []types.Exercise) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := fmt.Sprintf("SELECT name, description FROM exercises ORDER BY popularity_score DESC LIMIT %d", limit_value)
	rows, err := db.Query(getQuery)
	if err != nil {
		log.Println(err)
		errSlice := make([]types.Exercise, 1)
		errSlice[0] = types.Exercise{Name: "failure", Description: "failure"}
		exercises <- errSlice
		return
	}
	defer rows.Close()

	var exs []types.Exercise
	for rows.Next() {
		var ex types.Exercise
		err := rows.Scan(&ex.Name, &ex.Description)
		if err != nil {
			log.Println(err)
			errSlice := make([]types.Exercise, 1)
			errSlice[0] = types.Exercise{Name: "failure", Description: "failure"}
			exercises <- errSlice
			return
		}
		exs = append(exs, ex)
	}
	err = rows.Err()
	if err != nil {
		log.Println(err)
		errSlice := make([]types.Exercise, 1)
		errSlice[0] = types.Exercise{Name: "failure", Description: "failure"}
		exercises <- errSlice
		return
	}

	exercises <- exs
}

func GetMostRecentExercises(exercises chan<- []types.Exercise) { // Restituisce gli esercizi aggiunti (E NON AGGIORANTI!) negli ultimi due giorni
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	twoDaysAgo := time.Now().Add(-48 * time.Hour).Format("2006-01-02 15:04:05") // Il campo Format ha una data d'esempio solo per stabilire il formato della stringa da ritornare

	getQuery := "SELECT name, description FROM exercises WHERE created_at >= ? ORDER BY created_at DESC"
	rows, err := db.Query(getQuery, twoDaysAgo)
	if err != nil {
		log.Println(err)
		errSlice := make([]types.Exercise, 1)
		errSlice[0] = types.Exercise{Name: "failure", Description: "failure"}
		exercises <- errSlice
		return
	}
	defer rows.Close()

	var exs []types.Exercise
	for rows.Next() {
		var ex types.Exercise
		err := rows.Scan(&ex.Name, &ex.Description)
		if err != nil {
			log.Println(err)
			errSlice := make([]types.Exercise, 1)
			errSlice[0] = types.Exercise{Name: "failure", Description: "failure"}
			exercises <- errSlice
			return
		}
		exs = append(exs, ex)
	}

	// Controllo errori verificatisi durante l'iterazione sulle righe
	err = rows.Err()
	if err != nil {
		log.Println(err)
		errSlice := make([]types.Exercise, 1)
		errSlice[0] = types.Exercise{Name: "failure", Description: "failure"}
		exercises <- errSlice
		return
	}

	exercises <- exs
}

// Funzioni per la gestione dei dati relativi ai muscoli

func AddMuscle(name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
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

func EditMuscleName(old_name, new_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE muscles SET name = ? WHERE BINARY name = ?"
	result, err := db.Exec(editQuery, new_name, old_name)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		done <- false
		return
	}
	fmt.Printf("Muscle added successfully!")

	done <- true
}

func getMuscleID(muscle string) (muscle_id int) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
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

// Quando elimini un muscolo -> eliminare prima exercise_muscles e preferred_muscles
func DeleteMuscle(name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	muscle_id := getMuscleID(name)
	if muscle_id == -1 {
		log.Println("Muscle not found!")
		done <- false
		return
	}

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	// Prima di cancellare il muscolo scelto, bisogna cancellare gli accompiamenti con i vari esercizi in cui è presente
	deleteQuery := "DELETE FROM exercise_muscles WHERE muscleid = ?"
	_, err = tx.Exec(deleteQuery, muscle_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	deleteQuery = "DELETE FROM preferred_muscles WHERE muscleid = ?"
	_, err = tx.Exec(deleteQuery, muscle_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	deleteQuery = "DELETE FROM muscles WHERE id = ?"
	result, err := tx.Exec(deleteQuery, muscle_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + deleteQuery)
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

func GetMuscles(muscles chan<- []string) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getQuery := "SELECT name FROM muscles"
	rows, err := db.Query(getQuery)
	if err != nil {
		log.Println(err)
		errSlice := make([]string, 1)
		errSlice[0] = "failure"
		muscles <- errSlice
		return
	}
	defer rows.Close()

	var mscls []string
	for rows.Next() {
		var m string
		err := rows.Scan(&m)
		if err != nil {
			log.Println(err)
			errSlice := make([]string, 1)
			errSlice[0] = "failure"
			muscles <- errSlice
			return
		}
		mscls = append(mscls, m)
	}

	err = rows.Err()
	if err != nil {
		log.Println(err)
		errSlice := make([]string, 1)
		errSlice[0] = "failure"
		muscles <- errSlice
		return
	}

	muscles <- mscls
}

// Funzioni per la gestione dei dati dei muscoli relativamente agli esercizi
func AddMuscleExercise(ex_name, muscle_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	ex_id := getExerciseID(ex_name)
	if ex_id == -1 {
		log.Println("Exercise not found!")
		done <- false
		return
	}

	muscle_id := getMuscleID(muscle_name)
	if muscle_id == -1 {
		log.Println("Muscle not found!")
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

func DeleteMuscleExercise(ex_name, muscle_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	ex_id := getExerciseID(ex_name)
	if ex_id == -1 {
		log.Println("Exercise not found!")
		done <- false
		return
	}

	muscle_id := getMuscleID(muscle_name)
	if muscle_id == -1 {
		log.Println("Muscle not found!")
		done <- false
		return
	}

	deleteQuery := "DELETE FROM exercise_muscles WHERE exerciseid = ? AND muscleid = ?"
	result, err := db.Exec(deleteQuery, ex_id, muscle_id)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + deleteQuery)
		done <- false
		return
	}
	fmt.Println("Query run successfully!")

	done <- true
}

// Funzioni per la gestione dei dati dei muscoli preferiti dell'utente
func AddPreferredMuscle(user_email, muscle_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		log.Println("User not found!")
		done <- false
		return
	}

	muscle_id := getMuscleID(muscle_name)
	if muscle_id == -1 {
		log.Println("Muscle not found!")
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

func DeletePreferredMuscle(user_email, muscle_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		log.Println("User not found!")
		done <- false
		return
	}

	muscle_id := getMuscleID(muscle_name)
	if muscle_id == -1 {
		log.Println("Muscle not found!")
		done <- false
		return
	}

	deleteQuery := "DELETE FROM preferred_muscles WHERE userid = ? AND muscleid = ?"
	result, err := db.Exec(deleteQuery, uid, muscle_id)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + deleteQuery)
		done <- false
		return
	}
	fmt.Println("Query run successfully!")

	done <- true
}

// Funzioni per la gestione delle schede di allenamento (tabella users - campi workout_name e workout_description)
func UpdateUserWorkoutName(user_email, wp_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET workout_name = ? WHERE email = ?"
	result, err := db.Exec(editQuery, wp_name, user_email)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		done <- false
		return
	}
	fmt.Println("Workout plan for user added successfully!")

	done <- true
}

func UpdateUserWorkoutDescription(user_email, wp_desc string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	editQuery := "UPDATE users SET workout_description = ? WHERE email = ?"
	result, err := db.Exec(editQuery, wp_desc, user_email)
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		done <- false
		return
	}
	fmt.Println("Workout plan for user added successfully!")

	done <- true
}

func DeleteUserWorkout(user_email string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		log.Println("User not found!")
		done <- false
		return
	}

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	deleteQuery := "DELETE FROM user_exercises WHERE userid = ?"
	result, err := tx.Exec(deleteQuery, uid)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + deleteQuery)
		done <- false
		tx.Rollback()
		return
	}

	// Reset delle informazioni della scheda dell'utente (nome e descrizione a NULL)
	//TO_DO: se lascio questa funzione (penso di no), dovrei diminuire il popularity_score degli esercizi presenti nella scheda dell'utente
	editQuery := "UPDATE users SET workout_name = NULL, workout_description = NULL WHERE id = ?"
	result, err = tx.Exec(editQuery, uid)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	rowsAffected, err = result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
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
	fmt.Println("User workout deleted successfully")
	fmt.Println("Reset User workout info done!")
	fmt.Println("Transaction completed successfully!")

	done <- true
}

// Funzioni per la gestione dei dati relativi alle schede di allenamento
func AddExerciseWorkoutplan(user_email, ex_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		log.Println("User not found!")
		done <- false
		return
	}

	ex_id := getExerciseID(ex_name)
	if ex_id == -1 {
		log.Println("Exercise not found!")
		done <- false
		return
	}

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	addQuery := "INSERT INTO user_exercises (userid, exerciseid) VALUES (?, ?)"
	_, err = tx.Exec(addQuery, uid, ex_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	editQuery := "UPDATE exercises SET popularity_score = popularity_score+1 WHERE id = ?"
	result, err := tx.Exec(editQuery, ex_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
		done <- false
		return
	}

	err = tx.Commit()
	if err != nil {
		log.Println(err)
		done <- false
		return
	}
	fmt.Println("Transaction completed successfully!")

	done <- true
}

func DeleteExerciseWorkoutplan(user_email, ex_name string, done chan<- bool) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		log.Println("User not found!")
		done <- false
		return
	}

	ex_id := getExerciseID(ex_name)
	if ex_id == -1 {
		log.Println("Exercise not found!")
		done <- false
		return
	}

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	deleteQuery := "DELETE FROM user_exercises WHERE userid = ? AND exerciseid = ?"
	result, err := tx.Exec(deleteQuery, uid, ex_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	rowsAffected, err := result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + deleteQuery)
		done <- false
		tx.Rollback()
		return
	}

	editQuery := "UPDATE exercises SET popularity_score = popularity_score-1 WHERE id = ?"
	result, err = tx.Exec(editQuery, ex_id)
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	rowsAffected, err = result.RowsAffected()
	if err != nil {
		log.Println(err)
		done <- false
		tx.Rollback()
		return
	}

	if rowsAffected == 0 {
		log.Println("Query failed: " + editQuery)
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
	fmt.Println("Transaction completed successfully!")

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

func GetWorkoutPlan(user_email string, workout_plan chan<- []types.ExerciseWorkout) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	getExerciseQuery := "SELECT E.name, E.description FROM exercises AS E JOIN user_exercises AS UE ON E.id = UE.exerciseid JOIN users AS U ON UE.userid = U.id WHERE U.email = ?"
	rows, err := db.Query(getExerciseQuery, user_email)
	if err != nil {
		log.Println(err)
		exs := make([]types.ExerciseWorkout, 1)
		exs[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}, Muscles: []string{"failure"}}
		workout_plan <- exs
		return
	}
	defer rows.Close()

	var workout []types.ExerciseWorkout
	for rows.Next() {
		var ex types.ExerciseWorkout
		err := rows.Scan(&ex.Exercise.Name, &ex.Exercise.Description)
		if err != nil {
			log.Println(err)
			exs := make([]types.ExerciseWorkout, 1)
			exs[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}, Muscles: []string{"failure"}}
			workout_plan <- exs
			return
		}
		getMuscleQuery := "SELECT M.name from muscles AS M JOIN exercise_muscles AS EM ON EM.muscleid = M.id JOIN exercises AS E ON E.id = EM.exerciseid WHERE E.name = ?"
		rows1, err := db.Query(getMuscleQuery, ex.Exercise.Name)
		if err != nil {
			log.Println(err)
			exs := make([]types.ExerciseWorkout, 1)
			exs[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}, Muscles: []string{"failure"}}
			workout_plan <- exs
			return
		}
		defer rows1.Close()

		for rows1.Next() {
			var muscle_name string
			err := rows1.Scan(&muscle_name)
			if err != nil {
				log.Println(err)
				exs := make([]types.ExerciseWorkout, 1)
				exs[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}, Muscles: []string{"failure"}}
				workout_plan <- exs
				return
			}

			ex.Muscles = append(ex.Muscles, muscle_name)
		}

		err = rows1.Err()
		if err != nil {
			log.Println(err)
			exs := make([]types.ExerciseWorkout, 1)
			exs[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}, Muscles: []string{"failure"}}
			workout_plan <- exs
			return
		}

		workout = append(workout, ex)
	}
	err = rows.Err()
	if err != nil {
		log.Println(err)
		exs := make([]types.ExerciseWorkout, 1)
		exs[0] = types.ExerciseWorkout{Exercise: types.Exercise{Name: "failure", Description: "failure"}, Muscles: []string{"failure"}}
		workout_plan <- exs
		return
	}
	fmt.Println("Queries run successfully!")

	workout_plan <- workout
}

func GetPreferredMuscles(user_email string, muscles chan<- []string) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		log.Println("User not found!")
		muscles <- []string{"failure"}
		return
	}

	getQuery := "SELECT M.name FROM muscles AS M JOIN preferred_muscles AS PM ON M.id = PM.muscleid JOIN users AS U ON U.id = PM.userid WHERE PM.userid = ?"
	rows, err := db.Query(getQuery, uid)
	if err != nil {
		log.Println(err)
		muscles <- []string{"failure"}
		return
	}
	defer rows.Close()

	var muscle_list []string
	for rows.Next() {
		var muscle string
		err := rows.Scan(&muscle)
		if err != nil {
			log.Println(err)
			muscles <- []string{"failure"}
			return
		}

		muscle_list = append(muscle_list, muscle)
	}
	err = rows.Err()
	if err != nil {
		log.Println(err)
		muscles <- []string{"failure"}
		return
	}
	fmt.Println("Query run successfully!")

	muscles <- muscle_list
}

func ModifyPreferredMuscles(user_email string, new_preferred_muscles []string, usr chan<- string) {
	db, err := ConnectDB("admin", "admin", "mysql", "3306", "workoutnow")
	if err != nil {
		log.Fatal(err)
	}
	defer db.Close()

	uid := getUID(user_email)
	if uid == -1 {
		log.Println("User not found!")
		usr <- "failure"
		return
	}

	tx, err := db.Begin()
	if err != nil {
		log.Fatal(err)
	}

	old_preferred_muscles := make(chan []string)
	go GetPreferredMuscles(user_email, old_preferred_muscles)

	removedMuscles, addedMuscles := utility.FindDifferentStrings(<-old_preferred_muscles, new_preferred_muscles)

	if len(removedMuscles) > 0 {
		for _, muscle := range removedMuscles {
			mid := getMuscleID(muscle)
			if mid == -1 {
				log.Println("Muscle not found!")
				usr <- "failure"
				return
			}
			deleteQuery := "DELETE FROM preferred_muscles WHERE userid = ? AND muscleid = ?"
			result, err := tx.Exec(deleteQuery, uid, mid)
			if err != nil {
				log.Println(err)
				usr <- "failure"
				tx.Rollback()
				return
			}

			rowsAffected, err := result.RowsAffected()
			if err != nil {
				log.Println(err)
				usr <- "failure"
				tx.Rollback()
				return
			}

			if rowsAffected == 0 {
				log.Println("Query failed: " + deleteQuery)
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
				usr <- "failure"
				return
			}
			addQuery := "INSERT INTO preferred_muscles (userid, muscleid) VALUES (?, ?)"
			_, err = tx.Exec(addQuery, uid, mid)
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
