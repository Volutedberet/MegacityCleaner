using UnityEngine;

public class PlayerController : MonoBehaviour{
    public bool canMove;

    [Space(10)]
    [SerializeField] float movementSpeed;
    [SerializeField] float sensitivity;
    [SerializeField] Transform mainCamera;
    [SerializeField] CharacterController cc;

    [Space(10)]
    [SerializeField] float headSideTiltMax;
    [SerializeField] float headSideTiltSpeed;
    
    private float pitch;
    private float yaw;
    private float headTiltCurrent = 0.5f;

    public static PlayerController instance;
    
    void Awake(){
        if(instance == null){
            instance = this;
            return;
        }
        Destroy(this);
    }

    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update(){
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

            Vector2 input = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            input.Normalize();
            Vector3 movementDir = (this.transform.forward * input.x) + (this.transform.right * input.y);
            //movementDir = movementDir.normalized;

            cc.Move((movementDir * movementSpeed * Time.deltaTime) + -Vector3.up * 9.7f * Time.deltaTime);            
        }
    }

    public void UpdatePos(Transform pos){
        cc.enabled = false;
        this.transform.position = pos.position;
        yaw = pos.rotation.y;
        pitch = 0;
        cc.enabled = true;
    }
}