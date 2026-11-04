using UnityEngine;

public class RotateOvertime : MonoBehaviour{
    public Vector3 speed;
    public bool unscaled;

    void Update(){
        if(unscaled){
            this.transform.Rotate(speed * Time.unscaledDeltaTime);
        }else{
            this.transform.Rotate(speed * Time.deltaTime);
        }
    }
}