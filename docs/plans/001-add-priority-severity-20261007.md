# Προσθήκη Severity στο Priority

## Στόχος

Να προστεθεί στο lookup entity `Priority` η ιδιότητα `Severity` ως ακέραιος
αριθμός, υποχρεωτικός και μοναδικός, με τους κανόνες να δηλώνονται μέσω
Entity Framework Core configuration.

## Προτεινόμενα βήματα

1. Προσθήκη της `int Severity` ιδιότητας στο `Aviation.Api/Entities/Priority.cs`.
2. Δημιουργία `PriorityConfiguration` στον φάκελο
   `Aviation.Api/Data/Configurations/`, βασισμένης στην
   `BaseEntityConfiguration<Priority>`. Η ρύθμιση θα ορίζει το `Severity` ως
   required και θα προσθέτει unique index σε αυτό.
3. Ενημέρωση των αρχικών δεδομένων για τα `Priorities` στο
   `Aviation.Api/Program.cs`, ώστε κάθε εγγραφή να έχει μοναδικό `Severity`.
   Προτεινόμενη αντιστοίχιση: `Low = 1`, `Medium = 2`, `Critical = 3`.
4. Επαλήθευση της καταχώρισης EF Core model metadata για required property και
   unique index, και επιβεβαίωση ότι το API project κάνει build επιτυχώς.
5. Μετά την επιτυχή υλοποίηση, ενημέρωση του `CHANGELOG.md` και bump της
   έκδοσης στο `VERSION` από `0.2.0` σε `0.3.0` (MINOR για νέα δυνατότητα).

## Πεδίο εφαρμογής και παραδοχές

- Δεν προστίθενται νέα endpoints· τα υπάρχοντα GET endpoints θα εκθέτουν την
  ιδιότητα μέσω του `Priority` response model.
- Η βάση δεδομένων είναι In-Memory. Η μοναδικότητα θα δηλωθεί στο EF Core model
  μέσω unique index· ο In-Memory provider δεν εγγυάται επιβολή περιορισμών όπως
  ένας σχεσιακός provider.
- Οι τιμές Severity 1, 2 και 3 ακολουθούν την αυξανόμενη σειρά των `Low`,
  `Medium` και `Critical`.
