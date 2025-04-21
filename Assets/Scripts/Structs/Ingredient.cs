using System;
using UnityEngine;

[Serializable]
public struct Ingredient : IObject
{
    public Sprite sprite { get; set; }
    public string name { get; set; }
    public ObjectType type { get; set; }
    public int id { get; set; }
}