using UnityEngine;

public class NearbyRadioGlow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Light glow;
    [SerializeField] Renderer indicator;
    void OnDisable()
    {
        if (glow != null) glow.enabled = false;
        if (indicator != null) indicator.enabled = false;
    }
    void Update()
    {
        bool near = GameScreens.Started && Vector3.Distance(player.position, transform.position) < 1;
        glow.enabled = near;
        if (indicator != null) indicator.enabled = near;
    }
}
