# Foreign Keys και navigation properties για MaintenanceTask

## Στόχος

Να αντικατασταθούν οι ελεύθερες string τιμές κατάστασης και προτεραιότητας
στο `MaintenanceTask` με πραγματικές σχέσεις προς τους πίνακες `Statuses` και
`Priorities`. Οι σχέσεις θα προστατεύουν την ακεραιότητα των δεδομένων και
δεν θα διαγράφουν tasks αν διαγραφεί lookup εγγραφή.

## Προτεινόμενα βήματα

1. Στο `Aviation.Api/Entities/MaintenanceTask.cs`, πρόσθεσε required foreign
   key properties `StatusId` και `PriorityId`, καθώς και navigation properties
   `Status` και `Priority`. Χρησιμοποίησε τύπο `long` για τα FK properties:
   το `BaseEntity.Id` των `Status` και `Priority` είναι `long`, οπότε τα FK
   πρέπει να έχουν τον ίδιο συμβατό τύπο. Η αρχική απαίτηση για `int` δεν
   ταιριάζει με το υπάρχον μοντέλο.
2. Πρόσθεσε συλλογές navigation properties στα `Status` και `Priority`, αν
   απαιτούνται από το επιλεγμένο Fluent API mapping για τις αντίστροφες
   πλευρές των 1-to-many σχέσεων.
3. Στο `Aviation.Api/Data/Configurations/MaintenanceTaskConfiguration.cs`,
   διαμόρφωσε ρητά τις σχέσεις:
   - πολλά `MaintenanceTask` προς ένα `Status`, με FK `StatusId`,
   - πολλά `MaintenanceTask` προς ένα `Priority`, με FK `PriorityId`,
   - `OnDelete(DeleteBehavior.Restrict)` και για τις δύο σχέσεις, ώστε η
     διαγραφή χρησιμοποιούμενου lookup να απορρίπτεται αντί να διαγράφει
     cascade τα σχετικά tasks.
4. Αφαίρεσε από το `MaintenanceTask` τις υπάρχουσες string properties
   `Status` και `Priority`. Οι FK και οι navigation properties θα αποτελούν
   τη μοναδική πηγή των σχέσεων και θα αποφεύγονται ασυνεπείς διπλές τιμές.
5. Ενημέρωσε το seeding στο
   `Aviation.Api/Extensions/DatabaseInitializer.cs`: διατήρησε πρώτα το
   idempotent seeding των lookup τιμών και δημιούργησε τα πέντε tasks με
   `StatusId` και `PriorityId` που αντιστοιχούν στα σωστά lookup records.
   Αντί για υπόθεση ότι τα identity IDs είναι πάντα 1, 2, 3, αναζήτησε τα
   IDs από τα ονόματα των seeded `Status` και `Priority`.
6. Πρόσθεσε EF Core migration με όνομα `AddTaskRelationships`. Το migration
   πρέπει να υποστηρίζει και την υπάρχουσα βάση που περιέχει tasks:
   - πρόσθεσε αρχικά τα νέα FK columns με τρόπο που δεν αποτυγχάνει λόγω
     υπάρχουσων εγγραφών,
   - αντιστοίχισε τις παλιές string τιμές `Status` και `Priority` με τις
     εγγραφές των αντίστοιχων lookup tables,
   - έλεγξε ότι κάθε υπάρχον task βρήκε έγκυρα lookup IDs· αν υπάρχουν
     άγνωστες τιμές, η migration να αποτυγχάνει με σαφήνεια αντί να επιλέγει
     σιωπηρά αυθαίρετη τιμή,
   - αφού ολοκληρωθεί το backfill, όρισε τα FK ως required, αφαίρεσε τις
     παλιές string columns και πρόσθεσε τα Foreign Key constraints.
7. Επαλήθευσε με build και SQL Server integration checks ότι:
   - το migration εφαρμόζεται σε υπάρχουσα βάση χωρίς απώλεια των task
     εγγραφών και σε καθαρή βάση,
   - τα seeded tasks δείχνουν στα αναμενόμενα statuses/priorities,
   - οι migrations και το seeding παραμένουν idempotent σε δεύτερη εκκίνηση,
   - διαγραφή `Status` ή `Priority` που χρησιμοποιείται από task απορρίπτεται,
     ενώ οι GET απαντήσεις παραμένουν συμβατές με τα routes και metadata.
8. Μετά την επιτυχή υλοποίηση, ενημέρωσε το `CHANGELOG.md` και αύξησε το
   `VERSION` από `0.8.0` σε `0.9.0` (MINOR για τη νέα σχεσιακή δυνατότητα).

## Πεδίο εφαρμογής και παραδοχές

- Τα `Status.Id` και `Priority.Id` κληρονομούνται από το `BaseEntity` και
  είναι `long`. Για να δημιουργηθούν έγκυρα EF Core relationships,
  χρησιμοποιούνται `long StatusId` και `long PriorityId` αντί για `int`.
- Τα υπάρχοντα string `MaintenanceTask.Status` και `.Priority` καταργούνται
  αφού μεταφερθούν οι τιμές τους στα νέα foreign keys.
- Η migration διατηρεί τις υπάρχουσες εγγραφές. Δεν διαγράφεται η τοπική ή
  οποιαδήποτε άλλη βάση ως μέρος αυτής της αλλαγής.
- Η συμπεριφορά `Restrict` σημαίνει ότι lookup εγγραφή που χρησιμοποιείται
  από task δεν μπορεί να διαγραφεί μέχρι να ενημερωθεί ή να αφαιρεθεί πρώτα
  το σχετικό task.
- Τα API routes δεν αλλάζουν. Θα αξιολογηθεί η σειριοποίηση των navigation
  properties, ώστε να μην προκληθούν κυκλικές αναφορές JSON ή ακούσιες
  αλλαγές payload πέρα από τη μετάβαση από string σε FK/navigation δεδομένα.
