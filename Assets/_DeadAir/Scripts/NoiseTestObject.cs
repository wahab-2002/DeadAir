using System.Collections;
using UnityEngine;

public class NoiseTestObject : MonoBehaviour
{
    public enum Kind { Radiator, BasementDoor }
    [SerializeField] Kind kind;
    [SerializeField] RadioChannel radio;
    [SerializeField] Captions captions;
    [SerializeField] RadiatorKnocks knocks;
    [SerializeField] LibraryEvents library;
    [SerializeField] AudioSource sound;
    [SerializeField] Interactable interaction;
    [SerializeField] ReadableNote sign;
    [SerializeField] AudioClip localClip;
    bool signShown;
    float nextBang;

    void Start()
    {
        if (localClip == null) localClip = radio.Placeholder(kind == Kind.Radiator ? "radiator_clank" : "door_thud", 0.7f, kind == Kind.Radiator ? "clank" : "thud");
    }

    public void Use()
    {
        if (kind == Kind.BasementDoor && !signShown)
        {
            signShown = true;
            sign.Read();
            interaction.SetPrompt("E  Bang on door");
            return;
        }
        if (Time.time < nextBang) return;
        nextBang = Time.time + 2.8f;
        sound.PlayOneShot(localClip);
        captions.Show(kind == Kind.Radiator ? "[a metallic clank at the radiator]" : "[a heavy thud at the basement door]", 3);
        if (kind == Kind.Radiator) knocks.Reply();
        if (radio.State == RadioChannel.ChannelState.LouOpenFeed && !radio.BatteryEnding)
        {
            library.MarkTest();
            StartCoroutine(Echo());
        }
    }

    IEnumerator Echo()
    {
        yield return new WaitForSeconds(0.4f);
        if (radio.State != RadioChannel.ChannelState.LouOpenFeed || radio.BatteryEnding) yield break;
        radio.FeedSound(localClip, kind == Kind.Radiator ? "[Lou's radio: a faint clank]" : "[Lou's radio: a faint thud]", 0.18f);
        yield return new WaitForSeconds(localClip.length);
        radio.ReactToTest(kind == Kind.Radiator ? 0 : 2);
    }
}
