using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ItemDatabase", order = 0)]
public class ItemDatabase : ScriptableObject {
    public List<ItemData> items = new List<ItemData>();

    public void SetupItemIDs(){
        int current = 0;
        foreach(var item in items){
            item.itemID = current;
            current++;
        }
    }
}