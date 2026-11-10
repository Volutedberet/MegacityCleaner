using UnityEngine;

public class OxygenArea : MonoBehaviour{
    public Vector3 size;
    public float recoveryRate;
    Bounds bounds;

    void Start(){
        bounds = new Bounds(this.transform.position, size/2);
    }

    public bool IsInArea(Vector3 checkPos){
        return bounds.Contains(checkPos);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(this.transform.position, size);
    }
}