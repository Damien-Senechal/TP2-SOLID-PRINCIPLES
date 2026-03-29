# JUSTIFICATION — Principes SOLID

## Exercice 1 — SRP (Single Responsibility Principle)

---

### 1.1 — ReservationService

**Problème :** La classe faisait tout : elle loggait, vérifiait la dispo, calculait le prix et orchestrait la création. Trois acteurs différents (ops, métier, chef de projet) pouvaient demander des changements dessus.

**Solution :** On a séparé en 3 couches :
- `RoomRepository` / `ReservationRepository` → stockage (infra)
- `ReservationDomainService` → règles métier (dispo, capacité, prix)
- `ReservationService` → orchestration uniquement (applicatif)

**Bénéfice :** Si on change la règle de disponibilité, on touche uniquement `ReservationDomainService`. Si on passe à une vraie BDD, seul le repository change. Les modifications sont isolées.

---

### 1.2 — CheckInService

**Problème :** `ProcessCheckIn` mélange des opérations de haut niveau (validation, frais) avec des détails bas niveau (cache, SMS). Difficile à lire et à faire évoluer.

**Solution :** La méthode principale ne contient plus que des appels haut niveau (`ValidateCheckIn`, `ApplyLateCheckInFeeIfNeeded`, `UpdateStatus`, `NotifyRoomOccupied`). Les détails sont délégués à des méthodes privées dédiées.

**Bénéfice :** On comprend l'intention de la méthode en 4 lignes. Changer le système de notification ne risque pas de casser la logique de validation.

---

### 1.3 — Reservation

**Problème :** La classe servait 3 acteurs distincts :
- **Le réceptionniste** : `Cancel()` — règles d'annulation
- **Le comptable** : `CalculateTotal()`, `GenerateInvoiceLine()` — facturation
- **La gouvernante** : `GetLinenChangeDays()` — planning ménage

Si le comptable demande un changement de TVA, on modifie une classe que le réceptionniste et la gouvernante utilisent aussi.

**Solution :** On a extrait `BillingCalculator` (comptable) et `HousekeepingScheduler` (gouvernante). `Reservation` garde uniquement les données et `Cancel()` pour le réceptionniste.

**Bénéfice :** Chaque classe n'a plus qu'une seule raison de changer. On peut tester et faire évoluer la facturation sans toucher au planning de ménage.