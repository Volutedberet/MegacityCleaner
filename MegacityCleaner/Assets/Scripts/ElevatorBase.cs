using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Networking;
using Unity.Netcode;

public class ElevatorBase : NetworkBehaviour{
    public Interactable interact;
    public float speed;

    bool isDown;

    void Update(){
        switch(GameManager.instance.gameState){
            case GameState.preDayStart:
                interact.action = "It's Too Early For This";
                break;
            case GameState.daySelling:
                interact.action = "It's Too Early For This";
                break;
            case GameState.dayDone:
                interact.action = "It's Almost Time";
                break;
            case GameState.scrapNight:
                if(AllPlayersIn()){
                    interact.action = "Start Descent";                
                }else{
                    interact.action = "Waiting for Everyone";         
                }

                break;
            case GameState.preSleepNight:
                interact.action = "It's Too Late For This";
                break;
        }

        if(IsHost){
            if(isDown){
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x, -94.58f, this.transform.position.z), speed * Time.deltaTime);
            }else{
                this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.position.x, 3.96f, this.transform.position.z), speed * Time.deltaTime);
            }
        }
    }

    public void TryStartDescent(){
        if(GameManager.instance.gameState == GameState.scrapNight){
            if(AllPlayersIn()){
                StartDescentServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartDescentServerRpc(){
        isDown = true;
    }

    public bool AllPlayersIn(){
        return true;
    }
}