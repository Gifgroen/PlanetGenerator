using UnityEngine;

[CreateAssetMenu(fileName = "New Shape Settings", menuName = "Planet/New Shape Settings", order = 0)]
public class ShapeSettings : ScriptableObject
{
    [SerializeField] public int radius;
}
