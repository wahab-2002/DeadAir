using UnityEngine;

public class RadioVoiceSettings : MonoBehaviour
{
    [Tooltip("Removes low frequencies from voices on both radios (Hz).")]
    [Range(10, 22000)] public float highPassCutoff = 320;
    [Tooltip("Removes high frequencies from voices on both radios (Hz).")]
    [Range(10, 22000)] public float lowPassCutoff = 3400;
    [Tooltip("Radio voice distortion on both radios.")]
    [Range(0, 0.5f)] public float distortion = 0.08f;
    [Tooltip("Voice level on both radios; radio #4 still uses distance attenuation.")]
    [Range(0, 1)] public float voiceVolume = 0.6f;
    [Tooltip("Hiss level underneath recorded radio speech.")]
    [Range(0, 1)] public float hissBed = 0.02f;
    [Tooltip("Play radio_button_click before and radio_release_hiss after each recorded Lou line.")]
    public bool squelch = true;
    [SerializeField] AudioSource[] voices, hiss;
    [SerializeField] AudioSource controls;
    [SerializeField] RadioChannel radio;
    [SerializeField] Captions captions;
    bool talking, warnedMissing;
    bool Available(Object reference)
    {
        if (reference != null) return true;
        if (!warnedMissing) { warnedMissing = true; Debug.LogWarning("RadioVoiceSettings: optional source or filter missing; available radio audio continues.", this); }
        return false;
    }
    AudioClip click, release, noise;
    void Start()
    {
        if (!Available(radio)) return;
        click = radio.Placeholder("radio_button_click", 0.06f, "click");
        release = radio.Placeholder("radio_release_hiss", 0.25f, "hiss");
        noise = radio.Placeholder("radio_voice_hiss", 2, "hiss");
        foreach (AudioSource source in hiss ?? new AudioSource[0]) { if (!Available(source)) continue; source.clip = noise; source.loop = true; }
    }
    void Update()
    {
        foreach (AudioSource source in voices ?? new AudioSource[0])
        {
            if (!Available(source)) continue;
            source.volume = voiceVolume;
            var high = source.GetComponent<AudioHighPassFilter>();
            if (Available(high)) high.cutoffFrequency = highPassCutoff;
            var low = source.GetComponent<AudioLowPassFilter>();
            if (Available(low)) low.cutoffFrequency = lowPassCutoff;
            var drive = source.GetComponent<AudioDistortionFilter>();
            if (Available(drive)) drive.distortionLevel = distortion;
        }
        foreach (AudioSource source in hiss ?? new AudioSource[0])
        {
            if (!Available(source)) continue;
            source.volume = hissBed;
            var high = source.GetComponent<AudioHighPassFilter>();
            if (Available(high)) high.cutoffFrequency = highPassCutoff;
            var low = source.GetComponent<AudioLowPassFilter>();
            if (Available(low)) low.cutoffFrequency = lowPassCutoff;
            if (talking && !source.isPlaying) source.Play();
            if (!talking) source.Stop();
        }
    }
    public void Begin(bool lou, bool transmit)
    {
        talking = lou || transmit;
        if (lou && squelch) { if (Available(controls) && click != null) controls.PlayOneShot(click); if (Available(captions)) captions.Show("[radio: click]", 1); }
    }
    public void End(bool lou)
    {
        talking = false;
        foreach (AudioSource source in hiss ?? new AudioSource[0]) if (Available(source)) source.Stop();
        if (lou && squelch) { if (Available(controls) && release != null) controls.PlayOneShot(release); if (Available(captions)) captions.Show("[radio: short hiss]", 1); }
    }
    public void Cancel() { talking = false; foreach (AudioSource source in voices ?? new AudioSource[0]) if (Available(source)) source.Stop(); foreach (AudioSource source in hiss ?? new AudioSource[0]) if (Available(source)) source.Stop(); }
}
