# CORS στο API και Angular HTTP Service

## Στόχος

Να επιτραπεί στο Angular SPA να διαβάζει δεδομένα από τα υπάρχοντα API
endpoints και να προστεθούν τα TypeScript contracts και το HTTP service για
Tasks, Statuses και Priorities. Το scope περιορίζεται στη ρύθμιση CORS και
στην ανάκτηση δεδομένων· δεν θα δημιουργηθούν οθόνες ή UI.

## Προτεινόμενα βήματα

1. Στο `Aviation.Api/Program.cs`, δήλωσε CORS policy με όνομα
   `AllowAviationSpa`. Ρύθμισε `WithOrigins("http://localhost:4200")`,
   `AllowAnyMethod()` και `AllowAnyHeader()`. Μην ενεργοποιήσεις
   `AllowCredentials()`, αφού δεν ζητείται χρήση credentials από cross-origin
   requests.
2. Εφάρμοσε το policy με `app.UseCors("AllowAviationSpa")` πριν από το
   `UseAuthorization()` και πριν από τη χαρτογράφηση των endpoints, ώστε να
   καλύπτονται σωστά τα αιτήματα και τα preflight checks.
3. Δημιούργησε `Aviation.Spa/src/app/models/maintenance-task.model.ts`,
   `status.model.ts` και `priority.model.ts` με TypeScript interfaces που
   αντιστοιχούν στα API entities:
   - `MaintenanceTask`: `id`, `aircraftTailNumber`,
     `aircraftRegistration`, `component`, `description`, `statusId`,
     `priorityId`, `createdAt`, τα κοινά audit πεδία και `rowVersion`.
   - `Status`: `id`, `name`, τα κοινά audit πεδία και `rowVersion`.
   - `Priority`: `id`, `name`, `severity`, τα κοινά audit πεδία και
     `rowVersion`.
   Χρησιμοποίησε `number` για τα JSON numeric IDs και severity, `string` για
   ημερομηνίες και `rowVersion` (το `byte[]` του .NET serialized ως
   base64 string), και τύπους που επιτρέπουν `null` για τις navigation
   properties `status`/`priority` του task, αφού τα υπάρχοντα GET handlers
   δεν κάνουν eager loading των σχετικών entities.
4. Δημιούργησε
   `Aviation.Spa/src/app/services/maintenance-task.service.ts` ως Angular
   injectable service με `HttpClient` και Observable-based μεθόδους:
   - `getMaintenanceTasks()` → `GET /api/MaintenanceTasks`
   - `getStatuses()` → `GET /api/Statuses`
   - `getPriorities()` → `GET /api/Priorities`
   Οι επιστρεφόμενοι τύποι να χρησιμοποιούν τα interfaces του προηγούμενου
   βήματος. Ρύθμισε ένα σαφές API base URL από το υπάρχον API launch profile
   (`https://localhost:7201` ή `http://localhost:5225`) και κράτησέ το σε
   κεντρικό σημείο της ρύθμισης του frontend, όχι σε κάθε μέθοδο ξεχωριστά.
   Μην προσθέσεις χειρισμό σφαλμάτων που καταπίνει αποτυχίες.
5. Πρόσθεσε `provideHttpClient()` στα providers του
   `Aviation.Spa/src/app/app.config.ts`, διατηρώντας τον υπάρχοντα router και
   τους υπόλοιπους providers.
6. Πρόσθεσε στο `CHANGELOG.md` νέα καταχώριση `[0.11.0]` με ημερομηνία
   `2026-10-14`. Εξήγησε τη νέα CORS policy,
   τα TypeScript models και το typed Angular HTTP service. Αύξησε το
   `VERSION` από `0.10.0` σε `0.11.0` ως MINOR bump.
7. Επαλήθευσε με:
   - `dotnet build` για το API,
   - Angular build και tests,
   - HTTP checks ότι τα τρία service routes δείχνουν στα υπάρχοντα GET
     endpoints και ότι requests από `http://localhost:4200` επιτρέπονται από
     το CORS policy,
   - έλεγχο ότι άλλο origin δεν λαμβάνει `Access-Control-Allow-Origin`,
   - έλεγχο ότι η αλλαγή δεν προσθέτει UI ή νέα API endpoints.

## Πεδίο εφαρμογής και παραδοχές

- Το SPA origin που επιτρέπεται είναι μόνο το `http://localhost:4200`, όπως
  ζητήθηκε για το τοπικό Angular development server. Άλλα origins και
  production domains δεν προστίθενται στο scope.
- Το API εκθέτει ήδη τα routes `/api/MaintenanceTasks`, `/api/Statuses` και
  `/api/Priorities`. Το service θα χρησιμοποιήσει τα υπάρχοντα GET routes·
  δεν αλλάζουν τα endpoints του backend.
- Τα entity IDs είναι `long` στο .NET αλλά μεταφέρονται ως JavaScript
  `number` στο JSON. Το `RowVersion` είναι `byte[]` στο .NET και αναπαρίσταται
  ως base64 `string` στο JSON.
- Δεν θα προστεθούν οθόνες, components, forms, write operations ή
  authentication.
- Το όνομα και η ημερομηνία του πλάνου αντιστοιχούν στην προγραμματισμένη
  ημερομηνία του συγκεκριμένου βήματος.
