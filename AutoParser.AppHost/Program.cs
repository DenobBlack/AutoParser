var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AutoParser>("autoparser");

builder.Build().Run();
