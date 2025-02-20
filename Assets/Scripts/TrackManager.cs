using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> checkpoints;
    [SerializeField] private int currentCheckpoint;

    private void Start()
    {
        currentCheckpoint = 0;
        for (int i=0;i < checkpoints.Count;i++)
        {
            checkpoints[i].Init(this, i);
        }
    }

    public void checkCheckpoint(int id)
    {
        if (id != currentCheckpoint + 1) return;
        currentCheckpoint += 1;
    }
}
