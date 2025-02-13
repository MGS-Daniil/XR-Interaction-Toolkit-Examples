using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class BoatController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float verticalSpeed = 20f;
    [SerializeField] private Camera mainCamera, camera1;
    [SerializeField] private XRKnob knob;
    [SerializeField] private XRJoystick joystick;
    [SerializeField] private Transform panelTransform;
    [SerializeField] private Transform standTransform;
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private bool isStarted;
    private Rigidbody rigidbody;
    private GameObject lightGameObject;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        lightGameObject = controlPanel.transform.GetChild(8).gameObject;
    }

    void Update()
    {
        //panelTransform.position = standTransform.position;
        //panelTransform.rotation = standTransform.rotation;
        
        if(!isStarted) return;
        // var horizontal = Input.GetAxis("Horizontal");
        var vertical = -joystick.value.y;
        var horizontal = (knob.value - 0.5f) * Mathf.Abs(vertical);

        if (vertical > 0.1f)
        {
            audioSource.pitch = vertical * 2.0f;
        }
        
        rigidbody.AddForce(transform.forward * vertical * verticalSpeed);
        rigidbody.AddTorque(transform.up * horizontal * horizontalSpeed);

        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     SwitchCamera();
        // }
    }

    public void ChangeMotorState()
    {
        isStarted = !isStarted;
        lightGameObject.SetActive(isStarted);
        if (isStarted)
        {
            audioSource.Play();
            audioSource.pitch = 1f;
        }
        else
        {
           audioSource.Stop(); 
        }
    }

    public void SwitchCamera()
    {
        mainCamera.enabled = !mainCamera.enabled;
        camera1.enabled = !mainCamera.enabled;
    }
}