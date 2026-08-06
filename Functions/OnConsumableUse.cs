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
        public void Run(CombatEntity unitCombat, ActionEntity action)
        {
            if (unitCombat == null)
            {
                Debug.Log("[PCM] - CombatEntity unitCombat non trovato.");
                return;
            }

            var pilot = IDUtility.GetLinkedPersistentEntity(unitCombat);
            if (pilot == null)
            {
                Debug.Log("[PCM] - Pilota non trovato.");
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

            if (!consumablePart.hasDataLinkSubsystem)
            {
                Debug.Log($"[PCM] - Part consumabile {consumablePart.nameInternal.s} non ha un DataLinkSubsystem.");
                return;
            }

            var consumableBlueprint = consumablePart.dataLinkSubsystem.data;
            if (consumableBlueprint == null)
            {
                Debug.Log("[PCM] - Consumable blueprint non trovato.");
                return;
            }

            var hp = DataHelperStats.GetCachedStatForPart("hp", consumablePart);
            var charge = DataHelperStats.GetCachedStatForPart("act_charges", consumablePart);

            consumableBlueprint.TryGetFloat("healing_efficiency", out var efficiency, 1f);

            var totalHeal = hp * efficiency;
            pilot.OffsetPilotStat("hp", totalHeal);

            if (charge <= 0f && !consumablePart.isDestroyed)
            {
                consumablePart.Destroy();
            }

            Debug.Log($"[PCM] - HP: {hp} - Curato con successo di: {totalHeal}");
        }
    }
}