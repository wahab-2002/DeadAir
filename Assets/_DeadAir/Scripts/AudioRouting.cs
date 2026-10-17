using UnityEngine;
using UnityEngine.Audio;

// Routing only: source levels, spatial settings and filters stay with their owners.
[DefaultExecutionOrder(-900)]
public class AudioRouting : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    AudioMixerGroup music, lou, sam, feed, ambience, sfx, ui;
    bool warnedMissing;
    float nextScan;
    void Awake() { LoadGroups(); RouteAll(); }
    void LoadGroups()
    {
        if (mixer == null) return;
        music = Group("Music"); lou = Group("Lou (radio)"); sam = Group("Sam");
        feed = Group("Radio feed"); ambience = Group("Ambience"); sfx = Group("SFX"); ui = Group("UI");
    }
    AudioMixerGroup Group(string name)
    {
        foreach (AudioMixerGroup group in mixer.FindMatchingGroups(""))
            if (string.Equals(group.name, name, System.StringComparison.OrdinalIgnoreCase)) return group;
        return null;
    }
    void Update()
    {
        // Covers sources added at runtime as well as Inspector assignments changed in Play mode.
        if (Time.unscaledTime < nextScan) return;
        nextScan = Time.unscaledTime + 0.5f;
        if (music == null) LoadGroups();
        RouteAll();
    }
    void RouteAll()
    {
        if (mixer == null)
        {
            if (!warnedMissing) { warnedMissing = true; Debug.LogWarning("AudioRouting: DeadAir mixer is not assigned; routing awaits the mixer asset.", this); }
            return;
        }
        foreach (AudioSource source in FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (source.outputAudioMixerGroup != null) continue;
            string name = source.name.ToLowerInvariant();
            AudioMixerGroup group;
            if (source.GetComponent<BackgroundMusic>() != null || name == "music") group = music;
            else if (name.Contains("dialogue")) group = sam; // VoiceLines selects the speaker before playback.
            else if (name.Contains("radio clicks") || name.Contains("feed") || name.Contains("voice hiss") || name == "sam radio voice" || name == "pta #4 voice") group = feed;
            else if (name.Contains("aquarium") || name.Contains("boiler") || name.Contains("room tone") || name.Contains("radiator") || source.GetComponent<SchoolClock>() != null || name == "game screens") group = ambience;
            else if (source.GetComponentInParent<Canvas>() != null) group = ui;
            else group = sfx;
            if (group != null) source.outputAudioMixerGroup = group;
        }
    }
    public void Voice(AudioSource source, bool isLou)
    {
        if (mixer == null || source == null) return;
        source.outputAudioMixerGroup = isLou ? lou : sam;
    }
}
