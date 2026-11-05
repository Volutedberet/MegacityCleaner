using UnityEngine;

[CreateAssetMenu(fileName = "EnviromentData", menuName = "EnviromentData", order = 0)]
public class EnviromentData : ScriptableObject {
    [ColorUsageAttribute(true,true,0f,8f,0.125f,3f)]
    public Color lightColor;

    public Color fogColor;
    public float fogDensity;
}