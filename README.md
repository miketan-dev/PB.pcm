# Pilot Consumable Mod - P.C.M

![Mod Version](https://img.shields.io/badge/Mod%20Version-v1.0.0-blue)
![Game Version](https://img.shields.io/badge/Phantom%20Brigade-%3E%3D%20v2.0-green)
![Framework](https://img.shields.io/badge/Framework-.NET%20v4.7.2-purple)
![Language](https://img.shields.io/badge/Language-C%23%207.3-informational)

| Metadata                 | Details                                         |
|:-------------------------|:------------------------------------------------|
| **Release Date**         | TBD                                             |
| **Update Date**          | TBD                                             |
| **Mod Version**          | `v1.0.0`                                        |
| **Repository**           | [PB.pcm](https://github.com/miketan-dev/PB.pcm) |
| **Programming Language** | C# 7.3 (.NET Framework v4.7.2)                  |
| **Minimum Game Version** | v2.0+                                           |
| **License**              | BSD-3 Clause License                            |

---

## CREDITS

- Harmony Framework for the patching;
- Phantom Brigade Modding System;
- Brace Yourself Games for the awesome game!

---

## MOD STATUS & DL LINK(S)

- [Steam Workshop](#) 🟡  
  
- [Nexus Mod](#) 🟡  
  

---

## INSTALLATION (EPIC GAME VERSION)

To install the mod:

1. Extract the mod folder into the following directory:
   <br>```[Drive]:\Users\[yourUser]\AppData\Local\PhantomBrigade\Mods```
   <br><br>
2. Launch the game; the mod will be automatically detected and activated.

> ⚠️ **[DISCLAIMER]** ⚠️
> <br>While the mod has been fully tested by covering most of the use cases, make sure to back up your save file before
> applying the mod to avoid any unintended (and negative) effects.
> <br><br>I will not be held responsible for any misuse of this mod or any damage caused to
> save files.
> <br>The present project repository is made public to adhere
> to [Brace Yourself Games' guidelines](https://braceyourselfgames.com/mod-policy/).<br>
> The mod author certifies that the present Library Code **DOES NOT CONTAIN/EXECUTE** any kind of malware, stating that the mod does not perform any file/folder creation, if any, outside mod's directory.
> <br><br>You are free to use my mod as a dependency for your mod(s) as long as you give
> credits to me.
> <br>The present project is under **BSD-3 License**, available [here](https://github.com/miketan-dev/PB.pcm/blob/master/LICENSE.md).

---

## Mod Intro

**Pilot Consumable Mod (P.C.M.)** is a library and functional mod for *Phantom Brigade* that introduces a system for pilots to use consumable items during combat.<br>
This mod extends the game's mechanics, allowing for new tactical possibilities such as repairing or enhancing units on the battlefield through equipped consumable items.

## Key Features

* **Consumable Usage:** Enables the use of a unique item called ***consumable items*** in combat.
* **Healing System (Healing consumables only):** Integrated logic to allow items to heal the pilot in combat.
* **Customizable:** Fully configurable through YAML, allowing modders to define base healing values and efficiency coefficients per item.
* **Smart Resource Management:** Automatically consumes charges and destroys the item once all charges are depleted. Unlike standard charges (used by Backpacks in vanilla game), the depletion of charges will make the consumable item permanently lost.

## Mod Mechanics

When a combat action assigned to a consumable item is triggered (via `OnConsumableUseCombat`):

1. **Unit & Pilot Identification:** The mod resolves the `CombatEntity` to its linked `PersistentEntity` and subsequently to the `Pilot` entity to retrieve current stats.
2. **Item Resolution:** It locates the `EquipmentEntity` associated with the active equipment part used in the combat action.
3. **Blueprint Retrieval (`SubsystemHelper`):**
    - The mod queries the `SubsystemHelper` to find the relevant `DataContainerSubsystem` blueprint.
    - It searches for subsystems matching specific keys:
        - `healing_efficiency` (`ConsumableKeys.HealingEfficiencyKey`)
        - `healing` (`ConsumableKeys.HealingKey`)
        - Or containing the fragment `consumable_heal` (`ConsumableKeys.HealConsumableKeyFragment`).
4. **Healing Calculation (Healing consumables only):**
    - The healing amount is derived from the blueprint: `totalHeal = healing * healing_efficiency`.
    - It respects the pilot's maximum HP by capping the heal: `finalHeal = Mathf.Min(totalHeal, availableSpace)`.
    - The pilot's HP stat is updated using `PilotUtility.OffsetPilotStat`.
5. **Consumption & Cleanup:**
    - The mod checks the item's `chargeCount`.
    - If charges reach 1, it uses reflection to call the internal `DestroySocket` method on `CIViewInternalCombatUnit` to destroy the consumable socket (defaulting to "back"), which also involves the visual item.


## YAML Configuration

To make an item consumable and functional with this mod, add the following structure to its YAML definition within the `custom:` block of your Subsystem config file.

***The following block is an example for healing consumables:***
```yaml
custom:
  floats:
    healing: 20            # Defined by ConsumableKeys.HealingKey
    healing_efficiency: 0.5 # Defined by ConsumableKeys.HealingEfficiencyKey
```

Alternatively, ensuring your subsystem key contains `consumable_heal:` (`ConsumableKeys.HealConsumableKeyFragment`) will also trigger the logic.
<br>This mod works best with items using charge stat `act_charges:` in the subsystem, commonly used in backpack subsystems.

