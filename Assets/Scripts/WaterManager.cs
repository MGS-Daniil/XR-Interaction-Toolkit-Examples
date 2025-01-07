using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private Mesh myMesh;
    private MeshFilter meshFilter;

    public static WaterManager instance;
    public float amplitude, frequency;
    [SerializeField] private Vector2 planeSize = new Vector2(1, 1);
    [SerializeField] private int planeResolution = 1;
    [SerializeField] private float speed = 1f;

    private List<Vector3> vertices;
    private List<int> triangels;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        CreatePlane();
    }

    [Button]
    private void CreatePlane()
    {
        myMesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = myMesh;
        GeneratePlane(planeSize, planeResolution);
        Noise(Time.timeSinceLevelLoad);
        AssignMesh();
    }


    private void Update()
    {
        Noise(Time.timeSinceLevelLoad);
        AssignMesh();

        // Vector3[] vertices = meshFilter.mesh.vertices;
        // for (int i = 0; i < vertices.Length; i++)
        // {
        //     vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x);
        // }
        //
        // meshFilter.mesh.vertices = vertices;
        // meshFilter.mesh.RecalculateNormals();
    }

    void GeneratePlane(Vector2 size, int resolution)
    {
        vertices = new List<Vector3>();
        float xPerStep = size.x / resolution;
        float yPerStep = size.y / resolution;

        for (int y = 0; y < resolution + 1; y++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                vertices.Add(new Vector3(x * xPerStep, 0, y * yPerStep));
            }
        }

        triangels = new List<int>();
        for (int row = 0; row < resolution; row++)
        {
            for (int column = 0; column < resolution; column++)
            {
                int i = row * resolution + row + column;

                triangels.Add(i);
                triangels.Add(i + (resolution) + 1);
                triangels.Add(i + (resolution) + 2);

                triangels.Add(i);
                triangels.Add(i + resolution + 2);
                triangels.Add(i + 1);
            }
        }
    }

    void AssignMesh()
    {
        myMesh.Clear();
        myMesh.vertices = vertices.ToArray();
        myMesh.triangles = triangels.ToArray();
    }

    void LeftToRightSine(float time)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            vertex.y = Mathf.Sin(time + vertex.x);
            vertices[i] = vertex;
        }
    }

    void Noise(float time)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            vertex.y = Mathf.PerlinNoise((vertex.x + time * speed) / frequency, (vertex.z + time * speed) / frequency) *
                       amplitude;
            vertices[i] = vertex;
        }
    }

    public float GetWaveHeight(float x, float z)
    {
        return Mathf.PerlinNoise((x + Time.timeSinceLevelLoad * speed) / frequency,
            (z + Time.timeSinceLevelLoad * speed) / frequency) * amplitude;
    }
}