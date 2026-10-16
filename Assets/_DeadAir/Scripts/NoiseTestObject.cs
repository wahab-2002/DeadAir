using System.Collections;
using UnityEngine;

public class NoiseTestObject : MonoBehaviour
{
    public enum Kind { Radiator, BasementDoor, Far, Near, Warm, Bell, Decoy }
    [SerializeField] Kind kind;
    [SerializeField] RadioChannel radio;
    [SerializeField] Captions captions;
    [SerializeField] RadiatorKnocks knocks;
    [SerializeField] LibraryEvents library;
    [SerializeField] AudioSource sound;
    [SerializeField] bool locked;
    [SerializeField] AudioClip localClip;
    float nextBang;

    void Start()
    {
        string role = kind == Kind.Radiator ? "pipe_knock" : kind == Kind.Bell ? "desk_bell" : kind == Kind.Decoy ? "sheet_rustle" : locked ? "handle_rattle" : "object_bang";
        if (localClip == null) localClip = radio.Placeholder(role, 0.7f, kind == Kind.Radiator ? "clank" : kind == Kind.Bell ? "chime" : "rattle");
    }
    public void Use()
    {
        if (Time.time < nextBang) return;
        nextBang = Time.time + 1;
        sound.PlayOneShot(localClip);
        captions.Show(locked ? "[Locked.]" : kind == Kind.Radiator ? "[A metallic pipe knock.]" : kind == Kind.Bell ? "[The desk bell rings.]" : kind == Kind.Decoy ? "[The sheet rustles aside.]" : "[A bang.]", 3);
        if (radio.State == RadioChannel.ChannelState.LouOpenFeed && !radio.BatteryEnding)
        {
            library.MarkTest();
            StartCoroutine(Echo());
        }
    }
    IEnumerator Echo()
    {
        yield return new WaitForSeconds(0.4f);
        if (radio.State != RadioChannel.ChannelState.LouOpenFeed || radio.BatteryEnding) yield break;
        int level = kind == Kind.Radiator ? 0 : kind == Kind.Near || kind == Kind.Bell ? 4 : kind == Kind.Warm ? 5 : kind == Kind.Decoy ? 6 : 2;
        string description = level == 0 ? "a faint pipe knock" : level == 2 ? "faint and muffled" : level == 4 ? "clearer, still not loud" : level == 5 ? "clear and close" : "a clear rustle";
        radio.FeedSound(localClip, "RADIO: [" + description + "]", level == 0 || level == 2 ? 0.18f : level == 4 ? 0.4f : 0.75f);
        if (kind == Kind.Radiator) knocks.Reply();
        yield return new WaitForSeconds(localClip.length);
        radio.ReactToTest(level);
    }
}
