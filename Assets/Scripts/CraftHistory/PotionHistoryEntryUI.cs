
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionHistoryEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI potionNameText;
    [SerializeField] private TextMeshProUGUI creationDateText;

    public void SetEntry(PotionHistoryEntry entry)
    {
        Debug.Log($"Setting entry: {entry.potion?.Name} at {entry.createdAt}");

        if (potionNameText != null)
            potionNameText.text = entry.potion?.Name ?? "Unnamed Potion";

        if (creationDateText != null)
            creationDateText.text = entry.createdAt.ToString("g");
    }
}
