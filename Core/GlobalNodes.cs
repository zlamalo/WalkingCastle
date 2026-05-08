using Godot;

public partial class GlobalNodes : Node
{
    private static GlobalNodes _instance;
    public static GlobalNodes Instance
    {
        get
        {
            _instance ??= new GlobalNodes();
            return _instance;
        }
    }

    [Signal]
    public delegate void PlayerReadyEventHandler(Blacksmith newPlayer);

    public Blacksmith Player { get; set; }

    public void SetPlayer(Blacksmith player)
    {
        Player = player;
        EmitSignal(SignalName.PlayerReady, player);
        GD.Print("Setting player in GlobalNodes.");
    }
}