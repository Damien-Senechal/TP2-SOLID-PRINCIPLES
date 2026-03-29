Justification

1.1

La classe ReservationService prenait trop de choses en charge ce qui n'est pas bien. J'ai a donc séparé la classe en 3 couches pour repartir les taches et permettre des modification plus simple.
RoomRepository et ReservationRepository s'occupe du stockage, ReservationDomainService gere la partie metier et ReservationService de l'applicatif.

1.2

ProcessCheckIn presentait un probleme similaire les taches n'etait pas séparé la methode faisait trop de choses. Elle avait acces a des details bas niveau et faisait tout d'un coup ce qui est une mauvaise pratique si on utilise une architecture SRP.
J'ai donc séparer cette methode en plusieurs methodes qui effectue une tache chacune : ValidateCheckIn, ApplyLateCheckInFeeIfNeeded, UpdateStatus, NotifyRoomOccupied

1.3

La classe Reservation se servait de 3 acteurs distinct Cancel, CalculateTotal et GenerateInvoiceLine, GetLinenChangeDays. J'ai donc extrait et séparé la partie comptabilité et gouvernance. Ce qui permet d'effectuer plus facilement des modification sur le code.

Exercice 2 — OCP (Open/Closed Principle)

2.1 — Les bons exemples déjà présents

ReservationEventDispatcher utilise le pattern Observer. On peut ajouter autant de handlers qu'on veut via Register sans jamais toucher au dispatcher, c'est un bon exemple OCP.
IPriceCalculator avec SeasonalSurchargeDecorator utilise le pattern Decorator. Pour ajouter une nouvelle règle de prix il suffit de créer un nouveau decorator qui enveloppe le calculateur existant, aucune classe existante n'est modifiée.
ICleaningPolicy avec StandardCleaningPolicy et VipCleaningPolicy utilise le pattern Strategy. Chaque politique est une classe séparée, ajouter une nouvelle politique de nettoyage ne demande que de créer une nouvelle implémentation.

2.2 — CancellationService

Le switch/case dans CancellationService était problématique car pour ajouter une nouvelle politique il fallait modifier directement la classe, ce qui viole OCP.
J'ai donc créé une interface ICancellationPolicy et une classe par politique : FlexiblePolicy, ModeratePolicy, StrictPolicy, NonRefundablePolicy. CancellationService récupère simplement la bonne politique par son nom et l'appelle. Ajouter une nouvelle politique ne nécessite plus de toucher au service.

