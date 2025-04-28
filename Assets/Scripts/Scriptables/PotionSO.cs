using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Items/Potion")]
public class PotionSO : ScriptableObject, IObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string potionName;
    private ObjectType _objectType = ObjectType.CraftedPotion; 
    [SerializeField] private int _count = 1; 
    
    public Sprite Sprite => icon;
    public string Name => potionName;
    public ObjectType ObjectType => _objectType;
    public int Id { get; set; }
    public int Count
    {
        get => _count;
        set => _count = value;
    }

    public IObject Clone()
    {
        return (IObject)MemberwiseClone();
    }
}
