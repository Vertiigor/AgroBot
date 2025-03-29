var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AgroBot>("agrobot");

builder.Build().Run();
