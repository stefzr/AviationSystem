using Aviation.Api.Data;
using Aviation.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aviation.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceTasksController : ControllerBase
    {
        private readonly AviationDbContext _context;
        private readonly ILogger<MaintenanceTasksController> _logger;

        // Dependency Injection: Φέρνουμε τη βάση και το Serilog (Logger)
        public MaintenanceTasksController(AviationDbContext context, ILogger<MaintenanceTasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 1. GET: api/MaintenanceTasks (Φέρνει όλα τα tasks)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceTask>>> GetTasks()
        {
            _logger.LogInformation("Ανάκτηση όλων των εργασιών συντήρησης (Maintenance Tasks).");
            return await _context.MaintenanceTasks.ToListAsync();
        }

        // 2. GET: api/MaintenanceTasks/5 (Φέρνει ένα συγκεκριμένο task με βάση το ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceTask>> GetTask(int id)
        {
            var task = await _context.MaintenanceTasks.FindAsync(id);

            if (task == null)
            {
                _logger.LogWarning("Η εργασία με ID {Id} δεν βρέθηκε.", id);
                return NotFound(); // Επιστρέφει 404
            }

            return task;
        }

        // 3. POST: api/MaintenanceTasks (Δημιουργεί ένα νέο task)
        [HttpPost]
        public async Task<ActionResult<MaintenanceTask>> PostTask(MaintenanceTask task)
        {
            _context.MaintenanceTasks.Add(task);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Δημιουργήθηκε νέα εργασία για το αεροσκάφος {AircraftRegistration}", task.AircraftRegistration);

            // Επιστρέφει 201 Created και το link για να δεις το νέο task
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }
    }
}