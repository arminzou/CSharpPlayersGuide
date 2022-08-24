namespace FinalBattle;

public class TheUncodedOne : Character
{
    public override string Name => "The Uncoded One";
    public override IAttack BasicAttack => new Unraveling();
    public TheUncodedOne() : base(15)
    {
    }
}
public class Unraveling : IAttack
{
    public string Name => "UNRAVELING";

    public AttackData Create() => new AttackData(new Random().Next(5), Type: DamageType.Decoding);
}