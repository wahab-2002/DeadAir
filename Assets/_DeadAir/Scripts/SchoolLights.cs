using UnityEngine;

public class SchoolLights : MonoBehaviour
{
    [SerializeField] Light pumpkin;
    bool warnedMissing;
    void Update()
    {
        if (pumpkin == null) { if (!warnedMissing) { warnedMissing = true; Debug.LogWarning("SchoolLights: pumpkin light missing; flicker skipped.", this); } return; }
        pumpkin.intensity = 0.85f + 0.12f * Mathf.Sin(Time.time * 7) + 0.06f * Mathf.Sin(Time.time * 19);
    }
}
