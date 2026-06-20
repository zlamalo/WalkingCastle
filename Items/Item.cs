using Godot;

[GlobalClass]
public partial class Item : Resource
{
    [Export]
    public ItemResource ItemResource { get; set; }
}
