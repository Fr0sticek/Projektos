using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;             // ikona itemu
    public TMP_Text amountText;    // počet kusů
    public float tooltipDelay = 1f; // za jak dlouho se ukáže popisek

    private InventorySlot currentSlot;
    private Coroutine tooltipRoutine;

    // Nastaví, co slot zobrazuje
    public void SetSlot(InventorySlot slot)
    {
        currentSlot = slot;

        if (slot.IsEmpty)
        {
            // prázdný slot
            icon.enabled = false;
            amountText.text = "";
            HideTooltip();
        }
        else
        {
            // slot s itemem
            icon.enabled = true;
            icon.sprite = slot.item.icon;
            amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";
        }
    }

    // Myš najela na slot: po prodlevě ukaž popisek
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentSlot == null || currentSlot.IsEmpty) return;
        HideTooltip();
        tooltipRoutine = StartCoroutine(ShowTooltipAfterDelay());
    }

    // Myš odjela: schovej popisek
    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    // Počká a pak zobrazí popisek vedle slotu
    private IEnumerator ShowTooltipAfterDelay()
    {
        yield return new WaitForSecondsRealtime(tooltipDelay);

        if (currentSlot != null && !currentSlot.IsEmpty && TooltipUI.Instance != null)
            TooltipUI.Instance.Show(currentSlot.item.description, (RectTransform)transform);
    }

    // Zruší čekání a schová popisek
    private void HideTooltip()
    {
        if (tooltipRoutine != null)
        {
            StopCoroutine(tooltipRoutine);
            tooltipRoutine = null;
        }
        if (TooltipUI.Instance != null)
            TooltipUI.Instance.Hide();
    }

    void OnDisable()
    {
        HideTooltip();
    }
}