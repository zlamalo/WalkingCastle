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
    public delegate void PlayerReadyEventHandler(Player newPlayer);

    public Player Player { get; set; }
    public InformationLogUi InformationLogUi { get; set; }

    public void SetPlayer(Player player)
    {
        Player = player;
        EmitSignal(SignalName.PlayerReady, player);
        GD.Print("Setting player in GlobalNodes.");
    }
}
