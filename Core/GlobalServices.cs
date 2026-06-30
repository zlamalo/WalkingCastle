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
    public InformationLogService InformationLogService = new();
    public RNGService RNGService = new();
    public ItemService ItemService = new();

    public GameSettings GameSettings = new();
}