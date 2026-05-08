using System;
using Godot;

[GlobalClass]
public partial class Item : Resource
{
    [Export]
    public string Name { get; set; }

    [Export]
    public Texture2D Icon { get; set; }

    [Export]
    public PackedScene ItemScene { get; set; }

    public Guid Id { get; private set; }

    public Item()
    {
        Id = Guid.NewGuid();
    }

}
