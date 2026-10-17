using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLines : MonoBehaviour
{
    [Serializable] public class Line
    {
        public string id;
        public AudioClip clip;
        [TextArea] public string caption;
        public Line(string id, string caption) { this.id = id; this.caption = caption; }
    }
    [SerializeField] RadioChannel radio;
    [SerializeField] Captions captions;
    [SerializeField] RadioVoiceSettings settings;
    [SerializeField] AudioRouting audioRouting;
    [SerializeField] AudioSource localVoice, radioVoice;
    public List<Line> lines = new List<Line>
    {
        new Line("lou_00", "—am? Sam, please. Anyone. Over."),
        new Line("lou_01", "Sam?! I've been calling for hours. I hid to jump out at you, and then I was falling, and now it's pitch black. I don't know where I am. My flashlight broke."),
        new Line("lou_02", "Boxes, I think. Sheets. And something laughs every time I move."),
        new Line("lou_02b", "That!"),
        new Line("lou_03", "My battery is almost dead. I'm gonna hold the button down so you can hear what I hear. Bang on stuff, Sam. I'll tell you if it's close."),
        new Line("lou_04", "Was that you? I heard it in the pipes… but it's everywhere."),
        new Line("lou_05", "I hear you, Sam. Keep going."),
        new Line("lou_06", "That was far away."),
        new Line("lou_07", "The laugh again... no, that one sounded different. Closer."),
        new Line("lou_08", "Closer... I think."),
        new Line("lou_09", "Warmer."),
        new Line("lou_10", "Close... but not there."),
        new Line("lou_11", "That! That's right next to me! Sam, I think that's what I fell out of... the light's blinking... please don't leave me down h—"),
        new Line("lou_12", "BOO!"),
        new Line("lou_13", "Got you."),
        new Line("lou_14", "Did you get the Knocking Ghost on tape?"),
        new Line("sam_00", "Lou? I fell asleep. Are you there? Over."),
        new Line("sam_01", "Lou! It's me! I fell asleep. Where are you?"),
        new Line("sam_02", "What's down there with you?"),
        new Line("sam_03", "She can't hear me while she's holding it."),
        new Line("sam_04", "Not without Lou."),
        new Line("sam_05", "Just pumpkin bread."),
        new Line("sam_06", "Just a wall."),
        new Line("sam_07", "Mom's PTA radio. It was stuck on talk the whole time."),
        new Line("sam_08", "Lou's candy pail. She was in here."),
        new Line("sam_09", "If I go down too, there's nobody to crank us back up."),
        new Line("sam_10", "Lou? Lou, can you hear me?"),
        new Line("sam_11", "She's at the bottom of that lift. I have to get her up."),
        new Line("sam_12", "Lou! The cupboard's back! Follow my voice!"),
        new Line("sam_13", "Keep coming! You're close!"),
        new Line("sam_14", "Climb in. I'll pull you up."),
        new Line("sam_15", "Yeah. It was you."),
        new Line("sam_h1", "Something's glowing under that crate."),
        new Line("sam_h2", "If she's holding the button, anything loud near her should come through."),
        new Line("sam_h3", "She said she fell. Fell from where?"),
        new Line("sam_h4", "The crank... I could send the lift down to her."),
        new Line("sam_h5", "She can't see it. It's pitch black down there."),
        new Line("sam_h6", "She needs something to follow."),
        new Line("sam_h7", "The PTA radio... if she could hear me down there."),
        new Line("sam_h8", "It's coming from the other side of this wall."),
    };
    public float TalkAllowedAt { get; private set; }
    public string CurrentId { get; private set; } = "";
    public bool Speaking { get; private set; }
    public readonly List<string> Played = new List<string>();
    public Line Find(string id) { return lines.Find(line => line.id == id); }
    public AudioClip Clip(string id)
    {
        return Find(id).clip;
    }
    public IEnumerator Speak(string id, bool transmit = false)
    {
        while (Speaking) yield return null;
        Speaking = true;
        CurrentId = id;
        Line line = Find(id);
        AudioClip clip = Clip(id);
        float duration = clip == null ? 1 + 0.07f * line.caption.Length : clip.length;
        bool lou = id.StartsWith("lou_");
        Played.Add(id);
        captions.Dialogue((id.StartsWith("lou_") ? "LOU (radio): " : "SAM: ") + line.caption, duration);
        if (clip != null)
        {
            if (audioRouting != null) { audioRouting.Voice(localVoice, lou); audioRouting.Voice(radioVoice, lou); }
            settings.Begin(lou, transmit);
            localVoice.PlayOneShot(clip);
            if (transmit || lou) radioVoice.PlayOneShot(clip);
        }
        yield return new WaitForSeconds(duration);
        if (clip != null) settings.End(lou);
        if (id.StartsWith("lou_")) TalkAllowedAt = Time.time + 1.5f;
        Speaking = false;
    }
    public void Cancel() { StopAllCoroutines(); settings.Cancel(); Speaking = false; }
    public void Say(string id) { StartCoroutine(Speak(id)); }
}
