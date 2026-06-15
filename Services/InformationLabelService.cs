using System.Collections.Generic;
using Godot;

public class TooltipService
{
    private PackedScene textPanelScene = GD.Load<PackedScene>("res://UserInterface/TextPanelUi.tscn");

    private List<string> shownTooltips = new();

    public void ShowTooltip(Node2D tooltipTarget, Vector2 offset, string text)
    {
        if (shownTooltips.Contains(text))
            return;

        TextPanelUi tooltip = textPanelScene.Instantiate<TextPanelUi>();
        tooltip.Position += offset;
        tooltip.UpdateText(text);
        tooltipTarget.AddChild(tooltip);
        shownTooltips.Add(text);

        Timer tooltipDurationTimer = new()
        {
            OneShot = true,
            WaitTime = 2,
        };
        tooltipTarget.AddChild(tooltipDurationTimer);
        tooltipDurationTimer.Start();
        tooltipDurationTimer.Timeout += () => { tooltip.QueueFree(); shownTooltips.Remove(text); };
    }
}
