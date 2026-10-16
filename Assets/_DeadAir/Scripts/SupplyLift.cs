using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SupplyLift : MonoBehaviour
{
    [SerializeField] RadioChannel radio;
    [SerializeField] VoiceLines voices;
    [SerializeField] Captions captions;
    [SerializeField] LiftDoor door;
    [SerializeField] PortableRadio portable;
    [SerializeField] RadiatorKnocks knocks;
    [SerializeField] LiftEnding ending;
    [SerializeField] Transform car, crank, pail, radioMount, lou, louRadioMount;
    [SerializeField] Transform player, view;
    [SerializeField] AudioSource rope, shaft, carSound, boiler, aquarium;
    [SerializeField] AudioClip lift_creak, lift_clunk, shaft_knock, witch_cackle;
    Vector3 top;
    float travel, direction = 1, lastCrank = -10, emptyAt = -1, goalAt = -1, nextAmbientCaption;
    bool afterCallDone, endpointWait;
    bool dead, goal, movedSinceGoal, heavyCaption, beaconBusy, pailSeen, endingStarted;
    bool hint4, hint5, hint6, hint7;
    public int BeaconStage { get; private set; }
    public bool LouInside => BeaconStage == 3;
    public bool AtTop => travel <= 0.0001f;
    public bool AtBottom => travel >= 0.9999f;
    public bool BeaconBusy => beaconBusy;
    public float Travel => travel;
    public bool GoalPlayed => goal;
#if UNITY_EDITOR
    static int requestedShortcut;
#endif

    System.Collections.IEnumerator Start()
    {
        while (!GameScreens.Started) yield return null;
        top = car.position;
        lou.gameObject.SetActive(false);
        if (lift_creak == null) lift_creak = radio.Placeholder("lift_creak", 1, "rattle");
        if (lift_clunk == null) lift_clunk = radio.Placeholder("lift_clunk", 0.45f, "thud");
        if (shaft_knock == null) shaft_knock = radio.Placeholder("pipe_knock", 0.7f, "clank");
        if (witch_cackle == null) witch_cackle = radio.Placeholder("witch_cackle", 1.2f, "cackle");
        rope.clip = lift_creak; rope.loop = true;
        boiler.clip = radio.Placeholder("boiler_roar_drip", 8, "background"); boiler.loop = true; boiler.Play();
        aquarium.clip = radio.Placeholder("aquarium_bubbling", 8, "library"); aquarium.loop = true; aquarium.volume = 0.08f; aquarium.Play();
#if UNITY_EDITOR
        if (requestedShortcut != 0) StartCoroutine(ApplyShortcut());
#endif
    }

    void Update()
    {
        if (!GameScreens.Started || GameScreens.Paused) return;
#if UNITY_EDITOR
        if (Keyboard.current != null)
        {
            if (Keyboard.current.f6Key.wasPressedThisFrame || Keyboard.current.f7Key.wasPressedThisFrame)
            {
                // Reloading also resets the ending locks and all hint timers.
                RestartForTest(Keyboard.current.f7Key.wasPressedThisFrame);
                return;
            }
        }
#endif
        if (lastCrank < Time.frameCount - 1) { endpointWait = false; if (rope.isPlaying) rope.Stop(); }
        if (dead && door.IsOpen && !goal) StartCoroutine(Goal());
        if (goalAt >= 0 && !hint4 && !movedSinceGoal && Time.time - goalAt >= 15 && !voices.Speaking)
        { hint4 = true; voices.Say("sam_h4"); }
        if (emptyAt >= 0 && BeaconStage == 0 && !voices.Speaking)
        {
            float waited = Time.time - emptyAt;
            if (!hint5 && waited >= 20) { hint5 = true; voices.Say("sam_h5"); }
            else if (!hint6 && waited >= 45) { hint6 = true; voices.Say("sam_h6"); }
            else if (!hint7 && waited >= 75) { hint7 = true; voices.Say("sam_h7"); }
        }
        if (!endingStarted && Time.time >= nextAmbientCaption)
        {
            nextAmbientCaption = Time.time + 15;
            if (Vector3.Distance(view.position, shaft.transform.position) < boiler.maxDistance && boiler.volume > 0.01f)
                captions.Ambient("[faint boiler roar and dripping, below the shaft]", 4);

        }
    }

    public bool Aim(RaycastHit hit, bool pressed, bool held, out string prompt)
    {
        prompt = "";
        if (!door.IsRevealed) return false;
        Transform target = hit.collider.transform;
        if (target == crank || target.IsChildOf(crank))
        {
            prompt = direction > 0 ? "Hold E: Lower the lift" : "Hold E: Raise the lift";
            if (held) Crank();
            return true;
        }
        if (target == car || target.IsChildOf(car))
        {
            if (!door.IsOpen || !AtTop) return true;
            if (portable.IsHeld) { prompt = "E: Put radio #4 in the lift"; if (pressed) { door.Touch(); portable.PlaceInCar(radioMount); } }
            else if (portable.InCar) { prompt = "E: Take radio #4"; if (pressed) { door.Touch(); portable.Take(); } }
            else if (target == pail || target.IsChildOf(pail)) { prompt = "E: Look at Lou's candy pail"; LookPail(); if (pressed) door.Touch(); }
            else { prompt = "E: Climb in"; if (pressed) { door.Touch(); if (!voices.Speaking) voices.Say("sam_09"); } }
            return true;
        }
        return false;
    }

    public void LookPail()
    {
        if (!pailSeen && !voices.Speaking) { pailSeen = true; voices.Say("sam_08"); }
        else if (pailSeen && !voices.Speaking) captions.Show("Lou's candy pail.", 2);
    }

    public void Crank()
    {
        if (endingStarted || beaconBusy) return;
        if (lastCrank < Time.frameCount - 1) door.Touch();
        lastCrank = Time.frameCount;
        if (endpointWait) return;
        if (door.IsOpen) { door.CloseForCrank(); return; }
        if (door.IsMoving) return;
        if (LouInside && direction < 0 && !heavyCaption)
        { heavyCaption = true; captions.Show("[The crank is heavy now.]", 4);
            captions.Show("[The boiler roar and dripping fade; aquarium bubbling returns.]", 7); }
        if (!rope.isPlaying) { rope.Play(); captions.Show("[rope creaks]", 3); }
        if (Time.frameCount % 90 == 0) captions.Show("[rope creaks]", 2);
        travel = Mathf.Clamp01(travel + direction * Time.deltaTime / (LouInside && direction < 0 ? 7 : 4));
        car.position = top + Vector3.down * (3 * travel);
        crank.Rotate(0, 0, direction * 150 * Time.deltaTime, Space.Self);
        if (goal) movedSinceGoal = true;
        if (LouInside) { boiler.volume = 0.22f * travel; aquarium.volume = Mathf.Lerp(0.35f, 0.08f, travel); }
        if ((direction > 0 && AtBottom) || (direction < 0 && AtTop))
        {
            rope.Stop(); carSound.PlayOneShot(lift_clunk); captions.Show("[lift clunks to a stop]", 3);
            if (AtBottom && !portable.InCar && emptyAt < 0) emptyAt = Time.time;
            direction = -direction; endpointWait = true;
        }
    }

    public void BatteryDead()
    {
        if (dead) return;
        dead = true; StartCoroutine(AfterClick());
    }
    IEnumerator AfterClick()
    {
        yield return new WaitForSeconds(2);
        while (radio.IsBusy || voices.Speaking) yield return null;
        yield return voices.Speak("sam_10", true);
        captions.Show("[No answer.]", 3);
        afterCallDone = true;
    }
    IEnumerator Goal()
    {
        goal = true;
        // The goal is queued, even if the player closes the door during another line.
        while (!afterCallDone || radio.IsBusy || voices.Speaking) yield return null;
        goalAt = Time.time; yield return voices.Speak("sam_11");
    }
    public void DoorOpened()
    {
        if (LouInside && AtTop && !endingStarted)
        {
            endingStarted = true; rope.Stop(); ending.Begin();
        }
    }
    public void Talk()
    {
        if (radio.State != RadioChannel.ChannelState.Silent || beaconBusy || voices.Speaking || endingStarted) return;
        StartCoroutine(Beacon());
    }
    IEnumerator Beacon()
    {
        beaconBusy = true;
        if (!portable.InCar || !AtBottom || LouInside)
        {
            yield return voices.Speak("sam_10", true); captions.Show("[No answer.]", 3);
            beaconBusy = false; yield break;
        }
        BeaconStage++;
        if (BeaconStage == 1) StartCoroutine(voices.Speak("sam_12", true));
        else yield return voices.Speak("sam_" + (11 + BeaconStage), true);
        if (BeaconStage == 1)
        {
            yield return new WaitForSeconds(1.5f); knocks.StopForever();
            captions.Show("[The radiator knocking stops.]", 3);
        }
        captions.Show(BeaconStage == 1 ? "[Three knocks, from somewhere below.]" : BeaconStage == 2 ? "[Three knocks. Closer.]" : "[One knock, right on the car. YES.]", 4);
        for (int i = 0; i < (BeaconStage == 3 ? 1 : 3); i++)
        {
            (BeaconStage == 3 ? carSound : shaft).PlayOneShot(shaft_knock, BeaconStage == 1 ? 0.35f : BeaconStage == 2 ? 0.65f : 1);
            yield return new WaitForSeconds(0.4f);
        }
        if (BeaconStage == 1)
        {
            shaft.PlayOneShot(witch_cackle, 0.25f); captions.Show("[distant witch cackle, below]", 3);
            yield return new WaitForSeconds(witch_cackle.length);
        }
        if (LouInside)
        {
            lou.gameObject.SetActive(true); lou.localScale = Vector3.one * 0.7f;
            portable.PlaceInCar(louRadioMount);
        }
        while (voices.Speaking) yield return null;
        beaconBusy = false;
    }
#if UNITY_EDITOR
    public void RestartForTest(bool ready)
    {
        requestedShortcut = ready ? 2 : 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    IEnumerator ApplyShortcut()
    {
        yield return null; // Let every component finish Start before changing the story.
        bool ready = requestedShortcut == 2; requestedShortcut = 0;
        Jump(ready);
    }

    public void Jump(bool ready)
    {
        radio.JumpToOpenFeed(); radio.GoSilent(); door.RevealInstantly();
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false; player.SetPositionAndRotation(door.Hatch.position + door.Hatch.forward * 1.8f + Vector3.down * (door.Hatch.position.y - 0.05f), Quaternion.LookRotation(-door.Hatch.forward));
        view.localRotation = Quaternion.identity; controller.enabled = true;
        if (ready)
        {
            travel = 1; direction = -1; car.position = top + Vector3.down * 3;
            portable.Reveal(); portable.PlaceInCar(radioMount);
        }
    }
#endif
}
