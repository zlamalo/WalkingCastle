using Godot;
using System;

public partial class GameWorld : Node2D
{
	public override void _Ready()
	{
		GlobalNodes.Instance.GameWorld = this;
	}
}
