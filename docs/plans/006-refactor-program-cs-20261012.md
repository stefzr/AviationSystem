# Refactoring του Program.cs

## Στόχος

Να περιοριστεί το `Program.cs` στη σύνθεση των υπηρεσιών και του HTTP
pipeline, μεταφέροντας την αρχικοποίηση της βάσης σε ξεχωριστό σημείο και
διατηρώντας τα endpoints οργανωμένα ανά resource.

## Προτεινόμενα βήματα

1. Έλεγχος της υπάρχουσας δομής. Τα
   `MaintenanceTaskEndpoints.cs`, `StatusEndpoints.cs` και
   `PriorityEndpoints.cs` ήδη περιέχουν static κλάσεις με extension methods
   (`Map...Endpoints(this IEndpointRouteBuilder app)`) και τα routes
   χαρτογραφούνται από το `Program.cs`. Να διατηρηθεί αυτό το μοτίβο και να
   αποφευχθεί άσκοπη μεταφορά ή διπλός ορισμός των routes.
2. Δημιουργία του φακέλου `Aviation.Api/Extensions/`, αν δεν υπάρχει, και
   μεταφορά της αρχικοποίησης βάσης από το `Program.cs` σε νέο
   `DatabaseInitializer.cs`. Η static μέθοδος `InitializeDatabaseAsync` θα
   λαμβάνει τα απαραίτητα services/configuration και θα:
   - ελέγχει αν ο SQL Server είναι διαθέσιμος και καταγράφει error πριν
     αποτύχει η εκκίνηση, όπως κάνει σήμερα,
   - εφαρμόζει τα εκκρεμή migrations με `MigrateAsync()`,
   - εκτελεί το υπάρχον seeding για statuses, priorities και maintenance
     tasks και αποθηκεύει τις νέες εγγραφές.
3. Αν ταιριάζει καλύτερα με τα υπάρχοντα extension-method conventions, να
   οριστεί ως application-builder extension, π.χ.
   `InitializeDatabaseAsync(this WebApplication app)`. Διαφορετικά να
   παραμείνει static initializer με σαφείς παραμέτρους· να επιλεγεί μία
   προσέγγιση χωρίς περιττά επίπεδα αφαίρεσης.
4. Απλοποίηση του `Program.cs`: διατήρηση της ρύθμισης host/services,
   Swagger και middleware pipeline, κλήση των τριών ήδη υπαρχόντων
   `Map...Endpoints()` και μία κλήση στον database initializer στη σωστή
   σειρά πριν αρχίσει να εξυπηρετεί αιτήματα. Να αφαιρεθούν imports και
   λεπτομέρειες seeding που δεν χρειάζονται πλέον στο αρχείο.
5. Επαλήθευση με build και εκτέλεση της εφαρμογής: να εφαρμόζονται migrations
   πριν από seeding, τα υπάρχοντα endpoints να παραμένουν διαθέσιμα και τα
   αρχικά δεδομένα να μην εισάγονται διπλά σε επανεκκίνηση.
6. Μετά την επιτυχή υλοποίηση, ενημέρωση του `CHANGELOG.md` και αύξηση του
   `VERSION` από `0.7.0` σε `0.8.0` (MINOR, σύμφωνα με το workflow του
   `CLAUDE.md`).

## Πεδίο εφαρμογής και παραδοχές

- Το αρχείο `Program.cs` σήμερα περιέχει τη ρύθμιση SQL Server, τον έλεγχο
  διαθεσιμότητας, το `MigrateAsync()` και ολόκληρο το data seeding.
- Τα endpoint extension methods υπάρχουν ήδη στα resource αρχεία· το βήμα
  endpoint αφορά την επαλήθευση και διατήρηση της σωστής οργάνωσης και
  χαρτογράφησής τους, όχι την αναδημιουργία τους.
- Η συμπεριφορά παραμένει ίδια: αποτυχία σύνδεσης καταγράφεται και εμποδίζει
  την εκκίνηση, και το seeding εκτελείται μόνο μετά από επιτυχή migrations.
- Δεν αλλάζουν routes, HTTP metadata, database schema, migration history ή
  seed values.
