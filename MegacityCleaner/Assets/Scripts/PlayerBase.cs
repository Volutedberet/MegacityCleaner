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

    void Start(){
        if(!IsOwner){
            uiRenderers.SetActive(false);
            fpsCamera.SetActive(false);
            playerTpsVisuals.SetActive(true);
            return;
        }

    
        uiRenderers.SetActive(true);
        fpsCamera.SetActive(true);
        playerTpsVisuals.SetActive(false);

        if(localPlayer == null){
            localPlayer = this;
            return;
        }
    }
}