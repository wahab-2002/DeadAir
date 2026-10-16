using System.Collections;
using UnityEngine;

public class SearchDecoy : MonoBehaviour
{
    [SerializeField] bool sheet;
    [SerializeField] RadioChannel radio;
    [SerializeField] Captions captions;
    [SerializeField] VoiceLines voices;
    [SerializeField] Interactable interaction;
    [SerializeField] NoiseTestObject noise;
    [SerializeField] Vector3 slideOffset = new Vector3(0.65f, 0, 0);
    [SerializeField] string contents = "[Paperback books.]";
    bool moved;
    public void Use()
    {
        noise.Use();
        if (moved) return;
        moved = true;
        if (sheet) interaction.enabled = false;
        else interaction.SetPrompt("E: Bang");
        if (!sheet)
        {
            GetComponent<AudioSource>().PlayOneShot(radio.Placeholder("crate_scrape", 0.6f, "scrape"));
            captions.Show("[The crate slides aside.]", 3);
        }
        StartCoroutine(Slide());
    }
    IEnumerator Slide()
    {
        Vector3 start = transform.position;
        if (sheet) slideOffset = Vector3.down * (start.y - 0.035f);
        for (float t = 0; t < 0.6f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(start, start + slideOffset, t / 0.6f);
            yield return null;
        }
        transform.position = start + slideOffset;
        if (sheet)
        {
            transform.localScale = new Vector3(transform.localScale.x, 0.06f, transform.localScale.z);
            foreach (Collider c in GetComponentsInChildren<Collider>()) c.enabled = false;
            captions.Show("[Just haunted-house props.]", 3);
        }
        else if (contents == "bread") yield return voices.Speak("sam_05");
        else captions.Show(contents, 3);
    }
}
