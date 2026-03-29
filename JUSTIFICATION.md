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

3.1 — ICancellable / FlexibleReservation / NonRefundableReservation

NonRefundableReservation implémentait ICancellable mais faisait un throw dans Cancel(). N'importe quel code qui appelle Cancel() sur un ICancellable pouvait donc planter au runtime sans qu'on s'y attende, ce qui viole LSP.
J'ai séparé l'interface en deux : IReservation qui est la base commune sans Cancel(), et ICancellableReservation qui étend IReservation et ajoute Cancel() uniquement pour les réservations annulables. NonRefundableReservation n'implémente que IReservation, donc l'appel à Cancel() est impossible à la compilation.

3.2 — CachedRoomRepository

CachedRoomRepository était censé remplacer IRoomRepository mais GetAvailableRooms ignorait les paramètres de date et retournait des données potentiellement périmées. Et Save() n'invalidait pas le cache. Substituer ce repo à la place d'un autre donnait des résultats incorrects, ce qui viole LSP.
J'ai corrigé GetAvailableRooms pour qu'elle délègue toujours au repo interne afin d'avoir des données fraîches. Et Save() invalide maintenant l'entrée correspondante dans le cache.

4.1 — IReservationRepository

L'interface avait 9 méthodes mais chaque consommateur n'en utilisait que 1 à 3. Par exemple HousekeepingService n'avait besoin que de GetByDateRange mais dépendait quand même des 8 autres. J'ai donc séparé en trois interfaces : IReservationReader pour la lecture, IReservationWriter pour l'écriture, et IReservationStats pour les statistiques. Chaque service ne dépend plus que de ce dont il a réellement besoin.

4.2 — InvoiceGenerator

InvoiceGenerator prenait une Reservation complète mais n'utilisait que 6 champs. J'ai créé une interface IInvoiceable avec uniquement ces champs. Reservation implémente cette interface et InvoiceGenerator ne dépend plus que d'elle. Si on ajoute un champ à Reservation qui ne concerne pas la facturation, InvoiceGenerator n'est plus impacté.

4.3 — INotificationService

L'interface regroupait 4 canaux de notification alors que chaque consommateur n'en utilisait qu'un. J'ai séparé en IEmailSender, ISmsSender, IPushNotifier et ISlackNotifier. Chaque service reçoit uniquement l'interface dont il a besoin.

