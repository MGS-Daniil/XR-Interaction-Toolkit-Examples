using UnityEngine;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidbody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.99f;

    private WaterManager currentWaterChunk;

    private void FixedUpdate()
    {
        rigidbody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        if (currentWaterChunk == null || !currentWaterChunk.gameObject.activeSelf)
        {
            currentWaterChunk = FindNearestWaterChunk();
            if (currentWaterChunk == null) return;
        }

        float waveHeight = currentWaterChunk.GetWaveHeight(transform.position.x, transform.position.z);

        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) *
                                           displacementAmount;

            rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f),
                transform.position, ForceMode.Acceleration);
            rigidbody.AddForce(displacementMultiplier * -rigidbody.velocity * waterDrag * Time.fixedDeltaTime,
                ForceMode.VelocityChange);
            rigidbody.AddTorque(
                displacementMultiplier * -rigidbody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime,
                ForceMode.VelocityChange);
        }
    }

    private WaterManager FindNearestWaterChunk()
    {
        WaterManager[] waterChunks = FindObjectsOfType<WaterManager>();
        WaterManager nearestChunk = null;
        float minDistance = float.MaxValue;

        foreach (var chunk in waterChunks)
        {
            float distance = Vector3.Distance(transform.position, chunk.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestChunk = chunk;
            }
        }

        return nearestChunk;
    }
}
