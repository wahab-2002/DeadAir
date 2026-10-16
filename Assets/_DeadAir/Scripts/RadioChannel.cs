using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RadioChannel : MonoBehaviour
{
    public enum ChannelState { LibraryFeed, LouTalking, LouOpenFeed, Silent }
    [Serializable]
    public class RadioLine
    {
        public string role;
        public string speaker;
        [TextArea(2, 5)] public string text;
        public AudioClip clip;
        public RadioLine(string role, string speaker, string text)
        { this.role = role; this.speaker = speaker; this.text = text; }
    }

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
    [SerializeField] float echoDelay = 0.15f;
    [SerializeField] float feedbackDistance = 1.5f;
    [SerializeField] float secondsPerCharacter = 0.06f;
    [SerializeField] List<RadioLine> lines = new List<RadioLine>
    {
        new RadioLine("sam_dead_air", "SAM:", "Lou? I fell asleep. Are you there? Over."),
        new RadioLine("lou_line_00", "LOU (radio):", "—am? Sam, please. Anyone. Over."),
        new RadioLine("sam_line_01", "SAM:", "Lou! It's me! I fell asleep. Where are you?"),
        new RadioLine("lou_line_01", "LOU (radio):", "Sam?! I've been calling for hours. I hid to jump out at you, and then I was falling, and now it's pitch black. I don't know where I am. My flashlight broke."),
        new RadioLine("sam_line_02", "SAM:", "What's down there with you?"),
        new RadioLine("lou_line_02", "LOU (radio):", "Boxes, I think. Sheets. And something laughs every time I move."),
        new RadioLine("lou_that", "LOU (radio):", "That!"),
        new RadioLine("lou_line_03", "LOU (radio):", "My batteries are almost dead. I'm gonna hold the button down so you can hear what I hear. Find me. Please find me.")
    };
    [SerializeField] List<RadioLine> testLines = new List<RadioLine>
    {
        new RadioLine("lou_radiator_first", "LOU (radio):", "Was that you? I heard it in the pipe... but it's everywhere."),
        new RadioLine("lou_radiator_later", "LOU (radio):", "I hear you. Keep going."),
        new RadioLine("lou_door_thud", "LOU (radio):", "That was far away."),
        new RadioLine("lou_witch_echo", "LOU (radio):", "The laugh again... no, that one sounded different. Closer."),
        new RadioLine("lou_battery_last", "LOU (radio):", "That! That's right next to me! Sam, I think that's what I fell out of... the light's blinking... please don't leave me down h—"),
        new RadioLine("sam_silent_call", "SAM:", "Lou? Lou, can you hear me?")
    };
    AudioClip[] testVoices;
    bool[] reactionsPlayed = new bool[4];
    bool batteryEnding;
    public bool BatteryEnding => batteryEnding;
    readonly List<AudioClip> generated = new List<AudioClip>();
    AudioClip libraryLoop, openLoop, click, releaseHiss, squeal, scrape, cackle, background;
    AudioClip[] voices;
    bool talkHeld, busy, receiving, feedbackPlayed, openWarningShown, breakthrough;
    bool talkClickPending;
    int nextExchange;
    float nextReply, nextFeedCaption;
    string currentCaption = "";
    float captionUntil;
    public ChannelState State => state;
    public bool IsBusy => busy;

    void Awake()
    {
        voices = new AudioClip[lines.Count];
        for (int i = 0; i < lines.Count; i++)
            voices[i] = lines[i].clip != null ? lines[i].clip : MakeSound(lines[i].role, Mathf.Max(1.2f, lines[i].text.Length * secondsPerCharacter), "voice");
        testVoices = new AudioClip[testLines.Count];
        for (int i = 0; i < testLines.Count; i++)
            testVoices[i] = testLines[i].clip != null ? testLines[i].clip : MakeSound(testLines[i].role, Mathf.Max(1.2f, testLines[i].text.Length * secondsPerCharacter), "voice");
        libraryLoop = MakeSound("library_feed_hiss_bubbling_ticking", 8, "library");
        openLoop = MakeSound("lou_open_breathing_dripping_roar", 8, "open");
        click = MakeSound("radio_button_click", 0.06f, "click");
        releaseHiss = MakeSound("radio_release_hiss", 0.25f, "hiss");
        squeal = MakeSound("radio_feedback_squeal", 0.3f, "squeal");
        scrape = MakeSound("crate_scrape", 0.6f, "scrape");
        cackle = MakeSound("witch_cackle_muffled", 1.2f, "cackle");
        background = MakeSound("lou_dripping_deep_roar", 2, "background");
    }

    void Start() { StartFeed(); }

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
        txLight.color = held && !receiving && !breakthrough && state != ChannelState.LouOpenFeed ? Color.red : new Color(0.25f, 0.06f, 0.06f);
        rxLight.color = receiving || (state == ChannelState.LibraryFeed && !held) ? Color.green : new Color(0.04f, 0.2f, 0.04f);

        if (state == ChannelState.LouTalking && !busy && !held && nextExchange >= 1 && Time.time >= nextReply)
            StartCoroutine(Exchange());
        if ((state == ChannelState.LibraryFeed || state == ChannelState.LouOpenFeed) && !busy && !held && Time.time >= nextFeedCaption && Time.time >= captionUntil)
            ShowFeedCaption();
    }

    public void BeginTalk()
    {
        if ((receiving && state != ChannelState.LouOpenFeed) || busy) return;
        controls.PlayOneShot(click);
        if (state == ChannelState.LouOpenFeed)
        {
            // The first blocked attempt explains why; subsequent attempts only click.
            if (!openWarningShown)
            {
                openWarningShown = true;
                Say("SAM: She can't hear me while she's holding it.\n[radio: click]", 4);
            }
            else Cue("[radio: click]");
            return;
        }
        if (state == ChannelState.Silent)
        {
            StartCoroutine(SilentCall());
            return;
        }
        talkClickPending = true;
        if (state == ChannelState.LibraryFeed) StartCoroutine(DeadAirCall());
        else StartCoroutine(Exchange());
    }

    void EndTalk()
    {
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
        talkClickPending = false;
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
        yield return new WaitForSeconds(2);
        receiving = true;
        yield return Speak(1, true);
        receiving = false;
        breakthrough = false;
        busy = false;
    }

    IEnumerator Exchange()
    {
        busy = true;
        if (nextExchange == 0 || nextExchange == 1)
        {
            yield return Speak(nextExchange == 0 ? 2 : 4, false);
            // Finish the line and wait for Sam to let go before Lou answers.
            while (talkHeld) yield return null;
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
        AudioClip voice = voices[index];
        float duration = Mathf.Max(voice.length, lines[index].text.Length * secondsPerCharacter, 1.5f);
        Say(lines[index].speaker + " " + lines[index].text + (talkClickPending ? "\n[radio: click]" : ""), duration);
        talkClickPending = false;
        samVoice.PlayOneShot(voice);
        if (lou) radioFourVoice.PlayOneShot(voice);
        else if (state == ChannelState.LouTalking)
        {
            yield return new WaitForSeconds(echoDelay);
            radioFourVoice.PlayOneShot(voice);
            if (!feedbackPlayed && Vector3.Distance(view.position, radioFourVoice.transform.position) <= feedbackDistance)
            {
                feedbackPlayed = true;
                radioFourVoice.PlayOneShot(squeal);
                Cue("[radio #4: feedback squeal; Sam's voice echoes]");
            }
            else Cue("[radio #4: Sam's voice echoes]");
            duration -= echoDelay;
        }
        yield return new WaitForSeconds(Mathf.Max(0, duration));
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
        Say(state == ChannelState.LibraryFeed ? "[radio: hiss... bubbling... ticking]" :
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
        if (index == 0 && reactionsPlayed[0]) index = 1;
        if (reactionsPlayed[index]) return;
        reactionsPlayed[index] = true;
        StartCoroutine(TestReaction(index));
    }

    IEnumerator TestReaction(int index)
    {
        while (busy && !batteryEnding) yield return null;
        if (state != ChannelState.LouOpenFeed || batteryEnding) yield break;
        busy = true;
        yield return TestLine(index, true);
        busy = false;
    }

    IEnumerator TestLine(int index, bool lou)
    {
        RadioLine line = testLines[index];
        AudioClip voice = testVoices[index];
        float duration = Mathf.Max(voice.length, line.text.Length * secondsPerCharacter);
        Say(line.speaker + " " + line.text, duration);
        samVoice.PlayOneShot(voice);
        if (!lou) yield return new WaitForSeconds(echoDelay);
        radioFourVoice.PlayOneShot(voice);
        if (!lou) captions.Show("[radio #4: Sam's voice echoes]", duration);
        yield return new WaitForSeconds(duration);
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
        while (busy) yield return null;
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
        state = ChannelState.Silent;
        busy = receiving = breakthrough = talkClickPending = false;
        batteryEnding = false;
        rxLight.color = new Color(0.04f, 0.2f, 0.04f);
        captions.Clear();
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
        state = ChannelState.LouOpenFeed;
        busy = false;
        breakthrough = false;
        StartFeed();
    }
#endif

    AudioClip MakeSound(string role, float seconds, string kind)
    {
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
                    value = noise * 0.065f + Mathf.Sin(2 * Mathf.PI * (380 * t + 8 * Mathf.Sin(t * 9))) * Mathf.Pow(Mathf.Max(0, Mathf.Sin(t * 7)), 8) * 0.18f;
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
