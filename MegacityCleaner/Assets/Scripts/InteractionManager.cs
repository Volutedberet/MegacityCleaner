using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionManager : MonoBehaviour{
    public float interactionDistance;
    [SerializeField] Transform interactPos;
    [SerializeField] Camera cam;

    [Space(10)]
    [SerializeField] RectTransform interactHud;
    [SerializeField] TextMeshProUGUI actionText;
    [SerializeField] Vector3 offset;
    [SerializeField] LayerMask ignore;

    Interactable currentHighlighted;

    void Update(){
        if(Physics.Raycast(interactPos.position, interactPos.forward, out RaycastHit interactHit, interactionDistance, ~ignore)){
            if(interactHit.collider.gameObject.TryGetComponent<Interactable>(out Interactable inter)){
                if(inter != currentHighlighted){
                    if(currentHighlighted != null){
                        
                    }

                    currentHighlighted = inter;
                }

    
                Vector2 viewportPos = cam.WorldToViewportPoint(inter.transform.position);
                interactHud.anchoredPosition = new Vector2(1920 * viewportPos.x, 1080 * viewportPos.y);

                actionText.text = inter.action;
                interactHud.sizeDelta = inter.scale / Vector3.Distance(this.transform.position, inter.transform.position);
                
                interactHud.gameObject.SetActive(true);

                if(Input.GetKeyDown("e")){
                    currentHighlighted.onInteract.Invoke();
                }
            }else{
                if(currentHighlighted != null){
                    currentHighlighted = null;
                }
                interactHud.gameObject.SetActive(false);
            }
        }else{
            if(currentHighlighted != null){
                currentHighlighted = null;
            }
            interactHud.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(interactPos.position, interactPos.position + interactPos.forward * interactionDistance);
    }
}