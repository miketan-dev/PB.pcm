using Content.Code.Utility;
using PhantomBrigade;
using PhantomBrigade.Data;
using PhantomBrigade.Functions;
using UnityEngine;

namespace PB.pcm.Functions
{
    [TypeHintedPrefix("pcm.functions")]
    public sealed class OnConsumableUseCombat : ICombatActionExecutionFunction
    {
        private const string HealingKey = "healing";
        private const string HealingEfficiencyKey = "healing_efficiency";
        private const string HealConsumableKeyFragment = "consumable_heal";

        public void Run(CombatEntity unitCombat, ActionEntity action)
        {
            if (unitCombat == null)
            {
                Debug.Log("[PCM] - CombatEntity unitCombat non trovato.");
                return;
            }

            var unitPersistent = IDUtility.GetLinkedPersistentEntity(unitCombat);
            if (unitPersistent == null)
            {
                Debug.Log("[PCM] - PersistentEntity dell'unità non trovata.");
                return;
            }

            var pilot = IDUtility.GetLinkedPilot(unitPersistent);
            if (pilot == null)
            {
                Debug.Log($"[PCM] - Pilota non trovato per unità persistent: {IDUtility.ToLog(unitPersistent)}");
                return;
            }

            EquipmentEntity consumablePart = null;

            if (action != null && !action.isDisposed && action.hasActiveEquipmentPart)
            {
                consumablePart = IDUtility.GetEquipmentEntity(action.activeEquipmentPart.equipmentID);
            }

            if (consumablePart == null)
            {
                Debug.Log("[PCM] - Consumabile non trovato nell'azione corrente.");
                return;
            }

            var consumableBlueprint = GetConsumableSubsystemBlueprint(consumablePart);
            if (consumableBlueprint == null)
            {
                Debug.Log(
                    $"[PCM] - Subsystem blueprint consumabile non trovato per la parte: {IDUtility.ToLog(consumablePart)}");
                return;
            }

            var charge = DataHelperStats.GetCachedStatForPart("act_charges", consumablePart);

            consumableBlueprint.TryGetFloat(HealingEfficiencyKey, out var efficiency);
            consumableBlueprint.TryGetFloat(HealingKey, out var healing);

            float currentHp = pilot.GetPilotStat("hp");
            float maxHp = pilot.GetPilotStatMax("hp");

            if (currentHp > 0 && currentHp < maxHp)
            {
                float availableSpace = maxHp - currentHp;
                var totalHeal = healing * efficiency;
                float finalHeal = Mathf.Min(totalHeal, availableSpace);

                PilotUtility.OffsetPilotStat(pilot, "hp", finalHeal, false);
                Debug.Log($"[PCM] - HP: {healing} - Curato con successo di: {totalHeal}");
            }

            if (charge <= 0f && !consumablePart.isDestroyed)
            {
                consumablePart.Destroy();
                Debug.Log($"[PCM] - Consumabile: {IDUtility.ToLog(consumablePart)} - Distrutto");
            }

            Debug.Log($"[PCM] - Cura effettuata con successo.");
        }

        private static DataContainerSubsystem GetConsumableSubsystemBlueprint(EquipmentEntity part)
        {
            if (part == null)
            {
                return null;
            }

            var primaryBlueprint = GetPrimaryActivationSubsystemBlueprint(part);
            if (primaryBlueprint != null)
            {
                return primaryBlueprint;
            }

            var subsystemBlueprints = EquipmentUtility.GetSubsystemBlueprintsInPart(part);
            if (subsystemBlueprints == null || subsystemBlueprints.Count == 0)
            {
                EquipmentUtility.RefreshPartSubsystemLookup(part);
                subsystemBlueprints = EquipmentUtility.GetSubsystemBlueprintsInPart(part);
            }

            if (subsystemBlueprints == null || subsystemBlueprints.Count == 0)
            {
                return null;
            }

            foreach (var subsystemBlueprint in subsystemBlueprints)
            {
                if (subsystemBlueprint == null)
                {
                    continue;
                }

                if (subsystemBlueprint.TryGetFloat(HealingEfficiencyKey, out _, 1f))
                {
                    return subsystemBlueprint;
                }

                if (subsystemBlueprint.TryGetFloat(HealingKey, out _, 1f))
                {
                    return subsystemBlueprint;
                }
            }

            foreach (var subsystemBlueprint in subsystemBlueprints)
            {
                if (subsystemBlueprint != null
                    && subsystemBlueprint.key != null
                    && subsystemBlueprint.key.Contains(HealConsumableKeyFragment))
                {
                    return subsystemBlueprint;
                }
            }

            return subsystemBlueprints.Count == 1 ? subsystemBlueprints[0] : null;
        }

        private static DataContainerSubsystem GetPrimaryActivationSubsystemBlueprint(EquipmentEntity part)
        {
            if (part == null || !part.hasPrimaryActivationSubsystem)
            {
                return null;
            }

            var subsystem = IDUtility.GetEquipmentEntity(part.primaryActivationSubsystem.equipmentID);
            if (subsystem == null || !subsystem.hasDataLinkSubsystem)
            {
                return null;
            }

            return subsystem.dataLinkSubsystem.data;
        }
    }
}