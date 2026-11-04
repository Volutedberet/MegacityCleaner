using UnityEngine;

public class MoveWobble : MonoBehaviour{
    [SerializeField] float moveSpeed;
    [SerializeField] float resetSpeed;
    [SerializeField] float swingAmount;
    Vector3 currentTarget;
    Vector3 curRot;

    void Update(){
        currentTarget = Vector3.Lerp(currentTarget, Vector3.zero, resetSpeed * Time.deltaTime);
        curRot = Vector3.Lerp(curRot, currentTarget, resetSpeed * Time.deltaTime);
        this.transform.localEulerAngles = curRot;

        if(Input.GetKeyDown("w")){
            currentTarget = new Vector3((swingAmount), 0, currentTarget.z);
        }
        if(Input.GetKeyDown("s")){
            currentTarget = new Vector3(-(swingAmount), 0, currentTarget.z);
        }
        if(Input.GetKeyDown("a")){
            currentTarget = new Vector3(currentTarget.x, 0, -(swingAmount));
        }
        if(Input.GetKeyDown("d")){
            currentTarget = new Vector3(currentTarget.x, 0, (swingAmount));
        }
    }
}