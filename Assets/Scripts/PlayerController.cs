using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputAction _movement;
    [SerializeField] private float movementSpeed = 4f;

    Camera mainCamera;

    private void Awake(){
        mainCamera = Camera.main;
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


        transform.position += velocity * Time.deltaTime;


    }
}
