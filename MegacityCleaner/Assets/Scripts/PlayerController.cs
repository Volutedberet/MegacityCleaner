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

    [Space(10)]
    [SerializeField] GameObject staminaMeter;
    [SerializeField] Image staminaBar;
    [SerializeField] float staminaRecovery;
    [SerializeField] float staminaLoss;
    float stamina = 1;
    
    bool isSprinting;
    Vector3 groundNormal;

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

            isSprinting = Input.GetKey(KeyCode.LeftShift);

            transform.localRotation = Quaternion.Euler(0, yaw, 0);
            mainCamera.localRotation = Quaternion.Euler(pitch, 0, Mathf.Lerp(-headSideTiltMax, headSideTiltMax, headTiltCurrent));      

            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Vertical")) == 0){
                col.material = matStatic;
                stamina += staminaRecovery * Time.deltaTime;
            }else{
                col.material = matMove;                
                
                if(isSprinting){
                    stamina -= staminaLoss * Time.deltaTime;
                }else{
                    stamina += staminaRecovery * Time.deltaTime;
                }
            }

            staminaBar.fillAmount = stamina;
            stamina = Mathf.Clamp(stamina, 0, 1);
            staminaMeter.SetActive(stamina != 1);
        }
    }

    private void FixedUpdate(){
        if(!IsOwner || !canMove){
            return;
        } 

        bool isGrounded = IsGrounded();

        float mult = 1f;
        if(isSprinting && stamina > 0) mult = 2f;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool hasMovementInput = input.sqrMagnitude > 0.001f;
        if(input.sqrMagnitude > 1f) input.Normalize();

        Vector3 moveDirection = transform.TransformDirection(new Vector3(input.x, 0f, input.y));
        if(isGrounded){
            moveDirection = Vector3.ProjectOnPlane(moveDirection, groundNormal);
            if(moveDirection.sqrMagnitude > 0.001f) moveDirection.Normalize();
        }

        if(hasMovementInput) rigidBody.AddForce(moveDirection * moveForce, ForceMode.Acceleration);

        Vector3 planarVelocity = isGrounded ? Vector3.ProjectOnPlane(rigidBody.linearVelocity, groundNormal) : new Vector3(rigidBody.linearVelocity.x, 0f, rigidBody.linearVelocity.z);
        float currentMaxSpeed = maxMoveSpeed * mult;

        if(planarVelocity.magnitude > currentMaxSpeed){
            Vector3 limitedVelocity = planarVelocity.normalized * currentMaxSpeed;
            Vector3 perpendicularVelocity = Vector3.Project(rigidBody.linearVelocity, groundNormal);
            rigidBody.linearVelocity = limitedVelocity + perpendicularVelocity;
        }

        if(!hasMovementInput){
            if(isGrounded){
                Vector3 stoppingVelocity = Vector3.ProjectOnPlane(rigidBody.linearVelocity, groundNormal);
                rigidBody.AddForce(-stoppingVelocity * groundSlowdown, ForceMode.Acceleration);
            }else{
                Vector3 airVelocity = new Vector3(rigidBody.linearVelocity.x, 0f, rigidBody.linearVelocity.z);
                rigidBody.AddForce(-airVelocity * airSlowdown, ForceMode.Acceleration);
            }
        }

        rigidBody.AddForce(Physics.gravity * -gravity, ForceMode.Acceleration);
    }

    private bool IsGrounded(){
        if (Physics.Raycast(checkSphere.position, Vector3.down, out RaycastHit hit, checkSphereRadius + 0.1f, groundMask, QueryTriggerInteraction.Ignore)){
            groundNormal = hit.normal;
            return true;
        }else{
            groundNormal = Vector3.up;
            return true;
        }
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