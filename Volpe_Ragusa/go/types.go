package main

type Exercise struct {
	Name        string `json:"name"` // Struct tag che serve a dire a go come esportare la variabile in formato json
	Description string `json:"description"`
}

type ExerciseWorkout struct {
	Exercise Exercise `json:"exercise"`
	Muscles  []string `json:"muscles"`
}

type User struct {
	Id                 int    `json:"id"`
	Name               string `json:"name"`
	Surname            string `json:"surname"`
	Email              string `json:"email"`
	Age                int    `json:age`
	WorkoutName        string `json:"workout_name"`
	WorkoutDescription string `json:"workout_description"`
}
