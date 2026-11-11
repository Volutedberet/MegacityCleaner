using UnityEngine;

public enum HoldStyle{
    hovering,
    oneHanded,
    oneHandedLeft,
    twoHanded
}

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData", order = 0)]
public class ItemData : ScriptableObject {
    public string itemName;
    public Texture2D itemIcon;

    public int itemHeldVisualID;
    public GameObject groundItem;

    public HoldStyle holdStyle;

    [HideInInspector]
    public int itemID; 

    public bool canBeSold;
    public int basePrice;
}