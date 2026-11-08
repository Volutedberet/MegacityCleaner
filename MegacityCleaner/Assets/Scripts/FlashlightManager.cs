using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class FlashlightManager : NetworkBehaviour{
    [SerializeField] Animator anim;
    bool isActive;

    void Update(){
        if(!IsOwner){
            return;
        }

        if(Input.GetKeyDown("l")){
            isActive = !isActive;
            PlayerBase.localPlayer.ToggleFlashlightServerRpc(isActive);

            if(isActive){
                anim.Play("Base Layer.FlashlightEquip");
            }else{
                anim.Play("Base Layer.FlashlightUnequip");
            }
        }
    }
}