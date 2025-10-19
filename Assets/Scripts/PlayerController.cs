using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputAction _movement;
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float orientationSpeed = 360f;

    Camera mainCamera;
    CharacterController characterController;

    private void Awake(){
        mainCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        
    }

    private void OnEnable(){
        _movement.Enable();
    }

    private void OnDisable(){
        _movement.Disable();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementValue = _movement.ReadValue<Vector2>();     //vector2 tiene X e Y, por lo que hacer esto solo hace que se mueva en la altura (Y)
        Vector3 movementValueXZ = new Vector3(movementValue.x, 0f, movementValue.y);   //transformamos para tener el Y en el Z
        
        float oldMagnitudeXZ = movementValueXZ.magnitude;
        movementValueXZ = mainCamera.transform.TransformDirection(movementValueXZ);     //para cambiar la dirección según la cámara
        movementValueXZ = Vector3.ProjectOnPlane(movementValueXZ, Vector3.up);
        movementValueXZ = movementValueXZ.normalized * oldMagnitudeXZ;


        Vector3 velocity = movementValueXZ * movementSpeed;

        //transform.position += velocity * Time.deltaTime;      //esto está mal porque hace que no choque con los colliders
        characterController.Move(velocity * Time.deltaTime);


        //ahora para girar el modelo según donde mire la cámara o donde se mueva
        if(movementValue.magnitude > 0.01f){     //solo nos giramos si nos movemos

            
            //Vector3 desiredForward = mainCamera.transform.forward;      //esto para que mire donde la cámara
            Vector3 desiredForward = movementValueXZ;                  //para que mire donde se mueve, mucho más cómodo

            desiredForward = Vector3.ProjectOnPlane(desiredForward, Vector3.up).normalized;
            Vector3 currentForward = transform.forward;
            float angleDifference = Vector3.SignedAngle(currentForward, desiredForward, Vector3.up);
            float angleToApply = Mathf.Min(Mathf.Abs(angleDifference), orientationSpeed * Time.deltaTime);
            angleToApply *= Mathf.Sign(angleDifference);

            Quaternion rotationToApply = Quaternion.AngleAxis(angleToApply, Vector3.up);
            transform.rotation = rotationToApply * transform.rotation;          //Con Quaternions el orden es importante y tiene que ir primero
        }

    }
}
