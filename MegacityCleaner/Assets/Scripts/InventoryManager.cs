using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Networking;
using Unity.Netcode;

public class InventoryManager : NetworkBehaviour{
    public List<InventorySlot> slots = new List<InventorySlot>();

    public List<HeldItemData> heldItemDatas = new List<HeldItemData>();

    int selectedSlot = 0;

    void Start(){
        if(!IsOwner){
            return;
        }

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
                if(slotID == selectedSlot){
                    BeginHoldItemServerRpc(slot.item.itemHeldVisualID);                
                }else{
                    BeginHoldItemServerRpc(-1);       
                }
            }else{
                slot.slotDisplay.DisplayNoItem();
            }

            slotID++;
        }
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