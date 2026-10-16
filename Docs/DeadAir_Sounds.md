# Dead Air — non-voice sounds

Put original recordings in `Assets/_DeadAir/Audio/SFX/<role>.wav`, then run **Dead Air → Assign Sound Clips** outside Play mode and save the scene. Missing files keep runtime-generated placeholders. Voice recordings use the separate Assign Voice Clips menu. No audio has been downloaded.

The overall mix uses `AudioListener.volume = 0.67`. The library feed's generated hiss is also reduced. Radio filters remain on the receiving sources. Use a seamless loop for roles marked loop.

| Role / file name | What it should sound like | Where it plays |
| --- | --- | --- |
| `library_feed_hiss_bubbling_ticking.wav` | Quiet hiss, distinct aquarium bubbles and clock ticking; loop | Sam's 2D radio while channel 3 is stuck |
| `lou_open_breathing_dripping_roar.wav` | Weak breathing, water drips, distant boiler; loop | Sam's radio and radio #4 during Lou's open feed |
| `radio_button_click.wav` | Small plastic push-to-talk click | Radio controls, crate release, battery death |
| `radio_release_hiss.wav` | Brief soft release hiss | Sam releases Talk |
| `radio_feedback.wav` | Sustained clean feedback whine; seamless loop, pitch varied in code | Sam's radio within 4 m of stuck radio #4 |
| `radio_feedback_squeal.wav` | Short feedback squeal | Radio #4 echoes Sam nearby after release |
| `crate_scrape.wav` | Light wooden slats scraping over the floor | Real crate slides aside |
| `witch_cackle.wav` | Short toy witch laugh | Desk witch, matching radio echo, distant witch below shaft |
| `witch_cackle_muffled.wav` | Same laugh, muffled | Behind Lou during her second exchange |
| `lou_dripping_deep_roar.wav` | Water dripping against a distant low boiler roar | Behind Lou's first reply |
| `pipe_knock.wav` | One resonant metallic pipe knock | Every radiator bang and repeated knock, Lou's three replies, shaft knocks, final car knock |
| `object_bang.wav` | One firm wooden/metal surface bang | Shelves, crates and existing door noise tests |
| `handle_rattle.wav` | Locked door handle shaken once | Rooms 11, 14 and 15 |
| `desk_bell.wav` | Small mechanical desk bell, one ring | Library circulation desk and graded echo |
| `sheet_rustle.wav` | Fabric sliding aside | All three decoration sheets and decoy feed rustles |
| `clock_tick.wav` | Quiet mechanical clock tick | Library clock; radio ticking is in its library-feed loop |
| `clock_chime.wav` | Recognizable midnight clock chime | Library clock at 120 seconds, simultaneously on Sam's radio if still receiving LibraryFeed |
| `lift_rattle.wav` | Close wooden hatch/car rattle | Lift sheet, door, crank or car; loud test in Lou's feed |
| `lift_creak.wav` | Hand-cranked rope and mechanism creaking; loop | Lift crank/shaft during motion |
| `lift_clunk.wav` | Car reaching a stop | Lift car at either endpoint |
| `boiler_roar_drip.wav` | Low boiler rumble with intermittent drips; loop | Below the shaft, fading as Lou rises |
| `aquarium_bubbling.wav` | Gentle aquarium bubbles; loop | 3D library aquarium, returning as Lou rises |
| `boo_sting.wav` | Brief sharp scare accent | Same frame as Lou's BOO |
| `faint_room_tone.wav` | Almost silent school interior; loop | Final quiet before end cards |

All repetitions of a knock use the same `pipe_knock` role. Knock counts and spacing are scripted; supply a single strike, not a three-knock recording. Footsteps are not implemented in the current game, so there is no unused footstep assignment.
