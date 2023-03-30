using Database_Final_Project.Entities;

namespace Database_Final_Project.Services;

internal class MenuService
{
    private readonly ComplaintService _complaintService = new();
    private readonly CustomerService _customerService = new();
    private readonly CommentService _commentService = new();

    public async Task Start()
    {
        string option;
        do
        {
            Console.WriteLine("Hej och välkommen till denna applikation där du kan fylla i dina klagomål på våra tjänster");
            Console.WriteLine("Du får nu välja vad du vill göra:");
            Console.WriteLine("1. Skapa ett nytt klagomål");
            Console.WriteLine("2. Visa ett specifikt klagomål");
            Console.WriteLine("3. Visa alla klagomål");
            Console.WriteLine("4. Skapa en kommentar på ett klagomål");
            Console.WriteLine("5. Uppdatera status på ett klagomål");
            option = Console.ReadLine() ?? "";
            switch (option)
            {
                case "1":
                    await CreateComplaintAsync();
                    break;
                case "2":
                    await ShowSpecificComplaint();
                    break;
                case "3":
                    await ShowAllComplaints();
                    break;
                case "4":
                    await CreateComment();
                    break;
                case "5":
                    await UpdateStatus();
                    break;
                default:
                    Console.WriteLine("Var snäll och gör ett giltligt val");
                    break;
            }
        }
        while (RunAgain());


    }

    private bool RunAgain()
    {
        bool again = false;
        Console.WriteLine("Vill du fortsätta köra programmet? Svara i sådana fall ja.");
        string answer = Console.ReadLine() ?? "";
        answer = answer.ToLower();

        if (answer == "ja")
        {
            again = true;
            Console.Clear();
        }

        return again;
    }


    private async Task CreateComplaintAsync()
    {
        int menuChoice;
        string complaint = null!;
        Console.WriteLine("Välj ett av följande val för att börja skapa ditt klagomål.(1 eller 2)");
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
            Console.WriteLine("Du har inte skrivit en siffra. Var snäll och välj 1 eller 2.");
            return;
        }

        while (string.IsNullOrEmpty(complaint))
        {
            Console.Write("Skriv in ditt klagomål: ");
            complaint = Console.ReadLine() ?? "";
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
        Console.WriteLine($"Ditt klagomål har nu registrerats som: {complaintEntity.Description}");
    }

    private async Task CreateComment()
    {
        int complaintId = 0;
        string comment = null!;
        Console.Write("Skriv in Id på klagomålet som du vill kommentera: ");
        int.TryParse(Console.ReadLine(), out complaintId);

        while (string.IsNullOrEmpty(comment))
        {
            Console.Write("Skriv din kommentar här: ");
            comment = Console.ReadLine() ?? "";
        }


        var commentEntity = new CommentEntity
        {
            ComplaintId = complaintId,
            Comment = comment,
            Created = DateTime.Now,
        };

        await _commentService.CreateAsync(commentEntity);

        Console.WriteLine("Din kommentar har nu skapats.");
    }
    private async Task ShowSpecificComplaint()
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
            Console.WriteLine($"Id: {complaint.Id}");
            Console.WriteLine($" Klagomål: {complaint.Description}");

            Console.WriteLine("Kommentarer:");
            foreach (var comment in complaint.Comments)
            {
                Console.WriteLine($"Kommentar: {comment.Comment}");
                Console.WriteLine($"Kommentar skapad: {comment.Created}");
            }
            Console.WriteLine($"Status: {complaint.Status.StatusName}");
            Console.WriteLine($"Skapad: {complaint.Created}");
            Console.WriteLine($"Ändrad: {complaint.Modified}");

            Console.WriteLine("");
        }
        else { Console.WriteLine($"Klagomål med id: {Id} hittades inte. "); }
    }
    private async Task ShowAllComplaints()
    {
        foreach (var complaint in await _complaintService.GetAllAsync())
        {
            {
                Console.WriteLine($"");
                Console.WriteLine($"Namn: {complaint.Customer.FirstName} {complaint.Customer.LastName}");
                Console.WriteLine($"E-postadress: {complaint.Customer.Email}");
                Console.WriteLine($"Telefonnummer: {complaint.Customer.Phone}");
                Console.WriteLine($"Id: {complaint.Id}");
                Console.WriteLine($" Klagomål: {complaint.Description}");
                if (complaint.Comments != null && complaint.Comments.Any())
                {
                    Console.WriteLine("Kommentarer:");
                    foreach (var comment in complaint.Comments)
                    {
                        Console.WriteLine($"Kommentar: {comment.Comment}");
                        Console.WriteLine($"Skapad: {comment.Created}");
                    }
                }
                Console.WriteLine($"Skapad: {complaint.Created}");
                Console.WriteLine($"Ändrad: {complaint.Modified}");
                Console.WriteLine($"Status: {complaint.Status.StatusName}");
                Console.WriteLine("");
            }
        }
    }

    private async Task UpdateStatus()
    {
        Console.WriteLine("Skriv in Id på det ärende du vill uppdatera status på:");
        int.TryParse(Console.ReadLine(), out int complaintId);

        var complaint = await _complaintService.UpdateStatusAsync(x => x.Id == complaintId);

        if (complaint != null)
        {
            await ShowSpecificComplaint();
        }
        else
        {
            Console.WriteLine($"Kunde inte hitta ett klagomål med Id {complaintId}");
        }

    }

    private async Task<CustomerEntity> AddCustomerAsync()
    {
        string firstName = null!;
        string lastName = null!;
        string email = null!;

        while (string.IsNullOrEmpty(firstName))
        {
            Console.WriteLine("Skriv in ditt förnamn");
            firstName = Console.ReadLine() ?? "";
        }

        while (string.IsNullOrEmpty(lastName))
        {
            Console.WriteLine("Skriv in ditt efternamn");
            lastName = Console.ReadLine() ?? "";
        }

        while (string.IsNullOrEmpty(email))
        {
            Console.WriteLine("Skriv in din email");
            email = Console.ReadLine() ?? "";
        }

        Console.WriteLine("Skriv in ditt telefonnummer: ");
        var phone = Console.ReadLine();


        var customer = new CustomerEntity
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
        };

        await _customerService.CreateCustomerAsync(customer);

        var addedCustomer = await _customerService.GetAsync(x => x.Email == email);


        Console.WriteLine("Du har nu blivit tillagd som kund, med denna information:  ");
        Console.WriteLine($"Namn: {addedCustomer.FirstName} {addedCustomer.LastName}");
        Console.WriteLine($"Email: {addedCustomer.Email}");
        if (addedCustomer.Phone != null)
        {
            Console.WriteLine($"Telefonnummer: {addedCustomer.Phone}");
        }

        return customer;
    }
}
