using Godot;
using System;

public partial class TextPanelUi : PanelContainer
{
	public void UpdateText(string text)
	{
		Label label = GetNode<Label>("MarginContainer/Label");
		label.Text = text;
	}
}
