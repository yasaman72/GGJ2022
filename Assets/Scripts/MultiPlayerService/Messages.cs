
namespace Multiplayer
{
    public interface GameMsg
    {

    }

    public class MoveMsg : GameMsg
    {
        public float X { get; set; }
    }

    public class StopMoveMsg : GameMsg
    {
    }

    public class FireMsg : GameMsg
    {
    }

    public class JumpMsg : GameMsg
    {
    }

    public class PlantMsg : GameMsg
    {
    }

    public class StartMsg : GameMsg
    {

    }

    public class SwitchGravityMsg : GameMsg
    {

    }
}