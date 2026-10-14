using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NoteViewer : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text noteText;
    int openedFrame = -1;
    int closedFrame = -1;
    public bool IsOpen => panel.activeSelf;
    public bool ClosedThisFrame => closedFrame == Time.frameCount;

    void Awake() { panel.SetActive(false); }

    public void Open(string text)
    {
        noteText.text = text;
        panel.SetActive(true);
        openedFrame = Time.frameCount;
    }

    void Update()
    {
        if (!IsOpen || Time.frameCount == openedFrame || Keyboard.current == null) return;
        if (Keyboard.current.eKey.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            panel.SetActive(false);
            closedFrame = Time.frameCount;
        }
    }
}
