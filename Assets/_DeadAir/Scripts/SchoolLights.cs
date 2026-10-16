using UnityEngine;

public class SchoolLights : MonoBehaviour
{
    [SerializeField] Light pumpkin;
    void Update()
    {
        pumpkin.intensity = 0.85f + 0.12f * Mathf.Sin(Time.time * 7) + 0.06f * Mathf.Sin(Time.time * 19);
    }
}
