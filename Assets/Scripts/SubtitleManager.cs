using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public TMP_Text subtitleText;

    public void ShowSubtitle(string text)
    {
        subtitleText.text = text;
    }

    public void ClearSubtitle()
    {
        subtitleText.text = "";
    }
}
