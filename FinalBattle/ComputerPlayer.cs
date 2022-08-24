namespace FinalBattle;

public class ComputerPlayer : IPlayer
{
    private static Random _random = new Random();
    public IAction ChooseAction(Battle battle, Character actor)
    {
        Thread.Sleep(500);

        bool hasPotion = battle.GetPartyFor(actor).Items.Count > 0;
        bool isHPUnderThreshold = actor.HP / (float)actor.MaxHP < 0.5;
        if (hasPotion && isHPUnderThreshold && _random.NextDouble() < 0.25)
            return new UseItemAction(battle.GetPartyFor(actor).Items[0]);

        if (actor.EquippedGear == null && battle.GetPartyFor(actor).Gears.Count > 0 && _random.NextDouble() < 0.5)
            return new EquipGearAction(battle.GetPartyFor(actor).Gears[0]);

        List<Character> potentialTargets = battle.GetEnemyPartyFor(actor).Characters;
        if (potentialTargets.Count > 0)
        {
            if (actor.EquippedGear != null)
                return new AttackAction(actor.EquippedGear.Attack, battle.GetEnemyPartyFor(actor).Characters[0]);
            else
                return new AttackAction(actor.BasicAttack, battle.GetEnemyPartyFor(actor).Characters[0]);
        }

        return new DoNothingAction();
    }
}
