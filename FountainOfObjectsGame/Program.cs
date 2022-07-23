
ConsoleHelper.WriteLine("Do you want to play small, medium, or large game?", ConsoleColor.White);
var input = Console.ReadLine();
DisplayIntro();

Console.ForegroundColor = ConsoleColor.Cyan;
FountainOfObjectsGame game = input switch
{
    "small" => CreateSmallGame(),
    "medium" => CreateMediumGame(),
    "large" => CreateLargeGame()
};

game.Run();

void DisplayIntro()
{
    Console.WriteLine("You enter the Cavern of Objects, a maze of rooms filled with dangerous pits in search of the Fountain of Objects.");
    Console.WriteLine("Light is visible only in the entrance, and no other light is seen anywhere in the caverns..");
    Console.WriteLine("You must navigate the Caverns with your other senses.");
    Console.WriteLine("Find the Fountain of Objects, activate it, and return to the entrance.");
    Console.WriteLine("Look out for pits. You will feel a breeze if a pit is in an adjacent room.If you enter a room with a pit, you will die.");
    Console.WriteLine("Maelstroms are violent forces of sentient wind.Entering a room with one could transport you to any other location in the caverns.You will be able to hear their growling and groaning in nearby rooms.");
    Console.WriteLine("Amaroks roam the caverns.Encountering one is certain death, but you can smell their rotten stench in nearby rooms.");
    Console.WriteLine("You carry with you a bow and a quiver of arrows.You can use them to shoot monsters in the caverns but be warned: you have a limited supply.");
}

FountainOfObjectsGame CreateSmallGame()
{
    Map map = new Map(4, 4);
    Location start = new Location(0, 0);
    map.SetRoomTypeAtLocation(start, RoomType.Entrance);
    map.SetRoomTypeAtLocation(new Location(0, 2), RoomType.Fountain);
    map.SetRoomTypeAtLocation(new Location(3, 0), RoomType.Pit);
    Monster[] monsters = new Monster[]
    {
         new Maelstrom(new Location(2,4)),
         new Amarok(new Location(1,3))
    };
    return new FountainOfObjectsGame(map, new Player(start), monsters);
}

FountainOfObjectsGame CreateMediumGame()
{
    Map map = new Map(6, 6);
    Location start = new Location(3, 0);
    map.SetRoomTypeAtLocation(start, RoomType.Entrance);
    map.SetRoomTypeAtLocation(new Location(4, 2), RoomType.Fountain);
    map.SetRoomTypeAtLocation(new Location(3, 5), RoomType.Pit);
    map.SetRoomTypeAtLocation(new Location(5, 6), RoomType.Pit);
    Monster[] monsters = new Monster[]
    {
         new Maelstrom(new Location(2,4)),
         new Amarok(new Location(1,3))
    };
    return new FountainOfObjectsGame(map, new Player(start), monsters);
}

FountainOfObjectsGame CreateLargeGame()
{
    Map map = new Map(8, 8);
    Location start = new Location(2, 5);
    map.SetRoomTypeAtLocation(start, RoomType.Entrance);
    map.SetRoomTypeAtLocation(new Location(6, 3), RoomType.Fountain);
    map.SetRoomTypeAtLocation(new Location(2, 4), RoomType.Pit);
    map.SetRoomTypeAtLocation(new Location(3, 1), RoomType.Pit);
    map.SetRoomTypeAtLocation(new Location(0, 5), RoomType.Pit);
    map.SetRoomTypeAtLocation(new Location(1, 2), RoomType.Pit);
    Monster[] monsters = new Monster[]
    {
         new Maelstrom(new Location(2,5)),
         new Amarok(new Location(1,3))
    };
    return new FountainOfObjectsGame(map, new Player(start), monsters);
}

public class FountainOfObjectsGame
{
    public Map Map { get; }

    public Player Player { get; }
    public Monster[] Monsters { get; set; }

    public bool IsFountainOn { get; set; }

    private readonly ISense[] _senses;

    public bool HasWon => CurrentRoom == RoomType.Entrance && IsFountainOn;

    public RoomType CurrentRoom => Map.GetRoomTypeAtLocation(Player.Location);

    public FountainOfObjectsGame(Map map, Player player, Monster[] monsters)
    {
        Map = map;
        Player = player;
        Monsters = monsters;

        _senses = new ISense[]
        {
            new LightInEntranceSense(),
            new FountainSense(),
            new PitSense(),
            new MaelstromSense(),
            new AmarokSense(),
        };
    }
    public void Run()
    {
        while (!HasWon && Player.IsAlive)
        {
            DisplayArrowsRemaining();
            DisplayStatus();
            ICommand command = GetCommand();
            command.Execute(this);

            foreach (var monster in Monsters)
            {
                if (Player.Location == monster.Location && monster.IsAlive) monster.Activate(this);
            }
        }

        if (CurrentRoom == RoomType.Pit)
        {
            Player.Kill("Fall into pit");
        }

        if (HasWon)
        {
            ConsoleHelper.WriteLine("The Fountain of Objects has been reactivated, and you have escaped with your life!", ConsoleColor.DarkGreen);
            ConsoleHelper.WriteLine("You win!", ConsoleColor.DarkGreen);
        }
        else
        {
            ConsoleHelper.WriteLine(Player.CauseOfDeath, ConsoleColor.Red);
            ConsoleHelper.WriteLine("You lost.", ConsoleColor.Red);
        }
    }

    private void DisplayStatus()
    {
        ConsoleHelper.WriteLine("--------------------------------------------------------------------------------", ConsoleColor.Gray);
        ConsoleHelper.WriteLine($"You are in the room at (Row={Player.Location.Row}, Column={Player.Location.Column}).", ConsoleColor.Gray);
        foreach (ISense sense in _senses)
            if (sense.CanSense(this))
                sense.DisplaySense(this);
    }
    private void DisplayArrowsRemaining()
    {
        ConsoleHelper.WriteLine("--------------------------------------------------------------------------------", ConsoleColor.Gray);
        ConsoleHelper.WriteLine($"You have {Player.ArrowsRemaining} arrows left in your bag.", ConsoleColor.Gray);
    }

    private ICommand GetCommand()
    {
        while (true)
        {
            ConsoleHelper.WriteLine("What do you want to do? ", ConsoleColor.White);
            Console.ForegroundColor = ConsoleColor.Cyan;
            string? input = Console.ReadLine();

            if (input == "move north") return new MoveCommand(Direction.North);
            if (input == "move south") return new MoveCommand(Direction.South);
            if (input == "move west") return new MoveCommand(Direction.West);
            if (input == "move east") return new MoveCommand(Direction.East);
            if (input == "enable fountain") return new EnableFountainCommand();
            if (input == "shoot north") return new ShootCommand(Direction.North);
            if (input == "shoot south") return new ShootCommand(Direction.South);
            if (input == "shoot west") return new ShootCommand(Direction.West);
            if (input == "shoot east") return new ShootCommand(Direction.East);
            if (input == "help") return new HelpCommand();

            ConsoleHelper.WriteLine($"I did not understand '{input}'.", ConsoleColor.Red);
        }
    }
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
        if (GetRoomTypeAtLocation(new Location(location.Row - 1, location.Column - 1)) == roomType) return true;
        if (GetRoomTypeAtLocation(new Location(location.Row + 1, location.Column + 1)) == roomType) return true;
        if (GetRoomTypeAtLocation(new Location(location.Row - 1, location.Column)) == roomType) return true;
        if (GetRoomTypeAtLocation(new Location(location.Row, location.Column - 1)) == roomType) return true;
        if (GetRoomTypeAtLocation(new Location(location.Row + 1, location.Column)) == roomType) return true;
        if (GetRoomTypeAtLocation(new Location(location.Row, location.Column + 1)) == roomType) return true;
        if (GetRoomTypeAtLocation(new Location(location.Row - 1, location.Column + 1)) == roomType) return true;
        if (GetRoomTypeAtLocation(new Location(location.Row + 1, location.Column - 1)) == roomType) return true;
        return false;
    }

    public bool IsOnMap(Location location)
    {
        return (location.Row >= 0 && location.Row < _room.GetLength(0)
            && location.Column >= 0 && location.Column < _room.GetLength(1));
    }

    public void SetRoomTypeAtLocation(Location location, RoomType type)
    {
        _room[location.Row, location.Column] = type;
    }
}

public record Location(int Row, int Column);


public class Player
{
    public Location Location { get; set; }
    public bool IsAlive { get; set; } = true;

    public string CauseOfDeath { get; set; } = "";
    public int ArrowsRemaining { get; set; } = 5;

    public Player(Location start)
    {
        Location = start;
    }

    public void Kill(string cause)
    {
        IsAlive = false;
        CauseOfDeath = cause;
    }
}

public abstract class Monster
{
    public Location Location { get; set; }
    public bool IsAlive { get; set; } = true;
    public Monster(Location start)
    {
        Location = start;
    }

    public abstract void Activate(FountainOfObjectsGame game);
}

public class Maelstrom : Monster
{
    public Maelstrom(Location start) : base(start)
    {
    }

    public override void Activate(FountainOfObjectsGame game)
    {
        ConsoleHelper.WriteLine("You have encountered a maelstrom and have been swept away to another room!", ConsoleColor.Magenta);
        game.Player.Location = Clamp(new Location(game.Player.Location.Row - 1, game.Player.Location.Column + 2), game.Map.Rows, game.Map.Columns);
        this.Location = Clamp(new Location(this.Location.Row + 1, this.Location.Column - 2), game.Map.Rows, game.Map.Columns);
    }

    private Location Clamp(Location location, int totalRows, int totalCols)
    {
        int row = location.Row;
        if (row < 0) row = 0;
        if (row > totalRows) row = totalRows - 1;

        int col = location.Column;
        if (col < 0) col = 0;
        if (col > totalCols) col = totalCols - 1;

        return new Location(row, col);
    }
}
public class Amarok : Monster
{
    public Amarok(Location start) : base(start)
    {
    }

    public override void Activate(FountainOfObjectsGame game)
    {
        ConsoleHelper.WriteLine("You have encountered an Amarok and have been killed!", ConsoleColor.Magenta);
        game.Player.Kill("Killed by Amarok.");
    }
}

public interface ICommand
{
    void Execute(FountainOfObjectsGame game);
}

public class MoveCommand : ICommand
{
    public Direction Direction { get; set; }
    public MoveCommand(Direction direction)
    {
        Direction = direction;
    }
    public void Execute(FountainOfObjectsGame game)
    {
        Location currLocation = game.Player.Location;
        Location newLocation = Direction switch
        {
            Direction.North => new Location(currLocation.Row - 1, currLocation.Column),
            Direction.South => new Location(currLocation.Row + 1, currLocation.Column),
            Direction.West => new Location(currLocation.Row, currLocation.Column - 1),
            Direction.East => new Location(currLocation.Row, currLocation.Column + 1),
        };

        if (game.Map.IsOnMap(newLocation))
            game.Player.Location = newLocation;
        else
            ConsoleHelper.WriteLine("There is a wall there.", ConsoleColor.Red);

    }
}
public class ShootCommand : ICommand
{
    public Direction Direction { get; set; }
    public ShootCommand(Direction direction)
    {
        Direction = direction;
    }
    public void Execute(FountainOfObjectsGame game)
    {
        if (game.Player.ArrowsRemaining == 0)
        {
            ConsoleHelper.WriteLine("You don't have any arrows left!", ConsoleColor.Red);
            return;
        }

        Location currLocation = game.Player.Location;
        Location targetLocation = Direction switch
        {
            Direction.North => new Location(currLocation.Row - 1, currLocation.Column),
            Direction.South => new Location(currLocation.Row + 1, currLocation.Column),
            Direction.West => new Location(currLocation.Row, currLocation.Column - 1),
            Direction.East => new Location(currLocation.Row, currLocation.Column + 1),
        };
        bool foundSomething = false;
        foreach (var monster in game.Monsters)
        {
            if (monster.Location == targetLocation)
            {
                monster.IsAlive = false;
                foundSomething = true;
            }
        }
        if (foundSomething) ConsoleHelper.WriteLine("You fired an arrow and hit something!", ConsoleColor.Green);
        else ConsoleHelper.WriteLine("You fired an arrow and missed", ConsoleColor.Magenta);

        game.Player.ArrowsRemaining--;
    }
}

public class HelpCommand : ICommand
{
    public void Execute(FountainOfObjectsGame game)
    {
        ConsoleHelper.WriteLine("help", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("    Displays this help information.", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("enable fountain", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("    Turns on the Fountain of Objects if you are in the fountain room, or does nothing if you are not.", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("move north", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("    Moves to the room directly north of the current room, as long as there are no walls.", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("move south", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("    Moves to the room directly south of the current room, as long as there are no walls.", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("move east", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("    Moves to the room directly east of the current room, as long as there are no walls.", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("move west", ConsoleColor.Gray);
        ConsoleHelper.WriteLine("    Moves to the room directly west of the current room, as long as there are no walls.", ConsoleColor.Gray);
    }
}

public class EnableFountainCommand : ICommand
{
    public void Execute(FountainOfObjectsGame game)
    {
        if (game.Map.GetRoomTypeAtLocation(game.Player.Location) == RoomType.Fountain) game.IsFountainOn = true;
        else ConsoleHelper.WriteLine("The fountain is not in this room. There was no effect.", ConsoleColor.Red);
    }
}

public interface ISense
{
    bool CanSense(FountainOfObjectsGame game);
    void DisplaySense(FountainOfObjectsGame game);
}

public class LightInEntranceSense : ISense
{
    public bool CanSense(FountainOfObjectsGame game) => game.Map.GetRoomTypeAtLocation(game.Player.Location) == RoomType.Entrance;
    public void DisplaySense(FountainOfObjectsGame game) => ConsoleHelper.WriteLine("You see light in this room coming from outside the cavern. This is the entrance.", ConsoleColor.Yellow);
}

public class FountainSense : ISense
{
    public bool CanSense(FountainOfObjectsGame game) => game.Map.GetRoomTypeAtLocation(game.Player.Location) == RoomType.Fountain;
    public void DisplaySense(FountainOfObjectsGame game)
    {
        if (game.IsFountainOn) ConsoleHelper.WriteLine("You hear the rushing waters from the Fountain of Objects. It has been reactivated!", ConsoleColor.DarkCyan);
        else ConsoleHelper.WriteLine("You hear water dripping in this room. The Fountain of Objects is here!", ConsoleColor.DarkCyan);
    }
}
public class PitSense : ISense
{
    public bool CanSense(FountainOfObjectsGame game) => game.Map.HasNeighborWithType(game.Player.Location, RoomType.Pit);
    public void DisplaySense(FountainOfObjectsGame game) => ConsoleHelper.WriteLine("You feel a draft.There is a pit in a nearby room.", ConsoleColor.DarkRed);
}

public class MaelstromSense : ISense
{
    public bool CanSense(FountainOfObjectsGame game)
    {
        foreach (var monster in game.Monsters)
        {
            if (monster is Maelstrom && monster.IsAlive)
            {
                if (Math.Abs(monster.Location.Row - game.Player.Location.Row) <= 1 && Math.Abs(monster.Location.Column - game.Player.Location.Column) <= 1) return true;
            }
        }
        return false;
    }

    public void DisplaySense(FountainOfObjectsGame game) => ConsoleHelper.WriteLine("You hear the growling and groaning of a maelstrom nearby.", ConsoleColor.Blue);
}

public class AmarokSense : ISense
{
    public bool CanSense(FountainOfObjectsGame game)
    {
        foreach (var monster in game.Monsters)
        {
            if (monster is Amarok && monster.IsAlive)
            {
                if (Math.Abs(monster.Location.Row - game.Player.Location.Row) <= 1 && Math.Abs(monster.Location.Column - game.Player.Location.Column) <= 1) return true;
            }
        }
        return false;
    }

    public void DisplaySense(FountainOfObjectsGame game) => ConsoleHelper.WriteLine("You can smell the rotten stench of an amarok in a nearby room.", ConsoleColor.Blue);
}

public class ConsoleHelper
{
    public static void WriteLine(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
    }
}
public enum RoomType { Empty, Entrance, Fountain, OffMap, Pit }
public enum Direction { North, South, West, East }


