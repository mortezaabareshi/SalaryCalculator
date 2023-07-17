using System.Reflection;
using Salaries.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
var locations = assemblies.Where(x => !x.IsDynamic).Select(x => x.Location).ToArray();
var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
    .Where(x => !locations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
    .ToList();
files.ForEach(x => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));

builder.Services.AddInfrastructure(assemblies, builder.Configuration);
var app = builder.Build();

app.UseInfrastructure()
    .UseRouting()
    .UseEndpoints(e => e.MapControllers());

app.Run();