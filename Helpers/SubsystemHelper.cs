using PB.pcm.Constants;
using PhantomBrigade;
using PhantomBrigade.Data;

namespace PB.pcm.Utils
{
    internal static class SubsystemHelper
    {
        public static DataContainerSubsystem GetConsumableSubsystemBlueprint(EquipmentEntity part)
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

                if (subsystemBlueprint.TryGetFloat(SubsystemKeys.HealingEfficiencyKey, out _, 1f) ||
                    subsystemBlueprint.TryGetFloat(SubsystemKeys.HealingKey, out _, 1f))
                {
                    return subsystemBlueprint;
                }
            }

            foreach (var subsystemBlueprint in subsystemBlueprints)
            {
                if (subsystemBlueprint?.key != null &&
                    subsystemBlueprint.key.Contains(SubsystemKeys.HealConsumableKeyFragment))
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