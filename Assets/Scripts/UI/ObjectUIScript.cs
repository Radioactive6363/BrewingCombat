using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUIScript : MonoBehaviour
{
    public IObject ObjectContained;
    [SerializeField] private GameObject objectUISprite;
    [SerializeField] private GameObject objectUIQuantity;
    [SerializeField] private GameObject objectUISelected;
    [SerializeField] private GameObject objectUIName;
    
    void Start()
    {
        objectUISprite.GetComponent<Image>().sprite = ObjectContained.Sprite;
        objectUIQuantity.GetComponent<TMPro.TextMeshProUGUI>().text = ObjectContained.Count.ToString();
        objectUIName.GetComponent<TMPro.TextMeshProUGUI>().text = ObjectContained.Name;
    }

    public void DisplayObjectOptions()
    {
        if(!objectUISelected.activeSelf)
        {
            objectUISelected.SetActive(true);
        }
        else
        {
            objectUISelected.SetActive(false);
        }
    }
    
    public void UseObject()
    {
        if (ObjectContained is IngredientSo ingredient)
        {
            FindFirstObjectByType<InventorySystem>().RemoveItem(ingredient);
            FindFirstObjectByType<CraftingController>().IngredientReceived(ingredient);
        }
        if (ObjectContained is PotionSo potion)
        {
            FindFirstObjectByType<InventorySystem>().RemoveItem(potion);
            FindFirstObjectByType<CombatManager>().PotionUsed(potion);
        }
    }
    
    public void UpdateQuantity()
    {
        objectUIQuantity.GetComponent<TMPro.TextMeshProUGUI>().text = ObjectContained.Count.ToString();
    }
}
