using UnityEngine;

[CreateAssetMenu(fileName = "DamageType", menuName = "Scriptable Objects/DamageType")]
public class DamageType : ScriptableObject
{
    public Sprite icon;
    public Color color, iconColour;
    public int priority;
}
