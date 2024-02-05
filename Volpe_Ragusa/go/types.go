package main

type Exercise struct {
	Name        string
	Description string
}

type ExerciseWorkout struct {
	exercise Exercise
	muscles  []string
}
