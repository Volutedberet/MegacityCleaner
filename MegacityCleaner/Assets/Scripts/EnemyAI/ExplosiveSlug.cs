using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class ExplosiveSlug : NetworkBehaviour{
    [SerializeField] float moveChance;
    [SerializeField] Animator anim;
    [SerializeField] float speed;

    bool isMoving;
    float moveTime;

    void Update(){
        if(!IsOwner){
            return;
        }

        if(isMoving){
            moveTime -= Time.deltaTime;
            if(moveTime < 0){
                isMoving = false;
            }

            this.transform.position += this.transform.forward * speed * Time.deltaTime;

            return;
        }

        moveTime += Time.deltaTime;
        if(moveTime > 1){
            if(Random.Range(0, 100) < moveChance){
                isMoving = true;
                moveTime = 1.25f;
                anim.Play("Base Layer.SlugMove");
            }else{
                moveTime = 0;
            }
        }
    }
}