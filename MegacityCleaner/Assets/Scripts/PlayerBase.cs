using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Networking;
using Unity.Netcode;

public class PlayerBase : NetworkBehaviour{
    public static PlayerBase localPlayer;
    
    public PlayerController controller;

    public GameObject playerTpsVisuals;
    public GameObject fpsCamera;
    public GameObject uiRenderers;
    public GameObject fog;

    public GameObject flashlight;

    public InventoryManager inventory;

    public TextMeshProUGUI dayText;
    public TextMeshProUGUI dayStateText;

    void Start(){
        if(!IsOwner){
            uiRenderers.SetActive(false);
            fpsCamera.SetActive(false);
            playerTpsVisuals.SetActive(true);
            fog.SetActive(false);
            return;
        }
    
        uiRenderers.SetActive(true);
        fpsCamera.SetActive(true);
        playerTpsVisuals.SetActive(false);
        fog.SetActive(true);

        if(localPlayer == null){
            localPlayer = this;
            return;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ToggleFlashlightServerRpc(bool state){
        ToggleFlashlightClientRpc(state);
    }

    [ClientRpc]
    public void ToggleFlashlightClientRpc(bool state){
        flashlight.SetActive(state);
    }
}