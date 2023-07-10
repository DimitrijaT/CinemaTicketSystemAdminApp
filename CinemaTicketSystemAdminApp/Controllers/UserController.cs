using CinemaTicketSystemAdminApp.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTicketSystemAdminApp.Controllers
{
    public class UserController : Controller
    {

        [HttpGet("[action]")]
        public IActionResult ImportUsers()
        {
            return View();
        }


        [HttpPost("[action]")]
        // The name of this variable should be equal to the name="file" in the view
        public IActionResult ImportUsers(IFormFile file)
        {

            // First we make a copy of the thing we are reading.
            // NOTE: Make sure the entire path exists already, missing folders will NOT be created automatically
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\Files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }

            // Second we read data from the copied file

            List<User> users = getAllUsersFromFile(file.FileName);

            // After we read the users we will make a Http client

            HttpClient client = new HttpClient();

            string URL = "https://localhost:44343/api/Admin/ImportAllUsers";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");

            string contentString = content.ReadAsStringAsync().Result;
            Console.WriteLine(contentString);

            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("ImportUsers", "Order");
        }

        private List<User> getAllUsersFromFile(string fileName)
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\Files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);



            List<User> users = new List<User>();

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {

                // Let's use the ExcelDataReader library:

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        // We read row by row from the copy of the excel file

                        users.Add(new Models.User
                        {
                            FirstName = reader.GetValue(0).ToString(),
                            LastName = reader.GetValue(1).ToString(),
                            Address = reader.GetValue(2).ToString(),
                            Email = reader.GetValue(3).ToString(),
                            Password = reader.GetValue(4).ToString(),
                            ConfirmPassword = reader.GetValue(5).ToString(),
                            PhoneNumber = reader.GetValue(6).ToString()
                        });
                    }
                }
            }
            return users;

        }

    }
}
