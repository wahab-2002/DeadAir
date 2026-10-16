using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
public class GameScreens : MonoBehaviour
{
    [SerializeField] CanvasGroup title, pause, credits;
    [SerializeField] GameObject hud;
    [SerializeField] FirstPersonController movement;
    [SerializeField] AudioSource titleHiss;
    [SerializeField] LiftEnding ending;
    public static bool Started { get; private set; }
    public static bool Paused { get; private set; }
    public static bool InputAllowed { get; private set; }
    [SerializeField] GameObject radioHint;
    [SerializeField] RadioChannel radio;
    [SerializeField] CanvasGroup storyCard;
    [SerializeField] TMPro.TMP_Text storyPrompt;
    bool storyReady, continueStory;
    float gameplayAt;
    bool fading, ended, previousMovementLock;

    void Awake()
    {
        Started = Paused = InputAllowed = false;
        Time.timeScale = 0; AudioListener.pause = false;
        title.gameObject.SetActive(true); title.alpha = 1;
        pause.gameObject.SetActive(false); credits.gameObject.SetActive(false);
        hud.SetActive(false); movement.Locked = true;
        radioHint.SetActive(false);
        storyCard.gameObject.SetActive(false);
    }
    void Start()
    {
        SetCursor(true);
        foreach (AudioSource source in FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None)) source.Stop();
        titleHiss.clip = FindFirstObjectByType<RadioChannel>().Placeholder("faint_room_tone", 4, "hiss");
        titleHiss.volume = 0.025f; titleHiss.loop = true; titleHiss.Play();
    }
    void Update()
    {
        radioHint.SetActive(Started && InputAllowed && !ended && !ending.Finished && !radio.HasTalked && Time.time - gameplayAt >= 5);
        if (storyReady && Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ContinueStory();
        if (!Started || fading || ended || ending.Finished) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (Paused) Resume(); else Pause();
        }
    }
    public void StartGame()
    {
        if (Started || fading) return;
        fading = true;
        titleHiss.Stop(); StartCoroutine(FadeTitle());
    }
    IEnumerator FadeTitle()
    {
        title.interactable = false;
        storyCard.gameObject.SetActive(true); storyCard.alpha = 0; storyPrompt.text = "";
        for (float t = 0; t < 1; t += Time.unscaledDeltaTime)
        { title.alpha = 1 - t; storyCard.alpha = t; yield return null; }
        title.gameObject.SetActive(false); storyCard.alpha = 1;
        yield return new WaitForSecondsRealtime(4);
        storyPrompt.text = "Press any key"; storyReady = true;
        while (!continueStory) yield return null;
        storyReady = false;
        // Scaled time stays frozen through the card and the fade into Room 12.
        for (float t = 0; t < 1; t += Time.unscaledDeltaTime)
        { storyCard.alpha = 1 - t; yield return null; }
        storyCard.gameObject.SetActive(false); hud.SetActive(true);
        Started = true; Time.timeScale = 1;
        gameplayAt = Time.time;
        movement.Locked = false; InputAllowed = true; fading = false; SetCursor(false);
    }
    public void ContinueStory()
    {
        if (storyReady) continueStory = true;
    }

    public void Pause()
    {
        if (!Started || fading || ended || Paused) return;
        Paused = true; InputAllowed = false;
        previousMovementLock = movement.Locked; movement.Locked = true;
        Time.timeScale = 0; AudioListener.pause = true;
        hud.SetActive(false); pause.gameObject.SetActive(true); SetCursor(true);
    }
    public void Resume()
    {
        if (!Paused) return;
        Paused = false; InputAllowed = true;
        movement.Locked = previousMovementLock;
        Time.timeScale = 1; AudioListener.pause = false;
        pause.gameObject.SetActive(false); hud.SetActive(true); SetCursor(false);
    }
    public IEnumerator ShowCredits()
    {
        ended = true; InputAllowed = false; movement.Locked = true;
        credits.alpha = 0; credits.gameObject.SetActive(true); SetCursor(true);
        for (float t = 0; t < 1; t += Time.unscaledDeltaTime)
        { credits.alpha = t; yield return null; }
        credits.alpha = 1;
    }
    public void Restart()
    {
        Time.timeScale = 1; AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Time.timeScale = 1; AudioListener.pause = false;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    void SetCursor(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
    }
    void OnDestroy() { Time.timeScale = 1; AudioListener.pause = false; }
}
