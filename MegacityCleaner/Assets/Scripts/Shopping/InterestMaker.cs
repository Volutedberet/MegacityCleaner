using UnityEngine;

public class InterestMaker : MonoBehaviour{
    public float range;
    public float interestGatheredOnSeen;

    float updateT;

    void Update(){
        updateT += Time.deltaTime;
        if(updateT < 0.4f){
            return;
        }else{
            updateT = 0;
        }

        ShoppingNpcAI[] npcAI = FindObjectsOfType<ShoppingNpcAI>();
        foreach(var npc in npcAI){
            if(Vector3.Distance(npc.transform.position, this.transform.position) < range){
                npc.TryAddInterest(this);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(this.transform.position, range);
    }
}