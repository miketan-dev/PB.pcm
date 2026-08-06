# OnConsumableUseCombat

The `OnConsumableUseCombat` class is a game logic module (combat action execution function) for Phantom Brigade. It handles the healing effect when a pilot uses a consumable item (e.g., a backpack with healing functions).

## How it works

When an action using a consumable is activated, the function:
1. Identifies the pilot using the item.
2. Retrieves the consumable item's statistics from the data blueprint.
3. Reads the custom parameters `healing` and `healing_efficiency` from the `custom` block to determine the healing amount.
4. Calculates the final healing, ensuring it does not exceed the pilot's maximum health (`Mathf.Min(healAmount, missingHealth)`).
5. Applies the healing to the pilot using `PilotUtility.OffsetPilotStat`.
6. If the item has no charges left (`act_charges <= 0`), it destroys it.

## YAML Configuration (Required)

To use this function, the consumable item in the YAML file must have the following definitions in the `custom` section:

### Healing Parameters
Define `healing` (base amount) and `healing_efficiency` (multiplier) under `custom/floats`.

```yaml
custom:
  floats:
    healing: 20            # Base healing amount
    healing_efficiency: 0.5 # Healing coefficient (e.g., 20 * 0.5 = 10)
```

## Calculation Example
The formula applied is:
`Final Heal = (healing * healing_efficiency)`

If `healing = 20` and `healing_efficiency = 0.5`, the total heal is `10`.
The actual healing applied will be `min(10, missing_health)`.
