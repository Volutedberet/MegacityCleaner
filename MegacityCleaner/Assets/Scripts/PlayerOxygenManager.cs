using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerOxygenManager : NetworkBehaviour{
    public float oxygenCap;
    public float oxygenCurrent;
    public float drainRate;
    public float recoveryRate;
    public Image oxygenBar;

    [Space(10)]
    [SerializeField] GameObject visorUIHud;
    [SerializeField] GameObject visorWorldModel;
    [SerializeField] GameObject oxyBarUI;

    bool isHelmetOn;

    void Update(){
        if(!IsOwner){
            return;
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            isHelmetOn = !isHelmetOn;

            visorUIHud.SetActive(isHelmetOn);
            oxyBarUI.SetActive(isHelmetOn);
            SetHelmetServerRpc(isHelmetOn);
        }

        OxygenArea area = TryGetOxygen();

        if(area != null){
            oxygenCurrent += area.recoveryRate * Time.deltaTime;    
        }else{
            if(isHelmetOn){
                oxygenCurrent -= drainRate * Time.deltaTime;  
            }else{
                //player dying
            }      
        }
        
        oxygenBar.fillAmount = oxygenCurrent/oxygenCap;      
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetHelmetServerRpc(bool isOn){
        SetHelmetClientRpc(isOn);
    }

    [ClientRpc]
    public void SetHelmetClientRpc(bool isOn){
        visorWorldModel.SetActive(isOn);
    }

    OxygenArea TryGetOxygen(){
        OxygenArea[] areas = FindObjectsOfType<OxygenArea>();

        foreach(var area in areas){
            if(area.IsInArea(this.transform.position)){
                return area;
            }
        }

        return null;
    }
}