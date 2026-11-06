using UnityEngine;

public class TrackTransform : MonoBehaviour{
    public Transform target;
    public bool isLocal;
    public bool xAsZ;

    void Update(){
        if(isLocal){
            if(xAsZ){
                target.localRotation = Quaternion.Euler(this.transform.localEulerAngles.z, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z);     
            }else{
                target.localRotation = this.transform.localRotation;              
            }
        }else{
            target.rotation = this.transform.rotation;        
        }
    }
}