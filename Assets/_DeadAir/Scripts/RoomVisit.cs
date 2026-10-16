using UnityEngine;

// Bounds are wired in the Inspector; no trigger physics or story locks.
public class RoomVisit : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Captions captions;
    [SerializeField] Vector3 minimum, maximum;
    [SerializeField] bool bookRoom;
    bool inside;
    public static bool EnteredBookRoom { get; private set; }
    void Awake() { if (bookRoom) EnteredBookRoom = false; }
    void Update()
    {
        if (!GameScreens.Started) return;
        Vector3 p = player.position;
        bool now = p.x > minimum.x && p.x < maximum.x && p.z > minimum.z && p.z < maximum.z;
        if (now && !inside) { captions.NewRoomVisit(); if (bookRoom) EnteredBookRoom = true; }
        inside = now;
    }
}
