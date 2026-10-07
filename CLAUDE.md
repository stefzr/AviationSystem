# AI Coding Guidelines for AviationSystem

1. **Architecture**: We use .NET 10 Minimal APIs. DO NOT generate MVC Controllers.
2. **Entities**: All domain models must inherit from `BaseEntity`. 
3. **Database**: Use Entity Framework Core 10. Entity configurations must be placed in the `Data/Configurations` folder using `IEntityTypeConfiguration`.

## AI Workflow & Planning (CRITICAL)
Every task or feature MUST strictly follow this 5-step lifecycle. Do NOT skip any step.

1. **Plan:** Do NOT write or modify application code immediately. Create a step-by-step Markdown plan inside the `docs/plans/` directory using the exact naming convention: `{xxx}-{description-slug}-YYYYMMDD.md` (e.g., `001-add-severity-20261008.md`), automatically incrementing `xxx`.
2. **Refine:** Stop and ask the user for feedback. Correct the plan based on the user's review.
3. **Approve:** Wait for explicit user approval (e.g., "approved", "implement") before touching any code.
4. **Execute:** Implement the approved changes in the codebase.
5. **Version Bump & Docs:** After successful execution, ALWAYS update the `CHANGELOG.md` and bump the version in the `VERSION` file. 
   - **MINOR:** Default for new features (e.g., 0.2.0 -> 0.3.0).
   - **PATCH:** For bugfixes only (e.g., 0.2.0 -> 0.2.1).
   - **MAJOR:** Only when explicitly instructed by the user.