# Dead Air — Game Design

Oct 13, 2026 · Wahab

**Dead Air** is a grounded walkie-talkie mystery whose answer is hidden in the background noise of a radio channel. The design is complete on paper and sized for the October 16 deadline; none of it has been playtested yet.

> Repo copy of the shared design doc, exported for Codex. The map drawing is replaced by a text layout. If the shared doc changes, re-export this file.

## 1. Dead Air: what actually happened (author only)

Nothing supernatural happens. The eeriness comes from what a scared kid perceives in a dark building, and every cause is ordinary.

**Setting:** Hollow Creek Elementary, Friday, October 31, 1997. The late 1990s give kids walkie-talkies but no cell phones; the year never needs to appear on screen.

**People:**

- **Sam**, 11, the player. Careful, a planner. Never seen; name and gender are yours to set.
- **Lou**, 11, Sam's best friend. Brave, loves pranks. Dressed as a bedsheet ghost.
- **Denise**, a PTA volunteer. Offscreen; she exists only as handwriting on one note.

### Timeline of the night

| Time | What happened |
| --- | --- |
| 6:00–9:00 PM | Halloween carnival in the cafeteria. Sam's mom lends the PTA her four walkie-talkies, all set to channel 3. Sam and Lou keep two of them. |
| 8:50 | Sam hides under a pile of coats in Room 12's cubby, as the pact says. Lou hides in the girls' bathroom. |
| About 9:15 | Sam, warm and crashing from sugar, falls asleep. |
| 9:20–9:30 | Volunteers send the haunted-house props down to basement storeroom B-2 in the library's old supply lift, a counterweighted dumbwaiter. Denise sets the bake-sale crate on PTA radio #4, pinning its side talk button. #4 starts broadcasting the library on channel 3. B-2 is padlocked. |
| 9:35 | The volunteers leave and the building is locked. |
| 9:40 | Lou finds the cafeteria chained, so their stage meet-up is off. She waits in the library, the only lit room. Her calls get nothing but hiss, and she decides Sam is pranking her with silence. |
| 9:50 | Lou climbs into a "cupboard" in the library wall to jump out when Sam comes looking. It's the supply lift. Nobody has set the brake and she outweighs the counterweight, so the car sinks to B-2. She tumbles out, breaks her flashlight, and the emptied car rises back up with her pumpkin pail inside. |
| 9:55–11:45 | Lou calls on channel 3 again and again; #4's stronger signal drowns her out. She bangs a steam pipe in threes, the pact's code for "come find me", and the pipes carry it to every radiator. A motion-activated witch among the props cackles whenever she moves. Her batteries drain. |
| 11:58 | Sam wakes to knocking. The game begins. |

### The rules this world runs on

All of these are how real things behave. The player is never told them; each is shown once before it matters.

- A walkie-talkie with its talk button held down transmits everything around it. Its partners play that room.
- Only one radio can be heard on a channel at a time. A radio stuck transmitting blocks everyone else.
- Your voice comes out of every other radio on the channel, wherever that radio is.
- You can't talk while someone else is talking.
- Steam pipes carry knocking through a whole building.
- A counterweighted lift sinks when it's heavier than its counterweight and rises when emptied, unless the brake at the crank holds it. Its plate warns that the car will move.

### The obvious questions, answered by the world

- **Why not leave and get help?** Sam could: the exits open from inside. But until Sam knows Lou is trapped there's no reason to, and by the time Sam knows, the way to reach her is in the same room. At the front doors Sam says, "Not without Lou."
- **Why not call someone?** No phone is in reach. Nobody is looking either, because of the sleepover cover story.
- **Why can't Lou get out?** B-2 is padlocked from the corridor side and has no windows, and the lift left without her.
- **Why didn't Lou just talk?** She did, for two hours. The stuck radio drowned her out.
- **Why can't Sam ride the lift down?** Then nobody is at the crank to bring them up. Sam says so.

**What the player can accomplish:** bring Lou up. The ending follows from the facts: the only way into B-2 is the lift, and the only way Lou can find the lift in the dark is a voice coming from inside it.

### The map (text version)

```
GROUND FLOOR (playable)

 +------------+                                      +--------------+
 |  Room 12   |                                      |  Cafeteria   |
 | beats 1-2  |                                      | chained;     |
 | wake, pact,|                                      | seen through |
 | dead air   |                                      | glass        |
 +-----||-----+                                      +------||------+
 +-----||--------------------------------------------------||---------+
 |  CORRIDOR, about 20 m (beat 3) - radiators knock in threes          |
 +----------------------------||--------------||---------------||-----+
                    +---------||----------+ +--||-----------+ +-||----------+
                    |  LIBRARY            | | Basement door | | Front doors |
                    |  beats 4-10         | | locked, roar  | | open, but   |
                    |  witch, crate,      | | (stairs down, | | "Not without|
                    |  radio #4,          | |  locked)      | |  Lou."      |
                    |  [LIFT] back wall   | +---------------+ +-------------+
                    +---------[LIFT]------+
                              | lift shaft
- - - - - - - - - - - - - - - | - - - - - - - - - - - - - - - - - - - - - -
BASEMENT (never built, only heard)
                    +---------[LIFT]------+ +---------------------------+
                    | B-2 storage,        | | B-1 boiler room           |
                    | padlocked: Lou in   | | the roar behind the       |
                    | the dark, props,    | | basement door and behind  |
                    | 2nd witch, a pipe   | | Lou's voice               |
                    +---------------------+ +---------------------------+
```

Sam walks from Room 12 down the corridor into the library. The supply lift drops through the floor into B-2, beside the boiler that roars behind the locked basement door.

## 2. The playthrough, beat by beat

About 9 minutes for a careful first-time player and 5 for a fast one. There's no waiting and no long walk: the corridor is about 20 m, and every stop teaches something.

**Controls:** WASD and mouse. E to interact; hold E to turn the crank. Hold Q (or right mouse) to talk. Captions are always on.

### Beat 1. Wake (0:00–0:45), Room 12

- **Where:** Lying under a heap of coats in the cubby nook, looking up at coat hooks. Moonlight, paper bats, a jack-o'-lantern with an electric candle on the teacher's desk. The wall clock says 11:58.
- **What pulls you:** Static hissing from the walkie by your hand. Three slow knocks from the radiator under the window. A sheet of paper taped to the cubby wall, right in front of your face.
- **You do:** Read it (E).
- **The pact:** "1. Hide till everyone leaves. 2. Meet on the CAFETERIA STAGE at 10. 3. Catch the Knocking Ghost on tape (it knocks in the walls on Halloween!!). 4. Radios on channel 3. Knock code: 1 = YES, 2 = NO, 3 = COME FIND ME. 5. Cover story: you're at my house, I'm at yours. 6. Whoever chickens out has to SAY IT on the radio. — Lou + Sam"
- **You learn:** You're here on purpose, and you missed the meet-up by two hours. Lou is somewhere in the building, nobody knows you're here, and three knocks means something.
- **Next:** Call Lou.

### Beat 2. Dead air (0:45–1:15), Room 12

- **You do:** Hold Talk. Sam whispers: "Lou? I fell asleep. Are you there? Over."
- **What changes:** No answer. Behind the hiss there's bubbling and slow ticking: "[radio: hiss... bubbling... ticking]". The radiator knocks three times again: "[three knocks, inside the radiator]".
- **You learn:** Someone's channel is open but silent. Is Lou pranking you, hiding, or is it the ghost?
- **Next:** Head for the cafeteria, where you were meant to meet.

### Beat 3. The corridor (1:15–2:30)

- **What pulls you:** Every radiator under the windows knocks in threes at the same moment, the whole building at once. The far end glows orange: the cafeteria. Halfway down, warm lamplight shows through the library's door windows.
- **The plan falls apart:** The cafeteria doors are chained. Through the glass: string lights, an empty stage, a sign reading "HAUNTED HOUSE (CLOSED)". Lou can't be in there.
- **Other stops, each a single response:** The basement door: "BASEMENT · B-1 BOILER ROOM · B-2 STORAGE · STAFF ONLY". It's locked and warm, with a deep roar behind it. The front doors would open, but Sam says, "Not without Lou."
- **The teaching moment:** At midnight, about two minutes in, the library clock chimes. You hear it twice at once: live from down the corridor, and inside your radio's hiss.
- **You learn:** The silent channel is coming from the library. With the meeting place gone, it's your only lead.
- **Next:** Go in.

### Beat 4. The library (2:30–3:20)

- **Where:** Shelves, a reading rug, a bubbling aquarium throwing blue light, the chiming clock. The circulation desk is dressed as a "Haunted Library": cobwebs, a candy bowl and a plastic witch.
- **What happens:** You step near the desk and the witch shrieks and cackles; it's motion-activated. An instant later the same cackle comes out of your radio.
- **What pulls you:** No Lou. A red light glows from under the bake-sale crate on the desk.
- **You do:** Lift the crate (E). Under it is a yellow walkie labeled "PTA #4", its talk button pinned down.
- **What changes:** The hiss cuts out. Two seconds of real silence. Then, weak and crackling: "—am? Sam, please. Anyone. Over."
- **You learn:** The dead air was this radio broadcasting the empty library all night, and it was hiding Lou. She has been calling the whole time, and she isn't here.
- **Next:** Answer her.

### Beat 5. Lou (3:20–4:15), the library

- **You do:** Take radio #4 and hold Talk. Sam: "Lou! It's me! I fell asleep. Where are you?" Your voice squeals back out of #4 in your other hand: radios speak wherever they are.
- **Lou:** "Sam?! I've been calling for hours. I hid to jump out at you, and then I was falling, and now it's pitch black. I don't know where I am. My flashlight broke." Behind her voice: dripping, and a deep roar she never mentions.
- **Sam:** "What's down there with you?" **Lou:** "Boxes, I think. Sheets. And something laughs every time I move." Muffled cackling. "That!"
- **Lou:** "My batteries are almost dead. I'm gonna hold the button down so you can hear what I hear. Find me. Please find me."
- **What changes:** Her feed stays open: breathing, dripping, the roar. Your receive light stays green and Talk does nothing. Sam: "She can't hear me while she's holding it."
- **You learn:** Lou fell somewhere dark, beside something that roars, with something that laughs. Her radio is now a microphone wherever she is.
- **Next:** Work out where that is.

### Beat 6. Finding her (4:15–5:45)

This is the investigation. Nobody tells the player what to try, and the first clue usually arrives by accident.

- **By accident:** You step past the desk and its witch cackles. An instant later a faint, muffled cackle comes through Lou's feed. Was that her witch, or this one? Do it again: the same echo. A noise you make here reaches her.
- **Testing:** Bang a radiator, and a faint clank comes through, then three knocks back through every radiator: she heard you through the pipe. But every radiator sounds the same. Bang the basement door: a faint thud, no closer than the witch.
- **The loud one:** The small wooden door in the library's back wall, with a crank beside it. Open it and the rattle comes through her feed loud and close. Lou gasps: "That! That's right next to me! Sam, I think that's what I fell out of... the light's blinking... please don't leave me down h—" A click, then true silence.
- **You learn:** The louder your noise comes through, the closer she is. She's at the bottom of whatever is behind that door. And now you can't hear her at all.
- **If stuck:** After 40 seconds with no testing, Sam: "If she's holding the button, anything loud near her should come through." After 80: "She said she fell. Fell from where?"
- **Next:** Look behind the door.

### Beat 7. The cupboard (5:45–6:30), the library

- **You do:** Look inside. There's a small wooden car with Lou's plastic pumpkin pail in it, candy spilled.
- **You read:** The plate: "SUPPLY LIFT · LIBRARY ⇄ B-2 STORAGE · OPERATE FROM THIS STATION ONLY · SET BRAKE BEFORE LOADING OR UNLOADING · CAR WILL MOVE". The note taped beside it: "Haunted House props → B-2 tonight. B-2 gets LOCKED after. Leave the library witch out for the display! — Denise"
- **You learn (the deduction):** Lou hid in the lift. With no brake set, her weight sank it to B-2, and when she tumbled out the empty car rose back up. B-2 holds the props, so the laughing thing is a second witch. It sits beside the boiler: the roar behind the basement door, and behind her voice.
- **A wrong idea you might try:** Climb in. Sam: "If I go down too, there's nobody to crank us back up."
- **Next:** Send the car down.

### Beat 8. The rescue that doesn't work (6:30–7:00)

- **You do:** Close the door and hold E on the crank. The rope creaks, the car sinks, and there's a clunk at the bottom.
- **What changes:** Nothing. The pipes keep knocking, three and three, exactly as before. After about 20 seconds Sam says, "She can't see it. It's pitch black down there."
- **You learn:** Getting the car to her isn't enough; she needs a way to find it.
- **Next:** What can Lou follow in the dark? Not light: you have none, and hers is broken. Sound. Your voice. Radio #4 speaks wherever it is.

### Beat 9. The beacon (7:00–8:00)

- **You do:** Crank the car back up, put radio #4 inside, close the door and crank it down.
- **You do:** Hold Talk. Sam: "Lou! The cupboard's back! Follow my voice!" You hear your own voice, faintly, from deep in the shaft.
- **What changes:** The knocking stops coming from everywhere. Three knocks come from below, closer, and somewhere down there the witch cackles as she moves. Talk again: "Keep coming! You're close!" Closer still. Talk again: "Climb in. I'll pull you up." One knock, on the car itself: YES. The crank goes heavy.
- **Next:** Bring her up.

### Beat 10. Up (8:00–9:00)

- **You do:** Hold E on the crank; it turns slowly now. As the car rises, the boiler's roar fades, the dripping stops and the aquarium's bubbling comes back. Clunk.
- **You do:** Open the door. A small bedsheet ghost is curled in the car, hugging the yellow radio.
- **Lou:** "...Boo." A beat. "Did you get the Knocking Ghost on tape?" **Sam:** "Yeah. It was you."
- **End:** In the walls, the pipes go quiet. On screen: "Neither of them chickened out." Then the title: DEAD AIR.

## 3. The central puzzle

One rule, used four different ways: **a walkie-talkie plays whatever its partner hears.** Holding Talk and saying nothing doesn't send silence; it sends the room, including any noise someone else makes in it.

### How the rule is taught, then used differently

| Use | Where | What the player does | What's new about it |
| --- | --- | --- | --- |
| 1. The background tells you where | Corridor | The meeting place is chained; the midnight chime sounds live and on the radio at once; you follow it to the library | Taught at low stakes, but it's now the only lead |
| 2. A stuck radio blocks the channel | Library desk | Lifts the crate: silence, then Lou breaks through | Understanding the system, not just reading it; the first reversal |
| 3. An open radio is a microphone you can test | Around the school | With Lou holding her button, makes noises in different places and listens for which ones reach her | The player runs the experiment; the answer is a place they find, not a line they hear |
| 4. Your voice as a beacon | The supply lift | Puts radio #4 in the car, lowers it, and talks Lou toward it | Reversed: you stop listening to a place and start speaking into one |

In the rubric's language, the ability is taught in use 1 and applied in new ways in uses 2–4. Half-duplex is real and does work for us: Talk does nothing while Lou is transmitting, so lines never overlap, and the beacon only becomes possible once her battery dies.

### What the player must know (the necessary clues)

1. The dead air sounds like the library: the doubled midnight chime.
2. Noises Sam makes near Lou come through her open radio: the first witch echo.
3. The lift is where her feed is loudest: the noise test.
4. Lou can't find the car in the dark: the lowering that doesn't work.
5. Radio #4 speaks wherever it is: the feedback squeal at the desk.

### The noise test

| Where Sam makes noise | What comes through Lou's feed | What it tells you |
| --- | --- | --- |
| A radiator, any of them | A faint clank, then Lou knocks three times back | She's on the pipe and can hear you, but the pipe runs everywhere |
| The basement door | A faint thud | Somewhere below, not close |
| The desk witch | A muffled cackle | Roughly below the library |
| The supply lift (door, crank or car) | A loud, close rattle; Lou cries out | She's at the bottom of the lift |

Any interaction with the lift while her feed is open counts as the loud test, so a player who opened it earlier only has to touch it again.

### Wrong attempts and what they do

| Attempt | What happens | What it teaches |
| --- | --- | --- |
| Talk before freeing #4 | Hiss and library sounds; no answer | The channel is occupied |
| Go to the cafeteria | Chained | The pact's plan is dead; follow the radio |
| Try the front doors | Sam: "Not without Lou." | The goal is her, not escape |
| Talk while Lou holds her button | Nothing; the receive light stays green | Listen instead |
| Bang every radiator | The same faint clank each time | Pipes give no location; try somewhere else |
| Lower the lift before freeing #4 | The car goes down; nothing changes | Nothing lost |
| Climb into the lift | Sam refuses: nobody would be left to crank | States the core constraint |
| Lower the empty car and wait | Knocking unchanged; a hint after 20 seconds | She needs something to follow |
| Leave #4 somewhere else and talk | Your voice plays from wherever you left it | Rehearses the beacon |
| Lower #4 in the car but don't talk | Nothing; radios carry only what you send | Talking is the action |
| Crank up before her knock | The car returns with only the radio | No penalty; lower it again |

### The solution

Free radio #4. Make noise around the school until the lift rattles loud in Lou's feed. Lower the lift with #4 inside. Talk three times while her knocks come closer. Hear one knock on the car. Crank up.

### What solving it resolves

It answers all three questions at once. Where is Lou? In B-2, under your feet. What is the Knocking Ghost? Lou, in a bedsheet. What was the dead air? The stuck radio, which is the same radio that brings her home.

### Hints, only if the player is stuck

- **The crate:** after 45 seconds in the library without lifting it, Sam: "Something's glowing under that crate."
- **Finding her:** at 40 seconds with no testing, "If she's holding the button, anything loud near her should come through." At 80, "She said she fell. Fell from where?"
- **Rescuing her:** 20 seconds after the empty car is lowered, "She can't see it." At 45, "She needs something to follow." At 75, "If she could hear me down there..."

### Why there's no checklist lock

No interaction appears or unlocks because the player read something. Lou's battery dies in reaction to the loud noise test, so it follows from what the player did, not from a timer. The beacon can't work before then because Lou is holding her button, which is how real radios behave.

## 4. Clue chain

Twenty clues in the order a careful player meets them. Six are strictly needed and five are events the player can't miss; the rest give motivation or a second route to the same conclusion. Lou now names nothing: every place in this chain is something the player finds.

| # | Clue | You observe | You can infer | It changes your plan | Needed? |
| --- | --- | --- | --- | --- | --- |
| 1 | The pact | A taped note: cafeteria stage at 10, channel 3, knock code, cover story | I'm here on purpose; Lou's in the building; nobody's coming; knocks may be a message | Go to the cafeteria; call Lou | Motivation |
| 2 | Dead air | Hiss with bubbling and ticking behind it; no reply | Someone's channel is open and silent | Listen for where those sounds are | Backup |
| 3 | Knocking radiators | Three knocks from every radiator at once | The ghost, or Lou using "3 = come find me"; the pipes join everything, so it's from nowhere in particular | Unease, urgency | Motivation |
| 4 | Chained cafeteria | A chain on the doors; an empty stage through the glass | The meeting place is gone; Lou can't be in there | Find another lead | Event |
| 5 | Midnight chime | The library clock chimes live and on the radio at the same moment | The radio is hearing the library | Go to the library | Yes |
| 6 | Witch echo | A cackle at the desk, then the same cackle on the radio | A transmitting radio is in this room | Search the room | Backup |
| 7 | Red light under the crate | A glow under the bake-sale crate | A radio is transmitting under there | Lift the crate | Yes |
| 8 | The breakthrough | The hiss stops; Lou's weak voice cuts in | #4 was blocking the channel; Lou's been calling all along; she isn't here | Talk to her | Event |
| 9 | Your voice from #4 | Feedback squeal; your words come out of #4 | A radio speaks wherever it is | Sets up the beacon | Yes |
| 10 | "I hid... I was falling... pitch black" | Lou's account; no place named | She fell from somewhere into somewhere | Where could a kid fall in a school? | Backup |
| 11 | Roar and dripping behind her voice | Sounds Lou never mentions | She's near something big and mechanical, maybe the roar behind the basement door | Check the basement door | Backup |
| 12 | "Something laughs when I move" | Muffled cackling behind her voice | A witch like the one on the desk, or that very one | Which witch? | Backup |
| 13 | Lou holds her button | "So you can hear what I hear"; her feed stays open; Talk does nothing | Her radio is now a microphone where she is | Make noise and listen | Event |
| 14 | Your noises in her feed | Radiator: faint clank, three knocks back. Basement door: faint thud. Desk witch: muffled cackle. Lift: loud rattle, and Lou cries out | The louder it comes through, the closer she is; she's at the bottom of the lift | Look behind the lift door | Yes |
| 15 | Battery dies | Lou's "don't leave me down h—", a click, then true silence | She can't hear me now | Find a way that doesn't need her radio | Event |
| 16 | The lift | A car with Lou's pail inside; plate: "SUPPLY LIFT · LIBRARY ⇄ B-2 STORAGE · SET BRAKE BEFORE LOADING OR UNLOADING · CAR WILL MOVE" | She hid in it; with no brake her weight sank it; the empty car rose; she's in B-2 | Lower the lift | Yes |
| 17 | PTA note | "Props → B-2. B-2 gets LOCKED." | The laughing thing is a second witch; she's locked in | Confirms 12 and 16 | Backup |
| 18 | Basement sign | "B-1 BOILER · B-2 STORAGE · STAFF ONLY", locked | B-2 sits beside the boiler, the roar in clue 11; the stairs are out | Confirms 11; the lift is the only route | Backup |
| 19 | Lowered car, no change | The knocking stays the same | She can't find it in the dark | Give her something to follow | Yes |
| 20 | Closer knocks | Each time you talk, the knocks come nearer the shaft; then one knock on the car | She's following; 1 = YES, she's in | Crank up | Event |

**Playing muted:** every sound is captioned with the same noun in both places, such as "[cackling laughter]" in Lou's feed and "[the witch cackles]" at the desk. Noise-test captions grade loudness ("[faint clank]", "[loud rattle, close]"). Captions name the sound, never the room, so matching them is still the player's deduction.

## 5. Stress test

These are desk simulations, not playtests. The design holds for all three players; the real risks are the beacon insight and a fast, lucky finish.

### A careful first-time player

They wake, read the pact (it's in their face), call Lou and hear dead air. They notice the bubbling but can't place it. In the corridor they try the basement door (warm, roaring), peer into the cafeteria, and hear the midnight chime doubled. In the library they jump at the witch, hear it echo on the radio, and spot the red light.

The breakthrough lands hard. When Lou starts holding her button, they step past the desk witch and hear its cackle echo, faintly, in her feed. They bang a radiator, get three knocks back, and test noises until the lift door rattles loud and Lou cries out. They read the plate and say "she's in the basement." They lower the car, wait, get the first hint, and reach the beacon idea within a minute. Likely total: about 9 minutes.

**Risk:** they may not grasp that the stuck radio was blocking Lou. Test first without an explanation, and add Sam's line "It was stuck on talk the whole time" only if testers are confused. That's a ready-made, honest playtest revision for your Game Script.

### A random experimenter

They skip the pact, mash Talk, sprint down the corridor and try every door. In the library they lift the crate because it's the obvious interactable, so they hear Lou whether they mean to or not. They may open the lift early and lower it; nothing happens, which is consistent. Touching the lift while Lou's feed is open sets off the loud test, so they reach her quickly. They may drop radio #4 into the car by chance and talk; once her battery has died, it works.

Fastest plausible run: about 4 minutes, still hearing every story beat, because Lou's lines can't be skipped.

**Can they break it?** Talk is disabled while Lou speaks, so lines can't overlap. The crank works both ways at any time, nothing is consumed, and nothing can softlock. If they lose radio #4, talking makes it speak, so they can find it by ear.

### A player who misses a clue

- **Skipped the pact:** no knock code. The final knock is captioned "[one knock, on the lift car]" and the crank goes heavy, so they still crank up; the moment just means less.
- **Ignored the witch and the boiler:** the noise test still finds her; the roar and the witch only confirm it. A player who never thinks to make noise usually gets the first echo by accident from the desk witch, then a hint at 40 seconds.
- **Missed the feedback squeal:** the beacon is harder. The hint ladder catches them by 75 seconds.
- **Playing muted:** captions carry every clue.

### Problems found, and fixes

| Problem | Why it matters | Fix |
| --- | --- | --- |
| Early finish via the lift | A lucky player could end in about 4 minutes | Accept it: the run is still coherent and typical play is longer. Don't add locks |
| "Why not leave and get help?" | Breaks belief | The exits open normally. Until Sam knows Lou is trapped there's no reason to leave, and once Sam knows, the way to her is in the same room. At the doors Sam says "Not without Lou." |
| "Why didn't Lou shout up the shaft?" | Plausibility | She did, until she was hoarse, from the far side of a dark room, with nobody in the library |
| The stuck-radio rule is unfamiliar | Players may not see why freeing #4 let Lou through | The cut from hiss to silence to her voice is the explanation; add one Sam line only if tests show confusion |
| "Something laughs" read as the library witch | Players might think Lou is in the library | Now intended: the doubt (her witch, or this one?) is what prompts the first noise test |
| Beacon idea not found | Stuck at minute 7 | The feedback squeal earlier, then the hint ladder |
| Lou's lines feel like exposition | Telling, not discovering | She now says only that she fell and can't see; the player finds where by making noises and listening for them in her feed |
| "It was just Lou" feels deflating | A Scooby-Doo letdown | The reveal isn't a gotcha: the ghost you came to catch was your friend calling for help |
| Cranking and walking feel like filler | Rubric penalty | The crank takes about 3 seconds each way; the corridor is about 20 m; every stop pays off |
| Two kids lying to their parents | Tone | A classic kid-adventure premise; the end text can imply consequences |
| Noise-testing turns into banging on everything | Random play | The feedback is graded (faint, muffled, loud), so even random banging points somewhere; the radiators deliberately all sound the same |

### Does it fail any fundamental test?

Not on paper. The question I can't settle without people is whether players feel clever at the beacon or feel led to it. That's what the first prototype has to answer.

## 6. Building it by Friday

One scene, three small spaces, about nine scripts and thirty sound clips. That's tight for three days, but realistic if an ugly, complete version exists by Wednesday night.

### What to build

**Spaces:**

- **Room 12**, about 6 × 7 m: coat cubby, teacher's desk, jack-o'-lantern, one radiator, windows.
- **Corridor**, about 20 × 3 m: lockers, three or four radiators, the basement door, the front doors, the cafeteria doors with orange light behind the glass.
- **Library**, about 8 × 10 m: shelves, aquarium, chiming clock, the desk with witch and crate, the supply lift in the back wall.
- **The lift:** a door, a car that travels about 3 m, a dark shaft. B-2 is never built; it exists only as sound.
- **Lou:** a bedsheet-ghost mesh sitting in the car for the last shot. No animation.

**Systems (one script each):**

1. First-person movement: a short CharacterController script.
2. Interaction: a raycast, a prompt, press and hold.
3. Note viewer for the pact, the plate and the PTA note.
4. The radio: hold to talk, play Sam's next line, track what's on the channel (library feed, then Lou, then Lou's open feed, then silence), play it on Sam's radio and at radio #4's position, block Talk while receiving.
5. Radio #4: take, put down, put in the lift.
6. The supply lift: door, crank both ways, car position, what's inside, the "climb in" refusal.
7. Knocks: radiators in sync; Lou knocking back when you bang one; later from the shaft, nearer each time; the single knock on the car.
8. Story beats: the witch, the midnight chime, the noise test (four spots, each with its own loudness in Lou's feed), Lou's battery, the hints, the ending.
9. Captions, plus title, pause and end screens.

**Interactables (12):** the pact, the radiators (bangable), the basement door (bangable), the front doors, the cafeteria doors, the crate, radio #4, the lift door, the crank, the plate, the PTA note, Lou's pail.

**Audio (about 30 clips):** hiss, bubbling, clock tick, chime, witch cackle, knock patterns, boiler roar, drip, crate scrape, radio clicks, feedback squeal, rope creak, clunk, footsteps, about six Sam lines and six Lou lines. Record the voices on a phone and run them through a radio filter; amateur voices sound right on a walkie. Use CC0 effects or your own recordings, and log each source for the Game Script.

### Biggest risks

1. **Time.** Everything else is secondary.
2. **Three builds.** Install the Windows and Linux build support modules in Unity Hub, and make a test build of an empty scene for all three platforms on Wednesday. A Mac build working doesn't prove the other two do; borrow a Windows or Linux machine to check.
3. **Sound clues on laptop speakers.** Make each signature distinct and captioned.
4. **The beacon insight.** The first test decides it.

### What to cut, in this order

1. The cafeteria doors.
2. Recorded voices: use captions plus radio crackle.
3. Lou's model: fade out on her knock and "Boo."
4. The middle noise tests (basement door, desk witch), keeping the radiator and the lift; then the hint ladders beyond their first lines.
5. The crank: replace it with a Lower/Raise lever and a 3-second move.
6. Radiator knocks beyond two radiators.

Never cut the midnight chime, freeing #4, Lou's open feed, the lift's noise test or the beacon. Those are the game.

### The first prototype, and what it must prove

1. **A radio play, 20 minutes, no Unity.** See `Docs/Playtests/DeadAir_PaperTest.md`. It passes if, unprompted, the tester heads for the library because of the chime, connects the crate to Lou's breakthrough, thinks to make noise and finds the lift by it, and reaches the beacon with at most the first hint.
2. **Wednesday: the ugly full game.** Every space as gray boxes, placeholder sounds, captions, every beat from waking to the ending. Commit it as v0.1. If the beacon failed on paper, test the hint ladder in this build.
3. **Wednesday night to Thursday: two or three unbriefed testers.** Watch, time them, and note where they stop. Revise and commit each change with what you saw and what you changed; that history is what the rubric's iteration rows grade.

### Schedule

1. **Tuesday night:** decide, run the radio play, install build modules.
2. **Wednesday:** gray-box end to end, test builds on all three platforms, first playtest.
3. **Thursday:** fixes from that playtest, sound and lighting pass, second playtest, fixes.
4. **Friday:** final builds by 6 PM; High Concept Document and Game Script; Box upload; add cwwalterOleMiss as a collaborator; Blackboard submission before 11:59 PM.
