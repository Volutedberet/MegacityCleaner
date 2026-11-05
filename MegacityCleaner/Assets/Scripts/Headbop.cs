using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbop : MonoBehaviour{
    public float speed;
    [SerializeField] float upPos;
    [SerializeField] float downPos;
    [SerializeField] AnimationCurve moveC;

    [Space(10)]
    [SerializeField] float groundRayLength;
    [SerializeField] AudioSource stepAudio;
    [SerializeField] LayerMask groundLayerMask;

    float bopT;
    bool didPlay = false;

    Vector3 upPosLocal;
    Vector3 downPosLocal;

    void Start(){
        upPosLocal = this.transform.localPosition + new Vector3(0, upPos, 0);
        downPosLocal = this.transform.localPosition + new Vector3(0, downPos, 0);
    }

    void Update(){
        bopT += Mathf.Abs(Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
        if(Input.GetAxisRaw("Vertical") == 0){
            bopT += Mathf.Abs(Input.GetAxisRaw("Horizontal")) * speed * Time.deltaTime;
        }

        if(stepAudio != null){
            if(Mathf.PingPong(bopT, 1) > 0.95f){
                if(!didPlay){
                    didPlay = true;

                    if(Physics.Raycast(this.transform.position, -Vector3.up, out RaycastHit hit, groundRayLength, groundLayerMask)){
                        if(hit.collider.gameObject.TryGetComponent<AudioSurface>(out AudioSurface surface)){
                            stepAudio.clip = surface.stepClip;
                        }
                    }

                    stepAudio.pitch = Random.Range(0.95f, 1.05f);
                    stepAudio.Play();                    
                }
            }else{
                didPlay = false;
            }            
        }
        
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.Lerp(upPosLocal, downPosLocal, moveC.Evaluate(Mathf.PingPong(bopT, 1))), 10 * Time.deltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(this.transform.position, this.transform.position - new Vector3(0, groundRayLength, 0));
    }
}
