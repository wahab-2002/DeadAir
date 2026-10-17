using UnityEngine;

public class NearbyRadioGlow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Light glow;
    [SerializeField] Renderer indicator;
    bool warnedMissing;
    void OnDisable()
    {
        if (glow != null) glow.enabled = false;
        if (indicator != null) indicator.enabled = false;
    }
    void Update()
    {
        if ((player == null || glow == null || indicator == null) && !warnedMissing)
        { warnedMissing = true; Debug.LogWarning("NearbyRadioGlow: optional glow reference missing; available indicators still work.", this); }
        bool near = player != null && GameScreens.Started && Vector3.Distance(player.position, transform.position) < 1;
        if (glow != null) glow.enabled = near;
        if (indicator != null) indicator.enabled = near;
    }
}
