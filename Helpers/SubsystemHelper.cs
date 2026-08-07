using System.Reflection;
using PB.pcm.Constants;
using PhantomBrigade;
using PhantomBrigade.Data;
using UnityEngine;

namespace PB.pcm.Helpers
{
    internal static class SubsystemHelper
    {
        internal static DataContainerSubsystem GetConsumableSubsystemBlueprint(EquipmentEntity part)
        {
            if (part == null)
            {
                return null;
            }

            if (!part.hasLevel)
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

                // HEALING CONSUMABLE
                if (subsystemBlueprint.TryGetFloat(ConsumableKeys.HealingEfficiencyKey, out _, 1f) ||
                    subsystemBlueprint.TryGetFloat(ConsumableKeys.HealingKey, out _, 1f))
                {
                    return subsystemBlueprint;
                }
            }

            foreach (var subsystemBlueprint in subsystemBlueprints)
            {
                // HEALING CONSUMABLE
                if (subsystemBlueprint?.key != null &&
                    subsystemBlueprint.key.Contains(ConsumableKeys.HealConsumableKeyFragment))
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

        // Metodo per distruggere un socket consumabile.
        // Essendo il metodo originale privato accedo tramite Reflection.
        internal static void DestroyConsumableSocket(string consumableSocket)
        {
            CIViewInternalCombatUnit ist = new CIViewInternalCombatUnit();

            var methodInfo = typeof(CIViewInternalCombatUnit)
                .GetMethod("DestroySocket", BindingFlags.Instance | BindingFlags.NonPublic);

            if (methodInfo == null)
            {
                Debug.Log("[PCM] - Metodo non trovato!");
                return;
            }

            object[] parameters = { consumableSocket };

            var result = (string)methodInfo.Invoke(ist, parameters);

            Debug.Log($"[PCM] - Metodo trovato: {methodInfo.Name}");
            Debug.Log($"[PCM] - Risultato: {result}");
        }
    }
}