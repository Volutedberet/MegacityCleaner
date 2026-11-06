using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Networking;
using Unity.Netcode;

public class GroundItem : NetworkBehaviour{
    public ItemData item;
    NetworkObject no;

    void Start(){
        no = this.gameObject.GetComponent<NetworkObject>();
    }

    public void PickMeUp(){
        PlayerBase.localPlayer.inventory.AddItem(item);
        RequestDestroyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestDestroyServerRpc(){
        no.Despawn(true);
    }
}