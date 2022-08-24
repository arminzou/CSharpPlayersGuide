namespace FinalBattle;

public class AttackAction : IAction
{
    private readonly IAttack _attack;
    private readonly Character _target;
    public AttackAction(IAttack action, Character target)
    {
        _attack = action;
        _target = target;
    }

    public void Run(Battle battle, Character actor)
    {
        var random = new Random();
        Console.WriteLine($"{actor.Name} uses {_attack.Name} on {_target.Name}");
        AttackData data = _attack.Create();

        if (random.NextDouble() > data.ProbabilityOfHitting)
        {
            ColoredConsole.WriteLine($"{actor.Name} MISSED!", ConsoleColor.DarkRed);
            return;
        }

        if(_target.DefensiveModifier != null)
        {
            data = _target.DefensiveModifier.Modify(data);
        }

        _target.HP -= data.Damage;

        Console.WriteLine($"{_attack.Name} dealt {data.Damage} damage to {_target.Name}");
        Console.WriteLine($"{_target.Name} is now at {_target.HP}/{_target.MaxHP}");

        if (!_target.IsAlive)
        {
            battle.GetPartyFor(_target).Characters.Remove(_target);
            Console.WriteLine($"{_target.Name} has been defeated!");

            if (_target.EquippedGear != null)
            {
                IGear acquiredGear = _target.EquippedGear;
                battle.GetPartyFor(actor).Gears.Add(acquiredGear);
                ColoredConsole.WriteLine($"{actor.Name}'s party has recovered {_target.Name}'s {acquiredGear.Name}.", ConsoleColor.Magenta);
            }
        }
    }
}
