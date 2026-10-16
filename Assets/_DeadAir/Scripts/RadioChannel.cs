using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioChannel : MonoBehaviour
{
    public enum ChannelState { LibraryFeed, LouTalking, LouOpenFeed, Silent }
    [SerializeField] VoiceLines voiceLines;
    [SerializeField] SupplyLift supplyLift;
    [SerializeField] ChannelState state = ChannelState.LibraryFeed;
    [SerializeField] Captions captions;
    [SerializeField] NoteViewer notes;
    [SerializeField] Transform player;
    [SerializeField] Transform view;
    [SerializeField] RadioCrate crate;
    [SerializeField] AudioSource samVoice;
    [SerializeField] AudioSource samFeed;
    [SerializeField] AudioSource controls;
    [SerializeField] AudioSource radioFourVoice;
    [SerializeField] AudioSource radioFourFeed;
    [SerializeField] UnityEngine.UI.Image txLight;
    [SerializeField] UnityEngine.UI.Image rxLight;
    [SerializeField] float replyWait = 15;
    [SerializeField] float captionInterval = 20;
    [SerializeField] float feedbackDistance = 1.5f;
    readonly string[] lines = { "sam_00", "lou_00", "sam_01", "lou_01", "sam_02", "lou_02", "lou_02b", "lou_03" };
    readonly string[] testLines = { "lou_04", "lou_05", "lou_06", "lou_07", "lou_11", "sam_10" };
    bool[] reactionsPlayed = new bool[7];
    bool batteryEnding;
    public bool BatteryEnding => batteryEnding;
    readonly List<AudioClip> generated = new List<AudioClip>();
    AudioClip libraryLoop, openLoop, click, releaseHiss, squeal, scrape, cackle, background;
    bool talkHeld, busy, receiving, feedbackPlayed, openWarningShown, breakthrough;
    int nextExchange;
    float nextReply, nextFeedCaption, nextCall;
    string currentCaption = "";
    float captionUntil;
    public ChannelState State => state;
    public bool IsBusy => busy || voiceLines.Speaking;
    [SerializeField] AudioSource searchFeedback;
    bool whining, talkAccepted;
    string warnedLine;

    void Awake()
    {
        libraryLoop = MakeSound("library_feed_hiss_bubbling_ticking", 8, "library");
        openLoop = MakeSound("lou_open_breathing_dripping_roar", 8, "open");
        click = MakeSound("radio_button_click", 0.06f, "click");
        releaseHiss = MakeSound("radio_release_hiss", 0.25f, "hiss");
        squeal = MakeSound("radio_feedback_squeal", 0.3f, "squeal");
        scrape = MakeSound("crate_scrape", 0.6f, "scrape");
        cackle = MakeSound("witch_cackle_muffled", 1.2f, "cackle");
        background = MakeSound("lou_dripping_deep_roar", 2, "background");
    }

    void Start() { AudioListener.volume = 0.67f; if (searchFeedback != null) searchFeedback.clip = Placeholder("radio_feedback", 1, "squeal"); StartFeed(); }

    void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current != null)
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                CharacterController controller = player.GetComponent<CharacterController>();
                controller.enabled = false;
                player.SetPositionAndRotation(new Vector3(10, 0.05f, -2), Quaternion.Euler(0, 210, 0));
                view.localRotation = Quaternion.identity;
                controller.enabled = true;
            }
            if (Keyboard.current.f2Key.wasPressedThisFrame) crate.LiftInstantly();
            if (Keyboard.current.f3Key.wasPressedThisFrame) JumpToOpenFeed();
            if (Keyboard.current.f4Key.wasPressedThisFrame) GoSilent();
        }
#endif
        bool held = (Keyboard.current != null && Keyboard.current.qKey.isPressed) ||
                    (Mouse.current != null && Mouse.current.rightButton.isPressed);
        if (notes.IsOpen || Time.timeScale == 0 || Cursor.lockState != CursorLockMode.Locked) held = false;
        if (held && !talkHeld) BeginTalk();
        if (!held && talkHeld) EndTalk();
        talkHeld = held;
        if (searchFeedback != null)
        {
            float proximity = Mathf.Clamp01(1 - Vector3.Distance(view.position, radioFourVoice.transform.position) / 4);
            bool whine = state == ChannelState.LibraryFeed && proximity > 0;
            if (whine && !whining) captions.Show("[Your radio whines.]", 3);
            whining = whine;
            searchFeedback.volume = whine ? proximity * 0.3f : 0;
            searchFeedback.pitch = Mathf.Lerp(0.5f, 1.6f, proximity);
            if (whine && !searchFeedback.isPlaying) searchFeedback.Play();
            if (!whine) searchFeedback.Stop();
        }
        txLight.color = held && !receiving && !breakthrough && state != ChannelState.LouOpenFeed ? Color.red : new Color(0.25f, 0.06f, 0.06f);
        bool louSpeaking = voiceLines.Speaking && voiceLines.CurrentId.StartsWith("lou_");
        rxLight.color = state == ChannelState.LibraryFeed ? Color.green :
            held && talkAccepted ? Color.red : state == ChannelState.LouOpenFeed ? Color.green :
            louSpeaking ? Color.green * (0.65f + 0.35f * Mathf.Abs(Mathf.Sin(Time.time * 23))) : Color.black;

        if (state == ChannelState.LouTalking && !busy && nextExchange == 0 && Time.time >= nextCall) StartCoroutine(RepeatCall());
        if (state == ChannelState.LouTalking && !busy && !voiceLines.Speaking && nextExchange >= 1 && Time.time >= nextReply)
            StartCoroutine(Exchange());
        if ((state == ChannelState.LibraryFeed || state == ChannelState.LouOpenFeed) && !busy && !held && Time.time >= nextFeedCaption && Time.time >= captionUntil)
            ShowFeedCaption();
    }

    public void BeginTalk()
    {
        talkAccepted = false;
        if (voiceLines.Speaking)
        {
            if (voiceLines.CurrentId.StartsWith("lou_") && warnedLine != voiceLines.CurrentId + voiceLines.Played.Count)
            {
                warnedLine = voiceLines.CurrentId + voiceLines.Played.Count;
                captions.Show("[Lou is still talking.]", 2);
            }
            return;
        }
        if ((receiving && state != ChannelState.LouOpenFeed) || busy) return;
        controls.PlayOneShot(click);
        Cue("[radio: click]");
        if (state == ChannelState.LouOpenFeed)
        {
            // The first blocked attempt explains why; subsequent attempts only click.
            if (!openWarningShown)
            {
                openWarningShown = true;
                voiceLines.Say("sam_03");
            }
            else Cue("[Nothing. Her light's still on.]\n[radio: click]");
            return;
        }
        talkAccepted = true;
        if (state == ChannelState.Silent)
        {
            supplyLift.Talk();
            return;
        }
        if (state == ChannelState.LibraryFeed) StartCoroutine(DeadAirCall());
        else StartCoroutine(Exchange());
    }

    void EndTalk()
    {
        if (!talkAccepted) return;
        talkAccepted = false;
        if (receiving || breakthrough || state == ChannelState.LouOpenFeed) return;
        if (state == ChannelState.Silent)
        {
            controls.PlayOneShot(click);
            Cue("[radio: click]");
            return;
        }
        controls.PlayOneShot(click);
        controls.PlayOneShot(releaseHiss);
        Cue("[radio: click, short hiss]");
    }

    IEnumerator DeadAirCall()
    {
        busy = true;
        yield return Speak(0, false);
        busy = false;
        if (state == ChannelState.LibraryFeed) ShowFeedCaption();
    }

    public void FreeChannel(float slideSeconds)
    {
        if (state != ChannelState.LibraryFeed) return;
        StopAllCoroutines();
        StopAudio();
        controls.PlayOneShot(click);
        captions.Show("[radio: click; the hiss stops]", 3);
        voiceLines.Cancel();
        state = ChannelState.LouTalking;
        nextExchange = 0;
        busy = true;
        receiving = false;
        breakthrough = true;
        StartCoroutine(Breakthrough(slideSeconds));
    }

    IEnumerator Breakthrough(float slideSeconds)
    {
        if (slideSeconds > 0)
        {
            controls.PlayOneShot(scrape);
            Say("[crate slides aside]", slideSeconds);
            yield return new WaitForSeconds(slideSeconds);
        }
        controls.Stop();
        captions.Clear();
        Say("", 0);
        yield return voiceLines.Speak("sam_07");
        yield return new WaitForSeconds(1.5f);
        receiving = true;
        yield return Speak(1, true);
        receiving = false;
        breakthrough = false;
        nextCall = Time.time + 10;
        busy = false;
    }

    IEnumerator RepeatCall()
    {
        busy = true; receiving = true;
        yield return voiceLines.Speak("lou_00", true);
        receiving = busy = false; nextCall = Time.time + 10;
    }

    IEnumerator Exchange()
    {
        busy = true;
        if (nextExchange == 0 || nextExchange == 1)
        {
            yield return Speak(nextExchange == 0 ? 2 : 4, false);
            // Finish the line and wait for Sam to let go before Lou answers.
            while (talkHeld && talkAccepted) yield return null;
            yield return new WaitForSeconds(0.35f);
        }
        receiving = true;
        if (nextExchange == 0)
        {
            yield return Speak(3, true);
            PlayBoth(background);
            Say("[behind her voice: dripping, a deep roar]", 2);
            yield return new WaitForSeconds(2);
        }
        else if (nextExchange == 1)
        {
            yield return Speak(5, true);
            PlayBoth(cackle);
            Say("[muffled cackling]", cackle.length);
            yield return new WaitForSeconds(cackle.length);
            yield return Speak(6, true);
        }
        else
        {
            yield return Speak(7, true);
            state = ChannelState.LouOpenFeed;
            busy = false;
            StartFeed();
            yield break;
        }
        nextExchange++;
        nextReply = Time.time + replyWait;
        receiving = false;
        busy = false;
    }

    IEnumerator Speak(int index, bool lou)
    {
        yield return voiceLines.Speak(lines[index], lou || state == ChannelState.LouTalking);
        if (!lou && state == ChannelState.LouTalking && !feedbackPlayed && Vector3.Distance(view.position, radioFourVoice.transform.position) <= feedbackDistance)
        {
            feedbackPlayed = true;
            radioFourVoice.PlayOneShot(squeal);
            Cue("[radio #4: feedback squeal; Sam's voice echoes]");
        }
    }

    void StartFeed()
    {
        receiving = state == ChannelState.LouOpenFeed;
        samFeed.clip = receiving ? openLoop : libraryLoop;
        samFeed.loop = true;
        samFeed.Play();
        if (receiving)
        {
            radioFourFeed.clip = openLoop;
            radioFourFeed.loop = true;
            radioFourFeed.Play();
        }
        ShowFeedCaption();
    }

    void ShowFeedCaption()
    {
        Say(state == ChannelState.LibraryFeed ? "RADIO: [hiss… bubbling… a clock ticking]" :
            "[Lou's radio, open: breathing... dripping... a deep roar]", 5);
        nextFeedCaption = Time.time + captionInterval;
    }

    void Say(string text, float seconds)
    {
        currentCaption = text;
        captionUntil = Time.time + seconds;
        captions.Show(text, seconds);
    }

    void Cue(string cue)
    {
        float remaining = captionUntil - Time.time;
        captions.Show(cue, Mathf.Max(1.2f, remaining));
    }

    void PlayBoth(AudioClip clip)
    {
        samVoice.PlayOneShot(clip);
        radioFourVoice.PlayOneShot(clip);
    }

    // Only feed sounds reach the receiving radios. Sam remains unable to transmit.
    public void FeedSound(AudioClip clip, string caption, float volume = 1)
    {
        if (state != ChannelState.LibraryFeed && state != ChannelState.LouOpenFeed) return;
        samVoice.PlayOneShot(clip, volume);
        if (state == ChannelState.LouOpenFeed) radioFourVoice.PlayOneShot(clip, volume);
        captions.Show(caption, Mathf.Max(3, clip.length));
    }

    public void ReactToTest(int index)
    {
        if (state != ChannelState.LouOpenFeed || batteryEnding) return;
        if (index != 6 && reactionsPlayed[index]) index = 1;
        if (index != 6 && reactionsPlayed[index]) return;
        reactionsPlayed[index] = true;
        StartCoroutine(TestReaction(index));
    }

    IEnumerator TestReaction(int index)
    {
        while ((busy || voiceLines.Speaking) && !batteryEnding) yield return null;
        if (state != ChannelState.LouOpenFeed || batteryEnding) yield break;
        busy = true;
        yield return voiceLines.Speak(index == 4 ? "lou_08" : index == 5 ? "lou_09" : index == 6 ? "lou_10" : testLines[index], true);
        busy = false;
    }

    IEnumerator TestLine(int index, bool lou)
    {
        yield return voiceLines.Speak(testLines[index], true);
    }

    IEnumerator SilentCall()
    {
        busy = true;
        captions.Show("[radio: click]", 2);
        yield return TestLine(5, false);
        busy = false;
    }

    public void LoudTest(AudioClip rattle)
    {
        if (state != ChannelState.LouOpenFeed || batteryEnding) return;
        batteryEnding = true;
        StartCoroutine(BatteryDies(rattle));
    }

    IEnumerator BatteryDies(AudioClip rattle)
    {
        // Finish any current reaction, then keep the last line uninterrupted.
        while (busy || voiceLines.Speaking) yield return null;
        busy = true;
        FeedSound(rattle, "[Lou's radio: loud rattle, close]");
        yield return new WaitForSeconds(rattle.length);
        yield return TestLine(4, true);
        GoSilent();
        controls.PlayOneShot(click);
        captions.Clear();
        Say("[click, then silence]", 4);
    }

    public void GoSilent()
    {
        StopAllCoroutines();
        StopAudio();
        voiceLines.Cancel();
        state = ChannelState.Silent;
        busy = receiving = breakthrough = false;
        batteryEnding = false;
        rxLight.color = new Color(0.04f, 0.2f, 0.04f);
        captions.Clear();
        supplyLift.BatteryDead();
    }

    // Generated clips are owned here and released on leaving Play mode.
    public AudioClip Placeholder(string role, float seconds, string kind)
    { return MakeSound(role, seconds, kind); }

    void StopAudio()
    {
        samVoice.Stop(); samFeed.Stop(); controls.Stop();
        radioFourVoice.Stop(); radioFourFeed.Stop();
    }

#if UNITY_EDITOR
    public void JumpToOpenFeed()
    {
        crate.LiftInstantly();
        StopAllCoroutines();
        StopAudio();
        voiceLines.Cancel();
        state = ChannelState.LouOpenFeed;
        busy = false;
        breakthrough = false;
        StartFeed();
    }
#endif

    AudioClip MakeSound(string role, float seconds, string kind)
    {
        SoundClips sounds = GetComponent<SoundClips>();
        AudioClip assigned = sounds == null ? null : sounds.Find(role);
        if (assigned != null) return assigned;
        const int sampleRate = 16000;
        float[] samples = new float[Mathf.CeilToInt(seconds * sampleRate)];
        // A fixed seed makes each placeholder consistent between play sessions.
        System.Random random = new System.Random(43);
        for (int i = 0; i < samples.Length; i++)
        {
            float t = (float)i / sampleRate;
            float noise = (float)random.NextDouble() * 2 - 1;
            float value = noise * 0.12f;
            switch (kind)
            {
                case "voice": value = noise * (t % 0.06f < 0.043f ? 0.32f : 0.025f) * (0.65f + 0.35f * Mathf.Sin(t * 23)); break;
                case "library":
                    value = noise * 0.025f + Mathf.Sin(2 * Mathf.PI * (380 * t + 8 * Mathf.Sin(t * 9))) * Mathf.Pow(Mathf.Max(0, Mathf.Sin(t * 7)), 8) * 0.18f;
                    if (t % 1.6f < 0.035f) value += noise * 0.3f;
                    break;
                case "open": case "background":
                    value = Mathf.Sin(t * 2 * Mathf.PI * 95) * 0.12f + noise * 0.035f;
                    if (kind == "open") value += noise * Mathf.Max(0, Mathf.Sin(t * 1.6f)) * 0.16f;
                    if (t % 1.3f < 0.1f) value += Mathf.Sin(t * 2 * Mathf.PI * 1100) * Mathf.Exp(-(t % 1.3f) * 35) * 0.25f;
                    break;
                case "knock": value = (noise * 0.35f + Mathf.Sin(t * 2 * Mathf.PI * 140) * 0.5f) * Mathf.Exp(-t * 35); break;
                case "clank": value = (Mathf.Sin(t * 2 * Mathf.PI * 670) + Mathf.Sin(t * 2 * Mathf.PI * 1090) * 0.5f + noise * 0.2f) * Mathf.Exp(-t * 7) * 0.45f; break;
                case "thud": value = (Mathf.Sin(t * 2 * Mathf.PI * 85) + noise * 0.3f) * Mathf.Exp(-t * 9) * 0.6f; break;
                case "rattle": value = (noise * 0.4f + Mathf.Sin(t * 2 * Mathf.PI * 180) * 0.15f) * Mathf.Max(0, Mathf.Sin(t * 32)); break;
                case "chime": value = (Mathf.Sin(t * 2 * Mathf.PI * 523) + Mathf.Sin(t * 2 * Mathf.PI * 659) * 0.5f) * Mathf.Exp(-(t % 1) * 2) * 0.3f; break;
                case "squeal": value = Mathf.Sin(t * 2 * Mathf.PI * 1800) * 0.28f; break;
                case "click": value = noise * Mathf.Exp(-t * 70) * 0.6f; break;
                case "scrape": value = noise * 0.3f; break;
                case "cackle": value = (Mathf.Sin(t * 2 * Mathf.PI * 650) + noise) * Mathf.Max(0, Mathf.Sin(t * 24)) * 0.16f; break;
            }
            float fade = Mathf.Min(1, t * 100, (seconds - t) * 100);
            samples[i] = value * Mathf.Max(0, fade);
        }
        AudioClip clip = AudioClip.Create(role, samples.Length, 1, sampleRate, false);
        clip.SetData(samples, 0);
        generated.Add(clip);
        return clip;
    }

    void OnDestroy() { foreach (AudioClip clip in generated) Destroy(clip); }
}
