using Godot;

[GlobalClass]
public partial class ValuesRange : Resource
{
    [Export]
    public int Min { get; set; }

    [Export]
    public int Max { get; set; }

    public ValuesRange()
    {
        Min = 1;
        Max = 1;
    }

    public ValuesRange(int min, int max)
    {
        if (min > max)
            GD.PrintErr($"Invalid values Range. Min value {min} is bigger than max value {max}");
        Min = min;
        Max = max;
    }
}