using UnityEngine;

// Vytvoření itemu: v Projectu pravý klik > Create > Inventory > Item
[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName = "Item";
    [TextArea] public string description;
    public Sprite icon;
    public int maxStack = 16;
}
