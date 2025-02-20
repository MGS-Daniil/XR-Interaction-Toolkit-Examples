using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private int index;
    private TrackManager trackManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Transport")) return;

        trackManager.checkCheckpoint(index);
    }

    public void Init(TrackManager trackManager, int index)
    {
        this.trackManager = trackManager;
        this.index = index;
    }
}
