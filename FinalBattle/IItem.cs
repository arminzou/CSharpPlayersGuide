using FinalBattle;

public interface IItem
{
    string Name { get; }
    void Use(Battle battle, Character character);
}

public class HealthPotion : IItem
{
    public string Name => "HEALTH POTION";

    public void Use(Battle battle, Character character)
    {
        int hpHealed;
        if (character.MaxHP - character.HP <= 10)
        {
            hpHealed = character.MaxHP - character.HP;
            character.HP += hpHealed;
        }
        else
        {
            hpHealed = 10;
            character.HP += 10;
        }
        Console.WriteLine($"{character.Name}'s HP was increased by {hpHealed}.");
    }
}

