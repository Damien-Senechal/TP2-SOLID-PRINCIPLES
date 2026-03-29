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

5.1 — BookingService

BookingService instanciait directement InMemoryReservationStore et FileLogger dans ses champs. Impossible de changer le stockage ou le logging sans modifier la classe elle-même. J'ai créé deux interfaces IBookingRepository et IBookingLogger dans le namespace métier, et BookingService les reçoit par constructeur. Les implémentations concrètes sont dans Infrastructure et implémentent ces interfaces. Le service métier ne connaît plus aucune classe d'infrastructure.

5.2 — HousekeepingService

HousekeepingService dépendait directement de EmailSender, une classe technique. Pour passer aux SMS il aurait fallu modifier le service métier. J'ai créé ICleaningNotifier dans le domaine, et EmailCleaningNotifier dans Infrastructure comme adapter autour de EmailSender. HousekeepingService ne reçoit que ICleaningNotifier par constructeur et ne sait plus du tout comment la notification est envoyée.

SRP : Dans le code de départ, plusieurs classes mélangeaient des responsabilités qui n'avaient rien à voir entre elles. ReservationService faisait de l'infra, du métier et de l'orchestration en même temps, Reservation servait trois acteurs différents (réceptionniste, comptable, gouvernante) et CheckInService mélangait des règles métier avec de la manipulation de cache. On a séparé chaque classe selon son acteur : les repositories pour l'infra, les domain services pour le métier, les application services pour l'orchestration. Ça permet de modifier une règle métier sans risquer de casser le stockage, et inversement.

OCP : CancellationService utilisait un switch/case sur le type de politique, ce qui obligeait à modifier la classe à chaque nouvelle politique. On a extrait une interface ICancellationPolicy avec une classe par politique. Maintenant ajouter une politique "SuperFlexible" ne demande que de créer une nouvelle classe sans toucher au service. Le code était déjà bien structuré pour OCP sur les événements (Observer), les prix (Decorator) et le nettoyage (Strategy), on a juste appliqué le même principe aux annulations.

LSP : NonRefundableReservation implémentait ICancellable mais faisait un throw dans Cancel(), ce qui faisait planter le code au runtime de façon inattendue. On a séparé IReservation (base sans Cancel) et ICancellableReservation (avec Cancel). NonRefundableReservation n'implémente que IReservation, donc l'appel à Cancel() est maintenant impossible à la compilation. Pour CachedRoomRepository, il ignorait les paramètres de date et retournait des données périmées, on a corrigé pour qu'il délègue toujours au repo interne.

ISP : IReservationRepository avait 9 méthodes alors que chaque consommateur n'en utilisait que 1 à 3. INotificationService regroupait 4 canaux alors que chaque service n'en utilisait qu'un. On a segmenté ces interfaces en interfaces plus petites et ciblées. InvoiceGenerator prenait une Reservation complète mais n'utilisait que 6 champs, on a créé IInvoiceable avec uniquement ces champs. Chaque consommateur ne dépend plus que de ce qu'il utilise vraiment.

DIP : BookingService instanciait directement InMemoryReservationStore et FileLogger, et HousekeepingService dépendait directement de EmailSender. Ces couplages forts rendaient impossible tout changement d'implémentation sans modifier le code métier. On a défini les interfaces IBookingRepository, IBookingLogger et ICleaningNotifier dans le namespace métier, et les implémentations concrètes dans Infrastructure. Les services reçoivent ces abstractions par constructeur et ne connaissent plus aucune classe d'infrastructure.