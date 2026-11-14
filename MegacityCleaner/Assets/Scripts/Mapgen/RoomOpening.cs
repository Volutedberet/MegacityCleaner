using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomOpening : MonoBehaviour{
    public bool didGen;
    public GameObject wallNoConnect;
    public GameObject doorframe;

    public void CheckDoor(){
        if(Physics.Raycast((this.transform.position + this.transform.forward * 0.2f), -Vector3.up, out RaycastHit hit, 5)){
            if(hit.collider.gameObject.tag == "Doormat"){
                if(wallNoConnect != null){
                    wallNoConnect.SetActive(false);         
                    doorframe.SetActive(true);       
                }
            }
        }
    }

    private void OnDrawGizmos(){
        if(didGen){
            Gizmos.color = new Color(1, 0, 0, 0.2f);
        }else{
            Gizmos.color = new Color(1, 1, 0, 0.5f);
        }
        
        Gizmos.DrawWireCube(this.transform.position + (this.transform.forward * 5), new Vector3(10, 10, 10));
        Gizmos.DrawLine(this.transform.position + this.transform.forward * 0.2f, this.transform.position + this.transform.forward * 0.2f - (Vector3.up * 10));
    }
}