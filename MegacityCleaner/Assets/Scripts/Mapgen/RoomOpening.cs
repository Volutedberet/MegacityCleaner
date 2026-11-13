using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomOpening : MonoBehaviour{
    public bool didGen;
    public GameObject wallNoConnect;

    private void OnDrawGizmos(){
        if(didGen){
            Gizmos.color = new Color(1, 0, 0, 0.2f);
        }else{
            Gizmos.color = new Color(1, 1, 0, 0.5f);
        }
        
        Gizmos.DrawWireCube(this.transform.position + (this.transform.forward * 5), new Vector3(10, 10, 10));
    }
}