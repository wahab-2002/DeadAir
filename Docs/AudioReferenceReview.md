# Reference and audio routing review

## Reference audit

Inspected 339 MonoBehaviour components, including inactive objects. No missing-script components, dangling serialized references, or unassigned scene-object fields remain. The 152 empty AudioClip slots are intentional runtime effect fallbacks and caption-only Sam lines; all supplied Lou recordings remain assigned.

Reconnected:

| Component / field | Connection |
| --- | --- |
| SupplyLift.aquarium | AudioSource added on the existing Aquarium blue marker |
| LibraryEvents.aquarium | Existing Aquarium blue transform |
| Corridor/Basement door: NoiseTestObject.knocks | Existing RadiatorKnocks controller |

The aquarium mesh/audio source had been removed. The surviving light marker supplies the original sound/caption location; no mesh or furniture was moved or recreated.

New routing references: VoiceLines.audioRouting → AudioRouting on Step 3 story sounds; AudioRouting.mixer → DeadAir.mixer; RadioChannel.crateSound → new Crate scrape source under the existing radio control source. The crate source copies the former shared source's level, spatial settings and filters, keeping the original source unchanged.

SupplyLift skips missing optional audio and warns once. BackgroundMusic, LibraryEvents, SchoolClock, SchoolLights, NearbyRadioGlow, RadioVoiceSettings, RadioChannel search feedback and PlayerInteraction's optional portable-radio reference also have one-time warnings and guards.

## Mixer

Assets/_DeadAir/Audio/DeadAir.mixer was created manually in Unity and saved as NewAudioMixer.mixer; it was moved to the requested path through AssetDatabase. Radio Feed's existing capitalization is preserved. Master contains Music, Voices, Radio Feed, Ambience, SFX, UI. Lou (radio) and Sam are children of Voices. Snapshot has no parameter overrides; all attenuation faders retain default 0 dB. Existing source settings and filters were compared against the pre-edit scene: only output mixer assignments changed.

| Group | Saved sources | Role |
| --- | ---: | --- |
| Music | 1 | Music |
| Voices / Lou (radio) | 0 initially | Both shared dialogue sources switch here for recorded Lou lines |
| Voices / Sam | 2 initially | Both dialogue sources switch here for recorded Sam lines |
| Radio Feed | 8 | Sam/#4 feeds, feed echoes, voice hiss, radio controls, search feedback |
| Ambience | 9 | Aquarium, boiler, title/ending room tone, radiators, clock |
| SFX | 97 | Object bangs, sheets/crates, witch, doors, lift, desk bell, boo sting |
| UI | 0 | No menu sound effects currently exist |

All 117 saved sources are routed. Runtime sources are scanned every 0.5 s and routed by role/name, with SFX as the fallback. Shared dialogue output switches before playback. Crate scrape is separate from radio hiss/clicks. Nothing is left unrouted.

## Title

Game Screens/Title/Hiss caption is disabled. Captions.Ambient ignores calls until GameScreens.Started, so gameplay ambience cannot queue on the title/story card.

## Verification

- Unity compilation passed.
- Temporarily clearing SupplyLift.aquarium in Play mode produced one warning across repeated Updates, with no errors, then the reference was restored. Nothing from this deliberate fault test was saved.
- Initial automated traversal completed title → story card → conversations → noise tests → battery loss → empty lift/hints → three beacon stages → occupied ascent → final dialogue → credits. No new Console errors. Gameplay time was accelerated 4×; public interaction methods were invoked by a temporary iterator outside Assets.
- Final routing traversal also reached credits, with zero new Console errors or warnings; 117/117 runtime sources were routed. Sam/Lou group selection was checked directly.
- A newly created runtime AudioSource acquired SFX automatically; probe removed.
- Targeted fault injection: all six SupplyLift optional audio references were cleared during the occupied ascent. The car reached the top with Lou inside, one warning total, and zero errors. Play mode was stopped so all saved references were restored.
- No audible fader test was performed: local speaker output cannot be judged here, group volumes have no exposed runtime parameters, and no UI sound effects or Sam recordings exist to audition those groups. Fader audibility remains a manual check.
- Existing transform preservation was checked in the live Editor during the scene repair: zero existing transforms changed. Disk comparisons can include previously unsaved Editor edits; no original furniture transforms were restored.

## Manual Play mode checks

Enter Play mode and start the game. Confirm title/card have no room-tone caption. Change Music's fader during music, Lou (radio) and parent Voices during Lou speech, Radio Feed during open-feed hiss, Ambience near the aquarium/radiators/clock, and SFX while banging or cranking. Return each fader to 0 dB. Sam/UI have no audible content to audition until recordings/menu sounds are supplied. Complete the rescue and confirm the Console stays clear through credits.

## Full saved-source routing

| AudioSource hierarchy | Mixer group |
| --- | --- |
| Aquarium blue | Ambience |
| Boo sting | SFX |
| Book Room/Props/AV cart | SFX |
| Book Room/Props/Metal textbook shelf 0 | SFX |
| Book Room/Props/Metal textbook shelf 1 | SFX |
| Book Room/Props/Metal textbook shelf 2 | SFX |
| Book Room/Props/Metal textbook shelf 3 | SFX |
| Book Room/Props/Supply box | SFX |
| Book Room/Props/Supply box | SFX |
| Book Room/Supply lift station/Boiler roar and drip | Ambience |
| Book Room/Supply lift station/Faint room tone | Ambience |
| Book Room/Supply lift station/Lift rope creak | SFX |
| Book Room/Supply lift station/Shaft knocks and witch | SFX |
| Book Room/Supply lift station/Small lift door | SFX |
| Book Room/Supply lift station/Supply lift car/Car clunk and knock | SFX |
| Corridor/Basement door | SFX |
| Corridor/Cafeteria doors | SFX |
| Corridor/Front doors | SFX |
| Corridor/Props/Corridor bin | SFX |
| Corridor/Props/Fire extinguisher cabinet | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Lockers/Locker | SFX |
| Corridor/Props/Trophy case | SFX |
| Corridor/Radiator | Ambience |
| Corridor/Radiator | Ambience |
| Corridor/Radiator | Ambience |
| Corridor/Radiator | Ambience |
| Corridor/Room 11 locked door | SFX |
| Corridor/Room 14 locked door | SFX |
| Corridor/Room 15 locked door | SFX |
| Game Screens | Ambience |
| Library/Circulation desk | SFX |
| Library/Desk witch | SFX |
| Library/Props/Back wall books | SFX |
| Library/Props/Back wall books | SFX |
| Library/Props/Back wall books | SFX |
| Library/Props/Back wall books | SFX |
| Library/Props/Book Fair/Bake-sale crate | SFX |
| Library/Props/Book Fair/Cooler | SFX |
| Library/Props/Book Fair/Folding Book Fair table | SFX |
| Library/Props/Book Fair/Paperback box | SFX |
| Library/Props/Book Fair/Paperback box | SFX |
| Library/Props/Book Fair/Plastic tote | SFX |
| Library/Props/Book Fair/PTA radio #4/PTA #4 open feed | Radio Feed |
| Library/Props/Book Fair/PTA radio #4/PTA #4 voice | Radio Feed |
| Library/Props/Book Fair/PTA radio #4/PTA #4 voice/Radio #4 dialogue | Sam |
| Library/Props/Book Fair/PTA radio #4/PTA #4 voice/Radio #4 voice hiss | Radio Feed |
| Library/Props/Book Fair/Pumpkin bread crate 1 | SFX |
| Library/Props/Book Fair/Pumpkin bread crate 2 | SFX |
| Library/Props/Book return cart | SFX |
| Library/Props/Book return cart | SFX |
| Library/Props/Card catalogue | SFX |
| Library/Props/Chiming clock/Tall clock | SFX |
| Library/Props/Computer desk | SFX |
| Library/Props/Desk bell | SFX |
| Library/Props/Left wall books | SFX |
| Library/Props/Left wall books | SFX |
| Library/Props/Library bin | SFX |
| Library/Props/Reading chair | SFX |
| Library/Props/Reading chair | SFX |
| Library/Props/Reading chair | SFX |
| Library/Props/Reading chair | SFX |
| Library/Props/Reading table | SFX |
| Library/Props/Shared-wall shelf 0 | SFX |
| Library/Props/Shared-wall shelf 1 | SFX |
| Library/Props/Shared-wall shelf 2 | SFX |
| Library/Props/Shared-wall shelf 3 | SFX |
| Library/Props/Stacks/S1/Double-sided stack | SFX |
| Library/Props/Stacks/S1/Double-sided stack | SFX |
| Library/Props/Stacks/S2/Double-sided stack | SFX |
| Library/Props/Stacks/S2/Double-sided stack | SFX |
| Library/Props/Stacks/S3/Double-sided stack | SFX |
| Library/Props/Stacks/S3/Double-sided stack | SFX |
| Library/Props/Step stool | SFX |
| Music | Music |
| Room12/Props/Room 12 bin | SFX |
| Room12/Props/Small classroom bookshelf | SFX |
| Room12/Props/Student desk 10 | SFX |
| Room12/Props/Student desk 11 | SFX |
| Room12/Props/Student desk 12 | SFX |
| Room12/Props/Student desk 2 | SFX |
| Room12/Props/Student desk 6 | SFX |
| Room12/Props/Student desk 7 | SFX |
| Room12/Props/Student desk 8 | SFX |
| Room12/Radiator under window | Ambience |
| Room12/Teacher desk | SFX |
| Sam/Player Camera/Radio clicks and crate scrape | Radio Feed |
| Sam/Player Camera/Radio clicks and crate scrape/Crate scrape | SFX |
| Sam/Player Camera/Radio search feedback | Radio Feed |
| Sam/Player Camera/Sam radio feed | Radio Feed |
| Sam/Player Camera/Sam radio voice | Radio Feed |
| Sam/Player Camera/Sam radio voice/Sam dialogue | Sam |
| Sam/Player Camera/Sam radio voice/Sam voice hiss | Radio Feed |

## Files changed in this step

- Assets/_DeadAir/Scenes/DeadAir.unity: scene references, added aquarium/crate sources, mixer routes, title caption disabled.
- Assets/_DeadAir/Audio/DeadAir.mixer and metadata: user-created mixer moved to the requested filename using Unity.
- Assets/_DeadAir/Scripts/AudioRouting.cs and metadata: new routing component.
- Assets/_DeadAir/Scripts/VoiceLines.cs: speaker-specific routing.
- Assets/_DeadAir/Scripts/RadioChannel.cs: separate crate playback and optional feedback guard.
- Assets/_DeadAir/Scripts/SupplyLift.cs: optional audio guards.
- Assets/_DeadAir/Scripts/Captions.cs: ambient captions start at gameplay.
- Assets/_DeadAir/Scripts/LibraryEvents.cs, BackgroundMusic.cs, RadioVoiceSettings.cs, SchoolClock.cs, SchoolLights.cs, NearbyRadioGlow.cs, PlayerInteraction.cs: one-time optional-reference warnings and guards.
- Docs/AudioReferenceReview.md: this report and complete source routing table.

No story, dialogue, puzzles, ending, existing source levels, or existing filters were changed in this step. No git staging or commit commands were run. Editor is left out of Play mode.
