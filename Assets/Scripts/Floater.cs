using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.99f;

    private void FixedUpdate()
    {
        rigidbody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        float waveHeight = WaterManager.instance.GetWaveHeight(transform.position.x, transform.position.z);

        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) *
                                           displacementAmount;

            // rigidbody.drag = 1f;
            rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f),
                transform.position, ForceMode.Acceleration);
            rigidbody.AddForce(displacementMultiplier * -rigidbody.velocity * waterDrag * Time.fixedDeltaTime,
                ForceMode.VelocityChange);
            rigidbody.AddTorque(
                displacementMultiplier * -rigidbody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime,
                ForceMode.VelocityChange);
        }
        // else if (rigidbody.position.y < waveHeight + 1f)
        // {
        //     rigidbody.drag = 0.5f;
        // }
        // else
        // {
        //     rigidbody.drag = 0f;
        // }
    }
}