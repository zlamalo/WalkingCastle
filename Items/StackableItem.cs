using System;
using Godot;

[GlobalClass]
public partial class StackableItem : Item
{
    [Export]
    public int Quantity { get; set; } = 1;
}
