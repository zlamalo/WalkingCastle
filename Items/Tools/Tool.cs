using Godot;

[GlobalClass]
public partial class Tool : ItemResource
{
    [Export]
    public ToolType Type { get; set; }

    [Export]
    public int ToolPower { get; set; } = 1;

    [Export]
    public ValuesRange DamageRange { get; set; }

    public ToolBase GetInstance()
    {
        if (ItemScene == null)
            return null;

        var instance = ItemScene.Instantiate();
        if (instance is ToolBase toolBase)
        {
            toolBase.Tool = this;
            return toolBase;
        }

        return null;
    }
}
