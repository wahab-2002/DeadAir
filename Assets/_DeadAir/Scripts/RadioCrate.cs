using System.Collections;
using UnityEngine;

public class RadioCrate : MonoBehaviour
{
    [SerializeField] RadioChannel channel;
    [SerializeField] PortableRadio portableRadio;
    [SerializeField] GameObject redGlow;
    [SerializeField] Interactable interaction;
    [SerializeField] Vector3 slideOffset = new Vector3(0.65f, 0, -0.2f);
    [SerializeField] float slideSeconds = 0.6f;
    bool lifted;

    public void Lift() { LiftCrate(false); }
    public void LiftInstantly() { LiftCrate(true); }

    void LiftCrate(bool instant)
    {
        if (lifted) { GetComponent<NoiseTestObject>().Use(); return; }
        lifted = true;
        interaction.SetPrompt("E: Bang");
        redGlow.SetActive(false);
        portableRadio.Reveal();
        channel.FreeChannel(instant ? 0 : slideSeconds);
        StartCoroutine(Slide(instant ? 0 : slideSeconds));
    }

    IEnumerator Slide(float seconds)
    {
        Vector3 start = transform.position;
        Vector3 end = start + slideOffset;
        float elapsed = 0;
        while (elapsed < seconds)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, elapsed / seconds);
            yield return null;
        }
        transform.position = end;
    }
}
