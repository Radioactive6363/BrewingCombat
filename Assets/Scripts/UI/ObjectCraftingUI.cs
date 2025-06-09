using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectCraftingUI : MonoBehaviour
{
    private IngredientSo _ingredientContained;
    [SerializeField] private GameObject objectUISprite;
    [SerializeField] private GameObject objectUIName;

    public void GetObjectData(IngredientSo ingredientReceived)
    {
        _ingredientContained = ingredientReceived;
        objectUISprite.GetComponent<Image>().sprite = _ingredientContained.Sprite;
        objectUIName.GetComponent<TMPro.TextMeshProUGUI>().text = _ingredientContained.Name;
    }
}
