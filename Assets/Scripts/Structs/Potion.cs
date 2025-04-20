using UnityEngine;

[System.Serializable]
public struct Potion : IObject
{
    public Sprite sprite { get; set; }
    public string name { get; set; }
    public ObjectType type { get; set; }
    public string resultId;
}