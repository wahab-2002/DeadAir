using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] SupplyLift supplyLift;
    [SerializeField] FirstPersonController movement;
    [SerializeField] Camera view;
    [SerializeField] NoteViewer notes;
    [SerializeField] TMP_Text prompt;
    [SerializeField] GameObject dot;
    [SerializeField] float reach = 2.5f;
    [SerializeField] PortableRadio portableRadio;

    bool warnedPortable;
    public bool WithinBangRange(Interactable target, Collider surface)
    {
        if (target.Prompt != "E: Bang") return true;
        return Vector3.Distance(view.transform.position, surface.ClosestPoint(view.transform.position)) <= 1.2f;
    }

    void Update()
    {
        if (portableRadio == null && !warnedPortable)
        { warnedPortable = true; Debug.LogWarning("PlayerInteraction: optional portable radio reference missing; other interactions continue.", this); }
        if (!GameScreens.InputAllowed) return;
        prompt.text = "";
        dot.SetActive(!notes.IsOpen);
        if (movement.Locked || notes.IsOpen || notes.ClosedThisFrame || Cursor.lockState != CursorLockMode.Locked) return;
        Ray ray = new Ray(view.transform.position, view.transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, reach, ~0, QueryTriggerInteraction.Ignore)) return;
        if (supplyLift.Aim(hit, Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame, Keyboard.current != null && Keyboard.current.eKey.isPressed, out string liftPrompt))
        { prompt.text = liftPrompt; return; }
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
        if (!WithinBangRange(target, hit.collider)) return;
        prompt.text = target.Prompt;
        if (Keyboard.current == null) return;
        bool use = target.HoldE ? Keyboard.current.eKey.isPressed : Keyboard.current.eKey.wasPressedThisFrame;
        if (use) target.Interact();
    }
}
