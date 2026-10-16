# Playtest 4 fixes — verification, October 16, 2026

Implemented sections 1–9, the three caption corrections, and section 11. Section 10 was **not** implemented: manual input, listening and build checks remain outstanding, so the condition that everything else be passing has not been established.

All scene changes were made through Unity MCP in the running Unity 6000.3.26f1 Editor. No scene, prefab, material or meta YAML was edited by hand. No packages were added and nothing was staged or committed. The existing self-test document was left untouched.

## Files changed

- `Assets/_DeadAir/Scenes/DeadAir.unity`: menu backing, fitted/hidden Lou, hint and story-card UI, audio filters/levels, three crate placements, Book Fair table skirt, replacement staff entrance, corridor lockers, three serialized captions, and overlap corrections.
- `Assets/_DeadAir/Scripts/GameScreens.cs`: story card, frozen gameplay until its fade ends, and first-talk hint.
- `Assets/_DeadAir/Scripts/LiftDoor.cs`: reveal/ending trigger after the hatch finishes opening.
- `Assets/_DeadAir/Scripts/SupplyLift.cs`: renderer visibility condition, preserve Lou's fitted scale, quieter basement cackle.
- `Assets/_DeadAir/Scripts/NoiseTestObject.cs`: revised bands, smooth echo gain, descriptive captions and floating-point boundary handling.
- `Assets/_DeadAir/Scripts/RadioChannel.cs`: comparison reactions, repeat cooldowns, quieter feeds and 2.5 m search whine.
- `Assets/_DeadAir/Scripts/PlayerInteraction.cs`: 1.2 m collider-surface limit for `E: Bang`.
- `Assets/_DeadAir/Scripts/LibraryEvents.cs`: quieter cackle echoes.
- New `Assets/_DeadAir/Scripts/StaffDoor.cs` and Unity-generated `.meta`: simple unlocked swinging door.
- `Assets/_DeadAir/Scripts/VoiceLines.cs` and `Docs/DeadAir_VoiceLines.md`: only the requested `lou_03`, `lou_04`, `lou_05` caption text corrections.
- This report and PNG evidence in `Docs/Build/Playtest4/`.

No furniture was restored from the level-pass generators. Desks, chairs, shelves, aquarium, carts and the extinguisher cabinet retain their existing placements. The named crates moved; wall-mounted decorations and locker banks changed for section 11. The Book Fair table stayed in place and received two plain skirt panels to conceal its decoy through the open legs.

## Actual Play-mode checks

These were script-driven checks through Unity MCP, with public interaction handlers and accelerated scaled time where noted. They are not a claim of a human keyboard/mouse playtest.

### 1. Title and pause

[Title screenshot](title.png). The first 88% backing attempt still showed faint pact text, so the final menus use an opaque black screen backing with an 88% rounded padded panel. The final capture has no readable pact behind it. Pause was opened and resumed through its handlers and its backing verified opaque.

On the title, the only playing AudioSource was `Game Screens`, clip `faint_room_tone`, volume `0.025`.

### 2. Noise sequence

The following sequence ran via each object's real `Use` → echo → radio reaction path, during the full progression run:

| Bang | Band | Lou's actual reply |
| --- | --- | --- |
| Room 11 door, corridor | Far | `lou_06`: “That was far away.” |
| Book Fair decoy crate | Near | `lou_08`: “Closer... I think.” |
| Reading table | Warm | `lou_09`: “Warmer.” |
| Room 11 door again | Far | `lou_06`: “That was far away.” |
| Shared-wall shelf 1 | Through the wall | `lou_10`: “Close... but not there.” |
| Lift sheet | Lift | `lou_11`, then battery failure and Silent state |

The full run's recorded voice history was:

`sam_00, sam_07, lou_00, sam_01, lou_01, sam_02, lou_02, lou_02b, lou_03, lou_06, lou_08, lou_09, lou_06, lou_10, lou_07, lou_11, sam_10, sam_11, sam_12, sam_13, sam_14, lou_12, lou_13, lou_14, sam_15`.

An isolated repeat check, with incidental witch triggers disabled only for that test, returned:

`Far → lou_06; Near → lou_08; Warm → lou_09; Far → lou_06; Near → lou_08; Near → lou_05; Near → no line; wait 31 seconds; Near → lou_05`.

A final radiator fallback check returned `lou_04`, then `lou_05`, then no line, preserving its earlier one-time fallback behavior. The final smoke session had zero console errors and no compilation failure.

Each test waited at least five scaled seconds after the previous reaction ended. Code enforces four seconds after a reaction ends. The exact boundary check returned Far at 9.01 and 9 m, Near at 8.99, 5.01 and 5 m, and Warm at 4.99 and 2.5 m. Tiny floating-point errors at exact boundaries use `Mathf.Approximately`.

Interpretation: ordinary bangs closer than 2.5 m remain Warm. Only the named lift parts trigger the Lift reaction. Radiators, sheets and the shared-wall special case retain their own sound/reaction routes.

### 3. Bang range

The actual `PlayerInteraction.WithinBangRange` gate was called in Play mode against real colliders:

| Object | 1.19 m from collider | 1.21 m from collider |
| --- | --- | --- |
| Reading table | Allowed | Blocked |
| Locker | Allowed | Blocked |
| Computer desk | Allowed | Blocked |

The normal raycast also stops at the first collider. Other prompts retain the existing interaction range. This was a direct gate check; physical E-key prompt handling still needs a manual check.

### 4. Crates and whine

Final world positions, in metres:

| Crate | Position | Placement |
| --- | --- | --- |
| R, `Bake-sale crate` | `(11.880, 0.220, -10.300)` | Back half, behind the third stack, off the shared wall |
| Decoy 1, `Pumpkin bread crate 1` | `(13.720, 0.220, -7.400)` | Turned end-on under the Book Fair table's rear edge |
| Decoy 2, `Pumpkin bread crate 2` | `(7.800, 0.180, -3.600)` | Behind the circulation desk |

R is **8.904 m** from the doorway floor centre `(10, 0, -1.6)` and **6.672 m** from the hatch. Pairwise crate distances are **3.434 m**, **7.845 m**, and **7.035 m**.

The radio and close-up glow moved by the same delta as R. Its side orientation and pinned-button arrangement were preserved. The real crate's floor height was raised 4 cm along with the radio/glow to keep its lowest tilted slat above the floor.

A stronger visibility check temporarily gave each crate a bright unlit magenta material, rendered the actual camera at 480×270 from doorway x=9.2, 10, 10.8 m, eye y=1.25, z=-1.8, and counted visible magenta pixels. Final result: **zero for all three crates at all three viewpoints**. Original materials and camera transforms were restored immediately. Earlier placements exposed small edges and were corrected. This samples three standing viewpoints, not every possible crouched/oblique camera position.

The final approach test found 20 clear interaction rays to R from `(12.7, 1.25, -10.3)`, including a slat hit at 1.13 m. Decoy 1 had 12 clear interaction rays from `(12.8, 1.25, -7.9)`, including a slat hit at 1.43 m. Decoy 2 was hit at 1.09 m from behind the desk. A centre ray can hit the radio through a slat gap; aim at the wood to lift R.

Search whine measured in Play mode:

| Radio distance | Source volume | Playing |
| --- | --- | --- |
| 2.6 m | 0 | No |
| 2.4 m | 0.0072 | Yes |
| 1 m | 0.1080 | Yes |

### 5. Book Room access

The corridor doorway, lintel, old open door and old plaque were removed; a full-height matching wall and five lockers fill that stretch. The new library staff door is unlocked, closed initially, labelled `STAFF ONLY · BOOK ROOM`, and its persistent Interactable event opens/closes it.

The first two route trials exposed furniture collisions; only the new doorway was adjusted. The final CharacterController route stayed at floor height and reached these targets:

- `(5.25, .05, -3.8)`, actual `(5.25, .05, -3.83)`;
- `(4.3, .05, -3.8)`, actual `(4.32, .05, -3.8)`;
- `(4.3, .05, -6.6)`, reached;
- `(4.5, .05, -7.4)`, reached, beside the lift/crank.

F6's `RestartForTest(false)` handler was invoked, including scene reload, Start and card dismissal: car Travel=0, radio Silent, hatch revealed, player about `(4.1, .02, -7.6)`. F7's `RestartForTest(true)` handler likewise gave Travel=1 at the same station. Physical function-key event delivery was not verified.

### 6. Story card and hint

[Story-card screenshot](story_card.png). At the card: `Started=False`, scaled Time=0, clock `11:58`, no midnight chime. Prompt appeared after the four-second wait. After continuing and fading, `Started=True` and the clock still read `11:58`. The full run's midnight chime then fired through the normal 120-second timer.

The hint was absent before five gameplay seconds, present afterward, and absent after the first accepted talk. It uses the caption font and a small rounded dark backing at top left.

Input simulation set key state but did not deliver a frame press recognized by the card, so the card was continued through its public handler. The physical “Press any key” flow remains a manual check.

### 7. Lou behind the closed hatch

[Front](ghost_closed_front.png), [left](ghost_closed_left.png), [right](ghost_closed_right.png). These were captured after three beacon calls and raising the occupied car. No ghost appears. Her renderers were also verified disabled in the basement and at the top with the hatch closed.

Lou's fitted mesh bounds at the top are approximately `(5.977, 1.010, -7.752)` to `(6.393, 1.567, -7.448)`, inside the car's inner space. The full run logged **0 enabled renderers with the occupied hatch closed**, then **7 after it finished opening** (Lou plus her carried radio). The existing ending camera turn and jump then ran to completion.

### 8. Full progression

A fresh Start-to-credits Play session completed without F-key state jumps. It used an 8× time scale for waits, public interaction handlers, teleports for long walks, and real CharacterController movement for the new staff route. It included the initial call, timed midnight chime, real crate lift, normal Lou conversation, the noise sequence above, lift-sheet reveal, battery failure, opening/loading/closing/lowering the car, three beacon calls, heavy return, hatch opening and the existing ending. [Credits capture](credits.png).

No new runtime errors appeared in that full progression session. Earlier exploratory test sessions produced an unassigned-cackle error during a shortcut setup and inactive-controller messages from a temporary camera-positioning test; both were absent in the fresh full run. The test scripts lived in `/tmp` and are not project scripts.

### 9. Audio settings and limits

Sam's continuous feed is now 0.18 rather than 0.30 (another 40% reduction). His feed and voice sources use a 300 Hz high-pass and 3.2 kHz low-pass, with no enabled distortion. Generated feed hiss remains lower than the signature layers. The desk cackle source changed from 1 to 0.501187, uses 3D linear rolloff to zero at 8 m, and its radio echoes were reduced similarly. Basement cackle gain and range were also reduced. Radiator sources changed from 1 to 0.707946.

These are configuration/runtime measurements, not an acoustic listening judgment. No audio was downloaded or replaced.

## Section 11: every overlap correction

[Room 11 after correction](room11_fixed.png).

| Location / objects | Finding | Final correction |
| --- | --- | --- |
| Room 11: `Library/North left` and `Corridor/South Book Room right` | Same wall faces over x=6–9, z=-1.7…-1.5; 9 m² duplicate face | Deleted `Library/North left`; retained the corridor wall |
| Beside library/Room 14: `Library/North right` and `Corridor/South beside library` | Same wall faces over x=11–14; 9 m² duplicate face | Deleted `Library/North right` |
| Room 12 left: `Room12/South wall left` and `Corridor/North west` | Duplicate 4.2 m² wall face | Deleted the Room 12 copy |
| Room 12 right: `Room12/South wall right` and `Corridor/North long` | Duplicate 7.2 m² wall face | Deleted the Room 12 copy |
| Room 12 west/east walls at corridor and north-wall corners | Coplanar corner strips | Trimmed side-wall z spans to 1.7…8.5, meeting the adjoining walls at their edges |
| Book Room outer wall at corridor and back wall | Coplanar corner strips | Trimmed z to -8.7…-1.7 |
| Book Room back wall at shared library wall | Coplanar corner strip | Trimmed x to 2.6…5.8 |
| Library shared rear wall at library back wall | Overlapping corner | Trimmed z minimum to -11.5 |
| Library east wall at back wall and corridor | Overlapping corners | Trimmed z to -11.5…-1.7 |
| New staff-door front jamb against corridor wall | Overlapping corner strip | Trimmed z maximum to -1.7 |
| Room 12/corridor floors and ceilings | 1.89 m² overlap on each horizontal face | Joined at z=1.6 |
| Library/corridor floors and ceilings | 2.49 m² overlap on each horizontal face | Joined at z=-1.6 |
| Book Room/corridor floors and ceilings | 0.48 m² overlap on each horizontal face | Joined at z=-1.6 |
| Book Room/library floors and ceilings | 0.37 m² overlap on each horizontal face | Joined at x=5.9 |
| Room 11, 14 and 15 false doors against corridor wall | Door backs penetrated wall by 20 mm | Moved door faces 24 mm toward corridor; backs now 4 mm clear |
| Two cafeteria glass panes | Each had grown to 3.15 m wide, overlapping over 3.762 m² | Trimmed each to 1.35 × 1.6 m at its existing centre |
| Extinguisher cabinet and south locker at x=12.1 | Cabinet embedded in the locker; top faces shared a plane | Removed that locker and its handle/vents; cabinet and extinguisher retain original positions |
| Chalkboard lesson panel and chalkboard | Panel rear crossed board front by 2.5 mm | Board is 4 mm off wall; lesson panel is 4 mm off board |
| Crossing library/desk cobweb strands | Shared-depth surfaces at crossings | Staggered each group of six strands in depth |
| Both clocks' minute/hour hands | Overlapping faces at the hub | Moved minute hands forward by 12 mm, leaving a 4 mm face gap |
| Corridor radiator fins/body | Fins penetrated body and shared top/bottom face planes | 4 mm gap behind fins; top/bottom inset 5 mm |
| Crank spokes at hub | Crossing shared-depth faces | Staggered spoke depth, preserving crank operation |
| Cubby left/back wall pieces | Overlapping top faces at corner | Trimmed left piece to end at back piece's front face |

The new corridor infill occupies only the former doorway x=3.4–5.6 and meets its neighbours at edges. No duplicate wall was added behind it. Locker banks already had spacing between separate lockers; their backs and attached face details were moved 46 mm toward the wall, giving 4 mm rear clearance. The five new lockers use the same clearance.

The following wall decorations were checked and set 4 mm off their supporting face: Room 11/14/15 number text, Room 12 number text, library sign, basement sign, cafeteria notice, exit panel/text, lift label, the five “My pumpkin” panels, READ! panel, both Book Fair posters, carnival notice, chalkboard/lesson panel and window paper bats. Panel lettering now sits 4 mm off each panel face. The new staff plaque/text were placed with the same clearance. Existing pact, tape and free-hanging signs already had separated faces and were retained.

Broad bounds checks also flag intentional prop joins (crate slats/posts, car boards, wheel-rim pieces, aquarium meeting its stand). These are not duplicate school wall panels. Rotated slats can have overlapping world-aligned bounds without overlapping faces. Furniture was not moved to eliminate bounding-box-only or hidden construction joints; the aquarium's existing 5 mm seating into its stand was retained.

## Still untested / manual follow-up

No Inspector wiring or setup is required. Open `Assets/_DeadAir/Scenes/DeadAir.unity` and press Play.

1. Click Start, wait for “Press any key”, press a physical key, and confirm Room 12 starts at 11:58.
2. Check Q/right mouse talk and the hint disappearing. Listen on the intended speakers/headphones; confirm the quieter mix feels right.
3. Aim at the table, locker and computer desk, walking across the 1.2 m limit; verify E and the prompt. Other interactions should retain their prior range.
4. Search from the library door, lift the three crates, and repeat the Far → Near → Warm → Far → shared-wall → lift noise sequence.
5. Open the staff door behind the desk, walk to the crank and complete the rescue with physical inputs. Check F6/F7 physically after starting/dismissing the card.
6. Walk past Room 11 and sweep the camera across plaques, posters, locker banks and the filled former corridor doorway. Static surface checks and sampled captures do not replace a continuous visual sweep on the target GPU.

No Windows/macOS/Linux builds were produced or tested in this step. No manual sound-off run, human full playthrough, every possible doorway angle, or perceptual audio comparison was performed. The full automated progression preceded the final crate concealment adjustment; the final positions were separately checked for concealment, spacing and interaction rays.

Unity was left out of Play mode. `DeadAir.unity` remains the only enabled Build Settings scene. C# and voice-document whitespace checks passed; the overall whitespace check reports Unity-generated empty YAML fields with trailing spaces, which were not hand-edited.
