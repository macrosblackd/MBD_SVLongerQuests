# MBD_SVLongerQuests

## Description

MBD_SVLongerQuests is a BepInEx plugin for Star Valor that increases the duration of delivery quests. By applying a configurable multiplier, this mod allows players to extend the time limits for local and regional delivery missions, making quest completion more flexible.

## Features

- Configurable quest duration multiplier (default: 3x).
- Option to disable delivery quest timeout entirely.
- Applies to both local and regional delivery quests.

## Installation

1. **Requirements**
   - Star Valor (game)
   - [BepInEx 5.x](https://github.com/BepInEx/BepInEx)

2. **Steps**
   1. Download and install BepInEx in your Star Valor game directory.
   2. Place the compiled `MBD_SVLongerQuests.dll` into the `BepInEx/plugins` folder.
   3. Launch Star Valor. The mod will automatically load.
   4. Configuration file (`MBD_SVLongerQuests.cfg`) will be generated in `BepInEx/config` after first launch. Adjust configuration options as desired.

## Configuration

- `QuestDurationMultiplier`: Sets how much longer delivery quests last (default: 3).
- `DisableDeliveryQuestTimeout`: If set to `true`, delivery quests will not expire due to time limits.