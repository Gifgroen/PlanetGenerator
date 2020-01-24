using UnityEngine;

public class TerrainFace
{
    private Mesh _mesh;
    private int _resolution;

    private readonly Vector3 _localUp;

    private readonly Vector3 _axisA;
    private readonly Vector3 _axisB;

    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
    {
        _mesh = mesh;
        _resolution = resolution;
        _localUp = localUp;

        _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        _axisB = Vector3.Cross(localUp, _axisA);
    }

    public void ConstructMesh(float radius)
    {
        int triangleResolution = _resolution - 1;
        Vector3[] vertices = new Vector3[_resolution * _resolution];
        int[] triangles = new int[triangleResolution * triangleResolution * 6];

        int triIndex = 0;
        for (var y = 0; y < _resolution; ++y)
        {
            for (var x = 0; x < _resolution; ++x)
            {
                Vector2 percent = new Vector2(x, y) / triangleResolution;
                Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _axisA +  (percent.y - 0.5f) * 2 * _axisB;

                int index = x + y * _resolution;
                vertices[index] = pointOnUnitCube.normalized * radius;

                if (x != triangleResolution && y != triangleResolution)
                {
                    triangles[triIndex] = index;
                    triangles[triIndex + 1] = index + _resolution + 1;
                    triangles[triIndex + 2] = index + _resolution;

                    triangles[triIndex + 3] = index;
                    triangles[triIndex + 4] = index + 1;
                    triangles[triIndex + 5] = index + _resolution + 1;

                    triIndex += 6;
                }
            }

            _mesh.Clear();
            _mesh.vertices = vertices;
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }
    }
}
