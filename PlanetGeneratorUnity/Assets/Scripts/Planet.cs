using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{
    private static readonly int BaseColorNameId = Shader.PropertyToID("_BaseColor");

    private const int FaceCount = 6;

    private readonly Vector3[] _directions =
    {
        Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
    };

    private TerrainFace[] _terrainFaces;

#pragma warning disable 649
    [SerializeField] private GameObject terrainFacePrefab;
    [Range(2, 256)] [SerializeField] private int resolution = 10;
    [SerializeField] private bool autoUpdate;
    
    [SerializeField] public ShapeSettings shapeSettings;
    [SerializeField] public ColourSettings colourSettings;

    [SerializeField, HideInInspector] private MeshFilter[] meshFilters;
#pragma warning restore 649

#if UNITY_EDITOR
    [HideInInspector] public bool shapeFoldout;
    [HideInInspector] public bool colorFoldout;
#endif

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
        if (!autoUpdate)
        {
            return;
        }
        Initialize();
        GenerateMesh();
    }

    public void OnColourSettingsUpdated()
    {
        if (!autoUpdate)
        {
            return;
        }
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
            m.GetComponent<MeshRenderer>().sharedMaterial.SetColor(BaseColorNameId, colourSettings.color);
        }
    }
}