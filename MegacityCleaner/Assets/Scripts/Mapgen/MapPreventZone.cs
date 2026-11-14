using UnityEngine;

public class MapPreventZone : MonoBehaviour{
    public Vector3 size;
    Bounds bounds;

    public bool IsInZone(Vector3 pos){
        bounds = new Bounds(this.transform.position, size/2);

        return bounds.Contains(pos);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, size);
    }
}