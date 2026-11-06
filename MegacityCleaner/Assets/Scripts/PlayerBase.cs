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
    
    public GameObject playerTpsVisuals;
    public GameObject fpsCamera;
    public GameObject uiRenderers;
    public GameObject fog;

    public InventoryManager inventory;

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
}