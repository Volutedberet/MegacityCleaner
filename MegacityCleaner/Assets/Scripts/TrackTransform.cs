using UnityEngine;

public class TrackTransform : MonoBehaviour{
    public Transform target;
    public bool isLocal;
    public Vector3 offset;

    void Update(){
        if(isLocal){
            target.localRotation = this.transform.localRotation;              
        }else{
            target.rotation = this.transform.rotation;        
            target.Rotate(offset);
        }
    }
}