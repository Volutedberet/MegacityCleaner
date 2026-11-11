using UnityEngine;

public class InterestMaker : MonoBehaviour{
    public float range;
    public float interestGatheredOnSeen;

    void Update(){
        
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(this.transform.position, range);
    }
}