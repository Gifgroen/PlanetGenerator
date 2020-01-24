using UnityEngine;

[CreateAssetMenu(fileName = "New Colour Settings", menuName = "Planet/New Colour Settings", order = 0)]
public class ColourSettings : ScriptableObject
{
    [SerializeField] public Color color;
}
