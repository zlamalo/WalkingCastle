using Godot;
using Godot.Collections;

public class ItemService
{
    public ItemBase GetItemInstance(Item item)
    {
        var itemInstance = item.ItemResource.ItemScene.Instantiate<ItemBase>();
        itemInstance.Item = item;
        return itemInstance;
    }

    public Array<ItemBase> GetLootInstances(Array<Loot> loots)
    {
        Array<ItemBase> drops = [];
        foreach (var loot in loots)
        {
            if (loot.Drops())
            {
                var item = loot.Item;
                var amountDropped = loot.AmountDropped();
                if (item is StackableItem stackableItem)
                {
                    stackableItem.Quantity = amountDropped;
                }
                else
                {
                    if (amountDropped != 1)
                        GD.PrintErr("Item thats not stackable must have quantity of 1");
                }
                drops.Add(GetItemInstance(item));
            }
        }
        return drops;
    }
}
