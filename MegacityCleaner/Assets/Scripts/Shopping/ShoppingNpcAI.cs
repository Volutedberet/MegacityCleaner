using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using UnityEngine.AI;
using TMPro;

public enum ShopNpcState{
    commute,
    idle,
    interested,
    buying,
    bought
}

public class ShoppingNpcAI : NetworkBehaviour{
    public ShopNpcState state;
    public Vector2 idleTimeRange;
    public float interestInBuying;

    [SerializeField] TextMeshProUGUI debugState;
    [SerializeField] TextMeshProUGUI debugInterest;

    NavMeshAgent agent;
    int pathProgress;

    Vector3 targetPos;
    float idleTime;

    void Start(){
        if(!IsOwner){
            return;
        }

        agent = this.gameObject.GetComponent<NavMeshAgent>();

        targetPos = GameManager.instance.npcPath[0].position + new Vector3(Random.Range(-GameManager.instance.npcWanderRange, GameManager.instance.npcWanderRange), 0, Random.Range(-GameManager.instance.npcWanderRange, GameManager.instance.npcWanderRange));
    }

    void Update(){
        if(!IsOwner){
            debugState.text = "STATE HANDLED BY HOST";
            debugInterest.text = $"Interest in Buying: {interestInBuying.ToString("F1")}";
            return;
        }

        debugState.text = $"State: {state.ToString()}";
        debugInterest.text = $"Interest in Buying: {interestInBuying.ToString("F1")}";

        switch(state){
            case ShopNpcState.commute:
                if(Vector3.Distance(this.transform.position, targetPos) < 1.5f){
                    idleTime = Random.Range(idleTimeRange.x, idleTimeRange.y);
                    state = ShopNpcState.idle;
                    agent.destination = this.transform.position;
                    pathProgress++;

                    if(pathProgress >= GameManager.instance.npcPath.Count - 1){
                        this.gameObject.GetComponent<NetworkObject>().Despawn(true);
                    }
                }else{
                    agent.destination = targetPos;
                }
                break;
            case ShopNpcState.idle:
                idleTime -= Time.deltaTime;
                if(idleTime <= 0){
                    state = ShopNpcState.commute;
                    targetPos = GameManager.instance.npcPath[pathProgress].position + new Vector3(Random.Range(-GameManager.instance.npcWanderRange, GameManager.instance.npcWanderRange), 0, Random.Range(-GameManager.instance.npcWanderRange, GameManager.instance.npcWanderRange));
                }
                break;
        }
    }
}