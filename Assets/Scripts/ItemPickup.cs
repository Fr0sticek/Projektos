using UnityEngine;
using UnityEngine.Playables;

public class ItemPickup : MonoBehaviour
{
    public ItemData item;
    public int amount = 1;

    public PlayableDirector cutscene; // přetáhneš sem Timeline objekt

    private bool pickedUp = false;

    void OnTriggerEnter(Collider other)
    {
        if (pickedUp) return;

        if (!other.CompareTag("Player")) return;

        var inv = other.GetComponentInParent<Inventory>();
        if (inv == null) return;

        int left = inv.AddItem(item, amount);

        if (left <= 0)
        {
            pickedUp = true;

            // spuštění cutscény
            if (cutscene != null)
                cutscene.Play();

            Destroy(gameObject);
        }
        else
        {
            amount = left;
        }
    }
}