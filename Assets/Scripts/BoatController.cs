using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class BoatController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private bool isStarted;
    private GameObject lightGameObject;
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 20f;
    public Camera mainCamera, camera1;
    public XRKnob knob;
    public XRJoystick joystick;
    public Transform panelTransform;
    public Transform standTransform;
    public GameObject controlPanel;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lightGameObject = controlPanel.transform.GetChild(8).gameObject;
    }

    void Update()
    {
        // var horizontal = Input.GetAxis("Horizontal");
        var horizontal = 0f;
        var vertical = 0f;
        if (isStarted)
        {
            horizontal = knob.value - 0.5f;
            vertical = joystick.value.x;
        }


        rigidbody.AddForce(transform.forward * vertical * verticalSpeed);
        rigidbody.AddTorque(transform.up * horizontal * horizontalSpeed);

        panelTransform.position = standTransform.position;
        panelTransform.rotation = standTransform.rotation;

        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     SwitchCamera();
        // }
    }

    public void ChangeMotorState()
    {
        isStarted = !isStarted;
        lightGameObject.SetActive(isStarted);
    }

    public void SwitchCamera()
    {
        mainCamera.enabled = !mainCamera.enabled;
        camera1.enabled = !mainCamera.enabled;
    }
}