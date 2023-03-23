using Database_Final_Project.Services;

Console.WriteLine("");
StatusService statusService = new StatusService();
await statusService.CreateStatusIfNotExistsAsync();
