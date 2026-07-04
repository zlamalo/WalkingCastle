using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Harvestable : Resource
{
    [Export]
    public Texture2D HarvestableTexture { get; set; }

    [Export]
    public ToolType HarvestableBy { get; set; }

    /// <summary>
    /// Number of hits required to harvest node
    /// </summary>
    [Export]
    public int HitPoints { get; set; } = 1;

    [Export]
    public Array<Loot> Loot = [];
}