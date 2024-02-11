package types

type Exercise struct {
	Name        string `json:"name"` // Struct tag che serve a dire a go come esportare la variabile in formato json
	Description string `json:"description"`
}

type ExerciseWorkout struct {
	Exercise Exercise `json:"exercise"`
	Muscles  []string `json:"muscles"`
}

type Muscle struct {
	Name string `json:"muscle"`
}

type MuscleExercise struct {
	Exercise string `json:"esercizio"`
	Muscle   string `json:"muscolo"`
}

type User struct {
	Id                 int    `json:"id"`
	Name               string `json:"name"`
	Surname            string `json:"surname"`
	Email              string `json:"email"`
	Age                int    `json:"age"`
	WorkoutName        string `json:"workout_name"`
	WorkoutDescription string `json:"workout_description"`
}

type LoginReq struct {
	Email    string `json:"email"`
	Password string `json:"password"`
}

type SignupReq struct {
	Name     string `json:"name"`
	Surname  string `json:"surname"`
	Email    string `json:"email"`
	Password string `json:"password"`
	Age      int    `json:"age"`
}

type EmailReq struct {
	Email string `json:"email"`
}

type MuscleReq struct {
	Email      string `json:"email"`
	MuscleName string `json:"muscle"`
}

type UserExerciseReq struct {
	Email    string `json:"email"`
	Exercise string `json:"exercise"`
}
