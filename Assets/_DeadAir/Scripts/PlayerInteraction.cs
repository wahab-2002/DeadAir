using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera view;
    [SerializeField] NoteViewer notes;
    [SerializeField] TMP_Text prompt;
    [SerializeField] GameObject dot;
    [SerializeField] float reach = 2.5f;
    [SerializeField] PortableRadio portableRadio;

    void Update()
    {
        prompt.text = "";
        dot.SetActive(!notes.IsOpen);
        if (notes.IsOpen || notes.ClosedThisFrame || Cursor.lockState != CursorLockMode.Locked) return;
        Ray ray = new Ray(view.transform.position, view.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, reach, ~0, QueryTriggerInteraction.Ignore)) return;
        Interactable target = hit.collider.GetComponentInParent<Interactable>();
        if (target == null)
        {
            if (portableRadio != null && portableRadio.CanPlace(hit))
            {
                prompt.text = "E  Put radio down";
                if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) portableRadio.PutDown(hit);
            }
            return;
        }
        if (!target.enabled) return;
        prompt.text = target.Prompt;
        if (Keyboard.current == null) return;
        bool use = target.HoldE ? Keyboard.current.eKey.isPressed : Keyboard.current.eKey.wasPressedThisFrame;
        if (use) target.Interact();
    }
}
