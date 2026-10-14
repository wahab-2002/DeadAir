using UnityEngine;

public class ReadableNote : MonoBehaviour
{
    [SerializeField] NoteViewer viewer;
    [TextArea(3, 12)] [SerializeField] string text;
    public void Read() { viewer.Open(text); }
}
