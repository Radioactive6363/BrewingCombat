using UnityEngine;

public enum ObjectType
{
    Ingredient,
    BasePotion,
    CraftedPotion
}

public interface IObject
{
    public Sprite sprite { get; set; }
    public string name { get; set; }
    public ObjectType type { get; set; }
}