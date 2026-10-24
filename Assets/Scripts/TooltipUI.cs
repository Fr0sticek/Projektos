using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public GameObject panel;   // panel s pozadím
    public TMP_Text text;      // text uvnitř panelu
    public float gap = 30f;    // mezera mezi slotem a popiskem

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(string description, RectTransform slot)
    {
        text.text = description;
        panel.SetActive(true);

        // panel se roztahuje doprava od své pozice
        RectTransform panelRect = (RectTransform)panel.transform;
        panelRect.anchorMin = panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0f, 0.5f);
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);

        // pravý střed slotu ve světových souřadnicích
        Vector3[] c = new Vector3[4];
        slot.GetWorldCorners(c);
        float rightX = Mathf.Max(c[0].x, c[1].x, c[2].x, c[3].x);
        float midY = (c[0].y + c[1].y) / 2f;

        // přepočet mezery podle měřítka Canvasu
        Canvas canvas = panel.GetComponentInParent<Canvas>();
        float scale = canvas != null ? canvas.scaleFactor : 1f;

        // umístit panel vedle slotu
        panelRect.position = new Vector3(rightX + gap * scale, midY, 0f);

        // zasunout panel do obrazovky (jen svisle)
        Vector3[] pc = new Vector3[4];
        panelRect.GetWorldCorners(pc);

        if (pc[1].y > Screen.height)
            panelRect.position -= new Vector3(0f, pc[1].y - Screen.height, 0f);
        if (pc[0].y < 0)
            panelRect.position += new Vector3(0f, -pc[0].y, 0f);

        // vykreslit nad vším
        panel.transform.SetAsLastSibling();
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}