# Step 5 verification — October 16, 2026

## Changes

- `GameScreens.cs`: title, one-second Start fade, pause/resume, reload, credits fade and Quit. Story time begins at Start; movement/look/use become available after the fade. Pause uses scaled time plus AudioListener.pause.
- Input gates in FirstPersonController, PlayerInteraction, RadioChannel and NoteViewer.
- Start gates in SchoolClock, LibraryEvents, RadiatorKnocks and SupplyLift. SchoolClock waits for its tick clip before updating.
- LiftEnding retains the existing ending/dialogue and displays credits after the final DEAD AIR card.
- PortableRadio remembers every child's original layer, switches to HeldItem while held, and restores layers on put-down or lift placement.
- Scene authored through Unity MCP: menu Canvas/buttons, one editable credits TMP object, Held Item Camera under Player Camera, Overlay camera stack, clear depth, matching 70-degree FOV, HeldItem-only mask. Main camera excludes HeldItem. Held pose adjusted to (0.32, -0.27, 0.65) to keep it in the lower-right view. Overlay near clip is 0.01 m.
- Player settings: Wahab Ahmed / Dead Air, resizable 1280 × 720 Windowed. The only build scene is Assets/_DeadAir/Scenes/DeadAir.unity; no custom Build Profiles were found.
- BuildAll.cs adds Dead Air → Build All, attempts all three targets, and writes results/warnings to Builds/BuildResults.txt.
- Unity generated .meta files for new scripts. Building also reserialized URP settings (DefaultVolumeProfile, PC_RPAsset and UniversalRenderPipelineGlobalSettings) and temporarily generated performance-test resource JSON assets under Assets/Resources; the build process removed those temporary resources afterward.

No story, puzzles or dialogue were edited. No Git staging/commit operations or ignore-file edits were performed. `git check-ignore` confirms all three requested build paths are ignored by the existing /[Bb]uilds/ rule.

## Play-mode checks actually performed

- Title rendered at 1280 × 720 over the Room 12 opening view. Quiet looping hiss source was playing; title includes its caption. Both clocks displayed 11:58. Time scale was zero and input was disabled.
- Queued W, Q and Esc on title: player stayed at (1.50, 0.05, 7.00), no Talk exchange began, and pause stayed off.
- Start tested through a simulated pointer click and button callback. After the fade, input was enabled, time scale was one, cursor was locked, and game feed/ambient audio sources were playing.
- Pause froze Time.time at 8.343552 across separate checks, set AudioListener.pause, locked movement and exposed cursor. Queued Esc resumed. Resume button also resumed.
- Restart button from pause reloaded Room 12, restored title, disabled input, cleared pause/audio-pause and restarted title hiss.
- The rescue/ending loop was run in Play mode using the existing SupplyLift.Jump(true) Editor shortcut to prepare the beacon, then three gameplay Talk calls, continuous Crank calls and ToggleDoor. It reached the existing ending text, DEAD AIR card, credits and Play again. This was a scripted shortcut-assisted check, not a complete manual playthrough from Room 12.
- Credits initially exposed a Canvas sort-order conflict; Game Screens now sorts at 200 above the existing ending Canvas at 100. The corrected credits were visually inspected. Credits text matches the requested content in one TMP object, with separate button labels.
- Esc on credits left pause off. Play again reloaded title. Title, pause and credits Quit button callbacks each exited Play mode. Credits Quit was also tested directly after activating the credits transition.
- Advanced scaled game time for clock testing: after passing 120 seconds, both clocks displayed 12:00 and LibraryEvents.HasChimed was true. This was accelerated timing, not two minutes of wall-clock waiting.
- A one-frame null tick warning found during the first Start test was corrected by guarding SchoolClock.Update until its tick clip exists. Current scripts compiled without errors.

## Held radio

- All radio transforms were layer 8 (HeldItem) after Take.
- All were restored to their original layer 0 after PutDown on the teacher's desk and after PlaceInCar through the lift test setup.
- No other runtime transforms were on HeldItem after lift placement. A file-ID comparison against HEAD also showed zero layer changes to existing saved scene GameObjects.
- Visually checked held rendering at Room 12 wall/window collision distance and beside the teacher's desk. The held radio remained visible over scene geometry, with no scene-depth clipping in those views.
- Its surfaces visibly changed brightness/color between scene-light positions. All scene lights' culling masks include HeldItem. No light culling mask was edited.

## Repeat by hand in Play mode

No required manual Editor wiring remains. To edit names, select Game Screens → Credits → Credits text and edit its TextMeshPro text.

1. Open DeadAir.unity and press Play. Wait on title, try WASD/mouse/Q/right mouse/E/Esc, then click Start. Check the one-second fade and 11:58 opening clock. Midnight should occur 120 seconds after Start, excluding pauses.
2. Press Esc during movement, audio and dialogue. Wait, then press Esc or click Resume. Repeat with an open note. Check that audio and captions continue from the paused point.
3. Pause → Restart: expect the original title and reset Room 12. Quit on title/pause/credits should stop Editor Play mode.
4. Free and take radio #4. Walk against walls, desk edges and windows while looking up/down. Put it down and pick it up again, then place it in the lift. It should remain visible only while held and return to ordinary scene occlusion once placed.
5. Complete the rescue normally (or use existing F7 shortcut after Start for a quicker beacon test). Check the ending, credits, Play again and a second Start.

## Limits

- No full manual nine-minute playthrough was performed for this step. Shortcut-assisted ending and individual input/button checks were performed instead.
- Exhaustive clipping checks at every position/view angle, perceptual audio listening, all hint delays, and repeated pause/resume during every dialogue beat remain manual checks.
- Windows and Linux execution require their respective operating systems and were not tested here.

## Build results

All three were attempted by BuildAll. Sizes below are Unity BuildReport.totalSize (not zip sizes).

| Target | Path | Result | Bytes | MiB | Warnings | Reported errors |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| Windows x86_64 | Builds/Windows/DeadAir.exe | Succeeded | 106,064,155 | 101.15 | 19 | 0 |
| macOS | Builds/Mac/DeadAir.app | Succeeded | 123,627,355 | 117.90 | 1 | 1 |
| Linux x86_64 | Builds/Linux/DeadAir.x86_64 | Succeeded | 102,883,655 | 98.12 | 1 | 0 |

- Every target warns: `Pipeline: No RuntimePipelineConfig asset found (Project Settings > Pipeline > Runtime). Pipeline will be disabled in Player builds.` This concerns the Editor automation package, not the URP rendering pipeline. No package/configuration was added to enable runtime automation.
- Windows also reports 18 unsupported ray-tracing shader compilation warnings: TraceRays, RisEstimation, TraceRenderingLayerMask, UniformEstimation, TraceTransparentRays, DynamicGISkyOcclusion, TraceRaysAndFetchAttributes, RestirCandidateTemporal and TraceVirtualOffset (each twice).
- macOS's one reported error is the automation request timing out after 120000 ms while building: `Failed to handle /api/exec request: Main thread operation timed out after 120000ms`. BuildPipeline continued and returned Succeeded. Linux subsequently completed as well.
- Full raw build messages are in ignored `Builds/BuildResults.txt`.
- Disk allocation including companion files: Windows 104,072 KiB; Mac 121,084 KiB; Linux 100,988 KiB.

The Mac app was launched using `open`, which returned zero. A Dead Air window was enumerated, and Player.log confirmed Unity 6000.3.26f1, Metal initialization on Apple M4, assembly loading and PhysX startup. No runtime error appeared in the inspected log. macOS denied screenshot capture (`CGPreflightScreenCaptureAccess == false`) and input accessibility (`AXIsProcessTrusted == false`), so native title visibility and Start could not be independently confirmed. A pointer click was attempted, but it is not counted as a passed Start test. User confirmation was requested separately.

## Final checks

- After building, Editor compilation reported no failure; build scenes still contained only DeadAir.unity; main camera stack contained Held Item Camera; Game Screens Canvas sorting order was 200.
- C# and report whitespace checks passed. A whole-project `git diff --check` flags Unity-generated empty serialized fields in DeadAir.unity; those were left as written by Unity rather than hand-editing scene YAML.
- A further post-build Play-mode automation attempt could not connect to the Pipeline eval server. It was stopped; this additional run is not counted as a test. The earlier Play-mode observations above are the completed checks.
- Editor was left out of Play mode. The Mac app was launched; native title/Start confirmation remains pending the user's observation because OS capture/input permissions were unavailable.
