using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Networking;
using Unity.Netcode;

public enum GameState{
    preDayStart,
    daySelling,
    dayDone,
    scrapNight,
    preSleepNight
}

public class GameManager : NetworkBehaviour{
    public GameState gameState;
    public int currentDay = 1;

    void Update(){
        if(IsHost){
            if(Input.GetKeyDown("p")){
                ProgressState();
            }
        }
    }

    public void ProgressState(){
        switch(gameState){
            case GameState.preDayStart:
                gameState = GameState.daySelling;
                break;
            case GameState.daySelling:
                gameState = GameState.dayDone;
                break;
            case GameState.dayDone:
                gameState = GameState.scrapNight;
                break;
            case GameState.scrapNight:
                gameState = GameState.preSleepNight;
                break;
            case GameState.preSleepNight:
                gameState = GameState.preDayStart;
                currentDay++;
                break;
        }
        
        SyncStateClientRpc(gameState, currentDay);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestStateSyncServerRpc(){
        SyncStateClientRpc(gameState, currentDay);
    }

    [ClientRpc]
    public void SyncStateClientRpc(GameState state, int day){
        gameState = state;
        currentDay = day;
        UpdateUI();
    }

    public void UpdateUI(){
        PlayerBase.localPlayer.dayText.text = $"DAY {currentDay}";
    }
}