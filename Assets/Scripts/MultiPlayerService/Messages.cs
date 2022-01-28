
namespace Multiplayer
{
    public interface GameMsg
    {

    }

    public class MoveMsg : GameMsg
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class StartMsg : GameMsg
    {

    }

    public class SwitchMsg : GameMsg
    {

    }
}