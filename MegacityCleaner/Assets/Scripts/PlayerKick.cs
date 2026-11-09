using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class PlayerKick : NetworkBehaviour{
    [SerializeField] Animator anim;
    [SerializeField] float kickForce;
    [SerializeField] float kickForceUp;
    [SerializeField] LayerMask ignore;

    float cd;

    void Update(){
        if(!IsOwner){
            return;
        }

        cd -= Time.deltaTime;
        if(cd > 0){
            return;
        }

        if(Input.GetKeyDown("v")){
            cd = 1;
            anim.Play("Base Layer.LegKick");
            Invoke("KickPlayerTry", 0.15f);
        }
    }

    public void KickPlayerTry(){
        if(Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, 3, ~ignore)){
            if(hit.collider.gameObject.TryGetComponent<PlayerController>(out PlayerController pc)){
                if(pc != PlayerBase.localPlayer.controller){
                    pc.LaunchMeServerRpc(this.transform.forward * kickForce + Vector3.up * kickForceUp);
                }
            }
        }   
    }
}