using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Networking;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour{
    public bool canMove;

    [Space(10)]
    [SerializeField] float sensitivity;
    [SerializeField] Transform mainCamera;
    [SerializeField] Rigidbody rigidBody;

    [Space(10)]
    [SerializeField] float headSideTiltMax;
    [SerializeField] float headSideTiltSpeed;

    [Space(10)]
    [SerializeField] Collider col;
    [SerializeField] PhysicsMaterial matMove;
    [SerializeField] PhysicsMaterial matStatic;
    
    [Space(10)]
    [SerializeField] float moveForce;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float groundSlowdown;
    [SerializeField] float airSlowdown;
    [SerializeField] float jumpForce;
    
    [SerializeField] Transform checkSphere;
    [SerializeField] float checkSphereRadius;
    [SerializeField] LayerMask groundMask;

    [SerializeField] float gravity;

    private float pitch;
    private float yaw;
    private float headTiltCurrent = 0.5f;

    private void Start() {
        if(!IsOwner){
            return;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update(){
        if(!IsOwner){
            return;
        }

        if(canMove){
            yaw += Input.GetAxisRaw("Mouse X") * sensitivity;
            pitch -= Input.GetAxisRaw("Mouse Y") * sensitivity;
            pitch = Mathf.Clamp(pitch, -80, 55);

            if(Input.GetAxisRaw("Horizontal") > 0){
                headTiltCurrent += headSideTiltSpeed * Time.deltaTime;
                if(headTiltCurrent > 1){
                    headTiltCurrent = 1;
                }
            }else if(Input.GetAxisRaw("Horizontal") < 0){
                headTiltCurrent -= headSideTiltSpeed * Time.deltaTime;
                if(headTiltCurrent < 0){
                    headTiltCurrent = 0;
                }
            }else{
                if(headTiltCurrent > 0.5f){
                    headTiltCurrent -= headSideTiltSpeed * Time.deltaTime;
                    if(headTiltCurrent < 0.5f){
                        headTiltCurrent = 0.5f;
                    }
                }else if(headTiltCurrent < 0.5f){
                    headTiltCurrent += headSideTiltSpeed * Time.deltaTime;
                    if(headTiltCurrent > 0.5f){
                        headTiltCurrent = 0.5f;
                    }
                }
            }

            transform.localRotation = Quaternion.Euler(0, yaw, 0);
            mainCamera.localRotation = Quaternion.Euler(pitch, 0, Mathf.Lerp(-headSideTiltMax, headSideTiltMax, headTiltCurrent));      

            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) == 0){
                col.material = matStatic;
            }else{
                col.material = matMove;
            }
        }
    }

    private void FixedUpdate() {
        if(!IsOwner){
            return;
        }

        if(!canMove){
            return;
        }        
        
        if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) == 0){
            if(Physics.CheckSphere(checkSphere.position, checkSphereRadius, groundMask)){
                Vector3 velM = new Vector3(rigidBody.linearVelocity.x, 0, rigidBody.linearVelocity.z);
                rigidBody.AddForce(-velM * groundSlowdown, ForceMode.Acceleration);
            }
        }else{
            Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movement = movement.normalized;
            rigidBody.AddRelativeForce(new Vector3(movement.x, 0, movement.y) * moveForce);            
        }

        Vector3 vel = new Vector3(rigidBody.linearVelocity.x, 0, rigidBody.linearVelocity.z);
        rigidBody.AddForce(-vel * (moveForce / maxMoveSpeed), ForceMode.Acceleration);

        rigidBody.AddForce(Physics.gravity.y * Vector3.up * -gravity, ForceMode.Acceleration);
    }

    [ServerRpc(RequireOwnership = false)]
    public void LaunchMeServerRpc(Vector3 dir){
        LaunchMeClientRpc(dir);
    }

    [ClientRpc]
    public void LaunchMeClientRpc(Vector3 dir){
        if(IsOwner){
            rigidBody.AddForce(dir);
        }
    }
}