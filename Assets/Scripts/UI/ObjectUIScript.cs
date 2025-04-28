using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUIScript : MonoBehaviour
{
    public IObject objectContained;
    [SerializeField] private GameObject objectUISprite;
    [SerializeField] private GameObject objectUIQuantity;
    [SerializeField] private GameObject objectUISelected;
    [SerializeField] private GameObject objectUIName;
    
    void Start()
    {
        objectUISprite.GetComponent<Image>().sprite = objectContained.Sprite;
        objectUIQuantity.GetComponent<TMPro.TextMeshProUGUI>().text = objectContained.Count.ToString();
        objectUIName.GetComponent<TMPro.TextMeshProUGUI>().text = objectContained.Name;
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
        FindFirstObjectByType<InventorySystem>().AddItem(objectContained);
    }
    
    public void RemoveObject()
    {
        FindFirstObjectByType<InventorySystem>().RemoveItem(objectContained);
    }
    
    public void UpdateQuantity()
    {
        objectUIQuantity.GetComponent<TMPro.TextMeshProUGUI>().text = objectContained.Count.ToString();
    }
}
