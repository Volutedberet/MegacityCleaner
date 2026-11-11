using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MapManager : NetworkBehaviour{
    public static MapManager instance;
    public List<MapData> mapDatas = new List<MapData>();
    
    int currentlyLoaded = 0;

    void Awake(){
        if(instance == null){
            instance = this;
            return;
        }
        Destroy(this);
    }

    [ContextMenu("Debug Load")]
    public void DebugMap(){
        LoadMapServerRpc(0);
    }

    public void PopulateMapLoot(){

    }

    public void UnloadMap(){
        if(currentlyLoaded != 0){

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoadMapServerRpc(int mapID){
        LoadMapClientRpc(mapID);
    }

    [ClientRpc]
    public void LoadMapClientRpc(int mapID){
        Debug.Log($"Requested Load of Map {mapID}");
        SceneManager.LoadScene(mapDatas[mapID].sceneID, LoadSceneMode.Additive);
        currentlyLoaded = mapID;
    }
}