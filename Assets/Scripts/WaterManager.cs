using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private List<Vector3> vertices;
    private List<int> triangles;

    private float amplitude;
    private float frequency;
    private float speed;
    private int resolution;
    private float chunkSize;

    private bool isInitialized = false;

    private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();

        if (WaterChunkManager.instance != null)
        {
            WaterChunkManager.instance.RegisterChunk(this);
        }
    }

    public void UpdateSettings(float newAmplitude, float newFrequency, float newSpeed, int newResolution)
    {
        amplitude = newAmplitude;
        frequency = newFrequency;
        speed = newSpeed;
        resolution = newResolution;
        chunkSize = WaterChunkManager.instance.chunkSize;

        GenerateMesh();
        isInitialized = true;
    }

    private void Update()
    {
        if (isInitialized)
        {
            UpdateWave();
        }
    }

    private void GenerateMesh()
    {
        mesh = new Mesh();
        vertices = new List<Vector3>();
        triangles = new List<int>();

        float stepSize = chunkSize / resolution;

        for (int z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++)
            {
                vertices.Add(new Vector3(x * stepSize, 0, z * stepSize));
            }
        }

        for (int z = 0; z < resolution; z++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = z * (resolution + 1) + x;

                triangles.Add(i);
                triangles.Add(i + resolution + 1);
                triangles.Add(i + 1);

                triangles.Add(i + 1);
                triangles.Add(i + resolution + 1);
                triangles.Add(i + resolution + 2);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        meshFilter.mesh = mesh;
    }

    private void UpdateWave()
    {
        Vector3[] updatedVertices = mesh.vertices;
        float time = Time.timeSinceLevelLoad;

        for (int i = 0; i < updatedVertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            vertex.y = Mathf.PerlinNoise((transform.position.x+ vertex.x*transform.localScale.x + time * speed) / frequency, (transform.position.z + vertex.z *transform.localScale.x+ time * speed) / frequency) * amplitude;
            updatedVertices[i] = vertex;
        }

        mesh.vertices = updatedVertices;
        mesh.RecalculateNormals();
    }

    public float GetWaveHeight(float x, float z)
    {
        return Mathf.PerlinNoise((x + Time.timeSinceLevelLoad * speed) / frequency,
            (z + Time.timeSinceLevelLoad * speed) / frequency) * amplitude;
    }
}
