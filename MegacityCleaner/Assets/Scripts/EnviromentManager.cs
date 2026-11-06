using UnityEngine;

public class EnviromentManager : MonoBehaviour{
    [SerializeField] Transform player;
    [SerializeField] float transitionSpeed;
    [SerializeField] Camera cam;

    void Update(){
        EnviromentZone selectedZone = null;
        EnviromentZone[] zones = FindObjectsOfType<EnviromentZone>();

        foreach(var zone in zones){
            if(zone.bounds.Contains(player.position)){
                if(selectedZone == null){
                    selectedZone = zone;
                }else{
                    if(zone.priority > selectedZone.priority){
                        selectedZone = zone;
                    }
                }
            }
        }
    }
}
