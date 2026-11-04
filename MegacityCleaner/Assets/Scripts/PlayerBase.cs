using UnityEngine;

public class PlayerBase : MonoBehaviour{
    public static PlayerBase instance;
    
    public GameObject player;

    void Awake(){
        if(instance == null){
            instance = this;
            return;
        }
        Destroy(this);
    }

    public void TogglePlayer(bool state){
        player.SetActive(state);
    }
}