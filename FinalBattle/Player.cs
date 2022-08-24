namespace FinalBattle;

public interface IPlayer
{
    IAction ChooseAction(Battle battle, Character actor);
}



