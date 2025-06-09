using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Items/Ingredient")]
public class IngredientSo : ScriptableObject, IObject
{ 
    public enum IngredientType
    {
        Red,
        Blue,
        Green,
        Black,
        Base,
    }
    [SerializeField] private Sprite sprite;
    [FormerlySerializedAs("IngredientName")] [SerializeField] private string ingredientName;
    [SerializeField] private IngredientType type; // "Rojo", "Azul", "Verde", "Negro", "Base"
    [SerializeField] private int potency;
    [SerializeField] private float chargeTime;
    private ObjectType _objectType = ObjectType.Ingredient; 
    [FormerlySerializedAs("_count")] [SerializeField] private int count = 1; 
    
    
    public int Potency => potency;
    public float ChargeTime => chargeTime;
    public Sprite Sprite => sprite;
    public string Name => ingredientName;
    public IngredientType Type => type;
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