using Godot;

public class GlobalResources
{
    private static GlobalResources _instance;
    public static GlobalResources Instance
    {
        get
        {
            _instance ??= new GlobalResources();
            return _instance;
        }
    }

    public Font DefaultFont = GD.Load<Font>("res://Assets/Fonts/PixelFont.TTF");
}