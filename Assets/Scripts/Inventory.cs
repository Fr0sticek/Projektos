using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData item;   // co je ve slotu
    public int amount;      // kolik kusů
    public bool IsEmpty => item == null || amount <= 0;
}

public class Inventory : MonoBehaviour
{
    public int slotCount = 20;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public event Action OnChanged;   // zavolá se při každé změně

    void Awake()
    {
        // vytvoř prázdné sloty
        while (slots.Count < slotCount)
            slots.Add(new InventorySlot());
    }

    // Přidá item. Vrací počet kusů, které se nevešly (0 = vše se vešlo).
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

    // Odebere kusy z daného slotu
    public void RemoveFromSlot(int index, int amount = 1)
    {
        if (index < 0 || index >= slots.Count) return;
        var s = slots[index];
        if (s.IsEmpty) return;

        s.amount -= amount;
        if (s.amount <= 0)
        {
            // slot se vyprázdnil
            s.item = null;
            s.amount = 0;
        }
        OnChanged?.Invoke();
    }
}