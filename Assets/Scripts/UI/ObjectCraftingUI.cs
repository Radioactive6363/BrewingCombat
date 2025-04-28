using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectCraftingUI : MonoBehaviour
{
    private IngredientSO ingredientContained;
    [SerializeField] private GameObject objectUISprite;
    [SerializeField] private GameObject objectUIName;

    public void GetObjectData(IngredientSO ingredientReceived)
    {
        ingredientContained = ingredientReceived;
        objectUISprite.GetComponent<Image>().sprite = ingredientContained.Sprite;
        objectUIName.GetComponent<TMPro.TextMeshProUGUI>().text = ingredientContained.Name;
    }
}
