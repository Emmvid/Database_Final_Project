using Database_Final_Project.Services;


// Skapar de olika statustyperna direkt när applikationen startas
StatusService statusService = new();
await statusService.CreateStatusIfNotExistsAsync();

MenuService menuService = new();
await menuService.ShowSpecificComplaint();