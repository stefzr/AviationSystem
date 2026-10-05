using Aviation.Api.Data;
using Aviation.Api.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Aviation.Api.Endpoints;

// Η κλάση πρέπει να είναι static. Αυτό μας επιτρέπει να φτιάξουμε ένα "Extension Method",
// δηλαδή μια μέθοδο που θα "κολλήσει" πάνω στο αντικείμενο app στο Program.cs.
public static class MaintenanceTaskEndpoints
{
    // Το 'this IEndpointRouteBuilder app' είναι ο τρόπος που το .NET καταλαβαίνει
    // ότι αυτή η μέθοδος επεκτείνει τις δυνατότητες του app.
    public static void MapMaintenanceTaskEndpoints(this IEndpointRouteBuilder app)
    {
        // 1. ROUTING GROUPING
        // Ομαδοποιούμε τα endpoints κάτω από ένα κοινό URL. 
        // Έτσι, γλιτώνουμε το να γράφουμε "/api/MaintenanceTasks" σε κάθε μεμονωμένο endpoint παρακάτω.
        var group = app.MapGroup("/api/MaintenanceTasks");

        // ---------------------------------------------------------
        // ENDPOINT 1: GET ALL (Φέρνει όλες τις εργασίες συντήρησης)
        // ---------------------------------------------------------
        // Όταν έρθει ένα HTTP GET request στο "/api/MaintenanceTasks", εκτελείται αυτή η συνάρτηση.
        // Το '(AviationDbContext context)' λέγεται Dependency Injection. Λέμε στο .NET: 
        // "Όποτε καλείται αυτό το endpoint, φέρε μου αυτόματα μια σύνδεση με τη βάση".
        group.MapGet("/", async (AviationDbContext context) =>
        {
            // Ζητάμε από το Entity Framework να πάει στον πίνακα MaintenanceTasks,
            // να πάρει όλες τις εγγραφές και να τις κάνει μια C# List (ασύγχρονα).
            var tasks = await context.MaintenanceTasks.ToListAsync();
            
            // Το Results.Ok() πακετάρει τη λίστα σε μορφή JSON και βάζει HTTP Status 200 (Επιτυχία).
            return Results.Ok(tasks);
        });

        // ---------------------------------------------------------
        // ENDPOINT 2: GET BY ID (Φέρνει μία συγκεκριμένη εργασία)
        // ---------------------------------------------------------
        // Το "{id}" στο URL είναι μεταβλητή. Αν ο χρήστης καλέσει "/api/MaintenanceTasks/5", το id γίνεται 5.
        // Προσέξαμε να ορίσουμε το id ως 'long', όπως ακριβώς ζήτησε ο υπεύθυνος.
        group.MapGet("/{id}", async (long id, AviationDbContext context) =>
        {
            // Το FindAsync ψάχνει αστραπιαία στη βάση χρησιμοποιώντας το Primary Key (το id).
            var task = await context.MaintenanceTasks.FindAsync(id);
            
            // Εδώ χρησιμοποιούμε τον ternary operator (συνθήκη ? αν_ναι : αν_όχι).
            // Αν το task βρέθηκε (δεν είναι null), επιστρέφουμε 200 OK με τα δεδομένα.
            // Αν είναι null (δεν βρέθηκε τέτοιο id), επιστρέφουμε 404 Not Found.
            return task is not null ? Results.Ok(task) : Results.NotFound();
        });

        // ---------------------------------------------------------
        // ENDPOINT 3: POST (Δημιουργεί νέα εργασία συντήρησης)
        // ---------------------------------------------------------
        // Το .NET διαβάζει αυτόματα το JSON που στέλνει ο χρήστης (π.χ. από Postman ή Swagger) 
        // και το μετατρέπει στο C# αντικείμενο 'MaintenanceTask task'.
        group.MapPost("/", async (MaintenanceTask task, AviationDbContext context) =>
        {
            // Λέμε στο Entity Framework να βάλει το νέο αντικείμενο στη "μνήμη παρακολούθησής" του.
            context.MaintenanceTasks.Add(task);
            
            // Αυτή η γραμμή στέλνει πραγματικά το INSERT query στη βάση δεδομένων.
            // Εκείνη τη στιγμή η βάση παράγει το νέο Id και το επιστρέφει πίσω στο αντικείμενό μας.
            await context.SaveChangesAsync();
            
            // Το Results.Created κάνει 3 πράγματα (HTTP Status 201):
            // Α. Ενημερώνει ότι η εγγραφή δημιουργήθηκε.
            // Β. Λέει στον χρήστη σε ποιο URL μπορεί να βρει την εγγραφή (Location header).
            // Γ. Επιστρέφει το ίδιο το αντικείμενο με συμπληρωμένο πλέον το Id του.
            return Results.Created($"/api/MaintenanceTasks/{task.Id}", task);
        });
    }
}