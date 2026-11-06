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

    public EnviromentZone cityZone;

    public EnviromentData dataDayStart;
    public EnviromentData dataDayEnd;
    public EnviromentData dataNight;

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
        UpdateDayLighting();
    }

    public void UpdateUI(){
        PlayerBase.localPlayer.dayText.text = $"DAY {currentDay}";
    }

    public void UpdateDayLighting(){
        switch(gameState){
            case GameState.preDayStart:
                cityZone.visualData = dataDayStart;
                break;
            case GameState.daySelling:
                cityZone.visualData = dataDayStart;
                break;
            case GameState.dayDone:
                cityZone.visualData = dataDayEnd;
                break;
            case GameState.scrapNight:
                cityZone.visualData = dataNight;
                break;
            case GameState.preSleepNight:
                cityZone.visualData = dataNight;
                currentDay++;
                break;
        }
    }
}