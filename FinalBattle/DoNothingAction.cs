namespace FinalBattle;

public class DoNothingAction : IAction
{
    public void Run(Battle battle, Character actor)
    {
        Console.WriteLine($"{actor.Name} did Nothing");
    }
}
