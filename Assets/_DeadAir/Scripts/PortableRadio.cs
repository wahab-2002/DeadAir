using UnityEngine;
using TMPro;

public class PortableRadio : MonoBehaviour
{
    [SerializeField] Transform hand;
    [SerializeField] TMP_Text holdingText;
    [SerializeField] Collider radioCollider;
    [SerializeField] Interactable takeInteraction;
    [SerializeField] Vector3 heldPosition = new Vector3(0.28f, -0.28f, 0.45f);
    [SerializeField] Vector3 halfSize = new Vector3(0.11f, 0.14f, 0.07f);
    public bool IsHeld { get; private set; }

    public void Reveal() { takeInteraction.enabled = true; }

    public void Take()
    {
        if (!takeInteraction.enabled || IsHeld) return;
        IsHeld = true;
        radioCollider.enabled = false;
        takeInteraction.enabled = false;
        transform.SetParent(hand, false);
        transform.localPosition = heldPosition;
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        holdingText.text = "Holding: PTA radio #4";
    }

    public bool CanPlace(RaycastHit hit)
    {
        if (!IsHeld || hit.normal.y < 0.95f) return false;
        // Keep the upright radio clear of walls, props and Sam's body.
        Vector3 centre = hit.point + Vector3.up * (halfSize.y + 0.015f);
        return !Physics.CheckBox(centre, halfSize, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);
    }

    public void PutDown(RaycastHit hit)
    {
        if (!CanPlace(hit)) return;
        transform.SetParent(null, true);
        transform.SetPositionAndRotation(hit.point + Vector3.up * (halfSize.y + 0.015f), Quaternion.identity);
        IsHeld = false;
        radioCollider.enabled = true;
        takeInteraction.enabled = true;
        holdingText.text = "";
    }
}
