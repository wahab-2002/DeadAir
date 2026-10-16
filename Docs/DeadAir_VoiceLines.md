# Dead Air — Voice lines and when they play

Every spoken line in the game. The ID is the audio file name: a real recording goes in `Assets/_DeadAir/Audio/Voice/<id>.wav`, and until then a generated placeholder is used. Captions use the line text exactly as written here.

Lou's lines always come through a radio. Sam's lines are spoken aloud; when Sam talks on the radio, the line also plays from radio #4 wherever it is.

## Lou

| ID | Line | When it plays |
| --- | --- | --- |
| lou_00 | "—am? Sam, please. Anyone. Over." | 1.5 s after `sam_07`, when the stuck radio is freed. Repeats every 10 s until Sam talks. |
| lou_01 | "Sam?! I've been calling for hours. I hid to jump out at you, and then I was falling, and now it's pitch black. I don't know where I am. My flashlight broke." | Reply to `sam_01`. Dripping and a deep roar behind her. |
| lou_02 | "Boxes, I think. Sheets. And something laughs every time I move." | Reply to `sam_02`. |
| lou_02b | "That!" | Right after a muffled cackle that follows `lou_02`. |
| lou_03 | "My batteries are almost dead. I'm gonna hold the button down so you can hear what I hear. Bang on stuff, Sam. I'll tell you if it's close." | After the next Talk, or 15 s after `lou_02b`. Her feed stays open from here (LouOpenFeed). |
| lou_04 | "Was that you? I heard it in the pipe... but it's everywhere." | Open feed: the first time Sam bangs a radiator. |
| lou_05 | "I hear you. Keep going." | Open feed: the first time a noise level whose line already played is heard again. Once per game. |
| lou_06 | "That was far away." | Open feed: the first Far noise. |
| lou_07 | "The laugh again... no, that one sounded different. Closer." | Open feed: the first time the desk witch cackles. |
| lou_08 | "Closer... I think." | Open feed: the first Near noise. |
| lou_09 | "Warmer." | Open feed: the first Warm noise. |
| lou_10 | "Close... but not there." | Open feed: each time a decoy sheet is pulled. |
| lou_11 | "That! That's right next to me! Sam, I think that's what I fell out of... the light's blinking... please don't leave me down h—" | Open feed: the first touch of the lift (its sheet, door, crank or car). Cut off by a click; her battery is dead (Silent). |
| lou_12 | "BOO!" | Ending: she jumps out of the lift car. |
| lou_13 | "Got you." | Ending, after `lou_12`. |
| lou_14 | "Did you get the Knocking Ghost on tape?" | Ending, after `lou_13`. |

## Sam

| ID | Line | When it plays |
| --- | --- | --- |
| sam_00 | "Lou? I fell asleep. Are you there? Over." | First Talk before the stuck radio is freed. |
| sam_01 | "Lou! It's me! I fell asleep. Where are you?" | First Talk after `lou_00`. |
| sam_02 | "What's down there with you?" | Next Talk after `lou_01`, or 15 s after it. |
| sam_03 | "She can't hear me while she's holding it." | First Talk during the open feed. Later Talks: caption only, "[Nothing. Her light's still on.]" |
| sam_04 | "Not without Lou." | Using the front doors. |
| sam_05 | "Just pumpkin bread." | Lifting a decoy crate. |
| sam_06 | "Just a wall." | Pulling a decoy sheet. |
| sam_07 | "Mom's PTA radio. It was stuck on talk the whole time." | Lifting the real crate, as the hiss cuts out. |
| sam_08 | "Lou's candy pail. She was in here." | First look at Lou's pail in the lift car. |
| sam_09 | "If I go down too, there's nobody to crank us back up." | Trying to climb into the lift car. |
| sam_10 | "Lou? Lou, can you hear me?" | 2 s after Lou's battery dies, then on any Talk while the channel is Silent and the beacon isn't set up. |
| sam_11 | "She's at the bottom of that lift. I have to get her up." | The first time the lift door is open after Lou's battery died. |
| sam_12 | "Lou! The cupboard's back! Follow my voice!" | Beacon Talk 1 (radio #4 in the car at the bottom). |
| sam_13 | "Keep coming! You're close!" | Beacon Talk 2. |
| sam_14 | "Climb in. I'll pull you up." | Beacon Talk 3. |
| sam_15 | "Yeah. It was you." | Ending, after `lou_14`. |

## Sam's hints (only if the player is stuck)

| ID | Line | When it plays |
| --- | --- | --- |
| sam_h1 | "Something's glowing under that crate." | 45 s in the library without lifting the real crate. |
| sam_h2 | "If she's holding the button, anything loud near her should come through." | 40 s into the open feed with no noise made. |
| sam_h3 | "She said she fell. Fell from where?" | 80 s into the open feed with the lift not touched. |
| sam_h4 | "The crank... I could send the lift down to her." | 15 s after `sam_11` if the car hasn't moved. |
| sam_h5 | "She can't see it. It's pitch black down there." | 20 s after the empty car first reaches the bottom. |
| sam_h6 | "She needs something to follow." | 45 s after the same. |
| sam_h7 | "The PTA radio... if she could hear me down there." | 75 s after the same. |

Hints `sam_h5`–`sam_h7` stop once the beacon has started.

## Not spoken (on-screen text only)

- "[In the walls, the pipes go quiet.]"
- "Neither of them chickened out."
- "DEAD AIR"
