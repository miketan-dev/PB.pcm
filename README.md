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
| **License**              | BSD 3-Clause License                            |

---

## CREDITS

- Harmony Framework for the patching,
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
> <br>The above code project is made public to adhere
> to [Brace Yourself Games' guidelines](https://braceyourselfgames.com/mod-policy/)
> mostly to certify the present Library Code **DOES NOT CONTAIN** any malware and/or trojan in every form, stating that the mod does not perform any file/folder creation outside mod's directory.
> <br><br>You are free to use my mod as a dependency to other mods as long as you give
> credit to me, as this mod is also covered under **BSD-3 License**.

---

## Mod Intro

**Pilot Consumable Mod (P.C.M.)** is a library and functional mod for *Phantom Brigade* that introduces a system for pilots to use consumable items during combat. 

This mod extends the game's mechanics, allowing for new tactical possibilities such as repairing or enhancing units on the battlefield through equipped consumable items.

## Key Features

* **Consumable Usage:** Enables the use of specific consumable items during combat actions.
* **Healing System:** Integrated logic to allow items to repair pilot/unit health.
* **Customizable:** Fully configurable through YAML, allowing modders to define base healing values and efficiency coefficients per item.
* **Smart Resource Management:** Automatically consumes charges and destroys the item once all charges are depleted.

## Mod Mechanics

When a combat action assigned to a consumable item is triggered:

1. **Identification:** The system identifies the pilot and the consumable item being used.
2. **Data Retrieval:** It accesses the item's blueprint to retrieve custom parameters defined in the `custom` block of the YAML configuration.
3. **Calculation:** The mod calculates the healing effect based on the following formula:
   `Final Heal = (healing * healing_efficiency)`
4. **Safety Limits:** It ensures the healing does not exceed the unit's maximum health by applying `Mathf.Min(healAmount, missingHealth)`.
5. **Execution:** The healing is applied to the pilot, and if the item's charges are exhausted, the item is destroyed.

## YAML Configuration

To make an item consumable and functional with this mod, add the following structure to its YAML definition:

```yaml
custom:
  floats:
    healing: 20            # Base healing amount
    healing_efficiency: 0.5 # Healing coefficient (e.g., 20 * 0.5 = 10)
```
