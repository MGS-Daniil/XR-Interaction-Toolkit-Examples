using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class BoatController : MonoBehaviour
{
    private Rigidbody rigidbody;
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 20f;
    public Camera mainCamera, camera1;
    public XRKnob knob;
    public XRJoystick joystick;
    public Transform panelTransform;
    public Transform standTransform;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // var horizontal = Input.GetAxis("Horizontal");
        var horizontal = knob.value-0.5f;
        var vertical = joystick.value.x;

        rigidbody.AddForce(transform.forward * vertical * verticalSpeed);
        rigidbody.AddTorque(transform.up * horizontal * horizontalSpeed);
        
        panelTransform.position = standTransform.position;
        panelTransform.rotation = standTransform.rotation;

        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     SwitchCamera();
        // }
    }

    public void SwitchCamera()
    {
        mainCamera.enabled = !mainCamera.enabled;
        camera1.enabled = !mainCamera.enabled;
    }
}