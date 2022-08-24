namespace FinalBattle;

public enum DamageType { Normal, Decoding };
public record AttackData(int Damage, DamageType Type = DamageType.Normal, double ProbabilityOfHitting = 1.0);

public interface IAttack
{
    public string Name { get; }
    AttackData Create();
}