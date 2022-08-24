namespace FinalBattle;

public class EquipGearAction : IAction
{
    private readonly IGear gear;

    public EquipGearAction(IGear gear)
    {
        this.gear = gear;
    }
    public void Run(Battle battle, Character actor)
    {
        Party party = battle.GetPartyFor(actor);

        if(actor.EquippedGear != null)
        {
            Console.WriteLine($"{actor.Name} has unequipped {actor.EquippedGear.Name}");
            party.Gears.Add(actor.EquippedGear);
            actor.EquippedGear = null;
        }

        Console.WriteLine($"{actor.Name} equipped {gear.Name}.");
        actor.EquippedGear = gear;
        party.Gears.Remove(gear);
    }
}