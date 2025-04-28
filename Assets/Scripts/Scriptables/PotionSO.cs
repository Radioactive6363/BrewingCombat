using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Items/Potion")]
public class PotionSO : ScriptableObject, IObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string potionName;
    [SerializeField] private int _count = 1; 
    public PotionEffectType typeOfEffect;
    private ObjectType _objectType = ObjectType.CraftedPotion;
    private StatStruct _stat = new StatStruct();

    public int Potency
    {
        get => _stat.potency;
        set => _stat.potency = value;
    }
    public float ChargeTime
    {
        get => _stat.chargeTime;
        set => _stat.chargeTime = value;
    }
    
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
