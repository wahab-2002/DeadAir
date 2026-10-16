using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LibraryEvents : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform doorway;
    [SerializeField] Transform desk;
    [SerializeField] Light[] witchEyes;
    [SerializeField] AudioSource witch;
    [SerializeField] AudioSource clock;
    [SerializeField] RadioChannel radio;
    [SerializeField] Captions captions;
    [SerializeField] LiftDoor lift;
    [SerializeField] AudioClip witch_cackle, clock_chime;
    float nextAquariumCaption;
    [SerializeField] Transform aquarium;
    float started, entered = -1, openStarted = -1, nextWitch, nextLouWitch;
    Vector3 lastPosition;
    bool chimed, tested, crateHint, noiseHint, fallHint;
    public bool HasChimed => chimed;
    public bool HasTested => tested;

    void Start()
    {
        foreach (Light eye in witchEyes) eye.GetComponent<Renderer>().enabled = false;
        started = Time.time;
        lastPosition = player.position;
        if (witch_cackle == null) witch_cackle = radio.Placeholder("witch_cackle", 1.2f, "cackle");
        if (clock_chime == null) clock_chime = radio.Placeholder("clock_chime", 4, "chime");
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current != null && Keyboard.current.f5Key.wasPressedThisFrame) Chime();
#endif
        if (!chimed && (Time.time - started >= 120)) Chime();
        if (aquarium != null && Vector3.Distance(player.position, aquarium.position) <= 4 && Time.time >= nextAquariumCaption)
        { captions.Show("[An aquarium bubbles.]", 3); nextAquariumCaption = Time.time + 30; }
        bool inLibrary = player.position.x > 6 && player.position.x < 14 && player.position.z < -1.8f && player.position.z > -11.5f;
        if (inLibrary && entered < 0) entered = Time.time;
        if (!crateHint && entered >= 0 && Time.time - entered >= 45 && radio.State == RadioChannel.ChannelState.LibraryFeed)
        {
            crateHint = true;
            radio.GetComponent<VoiceLines>().Say("sam_h1");
        }
        Vector3 offset = player.position - desk.position;
        offset.y = 0;
        if (offset.magnitude <= 2.5f && (player.position - lastPosition).sqrMagnitude > 0.000001f && Time.time >= nextWitch)
            TriggerWitch();
        lastPosition = player.position;
        if (radio.State != RadioChannel.ChannelState.LouOpenFeed) { openStarted = -1; return; }
        if (openStarted < 0)
        {
            openStarted = Time.time;
            nextLouWitch = Time.time + Random.Range(30f, 45f);
        }
        if (radio.BatteryEnding) return;
        if (Time.time >= nextLouWitch && !radio.IsBusy)
        {
            radio.FeedSound(witch_cackle, "[Lou's radio: cackling laughter]", 0.22f);
            nextLouWitch = Time.time + Random.Range(30f, 45f);
        }
        if (!noiseHint && !tested && Time.time - openStarted >= 40 && !radio.IsBusy)
        {
            noiseHint = true;
            radio.GetComponent<VoiceLines>().Say("sam_h2");
        }
        if (!fallHint && !lift.DoorTouched && Time.time - openStarted >= 80 && !radio.IsBusy)
        {
            fallHint = true;
            radio.GetComponent<VoiceLines>().Say("sam_h3");
        }
    }

    public void MarkTest()
    {
        if (radio.State == RadioChannel.ChannelState.LouOpenFeed) tested = true;
    }

    public void Chime()
    {
        if (chimed) return;
        chimed = true;
        clock.PlayOneShot(clock_chime);
        if (radio.State == RadioChannel.ChannelState.LibraryFeed)
            radio.FeedSound(clock_chime, "[A clock chimes midnight, down the corridor.]\nRADIO: [the same chime]");
        else captions.Show("[A clock chimes midnight, down the corridor.]", 5);
    }

    public void TriggerWitch()
    {
        if (Time.time < nextWitch) return;
        nextWitch = Time.time + 5;
        witch.PlayOneShot(witch_cackle);
        captions.Show("[The witch cackles.]", 3);
        StartCoroutine(Eyes());
        RadioChannel.ChannelState atTrigger = radio.State;
        if (atTrigger == RadioChannel.ChannelState.LibraryFeed || (atTrigger == RadioChannel.ChannelState.LouOpenFeed && !radio.BatteryEnding))
            StartCoroutine(WitchEcho(atTrigger));
    }

    IEnumerator Eyes()
    {
        foreach (Light eye in witchEyes) { eye.enabled = true; eye.GetComponent<Renderer>().enabled = true; }
        yield return new WaitForSeconds(1);
        foreach (Light eye in witchEyes) { eye.enabled = false; eye.GetComponent<Renderer>().enabled = false; }
    }

    IEnumerator WitchEcho(RadioChannel.ChannelState atTrigger)
    {
        yield return new WaitForSeconds(atTrigger == RadioChannel.ChannelState.LibraryFeed ? 0.2f : 0.5f);
        if (radio.State != atTrigger || radio.BatteryEnding) yield break;
        bool library = atTrigger == RadioChannel.ChannelState.LibraryFeed;
        radio.FeedSound(witch_cackle, library ? "RADIO: [the same cackle]" : "[Lou's radio: a faint, muffled cackle]", library ? 1 : 0.18f);
        if (!library)
        {
            MarkTest();
            yield return new WaitForSeconds(witch_cackle.length);
            radio.ReactToTest(3);
        }
    }
}
