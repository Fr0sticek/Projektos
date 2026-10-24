using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;   // co se sebere
    public int amount = 1;  // kolik kusů

    void OnTriggerEnter(Collider other)
    {
        // reaguj jen na hráče
        if (!other.CompareTag("Player")) return;

        // najdi inventář hráče
        var inv = other.GetComponentInParent<Inventory>();
        if (inv == null) return;

        // zkus přidat do inventáře
        int left = inv.AddItem(item, amount);

        if (left <= 0) Destroy(gameObject);   // všechno se vešlo, předmět zmizí
        else amount = left;                   // zbytek zůstane na zemi
    }
}