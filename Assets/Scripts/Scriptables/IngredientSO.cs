using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "Items/Ingredient")]
public class IngredientSO : ScriptableObject, IObject
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
    [SerializeField] private string IngredientName;
    [SerializeField] private IngredientType type; // "Rojo", "Azul", "Verde", "Negro", "Base"
    [SerializeField] private int potency;
    [SerializeField] private float chargeTime;
    private ObjectType objectType = ObjectType.Ingredient; 
    [SerializeField] private int _count = 1; 
    
    
    public int Potency => potency;
    public float ChargeTime => chargeTime;
    public Sprite Sprite => sprite;
    public string Name => IngredientName;
    public IngredientType Type => type;
    public ObjectType ObjectType => objectType;
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