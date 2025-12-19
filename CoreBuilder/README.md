CoreBuilder - initial skeleton

This folder contains initial EF Core entities, factories and a service to create tenant sites based on category templates.

How to wire into an ASP.NET Core project:

1. Add project reference or include these files in your web API project.
2. Register DbContext in Startup/Program:

   services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
   services.AddScoped<SiteGenerationService>();

3. Run EF Core migrations to create the database.

Notes:
- Factories are simple concrete classes; consider moving to DI with named registrations for extensibility.
- Add Azure Blob Storage and Theme Engine next steps.
