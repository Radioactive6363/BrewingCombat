using Unity.VisualScripting;
using UnityEngine;

public enum ObjectType
{
    Ingredient,
    BasePotion,
    CraftedPotion
}

public interface IObject
{
    public Sprite Sprite { get; }
    public string Name { get; }
    public ObjectType ObjectType { get; }
    public int Id { get; }
    public int Count { get; set; }

    public IObject Clone();
}