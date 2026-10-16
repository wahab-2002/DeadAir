# Playtest 3 — level pass verification

Scene: `Assets/_DeadAir/Scenes/DeadAir.unity`, Unity 6000.3.26f1, October 16, 2026.

The library remains 8 × 10 m. The Book Room occupies the west side, opposite the basement door, and is about 3 × 7 m. Its staff door is unlocked and left open. Props follow the mirrored drawn plan. The step stool is tucked toward the shelf edge to keep the aisle clear. Ordinary props under 1.6 m from the hatch use Warm; the prompt did not assign that distance to a separate ordinary-prop level.

The story, dialogue wording, beacon sequence and ending remain intact. `sam_h8` has its requested runtime placeholder. The user's pre-existing voice-line document changes were preserved.

## Changes

- Scene: Book Room, entire lift assembly and shaft audio relocated, library furniture and Book Fair clutter, corridor lockers and carnival dressing, Room 12 desks and dressing. Kenney furniture uses persisted URP materials; its licence remains in its source folder. Sheets have separate locations, draped meshes and permanent floor drops. Moving containers are excluded from static batching.
- Runtime scripts changed: `Captions`, `GameScreens`, `LibraryEvents`, `LiftDoor`, `NoiseTestObject`, `RadiatorKnocks`, `RadioChannel`, `SchoolClock`, `SearchDecoy`, `SupplyLift`, `VoiceLines`.
- Runtime scripts added: `RoomVisit`, `NearbyRadioGlow`. `LevelPassProbe` is guarded by `UNITY_EDITOR` and is excluded from builds.
- Editor helpers: `Scripts/Editor/LevelPass.cs` and `LevelPassFinish.cs`. These authored the scene through Unity MCP. The saved scene is the deliverable; do not rerun the initial construction helper on it.
- Assets added: Kenney import metadata, `Materials/LevelPass/` URP materials and small generated meshes, and screenshots in `Assets/Temp/`.
- Only DeadAir is enabled in the build scene list; there are no separate BuildProfile assets overriding that list.

## Actual checks

These are Editor Play-mode tests driven through Unity MCP and public interaction methods. The story probe calls existing methods and waits for their real coroutines; it does not walk a human player through the level or simulate reading notes.

1. **Screenshots:** `Assets/Temp/PT3_*.png`: library doorway; Book Fair from 2 m and 0.8 m; four aisles; shared-wall shelf; Book Room doorway; both corridor ends; Room 12 cubby. Additional title, F6 and credits captures. World screenshots use the player camera at Sam's height; title/credits use the Game view with UI.
2. **Crate-to-hatch distance:** 7.406922 m in 3D (about 7.30 m horizontally). Crate R is at (13.00, 0.18, −5.90); hatch at (5.90, 1.43, −7.60).
3. **Noise levels:** see table below. The voice repeat rule can replace a previously heard level's line with `lou_05`, once per game, then no further voice. Through-wall and decoy reactions remain `lou_10` every time.
4. **Sheets:** tested orders 012, 021, 102, 120, 201, 210, where 0/1 are decoys and 2 is the lift. All revealed the hatch; all three heap bounds stayed separate; all pulled sheet colliders were disabled. Repeated after the draped meshes were added.
5. **Lift:** empty-car hints `sam_h5`, `sam_h6`, `sam_h7`; beacon stages 1, 2, 3; heavy raise approximately 7 s; jump-out, final dialogue and credits. F6/F7 exact reload handlers were invoked, then Start: both land at (4.10, 0.02, −7.60), facing east at the hatch. F6: Silent, hatch revealed, car at top. F7: Silent, radio #4 in the car at bottom, stage 0.
6. **Title/audio/timing:** fresh title had only `faint_room_tone` playing, at 0.025 volume. Sam's library/open feed source is halved. Observed radiator intervals included 41.37 and 43.22 game seconds; each ordinary cycle now calls only one three-knock sequence. An attempted Talk immediately after Lou ended was ignored, with about 1.39 s of cooldown remaining.
7. **Reachability:** capsule clearance scan (0.45 m radius), connected on a 0.2 m grid from the corridor, followed by 2.5 m interaction raycasts: all 102 enabled targets had reachable approaches. Repeated with the hatch open: 102 passed, zero failures. The scan caught and prompted fixes for the pact/radiator approaches, Book Room clearance, bin placement and the open hatch sliding behind the shared wall.
8. **Performance:** five-second library sample averaged 94.16 FPS in this Editor session. Render timing snapshots ranged from about 6.8 to 14.1 ms CPU, with the latter GPU sample about 13.2 ms. These are local Editor observations, not platform build benchmarks.

| Target | Level | First response for that level |
| --- | --- | --- |
| Desk bell | Warm | `lou_09` |
| Card catalogue | Near | `lou_08` |
| S1 | Warm | `lou_09` |
| S3 | Warm | `lou_09` |
| Shared-wall shelf behind lift | Through the wall | `lou_10`, every time |
| Book Room shelf nearest door | Near | `lou_08` |
| Decoy 0 | Decoy | `lou_10`, every time |
| Decoy 1 | Decoy | `lou_10`, every time |
| Lift sheet | Lift | `lou_11`, then Silent |
| Corridor locker | Far | `lou_06` |
| Radiator | Pipe | `lou_04`, with three knocks back |

The glow was captured disabled at 2 m and enabled at 0.8 m. Both its 0.4 m point light and the tiny LED are gated by proximity; the close capture shows red through the slats and the distant capture does not.

## Manual Editor work and remaining limits

No Inspector wiring or manual scene construction is required. Open DeadAir and press Play, then Start. Use WASD/mouse and E to check the fair containers, shelves, all three sheet pulls and the relocated crank. Hold Q/right mouse through the radio exchange, place #4 in the car, lower it, call three times, raise it and open the hatch. F6/F7 reload the scene; press Start again after either shortcut. Depending on keyboard settings, physical function keys may require Fn.

Still untested: a continuous human keyboard-and-mouse playthrough, physical F6/F7 delivery (MCP key injection was unreliable), subjective clue concealment and sound balance, prompt comfort at every approach, long performance runs, and Windows/macOS/Linux standalone builds. No builds were made for this pass. The collision scan establishes approaches and raycasts, not a human navigation/comprehension result.

Source/doc whitespace checks passed. Unity's generated scene YAML contains its usual blank-value trailing spaces; it was not hand-edited to remove them.

Final run log and final checks are appended below.

The final lift sheet was widened to hide both the hatch and crank from the Book Room doorway; direct rays to both first hit the sheet. All six pull orders were repeated after this adjustment:

- Sheets 0,1,2: revealed=True, heaps separate=True, sheet colliders=0
- Sheets 0,2,1: revealed=True, heaps separate=True, sheet colliders=0
- Sheets 1,0,2: revealed=True, heaps separate=True, sheet colliders=0
- Sheets 1,2,0: revealed=True, heaps separate=True, sheet colliders=0
- Sheets 2,0,1: revealed=True, heaps separate=True, sheet colliders=0
- Sheets 2,1,0: revealed=True, heaps separate=True, sheet colliders=0
- Sheet orders complete; fresh title ready

The north corridor locker banks were fitted between the existing windows/radiators and cafeteria opening. The last saved-scene scan passed all 102 initially enabled targets with zero failures. The 103-target result above includes radio #4 after freeing it. All requested screenshots were refreshed; `PT3_book_room_door.png` shows the final cover.

The close radio test also confirmed LED visible before lifting, LED off afterward, and its glow object inactive. The final scripts compiled successfully; the final Play session produced no new Console warnings or errors. Play mode was stopped for handoff. No changes were staged or committed.

After the story run, the distance getter was expanded into simple `if` statements, the LED was made to turn off when the crate disables its glow object, and the stool was tucked farther under the shelf edge. The focused noise/caption/glow checks and final reachability scan followed those local changes.

Final scan with radio #4 available: 103 passed, zero failures. Separate rays from the relocated lift approach first hit the crank (1.68 m), pail (1.96 m) and car floor (1.93 m). No intervening wall blocked them.

Final focused audit:

- Noise audit Desk bell: level=5; response=lou_09
- Noise audit Card catalogue: level=4; response=lou_08
- Noise audit Library/Props/Stacks/S1/Double-sided stack: level=5; response=lou_05
- Noise audit Library/Props/Stacks/S3/Double-sided stack: level=5; response=
- Noise audit Shared-wall shelf 1: level=6; response=lou_10
- Noise audit Metal textbook shelf 0: level=4; response=
- Noise audit Decoy sheet 0: level=6; response=lou_10
- Noise audit Decoy sheet 1: level=6; response=lou_10
- Noise audit Locker: level=2; response=lou_06
- Noise audit Radiator: level=0; response=lou_04
- Noise audit lift sheet: Silent; lou_11=True
- Caption priority/two-caption cap=True
- Ambient suppressed on repeat=True
- Ambient returns on next visit=True

The uninterrupted Start-to-credits probe completed. Its observed results:

- Talk blocked after Lou: True, remaining cooldown=1.343174
- Radiator interval=42.32886
- Start, stuck crate and conversations reached open feed
- Desk bell: level=5, response=lou_09
- Card catalogue: level=4, response=lou_08
- Shared-wall shelf 1: level=6, response=lou_10
- Metal textbook shelf 0: level=4, response=lou_05
- Decoy sheet 0: level=6, response=lou_10
- Decoy sheet 1: level=6, response=lou_10
- Radiator: level=0, response=lou_04
- Radiator interval=45.47377
- Empty bottom hints: sam_h5,sam_h6,sam_h7
- Beacon stage=1
- Beacon stage=2
- Beacon stage=3
- Heavy raise seconds=7.003632
- Ending completed; lines=sam_00,sam_07,lou_00,sam_01,lou_01,sam_02,lou_02,lou_02b,lou_03,lou_09,lou_08,lou_10,lou_05,lou_10,lou_10,lou_04,lou_11,sam_h8,sam_10,sam_11,sam_h5,sam_h6,sam_h7,sam_12,sam_13,sam_14,lou_12,lou_13,lou_14,sam_15
