using Database_Final_Project.Entities;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Database_Final_Project.Services;

internal class MenuService
{
    private readonly ComplaintService _complaintService = new();
    private readonly CustomerService _customerService = new();
    public async Task ListAllComplaints()
    {
        var complaints = await _complaintService.GetAllAsync();

        foreach(var complaint in complaints)
        {
            Console.WriteLine($"Ärendenummer: {complaint.Id}");
            Console.WriteLine($"Beskrivning: {complaint.Description}");
            Console.WriteLine($"Skapad: {complaint.Created} av {complaint.Customer.FirstName} {complaint.Customer.LastName}");
            Console.WriteLine($"Status: {complaint.Status.StatusName}");
            Console.WriteLine();
            
        }

    }

    public async Task CreateComplaintAsync()
    {
        int menuChoice;
        string complaint = null!;
        Console.WriteLine("Välj ett av följande val.(1 eller 2)");
        Console.WriteLine("1. Om du vill registrera dig som kund. ");
        Console.WriteLine("2. Om du redan registrerat dig som kund");

        CustomerEntity customer = null!;

        if (int.TryParse(Console.ReadLine(), out menuChoice))
        {
            if (menuChoice == 1)
            {
                customer = await AddCustomerAsync();
            }
            else if (menuChoice == 2)
            {
                
                Console.WriteLine("Skriv in den email du registrerade dig med");
                var email = Console.ReadLine();
                var alreadyAddedCustomer = await _customerService.GetAsync(x => x.Email == email);
                if (alreadyAddedCustomer == null)
                {
                    Console.WriteLine("Du har uppgett en email som inte finns i vårt system, var snäll att registrera dig först, eller kontrollera att du har stavat rätt");
                    return;
                }
                customer = alreadyAddedCustomer;
            }
            else
            {
                Console.WriteLine("Du har inte valt 1 eller 2.");
                return;
            }
        }
        else
        {
            Console.WriteLine("Du har inte skrivit en siffra. Var snäll och välj ett eller 2.");
            return;
        }

        while (string.IsNullOrEmpty(complaint))
        {
            Console.WriteLine("Skriv in ditt klagomål");
            complaint = Console.ReadLine();
        }

        var complaintEntity = new ComplaintEntity
        {
            CustomerId = customer.Id,
            Description = complaint,
            Created = DateTime.Now,
            StatusId = 1,
        };

        await _complaintService.CreateAsync(complaintEntity);
        var addedComplaint = _complaintService.GetAsync(x => x.CustomerId == customer.Id);
        Console.WriteLine($"Ditt klagomål har nu registrerats som:{complaintEntity.Description}");
    }

    public async Task ShowSpecificComplaint()
    {
        int Id = 0;
        Console.WriteLine("Skriv Id på det klagomål du vill se");

        int.TryParse(Console.ReadLine(), out Id);

        var complaint = await _complaintService.GetAsync(x => x.Id == Id);
        if (complaint != null)
        {
            Console.WriteLine($"");
            Console.WriteLine($"Namn: {complaint.Customer.FirstName} {complaint.Customer.LastName}");
            Console.WriteLine($"E-postadress: {complaint.Customer.Email}");
            Console.WriteLine($"Telefonnummer: {complaint.Customer.Phone}");
            Console.WriteLine($" Klagomål: {complaint.Description}");
            if (complaint.Comments != null)
            {
                Console.WriteLine($"Kommentarer: {complaint.Comments}");
            }
            Console.WriteLine($"Skapad: {complaint.Created}");
            Console.WriteLine($"Ändrad: {complaint.Modified}");
            Console.WriteLine("");
        }
       else { Console.WriteLine($"Klagomål med id: {Id} hittades inte. "); }
    }
    public async Task ShowAllComplaints()
    {
        foreach( var complaint in  await _complaintService.GetAllAsync())
        {
            {
                Console.WriteLine($"");
                Console.WriteLine($"Namn: {complaint.Customer.FirstName} {complaint.Customer.LastName}");
                Console.WriteLine($"E-postadress: {complaint.Customer.Email}");
                Console.WriteLine($"Telefonnummer: {complaint.Customer.Phone}");
                Console.WriteLine($" Klagomål: {complaint.Description}");
                if (complaint.Comments != null && complaint.Comments.Any())
                {
                    Console.WriteLine("Kommentarer:");
                    foreach (var comment in complaint.Comments)
                    {
                        Console.WriteLine(comment.Comment);
                        Console.WriteLine($"Skapad: {comment.Created}");
                    }
                }
                Console.WriteLine($"Skapad: {complaint.Created}");
                Console.WriteLine($"Ändrad: {complaint.Modified}");
                Console.WriteLine("");
            }
        }
    }

    public async Task<CustomerEntity> AddCustomerAsync()
    {
        string firstName = null!;
        string lastName = null!;
        string email = null!;

        while (string.IsNullOrEmpty(firstName))
            {
            Console.WriteLine("Skriv in ditt förnamn");
            firstName = Console.ReadLine();
        }

        while (string.IsNullOrEmpty(lastName))
        {
            Console.WriteLine("Skriv in ditt efternamn");
             lastName = Console.ReadLine();
        }

        while (string.IsNullOrEmpty(email))
        {
            Console.WriteLine("Skriv in din email");
            email = Console.ReadLine();
        }

        Console.WriteLine("Skriv in ditt telefonnummer: ");
        var phone = Console.ReadLine();


        var customer = new CustomerEntity
        {
            FirstName = firstName,
            LastName   = lastName,
            Email = email,
            Phone = phone,
        };

        await _customerService.CreateCustomerAsync(customer);

        var addedCustomer = await _customerService.GetAsync(x => x.Email == email);


        Console.WriteLine("Du har nu blivit tillagd som kund, med denna information:  ");
        Console.WriteLine($"Namn: {addedCustomer.FirstName} {addedCustomer.LastName}");
        Console.WriteLine($"Email: {addedCustomer.Email}");
        if(addedCustomer.Phone != null)
        {
            Console.WriteLine($"Telefonnummer: {addedCustomer.Phone}");
        }
        Console.ReadKey();
        return customer;
    }
}
