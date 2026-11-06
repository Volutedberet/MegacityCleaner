using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Networking;
using Unity.Netcode;

public class InventoryManager : NetworkBehaviour{
    public ItemDatabase itemDatabase;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<HeldItemData> heldItemDatas = new List<HeldItemData>();

    [Space(10)]
    [SerializeField] Transform dropPos;

    int selectedSlot = 0;

    void Start(){
        if(!IsOwner){
            return;
        }
        itemDatabase.SetupItemIDs();
        UpdateSlots();
    }

    void Update(){
        if(!IsOwner){
            return;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            selectedSlot = 0;
            UpdateSlots();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            selectedSlot = 1;
            UpdateSlots();
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            selectedSlot = 2;
            UpdateSlots();
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            selectedSlot = 3;
            UpdateSlots();
        }
        if(Input.GetKeyDown(KeyCode.Alpha5)){
            selectedSlot = 4;
            UpdateSlots();
        }
        if(Input.GetKeyDown(KeyCode.Alpha6)){
            selectedSlot = 5;
            UpdateSlots();
        }
        if(Input.GetKeyDown(KeyCode.Alpha7)){
            selectedSlot = 6;
            UpdateSlots();
        }
        if(Input.GetKeyDown(KeyCode.Alpha8)){
            selectedSlot = 7;
            UpdateSlots();
        }
        if(Input.GetKeyDown(KeyCode.Alpha9)){
            selectedSlot = 8;
            UpdateSlots();
        }

        if(Input.GetKeyDown("q")){
            DropHeld();
        }
    }

    public void UpdateSlots(){        
        foreach(var held in heldItemDatas){
            held.visualFps.SetActive(false);
            held.visualTps.SetActive(false);
        }    

        int slotID = 0;
        foreach(var slot in slots){
            slot.slotDisplay.slotIDText.text = (slotID + 1).ToString();
            slot.slotDisplay.selectionIndicator.SetActive(slotID == selectedSlot);

            if(slot.item != null){
                slot.slotDisplay.DisplayItem(slot.item);
            }else{
                slot.slotDisplay.DisplayNoItem();
            }

            slotID++;
        }

        if(slots[selectedSlot].item != null){
            BeginHoldItemServerRpc(slots[selectedSlot].item.itemHeldVisualID);            
        }else{
            BeginHoldItemServerRpc(-1);     
        }
    }

    public void DropHeld(){
        if(slots[selectedSlot].item != null){
            SpawnItemGroundServerRpc(slots[selectedSlot].item.itemID);
            slots[selectedSlot].item = null;
        }
        UpdateSlots();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnItemGroundServerRpc(int itemID){
        GameObject temp = Instantiate(itemDatabase.items[itemID].groundItem, dropPos.position, dropPos.rotation);
        temp.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void BeginHoldItemServerRpc(int id){
        BeginHoldItemClientRpc(id);
    }

    [ClientRpc]
    public void BeginHoldItemClientRpc(int id){
        if(id == -1){
            foreach(var held in heldItemDatas){
                held.visualFps.SetActive(false);
                held.visualTps.SetActive(false);
            }
        }else{
            heldItemDatas[id].visualFps.SetActive(true);
            heldItemDatas[id].visualTps.SetActive(true);             
        }
    }

    public void AddItem(ItemData item){
        if(slots[selectedSlot].item == null){
            slots[selectedSlot].item = item;
            UpdateSlots();
            return;
        }

        foreach(var slot in slots){
            if(slot.item == null){
                slot.item = item;
                UpdateSlots();
                return;
            }
        }
    }
}

[System.Serializable]
public class InventorySlot{
    public InventorySlotDisplay slotDisplay;
    public ItemData item;
}

[System.Serializable]
public class HeldItemData{
    public GameObject visualFps;
    public GameObject visualTps;
}