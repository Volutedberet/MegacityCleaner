using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotDisplay : MonoBehaviour{
    public GameObject selectionIndicator;
    public Animator inventoryAnim;

    [Space(10)]
    public RawImage itemDisplay;
    public TextMeshProUGUI slotIDText;
    public TextMeshProUGUI itemNameText;

    public void DisplayItem(ItemData item){
        inventoryAnim.Play("Base Layer.SlotEquipItem");
        itemNameText.text = item.itemName;
        itemDisplay.texture = item.itemIcon;
    }

    public void DisplayNoItem(){
        inventoryAnim.Play("Base Layer.SlotUnequipItem");
    }
}