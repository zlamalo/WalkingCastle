using Godot;
using System;

public partial class InformationLogUi : Control
{
	private PackedScene informationLogEntryScene = GD.Load<PackedScene>("res://UserInterface/InformationLogEntryUi.tscn");

	private VBoxContainer logs;

	public override void _Ready()
	{
		GlobalNodes.Instance.InformationLogUi = this;
		logs = GetNode<VBoxContainer>("Logs");
	}

	public override void _ExitTree()
	{
		GlobalNodes.Instance.InformationLogUi = null;
	}

	public void AddLog(string text, Color? color)
	{
		var log = informationLogEntryScene.Instantiate<Label>();
		log.Text = text;
		if (color is Color newColor)
		{
			log.LabelSettings.FontColor = newColor;
		}

		Timer logTimer = new Timer
		{
			Autostart = true,
			OneShot = true,
			WaitTime = 3
		};
		logTimer.Timeout += log.QueueFree;
		logs.AddChild(log);
		log.AddChild(logTimer);
	}
}
