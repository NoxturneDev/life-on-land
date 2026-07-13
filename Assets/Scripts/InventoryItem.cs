[System.Serializable]
public struct InventoryItem
{
    public string itemID;
    public int quantity;

    public InventoryItem(string id, int qty)
    {
        itemID = id;
        quantity = qty;
    }
}
