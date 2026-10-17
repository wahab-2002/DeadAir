using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] AudioSource music;
    [SerializeField] VoiceLines voices;
    [Tooltip("Normal music level before ducking.")]
    [SerializeField, Range(0, 1)] float volume = 0.08f;
    float duck = 1, fade = 1;
    bool silent, warnedMissing;
    void Start() { if (music != null) music.Play(); }
    void Update()
    {
        if (music == null || voices == null)
        {
            if (!warnedMissing) { warnedMissing = true; Debug.LogWarning("BackgroundMusic: music or voice reference missing; available music continues without ducking.", this); }
            if (music == null) return;
        }
        if (GameScreens.Paused) return;
        float dt = Time.unscaledDeltaTime;
        bool speaking = voices != null && voices.Speaking;
        duck = Mathf.MoveTowards(duck, speaking ? 0.4f : 1, dt * 0.6f / (speaking ? 0.5f : 1.5f));
        fade = Mathf.MoveTowards(fade, silent ? 0 : 1, dt / 3);
        music.volume = volume * duck * fade;
    }
    public void BeforeScare() { silent = true; }
    public void Credits() { silent = false; if (music != null && !music.isPlaying) music.Play(); }
}
