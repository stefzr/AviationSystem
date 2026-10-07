# Changelog

Όλες οι σημαντικές αλλαγές στο AviationSystem API θα καταγράφονται σε αυτό το αρχείο.

## [0.2.0] - 2026-10-07
### Added
- Εμπλουτισμός του Swagger UI με metadata (`.WithTags()`, `.WithSummary()`, `.Produces()`) για σαφή ομαδοποίηση και περιγραφή των response schemas.
- Αυτοματοποιημένο Data Seeding κατά την εκκίνηση της εφαρμογής με ρεαλιστικούς αεροπορικούς όρους για Maintenance Tasks, Statuses και Priorities.

## [0.1.1] - 2026-10-06
### Added
- Προσθήκη οντοτήτων `Status` και `Priority` (lookup tables) και των αντίστοιχων Minimal API endpoints.
- Δημιουργία ξεχωριστών κλάσεων Entity Configurations (`IEntityTypeConfiguration`) για καθαρό διαχωρισμό των κανόνων της βάσης από τα domain models.
- Προσθήκη βασικών αρχείων τεκμηρίωσης στο root (`README.md`, `CLAUDE.md`, `VERSION`).

## [0.1.0] - 2026-10-05
### Added
- Αρχικό στήσιμο του project σε .NET 10 χρησιμοποιώντας αρχιτεκτονική Minimal APIs (μετάβαση σε `.slnx` solution).
- Δημιουργία κεντρικής κλάσης `BaseEntity` με πεδία ελέγχου (audit fields) για όλες τις οντότητες (`CreatedOn`, `CreatedBy`, κλπ).
- Ενσωμάτωση Entity Framework Core 10 με In-Memory βάση δεδομένων για local development.

### Removed
- Κατάργηση της παλιάς αρχιτεκτονικής MVC (φάκελος Controllers) για χάρη των Minimal APIs.