using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro; 
using System.Collections.Generic;

public class PotionHistoryUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentParent;
    public GameObject entryPrefab;

    private void Start()
    {
        LoadHistory();
    }

    public void LoadHistory()
    {
        if (entryPrefab == null || contentParent == null)
        {
            Debug.LogError("Missing references in PotionHistoryUIManager.");
            return;
        }

        if (PotionHistorySystem.Instance == null)
        {
            Debug.LogError("PotionHistorySystem.Instance is null!");
            return;
        }

        if (PotionHistorySystem.Instance.potionHistory == null || PotionHistorySystem.Instance.potionHistory.Count == 0)
        {
            Debug.Log("No potion history to show yet.");
            return;
        }

        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in PotionHistorySystem.Instance.potionHistory)
        {
            GameObject go = Instantiate(entryPrefab, contentParent);
            go.SetActive(true);
            go.transform.localScale = Vector3.one;
            

            var display = go.GetComponent<PotionHistoryEntryUI>();
            if (display != null)
            {
                display.SetEntry(entry);
            }
        }

      

    }
}
