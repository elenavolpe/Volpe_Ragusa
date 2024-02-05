public class ExerciseData
{
    public Exercise Exercise { get; set; }
    public List<string> Muscles { get; set; }
}

public class Exercise
{
    public string Name { get; set; }
    public string Description { get; set; }
}