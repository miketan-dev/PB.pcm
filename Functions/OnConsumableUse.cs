using Content.Code.Utility;
using PB.pcm.Constants;
using PB.pcm.Helpers;
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

            var unitPersistent = IDUtility.GetLinkedPersistentEntity(unitCombat);
            if (unitPersistent == null)
            {
                Debug.Log("[PCM] - PersistentEntity dell'unità non trovata.");
                return;
            }

            var pilot = IDUtility.GetLinkedPilot(unitPersistent);
            if (pilot == null)
            {
                Debug.Log($"[PCM] - Pilota non trovato per unità persistent: {unitPersistent}");
                return;
            }

            EquipmentEntity consumablePart = null;

            if (action != null && !action.isDisposed && action.hasActiveEquipmentPart)
            {
                consumablePart = IDUtility.GetEquipmentEntity(action.activeEquipmentPart.equipmentID);
                Debug.Log($"[PCM] - Consumabile trovato: {consumablePart}");
            }

            if (consumablePart == null)
            {
                Debug.Log("[PCM] - Consumabile non trovato nell'azione  corrente.");
                return;
            }

            var consumableBlueprint = SubsystemHelper.GetConsumableSubsystemBlueprint(consumablePart);
            if (consumableBlueprint == null)
            {
                Debug.Log(
                    $"[PCM] - Subsystem blueprint consumabile non trovato per la parte: {consumablePart}");
                return;
            }

            var charge = DataHelperStats.GetCachedStatForPart("act_charges", consumablePart);

            consumableBlueprint.TryGetFloat(SubsystemKeys.HealingEfficiencyKey, out var efficiency);
            consumableBlueprint.TryGetFloat(SubsystemKeys.HealingKey, out var healing);

            var currentHp = pilot.GetPilotStat("hp");
            var maxHp = pilot.GetPilotStatMax("hp");

            if (currentHp > 0 && currentHp < maxHp)
            {
                var availableSpace = maxHp - currentHp;
                var totalHeal = healing * efficiency;
                var finalHeal = Mathf.Min(totalHeal, availableSpace);

                PilotUtility.OffsetPilotStat(pilot, "hp", finalHeal, false);
                Debug.Log($"[PCM] - HP: {healing} - Curato con successo di: {totalHeal}");
            }

            consumablePart.isDestroyed = false;
            if (charge <= 0f && !consumablePart.isDestroyed || !consumablePart.isWrecked)
            {
                // TODO: il consumabile sembra non distruggersi come un braccio o altro; forzare il bool a true.
                consumablePart.isDestroyed = true;
                Debug.Log($"[PCM] - Consumabile {consumablePart} - Distrutto");
            }
            else
            {
                Debug.Log($"[PCM] - Consumabile {consumablePart} - Non ancora distrutto.");
            }

            Debug.Log($"[PCM] - Cura effettuata con successo.");
        }
    }
}