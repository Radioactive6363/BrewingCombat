using UnityEngine;

public class BattleUI : MonoBehaviour
{
    // The animator for the leftSidePanel GameObject. Used to move inventory to the right/left depending on if you want to open/close it.
    public Animator leftSidePanelAnimator;

    private bool inventoryOn = false;

    private void Start()
    {
        if (leftSidePanelAnimator == null)
        {
            Debug.LogError("Couldn't get leftSidePanelAnimator!");
        }
    }

    public void CreatePotion()
    {

    }

    public void UsePotion()
    {

    }

    public void Attack()
    {

    }
    
    // Shows the CRAFT inventory + CRAFT UI
    public void ShowCraftingPanel()
    {
        ShowInventory();
    }

    // Shows the USE inventory
    public void ShowUsePanel()
    {
        ShowInventory();
    }

    private void ShowInventory()
    {
        // If inventory was on and we clicked to open a panel, we close it. The opposite happens if the inventory was off.
        if (inventoryOn)
        {
            inventoryOn = false;
            leftSidePanelAnimator.Play("MoveLeft");
        }
        else
        {
            inventoryOn = true;
            leftSidePanelAnimator.Play("MoveRight");
        }
    }
}
