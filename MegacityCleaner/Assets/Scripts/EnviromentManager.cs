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

        if(selectedZone != null){
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, selectedZone.visualData.fogColor, transitionSpeed * Time.deltaTime);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, selectedZone.visualData.fogDensity, transitionSpeed * Time.deltaTime);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, selectedZone.visualData.lightColor, transitionSpeed * Time.deltaTime);
        }
    }
}
