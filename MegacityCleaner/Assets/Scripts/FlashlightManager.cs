using UnityEngine;

public class FlashlightManager : MonoBehaviour{
    [SerializeField] Animator anim;
    bool isActive;

    void Update(){
        if(Input.GetKeyDown("l")){
            isActive = !isActive;

            if(isActive){
                anim.Play("Base Layer.FlashlightEquip");
            }else{
                anim.Play("Base Layer.FlashlightUnequip");
            }
        }
    }
}