FountainOfObjectsGame CreateSmallGame()
{

}

public class FountainOfObjectsGame
{
    public FountainOfObjectsGame(Map map, Player player)
    {

    }
    public void Run()
    {

    }

    private void DisplayStatus()
    {

    }
    private ICommand GetCommand()
    {

    }

    public bool HasWon;

    public RoomType CurrentRoom;
}
public class Map
{
    private readonly RoomType[,] _room;
    public int Rows { get; }
    public int Columns { get; }

    public Map(int rows, int columns)
    {
        Rows = rows; Columns = columns;
        _room = new RoomType[Rows, Columns];
    }

    public RoomType GetRoomTypeAtLocation(Location location)
    {
        return IsOnMap(location) ? _room[location.Row, location.Column] : RoomType.OffMap;
    }
    public bool HasNeighborWithType(Location location, RoomType roomType)
    {

    }

    public bool IsOnMap(Location location)
    {
        return (location.Row >= 0 && location.Row < _room.GetLength(0)
            && location.Column >= 0 && location.Column < _room.GetLength(1));
    }

    public void SetRoomTypeAtLocation(Location location, RoomType type)
    {

    }
}

public record Location(int Row, int Column);


public class Player
{

}

public interface ICommand
{
    void Execute(FountainOfObjectsGame game);
}

public class MoveCommand : ICommand
{
    public MoveCommand(Direction direction)
    {

    }
    public void Execute(FountainOfObjectsGame game)
    {

    }
}

public class EnableFountainCommand : ICommand
{
    public void Execute(FountainOfObjectsGame game)
    {
        
    }
}

public interface ISense
{
    void CanSense();
    void DisplaySense();
}

public class LightInEntranceSense : ISense
{

}

public class FountainSense : ISense
{

}

public class ConsoleHelper
{
    public static void WriteLine(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
    }
}
public enum RoomType { Entrance, Fountain, Empty, OffMap }
public enum Direction { North, South, West, East }


