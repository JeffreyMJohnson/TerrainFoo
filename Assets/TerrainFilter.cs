using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class TerrainFilter : MonoBehaviour
{
    private const int MAX_FRAME_COUNT = 100;
    private const int MESH_WIDTH = 10;
    private const int MESH_HEIGHT = 10;

    private Terrain _terrain;
    private int _frameCounter = 0;
    private float _elapsedTime = 0;
    private float _framesPerSecond = 0;

    private Text _fpsText;


    // Use this for initialization
    void Start()
    {
        _terrain = FindObjectOfType<Terrain>();
        Debug.Assert(_terrain != null);
        _fpsText = FindObjectOfType<Canvas>().GetComponentsInChildren<Text>().Where(text => text.name == "fps").First();
        Debug.Assert(_fpsText != null);
        LoadData();

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        for (int i = 0; i < 10; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                Gizmos.DrawSphere(new Vector3(i, 1, j), .1f);
            }
        }

    }

    void LoadData()
    {
        Vector3 terrainSize = _terrain.terrainData.size;
        int resolution = _terrain.terrainData.heightmapResolution;
        float[,] heightData = _terrain.terrainData.GetHeights(0, 0, resolution, resolution);

        float maxHeight = terrainSize.y;
        Vector2 center = new Vector2((int)(resolution * .5), (int)(resolution * .5));

        for (int col = 0; col < heightData.GetLength(0); ++col)
        {
            for (int row = 0; row < heightData.GetLength(1); ++row)
            {

                float distanceFromCenter = Vector2.Distance(new Vector2(col, row), center);
                //float distanceFromCenter = (float)Math.Sqrt((col*center.x) + (row*center.y));

                float height = 1;
                if (distanceFromCenter != 0)
                {
                    height = (maxHeight / distanceFromCenter) / maxHeight;
                }
                //float height = (float)row/resolution;
                Debug.Log("height: " + height);

                heightData[col, row] = height;
            }
        }
        _terrain.terrainData.SetHeights(0, 0, heightData);
        _terrain.Flush();
    }

    void AddNoise()
    {
        int resolution = _terrain.terrainData.heightmapResolution;
        float[,] heightData = _terrain.terrainData.GetHeights(0, 0, resolution, resolution);

       
    }


    void Update()
    {
        if (_frameCounter >= MAX_FRAME_COUNT)
        {
            _framesPerSecond = _frameCounter / _elapsedTime;
            _elapsedTime = 0;
            _frameCounter = 0;
            _fpsText.text = "fps: " + _framesPerSecond;
        }
        else
        {
            ++_frameCounter;
            _elapsedTime += Time.deltaTime;
        }


    }
}
