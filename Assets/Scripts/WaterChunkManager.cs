using System;
using System.Collections.Generic;
using UnityEngine;

public class WaterChunkManager : MonoBehaviour
{
    public static WaterChunkManager instance;
    
    [Header("Manager Settings")]
    public GameObject waterChunkPrefab;
    public int viewDistance = 3; 
    
   
    [Header("Chunk Settings")]
    public int resolution = 10;
    public int chunkSize = 10;
    
    [Header("Noise Settings")]
    public float amplitude = 1f;
    public float frequency = 1f;
    public float speed = 1f;
    
    
    private Transform player;
    private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();
    private Queue<GameObject> chunkPool = new Queue<GameObject>();
    private List<WaterManager> waterChunks = new List<WaterManager>();
    

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        player = Camera.main.transform; // Камера как центр отслеживания
        UpdateChunks();
    }

    private void Update()
    {
        UpdateChunks();
    }
    
    public void RegisterChunk(WaterManager chunk)
    {
        if (!waterChunks.Contains(chunk))
        {
            waterChunks.Add(chunk);
            chunk.UpdateSettings(amplitude, frequency, speed, resolution);
        }
    }

    private void OnValidate()
    {
        UpdateAllChunks();
    }

    public void UpdateAllChunks()
    {
        foreach (var chunk in waterChunks)
        {
            chunk.UpdateSettings(amplitude, frequency, speed, resolution);
        }
    }

    private void UpdateChunks()
    {
        Vector2Int playerChunkPos = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.z / chunkSize)
        );

        HashSet<Vector2Int> neededChunks = new HashSet<Vector2Int>();

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                Vector2Int chunkPos = new Vector2Int(playerChunkPos.x + x, playerChunkPos.y + z);
                neededChunks.Add(chunkPos);

                if (!activeChunks.ContainsKey(chunkPos))
                {
                    GameObject chunk = GetChunk();
                    chunk.transform.position = new Vector3(chunkPos.x * chunkSize, 0, chunkPos.y * chunkSize);
                    chunk.SetActive(true);
                    activeChunks[chunkPos] = chunk;
                }
            }
        }

        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in activeChunks)
        {
            if (!neededChunks.Contains(chunk.Key))
            {
                chunk.Value.SetActive(false);
                chunkPool.Enqueue(chunk.Value);
                chunksToRemove.Add(chunk.Key);
            }
        }

        foreach (var chunkPos in chunksToRemove)
        {
            activeChunks.Remove(chunkPos);
        }
    }

    private GameObject GetChunk()
    {
        if (chunkPool.Count > 0)
        {
            return chunkPool.Dequeue();
        }

        return Instantiate(waterChunkPrefab,transform);
    }
}
