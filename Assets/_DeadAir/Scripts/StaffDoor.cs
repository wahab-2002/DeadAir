using System.Collections;
using UnityEngine;

public class StaffDoor : MonoBehaviour
{
    [SerializeField] Transform door;
    [SerializeField] Interactable interaction;
    [SerializeField] Captions captions;
    bool open, moving;
    public bool IsOpen => open;

    public void Use()
    {
        if (moving) return;
        open = !open;
        interaction.SetPrompt(open ? "E: Close" : "E: Open");
        captions.Show(open ? "[The staff door opens.]" : "[The staff door closes.]", 3);
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        moving = true;
        Quaternion start = door.localRotation;
        Quaternion end = Quaternion.Euler(0, open ? -90 : 0, 0);
        for (float t = 0; t < 0.6f; t += Time.deltaTime)
        {
            door.localRotation = Quaternion.Slerp(start, end, t / 0.6f);
            yield return null;
        }
        door.localRotation = end;
        moving = false;
    }
}
