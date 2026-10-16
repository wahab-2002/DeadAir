using UnityEngine;
using TMPro;

public class PortableRadio : MonoBehaviour
{
    [SerializeField] Transform hand;
    [SerializeField] TMP_Text holdingText;
    [SerializeField] Collider radioCollider;
    [SerializeField] Interactable takeInteraction;
    [SerializeField] Vector3 heldPosition = new Vector3(0.10f, -0.12f, 0.22f);
    [SerializeField] Vector3 halfSize = new Vector3(0.11f, 0.14f, 0.07f);
    Transform[] radioParts;
    int[] originalLayers;
    void Awake()
    {
        radioParts = GetComponentsInChildren<Transform>(true);
        originalLayers = new int[radioParts.Length];
        for (int i = 0; i < radioParts.Length; i++) originalLayers[i] = radioParts[i].gameObject.layer;
    }
    void SetHeldLayer(bool held)
    {
        for (int i = 0; i < radioParts.Length; i++)
            radioParts[i].gameObject.layer = held ? LayerMask.NameToLayer("HeldItem") : originalLayers[i];
    }
    public bool InCar { get; private set; }
    public void PlaceInCar(Transform mount)
    {
        InCar = true; IsHeld = false;
        SetHeldLayer(false);
        transform.SetParent(mount, false);
        transform.localPosition = Vector3.zero; transform.localRotation = Quaternion.identity;
        foreach (Collider item in GetComponentsInChildren<Collider>()) item.enabled = true; takeInteraction.enabled = true;
        holdingText.text = "";
    }
    public bool IsHeld { get; private set; }

    public void Reveal() { takeInteraction.enabled = true; }

    public void Take()
    {
        if (!takeInteraction.enabled || IsHeld) return;
        InCar = false;
        IsHeld = true;
        SetHeldLayer(true);
        foreach (Collider item in GetComponentsInChildren<Collider>()) item.enabled = false;
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
        SetHeldLayer(false);
        foreach (Collider item in GetComponentsInChildren<Collider>()) item.enabled = true;
        takeInteraction.enabled = true;
        holdingText.text = "";
    }
}
