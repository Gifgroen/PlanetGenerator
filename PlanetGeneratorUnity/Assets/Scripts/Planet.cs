using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{
    private const int FaceCount = 6;

    private readonly Vector3[] _directions =
    {
        Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
    };

    private TerrainFace[] _terrainFaces;

#pragma warning disable 649
    [SerializeField] private GameObject terrainFacePrefab;
    [SerializeField] private ShapeSettings shapeSettings;
    [SerializeField] private ColourSettings colourSettings;

    [Range(2, 256)] [SerializeField] private int resolution = 10;

    [SerializeField] private MeshFilter[] meshFilters;
#pragma warning restore 649

    private void OnValidate()
    {
        if (terrainFacePrefab == null)
        {
            return;
        }

        GeneratePlanet();
    }

    private void Initialize()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[FaceCount];
        }

        _terrainFaces = new TerrainFace[FaceCount];

        for (var i = 0; i < meshFilters.Length; ++i)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = Instantiate(terrainFacePrefab, transform);
                meshObj.name = $"TerrainFace{i}";
                meshFilters[i] = meshObj.GetComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh {name = $"Mesh{i}"};
            }

            _terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, _directions[i]);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColours();
    }

    public void OnShapeSettingsUpdated()
    {
        Initialize();
        GenerateMesh();
    }

    public void OnColourSettingsUpdated()
    {
        Initialize();
        GenerateColours();
    }

    private void GenerateMesh()
    {
        foreach (var face in _terrainFaces)
        {
            face.ConstructMesh(shapeSettings.radius);
        }
    }

    private void GenerateColours()
    {
        foreach (var m in meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.color;
        }
    }
}
