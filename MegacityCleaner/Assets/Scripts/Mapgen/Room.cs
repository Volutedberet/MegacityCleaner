using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour{
    public Transform roomCenter;
    public Vector3 roomSize;

    public List<GameObject> roomGeoBase = new List<GameObject>();

    public bool IsGeoMine(GameObject geo){
        foreach(var g in roomGeoBase){
            if(g == geo){
                return true;
            }
        }
        
        return false;
    }

    private void OnDrawGizmos() {
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawWireCube(roomCenter.localPosition, roomSize);
    }
}