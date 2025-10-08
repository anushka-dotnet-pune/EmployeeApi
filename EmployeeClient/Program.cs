using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EmployeeClient.Models;

namespace EmployeeClient
{
    class Program
    {
        static HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7179/") 
        };

        static void Main(string[] args)
        {
            RunMenu().GetAwaiter().GetResult();
        }

        static async Task RunMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("===== Employee CRUD Menu =====");
                Console.WriteLine("1. Get All Employees");
                Console.WriteLine("2. Get Employee By Id");
                Console.WriteLine("3. Get Employees By Department");
                Console.WriteLine("4. Add Employee");
                Console.WriteLine("5. Update Employee");
                Console.WriteLine("6. Update Employee Email");
                Console.WriteLine("7. Delete Employee");
                Console.WriteLine("8. Exit");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await GetAllEmployees();
                        break;
                    case "2":
                        await GetEmployeeById();
                        break;
                    case "3":
                        await GetEmployeesByDept();
                        break;
                    case "4":
                        await AddEmployee();
                        break;
                    case "5":
                        await UpdateEmployee();
                        break;
                    case "6":
                        await UpdateEmployeeEmail();
                        break;
                    case "7":
                        await DeleteEmployee();
                        break;
                    case "8":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        
        static async Task GetAllEmployees()
        {
            List<Employee> employees = await client.GetFromJsonAsync<List<Employee>>("api/employee");
            if (employees == null)
                employees = new List<Employee>();

            Console.WriteLine("All Employees:");
            foreach (Employee e in employees)
            {
                Console.WriteLine($"{e.Id} - {e.Name} - {e.Department} - {e.Email}");
            }
        }

        static async Task GetEmployeeById()
        {
            Console.Write("Enter Employee Id: ");
            int id = int.Parse(Console.ReadLine());
            Employee employee = await client.GetFromJsonAsync<Employee>($"api/employee/{id}");
            if (employee != null)
                Console.WriteLine($"{employee.Id} - {employee.Name} - {employee.Department} - {employee.Email}");
            else
                Console.WriteLine("Employee not found.");
        }

        static async Task GetEmployeesByDept()
        {
            Console.Write("Enter Department: ");
            string dept = Console.ReadLine();
            List<Employee> employees = await client.GetFromJsonAsync<List<Employee>>($"api/employee/department?dept={dept}");
            if (employees == null || employees.Count == 0)
                Console.WriteLine("No employees in this department.");
            else
            {
                foreach (Employee e in employees)
                {
                    Console.WriteLine($"{e.Id} - {e.Name} - {e.Department} - {e.Email}");
                }
            }
        }

        static async Task AddEmployee()
        {
            Employee emp = new Employee();
            Console.Write("Enter Id: "); emp.Id = int.Parse(Console.ReadLine());
            Console.Write("Enter Name: "); emp.Name = Console.ReadLine();
            Console.Write("Enter Department: "); emp.Department = Console.ReadLine();
            Console.Write("Enter MobileNo: "); emp.MobileNo = Console.ReadLine();
            Console.Write("Enter Email: "); emp.Email = Console.ReadLine();

            HttpResponseMessage response = await client.PostAsJsonAsync("api/employee", emp);
            Console.WriteLine($"POST Status: {response.StatusCode}");
        }

        static async Task UpdateEmployee()
        {
            Console.Write("Enter Employee Id to update: ");
            int id = int.Parse(Console.ReadLine());
            Employee emp = new Employee { Id = id };
            Console.Write("Enter new Name: "); emp.Name = Console.ReadLine();
            Console.Write("Enter new Department: "); emp.Department = Console.ReadLine();
            Console.Write("Enter new MobileNo: "); emp.MobileNo = Console.ReadLine();
            Console.Write("Enter new Email: "); emp.Email = Console.ReadLine();

            HttpResponseMessage response = await client.PutAsJsonAsync($"api/employee/{id}", emp);
            Console.WriteLine($"PUT Status: {response.StatusCode}");
        }

        static async Task UpdateEmployeeEmail()
        {
            Console.Write("Enter Employee Id: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Enter new Email: ");
            string email = Console.ReadLine();

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"),
                $"api/employee/email/{id}?email={email}");
            HttpResponseMessage response = await client.SendAsync(request);
            Console.WriteLine($"PATCH Status: {response.StatusCode}");
        }

        static async Task DeleteEmployee()
        {
            Console.Write("Enter Employee Id to delete: ");
            int id = int.Parse(Console.ReadLine());
            HttpResponseMessage response = await client.DeleteAsync($"api/employee/{id}");
            Console.WriteLine($"DELETE Status: {response.StatusCode}");
        }
    }
}
