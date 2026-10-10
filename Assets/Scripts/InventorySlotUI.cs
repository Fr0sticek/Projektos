using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Dej na prefab slotu (child Image "Icon" + child TMP text "Amount")
public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText;

    public void SetSlot(InventorySlot slot)
    {
        if (slot.IsEmpty)
        {
            icon.enabled = false;
            amountText.text = "";
        }
        else
        {
            icon.enabled = true;
            icon.sprite = slot.item.icon;
            amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";
        }
    }
}