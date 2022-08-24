namespace FinalBattle;

public class UseItemAction : IAction
{
    private readonly IItem item;

    public UseItemAction(IItem item)
    {
        this.item = item;
    }

    public void Run(Battle battle, Character actor)
    {
        Console.WriteLine($"{actor.Name} used {item.Name}");
        item.Use(battle, actor);
        battle.GetPartyFor(actor).Items.Remove(item);
    }
}
