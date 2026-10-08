# UTC audit timestamps και optimistic concurrency

## Στόχος

Να διασφαλιστεί ότι τα κοινά audit timestamps αποθηκεύονται και διαβάζονται
ως UTC, και να προστεθεί token optimistic concurrency στα entities μέσω
`BaseEntity`, με ρύθμιση συμβατή με SQL Server.

## Προτεινόμενα βήματα

1. Στο `Aviation.Api/Data/Configurations/BaseEntityConfiguration.cs`, πρόσθεσε
   `HasConversion` στα `CreatedOn` και `LastUpdatedOn`, ώστε οι τιμές να
   μετατρέπονται σε UTC κατά την αποθήκευση και να επιστρέφονται με
   `DateTimeKind.Utc` κατά την ανάγνωση.
2. Πρόσθεσε ιδιότητα `byte[] RowVersion` στο
   `Aviation.Api/Entities/BaseEntity.cs`.
3. Ρύθμισε το `RowVersion` στο `BaseEntityConfiguration` με `.IsRowVersion()`,
   ώστε το EF Core να το αντιμετωπίζει ως generated concurrency token.
4. Ενημέρωσε τους κανόνες Entities/Database στην αρχή του `CLAUDE.md`:
   τα κοινά `CreatedOn` και `LastUpdatedOn` αποθηκεύονται ως UTC και τα domain
   entities κληρονομούν το `RowVersion` concurrency token από το `BaseEntity`.
5. Επαλήθευσε με build και στοχευμένο έλεγχο του EF Core model metadata ότι οι
   δύο μετατροπές έχουν καταχωριστεί και ότι το `RowVersion` είναι concurrency
   token με generation κατά την προσθήκη/ενημέρωση.
6. Μετά την επιτυχή υλοποίηση, ενημέρωσε το `CHANGELOG.md` και αύξησε το
   `VERSION` από `0.3.0` σε `0.4.0` (MINOR για τη νέα δυνατότητα).

## Πεδίο εφαρμογής και παραδοχές

- Το υπάρχον `BaseEntity` χρησιμοποιεί `DateTime` για τα audit timestamps· οι
  αλλαγές δεν μετατρέπουν τους τύπους σε `DateTimeOffset`.
- Η μετατροπή κατά την εγγραφή χρησιμοποιεί UTC και η μετατροπή κατά την
  ανάγνωση επισημαίνει την αποθηκευμένη τιμή ως UTC, καθώς οι σχεσιακοί τύποι
  ημερομηνίας συνήθως δεν διατηρούν το `DateTimeKind`.
- Το `.IsRowVersion()` δηλώνει τη συμπεριφορά concurrency για providers που
  υποστηρίζουν generated row versions, όπως ο SQL Server. Η εφαρμογή αυτή τη
  στιγμή χρησιμοποιεί EF Core In-Memory· δεν περιλαμβάνεται αλλαγή provider ή
  απόδειξη της πραγματικής σύγκρουσης concurrency σε SQL Server.
- Στο `CLAUDE.md` δεν υπάρχει ξεχωριστό heading “Entities/Database”. Οι νέοι
  κανόνες θα ενσωματωθούν στις υφιστάμενες αριθμημένες οδηγίες Entities και
  Database.
