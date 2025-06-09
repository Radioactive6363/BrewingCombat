using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewPotion", menuName = "Items/Potion")]
public class PotionSo : ScriptableObject, IObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string potionName;
    [FormerlySerializedAs("_count")] [SerializeField] private int count = 1; 
    private PotionEffectType _typeOfEffect;
    private ObjectType _objectType = ObjectType.CraftedPotion;
    private StatStruct _stat = new StatStruct();

    public PotionEffectType EffectType
    {
        get => _typeOfEffect;
        set => _typeOfEffect = value;
    }
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
        get => count;
        set => count = value;
    }

    public IObject Clone()
    {
        return (IObject)MemberwiseClone();
    }
}
