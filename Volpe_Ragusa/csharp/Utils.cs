
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

public class InfoUtente
{
    public int id { get; set; }
    public string name { get; set; }
    public string surname { get; set; }
    public string email { get; set; }
    public int age { get; set; }
    public string workout_name { get; set; }
    public string workout_description { get; set; }
}