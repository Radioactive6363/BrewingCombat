using System;
using UnityEngine;

[Serializable]
public class PotionHistoryEntry
{
    public PotionSo potion;
    public DateTime createdAt;

    public PotionHistoryEntry(PotionSo potion)
    {
        this.potion = potion;
        this.createdAt = DateTime.Now;
    }
}
