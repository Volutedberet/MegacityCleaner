using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbop : MonoBehaviour{
    public float speed;
    [SerializeField] Transform handPos, handPosTop, handPosBottom;
    [SerializeField] AnimationCurve moveC;

    [Space(10)]
    [SerializeField] AudioSource stepAudio;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] Transform checkSphere;
    [SerializeField] float checkSphereRadius;
    float bopT;
    bool didPlay = false;

    void Update(){
        bopT += Mathf.Abs(Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
        if(Input.GetAxisRaw("Vertical") == 0){
            bopT += Mathf.Abs(Input.GetAxisRaw("Horizontal")) * speed * Time.deltaTime;
        }

        if(stepAudio != null){
            if(Mathf.PingPong(bopT, 1) > 0.95f){
                if(!didPlay){
                    if(Physics.CheckSphere(checkSphere.position, checkSphereRadius, groundLayerMask)){
                        didPlay = true;

                        if(Physics.Raycast(this.transform.position, -Vector3.up, out RaycastHit hit)){
                            if(hit.collider.gameObject.TryGetComponent<AudioSurface>(out AudioSurface surface)){
                                stepAudio.clip = surface.stepClip;
                            }
                        }

                        stepAudio.pitch = Random.Range(0.95f, 1.05f);
                        stepAudio.Play();                    
                    }
                }
            }else{
                didPlay = false;
            }            
        }
        
        handPos.position = Vector3.Lerp(handPos.position, Vector3.Lerp(handPosBottom.position, handPosTop.position, moveC.Evaluate(Mathf.PingPong(bopT, 1))), 10 * Time.deltaTime);
    }
}
