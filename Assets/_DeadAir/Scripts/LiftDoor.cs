using System.Collections;
using UnityEngine;

public class LiftDoor : MonoBehaviour
{
    [SerializeField] SupplyLift supplyLift;
    [SerializeField] Transform door, sheet;
    [SerializeField] Collider sheetCollider;
    [SerializeField] Interactable doorInteraction, sheetInteraction;
    [SerializeField] AudioSource sound;
    [SerializeField] RadioChannel radio;
    [SerializeField] Captions captions;
    [SerializeField] AudioClip lift_rattle, sheet_rustle;
    Vector3 closedPosition;
    bool revealed, open, moving;
    public bool DoorTouched { get; private set; }
    public bool IsRevealed => revealed;
    public bool IsOpen => open;
    public bool IsMoving => moving;
    public void Touch() { Rattle("[the lift rattles]"); }
    public void CloseForCrank() { if (open && !moving) ToggleDoor(); }
#if UNITY_EDITOR
    public void RevealInstantly()
    {
        revealed = true; sheet.gameObject.SetActive(false); doorInteraction.enabled = true;
    }
#endif

    void Start()
    {
        closedPosition = door.position;
        if (lift_rattle == null) lift_rattle = radio.Placeholder("lift_rattle", 0.8f, "rattle");
        if (sheet_rustle == null) sheet_rustle = radio.Placeholder("sheet_rustle", 0.7f, "scrape");
    }

    public void PullSheet()
    {
        if (revealed || moving) return;
        DoorTouched = true;
        radio.LoudTest(lift_rattle);
        moving = true;
        sheetInteraction.enabled = false;
        StartCoroutine(Reveal());
    }

    IEnumerator Reveal()
    {
        sound.PlayOneShot(sheet_rustle);
        captions.Show("[The sheet rustles aside.]", 3);
        yield return Slide(sheet, sheet.position + Vector3.left * 1.5f);
        revealed = true;
        sheetCollider.enabled = false;
        doorInteraction.enabled = true;
        moving = false;
        Rattle("[the sheet bumps the small door: a wooden rattle]");
    }

    public void ToggleDoor()
    {
        if (!revealed || moving || !supplyLift.AtTop) return;
        DoorTouched = true;
        open = !open;
        if (open) supplyLift.DoorOpened();
        doorInteraction.SetPrompt(open ? "E: Close" : "E: Open");
        Rattle("[the small door rattles]");
        StartCoroutine(MoveDoor());
    }

    IEnumerator MoveDoor()
    {
        moving = true;
        // A sliding placeholder hatch keeps the tiny opening clear.
        yield return Slide(door, closedPosition + (open ? Vector3.right * 0.7f : Vector3.zero));
        moving = false;
    }

    IEnumerator Slide(Transform target, Vector3 end)
    {
        Vector3 start = target.position;
        float elapsed = 0;
        while (elapsed < 0.7f)
        {
            elapsed += Time.deltaTime;
            target.position = Vector3.Lerp(start, end, Mathf.Clamp01(elapsed / 0.7f));
            yield return null;
        }
        target.position = end;
    }

    void Rattle(string caption)
    {
        sound.PlayOneShot(lift_rattle);
        captions.Show(caption, 3);
        radio.LoudTest(lift_rattle);
    }
}
