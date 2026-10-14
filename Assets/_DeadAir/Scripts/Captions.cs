using UnityEngine;
using TMPro;

public class Captions : MonoBehaviour
{
    [SerializeField] TMP_Text captionText;
    [SerializeField] GameObject background;
    float remaining;

    void Awake() { captionText.text = ""; background.SetActive(false); }

    public void Show(string text, float seconds)
    {
        captionText.text = text;
        remaining = Mathf.Max(0, seconds);
        background.SetActive(remaining > 0);
    }

    public void ShowFrontDoorMessage() { Show("Not without Lou.", 3); }

    void Update()
    {
        if (remaining <= 0) return;
        remaining -= Time.deltaTime;
        if (remaining <= 0) { captionText.text = ""; background.SetActive(false); }
    }
}
