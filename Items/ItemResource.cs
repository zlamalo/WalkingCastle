using System;
using Godot;


[GlobalClass]
public partial class ItemResource : Resource
{
    [Export]
    public string Name { get; set; }

    [Export]
    public Texture2D Icon { get; set; }

    [Export]
    public PackedScene ItemScene { get; set; }

    public Guid Id { get; private set; }

    public ItemResource()
    {
        Id = Guid.NewGuid();
    }
}
