using UnityEditor.AI;
using UnityEngine;

public class ProceduralTerrainGenerator : MonoBehaviour
{
    #region Variable Declarations
#pragma warning disable 0649
    [SerializeField] private int width = 500; // x
    [SerializeField] private int depth = 20; // y
    [SerializeField] private int height = 500; // z
    [SerializeField] private float scale = 20f;
#pragma warning restore 0649
    #endregion

    private void Awake ()
    {
        Terrain terrain = GetComponent<Terrain> ();
        terrain.terrainData = GenerateTerrain (terrain.terrainData);
        NavMeshBuilder.BuildNavMesh ();
    }

    TerrainData GenerateTerrain (TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3 (width, depth, height);
        terrainData.SetHeights (0, 0, GenerateHeights ());

        return terrainData;
    }

    float[, ] GenerateHeights ()
    {
        float[, ] heights = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight (x, y);
            }
        }

        return heights;
    }

    float CalculateHeight (int x, int y)
    {
        float xCoord = (float) x / width * scale;
        float yCoord = (float) y / height * scale;

        return Mathf.PerlinNoise (xCoord, yCoord);
    }
}