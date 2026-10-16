using System.Collections;
using UnityEngine;

public class RadiatorKnocks : MonoBehaviour
{
    [SerializeField] AudioSource[] radiators;
    [SerializeField] Transform player;
    [SerializeField] Captions captions;
    [SerializeField] RadioChannel radio;
    [SerializeField] AudioClip radiator_knock;
    public float LastAmbientKnocksAt { get; private set; } = -1;
    bool replying, stopped;
    public bool Stopped => stopped;
    public void StopForever()
    {
        stopped = true; StopAllCoroutines();
        foreach (AudioSource source in radiators) source.Stop();
    }

    System.Collections.IEnumerator Start()
    {
        while (!GameScreens.Started) yield return null;
        if (radiator_knock == null) radiator_knock = radio.Placeholder("pipe_knock", 0.7f, "clank");
        StartCoroutine(RepeatKnocking());
    }

    IEnumerator RepeatKnocking()
    {
        while (true)
        {
            float next = Time.time + Random.Range(40f, 60f);
            if (!replying)
            {
                yield return Three(false);

            }
            while (Time.time < next) yield return null;
        }
    }

    IEnumerator Three(bool reply)
    {
        if (!reply) LastAmbientKnocksAt = Time.time;
        bool near = false;
        foreach (AudioSource source in radiators)
            if (Vector3.Distance(player.position, source.transform.position) <= 6) near = true;
        if (reply) captions.Show("[three knocks back, through every radiator]", 3);
        else captions.Ambient(near ? "[three knocks, inside the radiator]" : "[knocking, somewhere in the walls]", 3);
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
        if (!replying && !stopped) StartCoroutine(KnockBack());
    }

    IEnumerator KnockBack()
    {
        replying = true;
        yield return new WaitForSeconds(1.5f);
        yield return Three(true);
        replying = false;
    }
}
