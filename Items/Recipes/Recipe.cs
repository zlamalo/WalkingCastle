using System;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Recipe : Resource
{
    [Export]
    public Item Result { get; set; }

    [Export]
    public Array<Item> Ingredients { get; set; }

    public Guid Id { get; private set; }

    public Recipe()
    {
        Id = Guid.NewGuid();
    }
}
