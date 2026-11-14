using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MapData", menuName = "MapData", order = 0)]
public class MapData : ScriptableObject {
    public string mapName;
    public int sceneID;

    public Vector2 enemyCountRange;
    public List<MapTile> mapTiles = new List<MapTile>();
    public Vector2 mapRoomCredits;
}

[System.Serializable]
public class MapTile{
    public GameObject tile;
    
    [Range(0, 100)]
    public float chance;
    public int additionalTokenCost;
}