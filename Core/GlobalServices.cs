public class GlobalServices
{
    private static GlobalServices _instance;
    public static GlobalServices Instance
    {
        get
        {
            _instance ??= new GlobalServices();
            return _instance;
        }
    }

    public TooltipService TooltipService = new();
    public RecipeService RecipeService = new();
    public ResourceLoaderService ResourceLoaderService = new();
}