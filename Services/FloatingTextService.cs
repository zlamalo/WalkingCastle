using System.Threading.Tasks;
using Godot;

public class FloatingTextService
{
    // public async Task ShowTextAsync(Vector2 position, string text, Color color, float duration = 1.5f)
    // {
    //     Label damageLabel = new()
    //     {
    //         Text = text,
    //         LabelSettings = new()
    //         {
    //             Font = GlobalResources.Instance.DefaultFont,
    //             FontColor = color,
    //             FontSize = GD.RandRange(8, 12),
    //             OutlineSize = 5,
    //             OutlineColor = Colors.Black
    //         },
    //         ZIndex = 10,
    //         GlobalPosition = position,
    //     };
    //     damageLabel.SetAnchorsPreset(Control.LayoutPreset.Center);

    //     GlobalNodes.Instance.GameWorld.AddChild(damageLabel);
    //     var tween = damageLabel.GetTree().CreateTween();
    //     tween.SetParallel(true);

    //     Vector2 newPosition = new Vector2(
    //         damageLabel.GlobalPosition.X + GD.RandRange(-7, 7),
    //         damageLabel.GlobalPosition.Y - GD.RandRange(10, 15)
    //     );
    //     tween.TweenProperty(damageLabel, "global_position", newPosition, 0.25f);
    //     tween.TweenProperty(damageLabel, "scale", new Vector2(0.5f, 0.5f), 0.25f).SetEase(Tween.EaseType.In).SetDelay(0.3f);

    //     await damageLabel.ToSignal(tween, "finished");
    //     damageLabel.QueueFree();
    // }

    public void ShowText(Vector2 position, string text, Color color, float duration = 1.5f)
    {
        Label damageLabel = new()
        {
            Text = text,
            LabelSettings = new()
            {
                Font = GlobalResources.Instance.DefaultFont,
                FontColor = color,
                FontSize = 8,
                OutlineSize = 5,
                OutlineColor = Colors.Black
            },
            ZIndex = 10,

        };
        damageLabel.GlobalPosition = position + new Vector2(-damageLabel.GetMinimumSize().X / 2f, 0);
        damageLabel.SetAnchorsPreset(Control.LayoutPreset.Center);

        GlobalNodes.Instance.GameWorld.AddChild(damageLabel);
        var tween = damageLabel.CreateTween();

        Vector2 newPosition = new Vector2(
            damageLabel.GlobalPosition.X/* + GD.RandRange(-7, 7)*/,
            damageLabel.GlobalPosition.Y - 10
        );
        tween.TweenProperty(damageLabel, "global_position", newPosition, 0.3f).SetEase(Tween.EaseType.Out);
        tween.TweenInterval(0.3f);
        tween.TweenCallback(Callable.From(damageLabel.QueueFree));
    }
}
