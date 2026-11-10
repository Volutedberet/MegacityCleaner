using UnityEngine;

public class Ladder : MonoBehaviour{
    private void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 1, 0.2f);
        
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}