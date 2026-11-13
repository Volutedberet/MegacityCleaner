using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MapData", menuName = "MapData", order = 0)]
public class MapData : ScriptableObject {
    public string mapName;
    public int sceneID;

    public Vector2 enemyCountRange;
    public List<GameObject> mapTiles = new List<GameObject>();
    public Vector2 mapRoomCredits;
}