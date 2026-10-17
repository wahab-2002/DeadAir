using UnityEngine;
using TMPro;

public class SchoolClock : MonoBehaviour
{
    [SerializeField] Transform minuteHand, hourHand;
    [SerializeField] TMP_Text timeText;
    [SerializeField] Transform player;
    [SerializeField] Captions captions;
    [SerializeField] bool audible;
    [SerializeField] AudioSource sound;
    [SerializeField] RadioChannel radio;
    bool warnedMissing;
    float started, nextCaption, nextTick;
    AudioClip tick;
    Quaternion minuteStart, hourStart;
    System.Collections.IEnumerator Start()
    {
        while (!GameScreens.Started) yield return null;
        started = Time.time;
        if (minuteHand != null) minuteStart = minuteHand.localRotation;
        if (hourHand != null) hourStart = hourHand.localRotation;
        if (radio != null) tick = radio.Placeholder("clock_tick", 0.06f, "click");
    }
    void Update()
    {
        if ((minuteHand == null || hourHand == null || timeText == null || (audible && (sound == null || radio == null || player == null || captions == null))) && !warnedMissing)
        { warnedMissing = true; Debug.LogWarning("SchoolClock: optional clock reference missing; available clock features continue.", this); }
        if (!GameScreens.Started || GameScreens.Paused) return;
        float seconds = 58 * 60 + Time.time - started;
        if (minuteHand != null) minuteHand.localRotation = minuteStart * Quaternion.Euler(0, 0, -seconds / 10);
        if (hourHand != null) hourHand.localRotation = hourStart * Quaternion.Euler(0, 0, -330 - seconds / 120);
        if (timeText != null) timeText.text = ((11 + (int)(seconds / 3600)) % 12 == 0 ? 12 : (11 + (int)(seconds / 3600)) % 12).ToString() + ":" + ((int)(seconds / 60) % 60).ToString("00");
        if (!audible) return;
        if (sound != null && tick != null && Time.time >= nextTick) { nextTick = Time.time + 1; sound.PlayOneShot(tick, 0.2f); }
        if (player != null && captions != null && Vector3.Distance(player.position, transform.position) <= 4 && Time.time >= nextCaption)
        { captions.Ambient("[A clock ticks.]", 3); nextCaption = Time.time + 30; }
    }
}
