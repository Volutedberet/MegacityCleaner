using UnityEngine;

public class EnviromentZone : MonoBehaviour{
    public string enviromentName;
    
    public Bounds bounds;
    public EnviromentData visualData;

    public int priority;

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(bounds.center, bounds.extents * 2);
    }
}