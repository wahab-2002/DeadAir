using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] string prompt = "E  Read";
    [SerializeField] bool holdE;
    [SerializeField] UnityEvent onInteract = new UnityEvent();

    public string Prompt => prompt;
    public void SetPrompt(string text) { prompt = text; }
    public bool HoldE => holdE;
    public UnityEvent OnInteract => onInteract;
    // Hold mode invokes once per frame while aimed at and holding E.
    // A later crank listener can use Time.deltaTime to move at a steady speed.
    public void Interact() { onInteract.Invoke(); }
}
