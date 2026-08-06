# OnConsumableUseCombat

La classe `OnConsumableUseCombat` è un modulo di logica di gioco (funzione di esecuzione azione in combattimento) per Phantom Brigade. Gestisce l'effetto di "cura" quando un pilota utilizza un oggetto consumabile (es. uno zaino con funzioni di cura).

## Funzionamento

Quando un'azione che utilizza un consumabile viene attivata, la funzione:
1.  Identifica il pilota che sta utilizzando l'oggetto.
2.  Recupera le statistiche dell'oggetto consumabile dal blueprint dei dati.
3.  Legge la statistica personalizzata `stat_heal` per determinare la quantità di HP base da curare.
4.  Legge l'opzione personalizzata `healing_efficiency` (se presente) per scalare la cura.
5.  Calcola la cura finale, assicurandosi di non superare la salute massima del pilota (`Mathf.Min(cura, saluteMancante)`).
6.  Applica la cura al pilota utilizzando `PilotUtility.OffsetPilotStat`.
7.  Se l'oggetto ha esaurito le cariche (`act_charges <= 0`), lo distrugge.

## Configurazione YAML (Richiesta)

Per utilizzare questa funzione, l'oggetto consumabile nel file YAML deve avere le seguenti definizioni nella sezione `custom`:

### Parametri di Cura
Definisci `healing` (quantità base) e `healing_efficiency` (moltiplicatore) sotto `custom/floats`.

```yaml
custom:
  floats:
    healing: 20            # Quantità base di cura
    healing_efficiency: 0.5 # Coefficiente di cura (es: 20 * 0.5 = 10)
```

## Esempio di Calcolo
La formula applicata è:
`Cura Finale = (healing * healing_efficiency)`

Se `healing = 20` e `healing_efficiency = 0.5`, la cura totale è `10`.
La cura effettiva applicata sarà `min(10, salute_mancante)`.
