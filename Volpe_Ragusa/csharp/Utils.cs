//TO_DO sistemarli esattamente con i label del json
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
    public string nome { get; set; }
    public string cognome { get; set; }
    public string email { get; set; }
    public int età { get; set; }
    public string workoutname { get; set; }
    public string workoutdescription { get; set; }
}