using UnityEngine;

// Dej na předmět ve scéně. Potřebuje Collider s "Is Trigger" zaškrtnutým.
// Hráč musí mít tag "Player" a komponentu Inventory.
public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    public int amount = 1;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = other.GetComponentInParent<Inventory>();
        if (inv == null) return;

        int left = inv.AddItem(item, amount);
        if (left <= 0) Destroy(gameObject);   // všechno se vešlo
        else amount = left;                   // zbytek zůstane na zemi
    }
}
