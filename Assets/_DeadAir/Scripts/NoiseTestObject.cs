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
    [SerializeField] Transform liftDoor;
    [SerializeField] bool inPuzzleRoom, throughWall;
    float nextBang, wallHintAt = -1;
    bool wallHintPlayed;
    public int Level
    {
        get
        {
            if (kind == Kind.Radiator) return 0;
            if (kind == Kind.Decoy) return 6;
            float distance = Vector3.Distance(transform.position, liftDoor.position);
            if (throughWall && distance <= 2.5f) return 6;
            if (!inPuzzleRoom) return 2;
            // Avoid rounding an exact boundary into the closer band.
            if (distance >= 9 || Mathf.Approximately(distance, 9)) return 2;
            if (distance >= 5 || Mathf.Approximately(distance, 5)) return 4;
            return 5;
        }
    }
    void Update()
    {
        if (wallHintAt >= 0 && !wallHintPlayed && !RoomVisit.EnteredBookRoom && Time.time >= wallHintAt && !radio.IsBusy && !radio.GetComponent<VoiceLines>().Played.Contains("sam_h8"))
        { wallHintPlayed = true; radio.GetComponent<VoiceLines>().Say("sam_h8"); }
    }

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
        int level = Level;
        if (throughWall && level == 6 && wallHintAt < 0) wallHintAt = Time.time + localClip.length + 20;
        float distance = Vector3.Distance(transform.position, liftDoor.position);
        string description = level == 0 ? "a faint pipe knock" : level == 2 ? "a very faint thud" : level == 4 ? "a muffled thud" : level == 5 ? "a clear thud, close to her" : "a clear rustle";
        float volume = level == 0 ? 0.18f : level == 6 ? 0.75f : !inPuzzleRoom ? 0.08f : Mathf.Lerp(0.08f, 0.75f, Mathf.Clamp01(1 - distance / 9));
        radio.FeedSound(localClip, "[In Lou's radio: " + description + ".]", volume);
        if (kind == Kind.Radiator) knocks.Reply();
        yield return new WaitForSeconds(localClip.length);
        radio.ReactToTest(level);
    }
}
