using UnityEngine;

public class TrackPointed : MonoBehaviour{
    [SerializeField] Transform camera;
    [SerializeField] LayerMask ignore;

    void Update(){
        Quaternion cRot = this.transform.rotation;

        if(Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, 150, ~ignore)){
            this.transform.LookAt(hit.point);
        }else{
            this.transform.LookAt(camera.position + camera.forward * 150);
        }

        this.transform.rotation = Quaternion.Lerp(cRot, this.transform.rotation, 3 * Time.deltaTime);
    }
}