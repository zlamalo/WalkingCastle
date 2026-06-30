using Godot;

[GlobalClass]
public partial class Loot : Resource
{
    private RNGService rngService = GlobalServices.Instance.RNGService;

    [Export]
    public Item Item { get; set; }

    [Export]
    public ValuesRange AmountRange { get; set; } = new(1, 1);

    [Export(PropertyHint.Range, "0,1")]
    public float DropChance { get; set; } = 1f;

    public bool Drops()
    {
        return rngService.IsSuccess(DropChance);
    }

    public int AmountDropped()
    {
        return rngService.RandomAmount(AmountRange);
    }
}
