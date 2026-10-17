using System.Collections;
using UnityEngine;
using TMPro;

public class LiftEnding : MonoBehaviour
{
    [SerializeField] BackgroundMusic music;
    [SerializeField] FirstPersonController movement;
    [SerializeField] PlayerInteraction interaction;
    [SerializeField] RadioChannel radio;
    [SerializeField] LibraryEvents library;
    [SerializeField] VoiceLines voices;
    [SerializeField] RadiatorKnocks knocks;
    [SerializeField] Captions captions;
    [SerializeField] Transform view, lou, door;
    [SerializeField] AudioSource sting, roomTone;
    [SerializeField] UnityEngine.UI.Image black;
    [SerializeField] TMP_Text endText;
    [SerializeField] GameObject hud;
    [SerializeField] AudioClip boo_sting;
    public bool Finished { get; private set; }

    public void Begin()
    {
        movement.Locked = true; interaction.enabled = false; radio.enabled = false; library.enabled = false;
        library.StopAllCoroutines();
        foreach (SchoolClock clock in FindObjectsByType<SchoolClock>(FindObjectsSortMode.None)) clock.enabled = false;
        foreach (AudioSource source in FindObjectsByType<AudioSource>(FindObjectsSortMode.None)) if (source.GetComponent<BackgroundMusic>() == null) source.Stop();
        StartCoroutine(Ending());
    }
    IEnumerator Ending()
    {
        Quaternion startRotation = view.rotation;
        Quaternion target = Quaternion.LookRotation(door.position - view.position);
        float elapsed = 0;
        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime; view.rotation = Quaternion.Slerp(startRotation, target, elapsed / 0.3f); yield return null;
        }
        yield return new WaitForSeconds(1);
        while (voices.Speaking) yield return null;
        if (boo_sting == null) boo_sting = radio.Placeholder("boo_sting", 0.4f, "chime");
        sting.PlayOneShot(boo_sting); captions.Show("[a sudden sting]", 2);
        StartCoroutine(voices.Speak("lou_12", true));
        lou.SetParent(null, true);
        Vector3 start = lou.position;
        Vector3 forward = Vector3.ProjectOnPlane(view.forward, Vector3.up).normalized;
        Vector3 end = view.position + forward * 0.55f; end.y = movement.transform.position.y + 0.08f;
        Quaternion face = Quaternion.LookRotation(-forward);
        Vector3 cameraStart = view.localPosition;
        elapsed = 0;
        while (elapsed < 0.22f)
        {
            elapsed += Time.deltaTime; float t = Mathf.Clamp01(elapsed / 0.22f);
            lou.position = Vector3.Lerp(start, end, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * 0.25f;
            lou.localScale = Vector3.one * Mathf.Lerp(0.7f, 1.08f, t); lou.rotation = face;
            view.localPosition = cameraStart - Vector3.forward * 0.05f + (elapsed < 0.25f ? Random.insideUnitSphere * 0.025f : Vector3.zero);
            yield return null;
        }
        lou.position = end;
        while (elapsed < 0.25f)
        {
            elapsed += Time.deltaTime;
            view.localPosition = cameraStart - Vector3.forward * 0.05f + Random.insideUnitSphere * 0.025f;
            yield return null;
        }
        view.localPosition = cameraStart;
        yield return new WaitForSeconds(0.6f);
        Vector3 settle = end + forward * 0.45f;
        for (float t = 0; t < 0.3f; t += Time.deltaTime)
        {
            lou.position = Vector3.Lerp(end, settle, t / 0.3f);
            lou.rotation = face * Quaternion.Euler(0, 0, 7 * t / 0.3f);
            yield return null;
        }
        lou.position = settle;
        while (voices.Speaking) yield return null;
        yield return voices.Speak("lou_13", true);
        yield return new WaitForSeconds(0.5f); yield return voices.Speak("lou_14", true);
        yield return new WaitForSeconds(0.5f); yield return voices.Speak("sam_15");
        knocks.StopForever();
        foreach (AudioSource source in FindObjectsByType<AudioSource>(FindObjectsSortMode.None)) if (source.GetComponent<BackgroundMusic>() == null) source.Stop();
        library.StopAllCoroutines(); radio.StopAllCoroutines();
        roomTone.clip = radio.Placeholder("faint_room_tone", 4, "hiss"); roomTone.loop = true; roomTone.volume = 0.025f; roomTone.Play();
        captions.Clear(); captions.Show("[In the walls, the pipes go quiet.]\n[faint room tone]", 4);
        yield return new WaitForSeconds(2);
        black.gameObject.SetActive(true); endText.text = "";
        elapsed = 0;
        while (elapsed < 2)
        {
            elapsed += Time.deltaTime; black.color = new Color(0, 0, 0, Mathf.Clamp01(elapsed / 2)); yield return null;
        }
        hud.SetActive(false);
        endText.text = "Over and out."; yield return new WaitForSeconds(3);
        endText.text = "DEAD AIR"; Finished = true;
        yield return new WaitForSeconds(3);
        yield return FindFirstObjectByType<GameScreens>().ShowCredits();
    }
}
