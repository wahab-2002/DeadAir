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

    void Awake() { captionText.text = ""; background.SetActive(false); }

    // Keep concurrent dialogue and sound clues readable instead of replacing them.
    public void Show(string text, float seconds)
    {
        if (string.IsNullOrEmpty(text)) return;
        int index = messages.IndexOf(text);
        if (index < 0) { messages.Add(text); expiry.Add(Time.time + seconds); }
        else expiry[index] = Time.time + seconds;
        Refresh();
    }

    public void Clear() { messages.Clear(); expiry.Clear(); Refresh(); }
    public void ShowFrontDoorMessage() { voices.Say("sam_04"); }

    void Update()
    {
        for (int i = messages.Count - 1; i >= 0; i--)
            if (Time.time >= expiry[i]) { messages.RemoveAt(i); expiry.RemoveAt(i); }
        Refresh();
    }

    void Refresh()
    {
        captionText.text = string.Join("\n", messages);
        background.SetActive(messages.Count > 0);
    }
}
