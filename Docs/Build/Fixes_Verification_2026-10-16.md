# Playtests 1–2 fixes — verification

This is an agent-driven Unity Editor verification, not an unbriefed human playtest. Unity MCP changed the scene and materials. Temporary C# routines outside Assets drove the CharacterController, invoked normal Interactable events / Talk / crank methods, and recorded the actual runtime results. No story shortcuts were used in the complete routes. Caption and voice timing ran at normal game speed.

## Changes

- `DeadAir.unity`: slatted crates and four wrapped loaves, hidden sideways PTA radio, dim red point light, four bangable shelves, desk bell, two bangable lockers, Rooms 11/14/15 fronts and plaques, three matching white sheets, running clock hands, lower-left radio display, darker lighting and imported Lou model/materials. References are wired in the scene. No package was added.
- `RadioChannel.cs`, `VoiceLines.cs`: voice-wide Talk guard, one warning per Lou line, feedback search loop, receive-light states, quieter hiss, graded reactions and shared repeat response, sound-role lookup.
- `NoiseTestObject.cs`, `RadioCrate.cs`, `SearchDecoy.cs`: noise grading and physical crate/sheet slides. Crates remain after lifting; radio #4 stays on the floor until taken.
- `LibraryEvents.cs`, `RadiatorKnocks.cs`, `SchoolClock.cs`, `SchoolLights.cs`: captions, five-second witch cooldown, timed chime, shared pipe knock and pumpkin flicker.
- `PortableRadio.cs`: held colliders off, camera parenting at `(0.09, -0.12, 0.22)`; camera near clip is 0.03.
- `SupplyLift.cs`: shaft/car knocks use `pipe_knock`, Lou uses uniform 0.7 scale in the car, aquarium captions use the shared wording. Lift motion, beacon requirements and end cards are retained.
- `LiftDoor.cs`, `LiftEnding.cs`: real sheet counts as a lift test; updated quiet pause, BOO/sting, spring, kick/shake and settle. Lou's supplied OBJ has one renderer with four named material slots; each has the requested URP material. The old primitive is removed. Its old white material is still used by the work-lamp bulb, so it is retained.
- `SoundClips.cs`, `Editor/AssignSoundClips.cs`, `Docs/DeadAir_Sounds.md`: 24 assignable non-voice sound roles. The menu was invoked with no SFX files and reported missing roles; placeholders remain.
- `Audio/SFX/README.txt` keeps the recording folder present in source control and explains assignments.
- Unity generated the new `.meta` files. Screenshots and runtime logs are in `Assets/Temp`.

## Replaced behavior

- Removed the library-door proximity trigger for midnight. Normal midnight now occurs after 120 seconds of gameplay, matching both clocks reaching 12:00.
- Replaced `radiator_clank`, `radiator_knock` and `shaft_knock` placeholder roles with the same metallic `pipe_knock` role.
- Replaced the radiator-only repeat reaction with one shared `lou_05`, once per game, for previously heard levels. Decoy sheets always get `lou_10`.
- Removed the basement door's first-use read interruption: E bangs immediately. Its sign remains on the door.
- Replaced the black lift sheet/paper bats with three matching white sheets, all using look-at interactions.
- Replaced the old primitive and 0.35-second lunge with the supplied ghost, quiet hold, 0.22-second spring, 0.25-second camera shake and settle.

The mix uses one global reduction: `AudioListener.volume = 0.67`. The generated library hiss is additionally quieter, as requested.

## Actual tests

A full unmuted route reached DEAD AIR at game time 165.18. Its voice history was:

`sam_00, sam_05, sam_05, sam_07, lou_00, sam_01, lou_01, sam_02, lou_02, lou_02b, lou_03, lou_04, lou_06, lou_05, lou_07, lou_08, lou_09, sam_06, lou_10, sam_06, lou_10, lou_11, sam_10, sam_11, sam_12, sam_13, sam_14, lou_12, lou_13, lou_14, sam_15`

It walked from Room 12 into the corridor/library and behind the rear shelf, tested both bread crates and the real crate, every noise level, both decoy sheets, the lift reaction, then loaded/lowered radio #4, completed all three beacon calls, raised Lou and completed the ending. Repeated `BeginTalk()` calls during lines did not cut or skip dialogue. Repeating Far produced `lou_05`; later repeated Near/Warm produced no extra line. Both decoy sheets produced `lou_10`.

An earlier run exposed the missing sting AudioSource after the old ghost was deleted. That reference was repaired, and the complete unmuted run above finished with no runtime errors. A subsequent lamp adjustment and finishing scene details are exercised by the muted run.

The muted run first left the stuck channel untouched for the clock check. Both displays advanced from 11:58 to 11:59 and then 12:00; the audit recorded both midnight captions together. It also recorded the live/radio witch wording, aquarium caption and feedback caption, with the receive light continuously green during LibraryFeed. The full corridor controller tour reached the front doors and returned. The muted route then completed every noise level, both decoy sheets, all three beacon calls and DEAD AIR at game time 307.38, with no runtime errors. It used the 15-second automatic advances for the middle exchanges. `MutedPlaythrough.txt` and `MutedCaptionAudit.txt` contain the route, dialogue/caption transitions and light states. All sound roles encountered had captions; the route completed without audio. This validates caption coverage, not whether an unfamiliar player will deduce the solution.

Held radio: the controller was pushed into the circulation desk, then the camera looked down 65 degrees. Renderer-bound overlap checks found no desk intersection; captures are saved. The close-wall/window check still found the yellow body intersecting the window collider. The requested camera parenting/collider/near-clip fix is in place, but this remaining clipping needs a separate URP overlay camera if approved. No overlay camera was built.

## Screenshots and lighting

| Capture in Assets/Temp | Assessment |
| --- | --- |
| `Room12.png` | Dim but navigable; furniture silhouettes and pact remain readable. |
| `Corridor.png` | Dark, with readable floor/walls and the EXIT/cafeteria pools providing direction. |
| `LibraryDoor.png` | Readable desk and shelf silhouettes; crate glow is hidden from the entrance. |
| `CrateCorner.png` | Darkest area; red leaks/slats read from about 2 m. Decoys take deliberate looking. Aim ray at this position found `E: Lift crate`. |
| `ScareClosest.png` | Captured at uniform scale 1.08, at the closest point. The revised work-lamp position makes the sheet/eye holes readable. No near-plane clipping is visible. The bright pool behind her is conspicuous, but her face is no longer lost in silhouette. |
| `HeldRadioWall.png`, `HeldRadioDesk.png`, `HeldRadioDeskDown.png` | Held-radio clearance checks. |

No extra fill light was added. Existing moonlight fills and lift visibility fill were disabled; ambient light was reduced. The existing work lamp was moved 0.6 m toward the library to light Lou's front after the first closest-moment capture showed an overly dark face against the lamp. No vignette/grain was added.

## Manual Editor work and how to test

No manual wiring is required. Open `Assets/_DeadAir/Scenes/DeadAir.unity` and enter Play mode. Use WASD/mouse, E for interactions, hold E at the crank, Q/right mouse for Talk and Esc for pause.

1. Watch the clocks for two minutes; at midnight verify the doubled caption while the crate is still covering #4. Follow the radio whine behind the rear shelf. Lift both bread crates, then the red-lit crate. Leave #4 on the floor through the conversation.
2. Spam Talk during every Sam/Lou line. Check one Lou warning per line, no cut-offs/skips, and the 15-second automatic advances when you stop pressing.
3. Bang a radiator, doors/lockers, pass the witch, ring the bell, bang front/rear shelves and crates, and pull both decoy sheets. Check faint → clearer → warm → loud feed captions and the one shared repeat response.
4. Pull the real sheet and touch the lift. After the battery dies, open the hatch for Sam's goal. Take #4, put it in the car, close/lower, Talk three times, raise and open. Watch the quiet pause, spring, settle and ending.
5. Repeat with sound muted; use only captions and receive light. Check the held radio against a wall and desk, including looking down.
6. To replace sounds, add original `<role>.wav` files to Audio/SFX, run Dead Air → Assign Sound Clips, and save the scene outside Play mode.

No standalone Windows/macOS/Linux builds, new-player discovery test, real-speaker/headphone listening assessment, or exhaustive input/pause/resolution combinations were tested. Keyboard simulation was attempted, but did not reliably move the player; movement clearance tests therefore used actual CharacterController.Move calls. The scripted interaction checks do not substitute for a complete human mouse/keyboard playthrough. New SFX recordings are absent, so assignment with real files remains untested.

## Final regression checks

After the full routes, the clock hands were changed to preserve each clock face's starting orientation. A further Play-mode inspection showed Room 12 at Y=0 and the library at Y=180 while both minute/hour angles advanced together. Timer/chime logic was unchanged. An ignored held Talk press now cannot block an automatic reply at the release-wait step. A focused virtual-keyboard check is recorded separately in `Assets/Temp/HeldTalkTimerTest.txt`.

The focused held-Talk test passed: a virtual Input System keyboard held Q after pressing during `lou_01`, with 531 frames sampled as held and cursor-locked. The automatic sequence still reached `sam_02` and `lou_02` without releasing Q. The virtual device was removed afterwards. Final scene audit found no missing MonoBehaviour scripts or required noise/decoy references; the build scene list contains only DeadAir.unity. Play mode was stopped and the temporary background-play setting restored.
