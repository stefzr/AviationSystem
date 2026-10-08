# Έλεγχος διαθεσιμότητας SQL Server και εφαρμογή migrations κατά την εκκίνηση

## Στόχος

Να αντικατασταθεί η αρχικοποίηση schema με `EnsureCreated()` από ελεγχόμενη
ρουτίνα εκκίνησης που ελέγχει τη σύνδεση στον SQL Server, εφαρμόζει EF Core
migrations και εκτελεί το υπάρχον data seeding μόνο αφού ολοκληρωθούν επιτυχώς
οι προηγούμενες ενέργειες.

## Προτεινόμενα βήματα

1. Επιβεβαίωση διαθεσιμότητας του SQL Server μέσω `await
   context.Database.CanConnectAsync(...)` πριν από migrations ή seeding.
   Αν δεν είναι διαθέσιμος, να καταγράφεται σαφές error log με Serilog και η
   εφαρμογή να αποτυγχάνει κατά την εκκίνηση (fail-fast), χωρίς να ξεκινά
   listener που δεν μπορεί να εξυπηρετήσει αιτήματα με βάση δεδομένων.
2. Όταν η σύνδεση είναι διαθέσιμη, αφαίρεση του `EnsureCreated()` και κλήση
   `await context.Database.MigrateAsync(...)`. Κατόπιν να παραμένει η υπάρχουσα
   λογική seeding και να αποθηκεύονται οι εγγραφές όπως σήμερα.
3. Δημιουργία του αρχικού migration `InitialCreate` από το repository root,
   με το SQL Server διαθέσιμο και το Development connection string ρυθμισμένο:

   ```powershell
   dotnet ef migrations add InitialCreate --project Aviation.Api/Aviation.Api.csproj --startup-project Aviation.Api/Aviation.Api.csproj
   ```

   Αν η εντολή `dotnet ef` δεν είναι διαθέσιμη, έλεγχος/ρύθμιση του εργαλείου
   `dotnet-ef` σύμφωνα με τις υπάρχουσες εκδόσεις EF Core 10, χωρίς αλλαγή των
   application package versions πέρα από ό,τι απαιτείται.
4. Έλεγχος των παραγόμενων migration και model snapshot αρχείων για ορθή
   δημιουργία των πινάκων, unique index, UTC-converted columns και SQL Server
   `rowversion`. Αφαίρεση κάθε εξάρτησης της ροής schema από `EnsureCreated()`.
5. Επαλήθευση με build και εκτέλεση της εφαρμογής: με διαθέσιμο SQL Server,
   να εφαρμόζεται το migration πριν από το seeding και τα lookup endpoints να
   επιστρέφουν τα seeded δεδομένα. Σε μη διαθέσιμη βάση, να καταγράφεται error
   και η εκκίνηση να τερματίζεται χωρίς να παραλείπονται σιωπηρά migrations ή
   seeding.
6. Μετά την επιτυχή υλοποίηση, ενημέρωση του `CHANGELOG.md` και αύξηση του
   `VERSION` από `0.5.0` σε `0.6.0` (MINOR για τη νέα ρουτίνα εκκίνησης και
   διαχείριση schema).

## Πεδίο εφαρμογής και παραδοχές

- Το API χρησιμοποιεί SQL Server και το `Microsoft.EntityFrameworkCore.Design`
  είναι ήδη δηλωμένο στο project. Δεν εντοπίστηκαν υπάρχοντα migration αρχεία.
- Το `InitialCreate` θα περιέχει το τρέχον μοντέλο EF Core και θα διατηρηθεί
  στο repository μαζί με το model snapshot.
- Η σύνδεση ελέγχεται μία φορά στην εκκίνηση. Δεν προστίθεται retry/backoff
  policy ή background retry service σε αυτό το scope.
- Σε αποτυχία σύνδεσης, προτείνεται fail-fast μετά το Serilog error log αντί
  συνέχισης χωρίς λειτουργική βάση δεδομένων.
