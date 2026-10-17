using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Captions : MonoBehaviour
{
    [SerializeField] VoiceLines voices;
    [SerializeField] TMP_Text captionText;
    [SerializeField] GameObject background;
    readonly List<string> messages = new List<string>();
    readonly List<float> expiry = new List<float>();

    readonly HashSet<string> ambient = new HashSet<string>();
    string dialogue = "";
    float dialogueUntil;
    public void NewRoomVisit() { ambient.Clear(); }
    public void Ambient(string text, float seconds)
    {
        if (!GameScreens.Started || Time.time < dialogueUntil || ambient.Contains(text)) return;
        ambient.Add(text); Show(text, seconds);
    }
    public void Dialogue(string text, float seconds)
    {
        dialogue = text; dialogueUntil = Time.time + seconds;
        messages.Clear(); expiry.Clear(); Refresh();
    }

    void Awake() { captionText.text = ""; background.SetActive(false); }

    // Keep concurrent dialogue and sound clues readable instead of replacing them.
    public void Show(string text, float seconds)
    {
        if (string.IsNullOrEmpty(text)) return;
        int index = messages.IndexOf(text);
        if (index < 0 && messages.Count >= (Time.time < dialogueUntil ? 1 : 2)) { messages.RemoveAt(0); expiry.RemoveAt(0); }
        if (index < 0) { messages.Add(text); expiry.Add(Time.time + seconds); }
        else expiry[index] = Time.time + seconds;
        Refresh();
    }

    public void Clear() { messages.Clear(); expiry.Clear(); dialogue = ""; dialogueUntil = 0; Refresh(); }
    public void ShowFrontDoorMessage() { voices.Say("sam_04"); }

    void Update()
    {
        for (int i = messages.Count - 1; i >= 0; i--)
            if (Time.time >= expiry[i]) { messages.RemoveAt(i); expiry.RemoveAt(i); }
        Refresh();
    }

    void Refresh()
    {
        captionText.text = (Time.time < dialogueUntil ? dialogue + (messages.Count > 0 ? "\n" : "") : "") + string.Join("\n", messages);
        background.SetActive(messages.Count > 0 || Time.time < dialogueUntil);
    }
}
