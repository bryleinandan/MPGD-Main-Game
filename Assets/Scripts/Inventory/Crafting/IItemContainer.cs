public interface IItemContainer
{
    int ItemCount(Item item);
    bool ContainsRequiredItem(Item item);
    bool RemoveItem(Item item);
    bool AddItem(Item item);
    bool IsFull();
}
