using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour{
    public bool isActive = true;

    public float smoothing;
    public float multiply;
    
    void Update(){
        float mouseX = Input.GetAxisRaw("Mouse X") * multiply;
        float mouseY = Input.GetAxisRaw("Mouse Y") * multiply;

        if(!isActive){
            mouseX = 0;
            mouseY = 0;
        }

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
 
        Quaternion target = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, smoothing * Time.deltaTime);
    }
}
