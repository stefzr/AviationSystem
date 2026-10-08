# Μετάβαση από In-Memory σε SQL Server με Docker

## Στόχος

Να αντικατασταθεί ο EF Core In-Memory provider με SQL Server, τον οποίο θα
εκκινεί Docker Compose για τοπική ανάπτυξη, και να ρυθμιστεί η εφαρμογή ώστε
να συνδέεται στη βάση μέσω connection string.

## Προτεινόμενα βήματα

1. Δημιουργία `docker-compose.yml` στο repository root με υπηρεσία SQL Server
   από το επίσημο image `mcr.microsoft.com/mssql/server`, αποδοχή του EULA,
   port mapping για τοπική σύνδεση, health check και named volume για
   διατήρηση των δεδομένων. Ο κωδικός του `sa` να παρέχεται από μεταβλητή
   περιβάλλοντος και όχι να αποθηκευτεί στο compose αρχείο.
2. Αντικατάσταση του `Microsoft.EntityFrameworkCore.InMemory` από το
   `Microsoft.EntityFrameworkCore.SqlServer` στο
   `Aviation.Api/Aviation.Api.csproj`.
3. Προσθήκη `ConnectionStrings:DefaultConnection` στο
   `Aviation.Api/appsettings.Development.json` για τοπικό SQL Server στο
   `localhost,1433`. Να αποφευχθεί η εγγραφή πραγματικού κωδικού στο
   version control· η ευαίσθητη τιμή να παρέχεται με development environment
   variable ή .NET User Secrets και να ευθυγραμμίζεται με την τιμή που
   διαβάζει το Docker Compose.
4. Αντικατάσταση του `.UseInMemoryDatabase(...)` με `.UseSqlServer(...)` στο
   `Aviation.Api/Program.cs`, διαβάζοντας το `DefaultConnection` από τη
   configuration. Διατήρηση του υπάρχοντος seed logic.
5. Επιλογή αρχικοποίησης σχήματος για τοπική ανάπτυξη: το προαιρετικό
   `context.Database.EnsureCreated()` μπορεί να προστεθεί κατά την εκκίνηση,
   αφού επιβεβαιωθεί ότι δεν υπάρχουν migrations που πρέπει να χρησιμοποιηθούν.
   Αν υπάρχουν ή προστεθούν migrations, προτίμηση στο `Database.Migrate()`
   αντί για `EnsureCreated()`.
6. Επαλήθευση με `docker compose config` και εκκίνηση της υπηρεσίας μέχρι να
   γίνει healthy, build του API, εκκίνηση της εφαρμογής με Development
   configuration και έλεγχο ότι οι πίνακες/seed data δημιουργούνται και τα
   lookup GET endpoints επιστρέφουν δεδομένα. Επιβεβαίωση ότι το SQL Server
   χρησιμοποιεί το `RowVersion` ως concurrency token.
7. Μετά την επιτυχή υλοποίηση, καταγραφή αλλαγών στο `CHANGELOG.md` και αύξηση
   του `VERSION` από `0.4.0` σε `0.5.0` (MINOR, σύμφωνα με το `CLAUDE.md`).

## Πεδίο εφαρμογής και παραδοχές

- Το Compose αρχείο βρίσκεται στο repository root· το API εκτελείται τοπικά
  από το host και συνδέεται στη δημοσιευμένη θύρα του SQL Server.
- Το Docker Compose σηκώνει μόνο τη βάση, όπως ζητήθηκε· δεν προστίθεται
  container για το API.
- Δεν προστίθενται migrations σε αυτό το πλάνο, εκτός αν ο έλεγχος του
  repository δείξει ότι ήδη χρησιμοποιούνται. Το `EnsureCreated()` αφορά
  μόνο τοπική ανάπτυξη και δεν αντικαθιστά migrations για διαχείριση schema
  σε παραγωγή.
- Ο κωδικός `sa` πρέπει να ικανοποιεί την πολιτική ισχυρού κωδικού του SQL
  Server και να παρέχεται τοπικά με ασφαλή τρόπο, χωρίς να δεσμευτεί στο
  repository.
