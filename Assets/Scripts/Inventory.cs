using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;
    public bool IsEmpty => item == null || amount <= 0;
}

// Dej na hráče
public class Inventory : MonoBehaviour
{
    public int slotCount = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public event Action OnChanged;

    void Awake()
    {
        while (slots.Count < slotCount)
            slots.Add(new InventorySlot());
    }

    /// <summary>Přidá item. Vrací počet kusů, které se nevešly (0 = vše se vešlo).</summary>
    public int AddItem(ItemData item, int amount = 1)
    {
        // 1) doplnit existující stacky
        foreach (var s in slots)
        {
            if (amount <= 0) break;
            if (!s.IsEmpty && s.item == item && s.amount < item.maxStack)
            {
                int add = Mathf.Min(item.maxStack - s.amount, amount);
                s.amount += add;
                amount -= add;
            }
        }

        // 2) použít prázdné sloty
        foreach (var s in slots)
        {
            if (amount <= 0) break;
            if (s.IsEmpty)
            {
                int add = Mathf.Min(item.maxStack, amount);
                s.item = item;
                s.amount = add;
                amount -= add;
            }
        }

        OnChanged?.Invoke();
        return amount;
    }

    public void RemoveFromSlot(int index, int amount = 1)
    {
        if (index < 0 || index >= slots.Count) return;
        var s = slots[index];
        if (s.IsEmpty) return;

        s.amount -= amount;
        if (s.amount <= 0)
        {
            s.item = null;
            s.amount = 0;
        }
        OnChanged?.Invoke();
    }
}
