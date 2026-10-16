using System.Collections;
using UnityEngine;

public class RadiatorKnocks : MonoBehaviour
{
    [SerializeField] AudioSource[] radiators;
    [SerializeField] Transform player;
    [SerializeField] Captions captions;
    [SerializeField] RadioChannel radio;
    [SerializeField] AudioClip radiator_knock;
    bool replying;

    void Start()
    {
        if (radiator_knock == null) radiator_knock = radio.Placeholder("radiator_knock", 0.25f, "knock");
        StartCoroutine(RepeatKnocking());
    }

    IEnumerator RepeatKnocking()
    {
        while (true)
        {
            float next = Time.time + Random.Range(25f, 35f);
            if (!replying)
            {
                yield return Three(false);
                yield return new WaitForSeconds(1);
                if (!replying) yield return Three(false);
            }
            while (Time.time < next) yield return null;
        }
    }

    IEnumerator Three(bool reply)
    {
        bool near = false;
        foreach (AudioSource source in radiators)
            if (Vector3.Distance(player.position, source.transform.position) <= 6) near = true;
        captions.Show(reply ? "[three knocks back, through every radiator]" :
            near ? "[three knocks, inside the radiator]" : "[knocking, somewhere in the walls]", 3);
        for (int i = 0; i < 3; i++)
        {
            if (!reply && replying) yield break;
            double start = AudioSettings.dspTime + 0.05;
            foreach (AudioSource source in radiators)
            {
                source.clip = radiator_knock;
                source.PlayScheduled(start);
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    public void Reply()
    {
        // Rapid repeated bangs still produce one clear three-knock response.
        if (!replying) StartCoroutine(KnockBack());
    }

    IEnumerator KnockBack()
    {
        replying = true;
        yield return new WaitForSeconds(1.5f);
        yield return Three(true);
        replying = false;
    }
}
