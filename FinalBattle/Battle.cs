namespace FinalBattle;

public class Battle
{
    public Party Heroes;
    public Party Monsters;

    public Battle(Party Heroes, Party monsters)
    {
        this.Heroes = Heroes;
        this.Monsters = monsters;
    }

    public void Run() 
    {
        while (!IsOver)
        {
            foreach (var party in new[] { Heroes, Monsters })
            {
                foreach (var character in party.Characters)
                {
                    GameStatus.Show(this, character);
                    Console.WriteLine($"{character.Name} is taking turn...");
                    party.Player.ChooseAction(this, character).Run(this, character);

                    if (IsOver) break;
                }
                if (IsOver) break;
            }
        }

        if (Heroes.Characters.Count > 0)
        {
            ColoredConsole.WriteLine("The HEROES have defeated the MONSTERS and looted their inventory.", ConsoleColor.Magenta);
            TransferInventory();
        }
    }

    private void TransferInventory()
    {
        foreach (IGear gear in Monsters.Gears)
        {
            ColoredConsole.WriteLine($"The HEROES have acquired {gear.Name}.", ConsoleColor.DarkMagenta);
            Heroes.Gears.Add(gear);
        }

        foreach (IItem item in Monsters.Items)
        {
            ColoredConsole.WriteLine($"The HEROES have acquired {item.Name}.", ConsoleColor.DarkMagenta);
            Heroes.Items.Add(item);
        }
    }

    public bool IsOver => Heroes.Characters.Count == 0 || Monsters.Characters.Count == 0;
    public Party GetEnemyPartyFor(Character character) => Heroes.Characters.Contains(character) ? Monsters : Heroes;
    public Party GetPartyFor(Character character) => Heroes.Characters.Contains(character) ? Heroes : Monsters;
}
