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
    float started, nextCaption, nextTick;
    AudioClip tick;
    Quaternion minuteStart, hourStart;
    System.Collections.IEnumerator Start()
    {
        while (!GameScreens.Started) yield return null;
        started = Time.time;
        minuteStart = minuteHand.localRotation;
        hourStart = hourHand.localRotation;
        tick = radio.Placeholder("clock_tick", 0.06f, "click");
    }
    void Update()
    {
        if (!GameScreens.Started || GameScreens.Paused || tick == null) return;
        float seconds = 58 * 60 + Time.time - started;
        minuteHand.localRotation = minuteStart * Quaternion.Euler(0, 0, -seconds / 10);
        hourHand.localRotation = hourStart * Quaternion.Euler(0, 0, -330 - seconds / 120);
        timeText.text = ((11 + (int)(seconds / 3600)) % 12 == 0 ? 12 : (11 + (int)(seconds / 3600)) % 12).ToString() + ":" + ((int)(seconds / 60) % 60).ToString("00");
        if (!audible) return;
        if (Time.time >= nextTick) { nextTick = Time.time + 1; sound.PlayOneShot(tick, 0.2f); }
        if (Vector3.Distance(player.position, transform.position) <= 4 && Time.time >= nextCaption)
        { captions.Show("[A clock ticks.]", 3); nextCaption = Time.time + 30; }
    }
}
