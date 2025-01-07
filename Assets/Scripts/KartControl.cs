using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;

    [SerializeField] private Transform _playerPosition;
    [SerializeField] private GameObject XRRig;
    [SerializeField] private bool inVR;
    [SerializeField] private Transform center;
    [SerializeField] private KartConfig config;

    WheelControl[] wheels;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass = center.localPosition;

        wheels = GetComponentsInChildren<WheelControl>();
        LoadConfig();
    }

    // Update is called once per frame
    void Update()
    {
        float vInput, hInput;
        if (Input.GetKeyDown(KeyCode.L)) LoadConfig();

        if (inVR)
        {
            XRRig.transform.rotation = Quaternion.Euler(XRRig.transform.rotation.eulerAngles.x,
                _playerPosition.rotation.eulerAngles.y, XRRig.transform.rotation.eulerAngles.z);
            XRRig.transform.position = _playerPosition.position;
            vInput = Input.GetAxis("XRI_Left_Primary2DAxis_Vertical") * -1;
            hInput = Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal");
        }
        else
        {
            vInput = Input.GetAxis("Vertical");
            hInput = Input.GetAxis("Horizontal");
        }

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);

        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // …and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }

                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }

    private void LoadConfig()
    {
        foreach (var wheel in wheels)
        {
            WheelCollider wheelCollider = wheel.WheelCollider;
            wheelCollider.mass = config.mass;
            wheelCollider.wheelDampingRate = config.wheelDampingRate;
            wheelCollider.suspensionDistance = config.suspensionDistance;

            var jointSpring = new JointSpring();
            jointSpring.spring = config.suspensionSpring.spring;
            jointSpring.damper = config.suspensionSpring.damper;
            jointSpring.targetPosition = config.suspensionSpring.targetPosition;
            wheelCollider.suspensionSpring = jointSpring;

            var q = new WheelFrictionCurve();
            q.extremumSlip = config.forwardFriction.extremumSlip;
            q.extremumValue = config.forwardFriction.extremumValue;
            q.asymptoteSlip = config.forwardFriction.asymptoteSlip;
            q.asymptoteValue = config.forwardFriction.asymptoteValue;
            q.stiffness = config.forwardFriction.stiffness;
            wheelCollider.forwardFriction = q;

            q.extremumSlip = config.sidewaysFriction.extremumSlip;
            q.extremumValue = config.sidewaysFriction.extremumValue;
            q.asymptoteSlip = config.sidewaysFriction.asymptoteSlip;
            q.asymptoteValue = config.sidewaysFriction.asymptoteValue;
            q.stiffness = config.sidewaysFriction.stiffness;
            wheelCollider.sidewaysFriction = q;
        }
    }
}