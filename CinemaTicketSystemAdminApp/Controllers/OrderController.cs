using CinemaTicketSystemAdminApp.Models;
using GemBox.Document;
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
    public class OrderController : Controller
    {

        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            HttpClient client = new HttpClient(); // We can do get and post request using this

            string URL = "https://localhost:44343/api/Admin/GetAllActiveOrders";

            HttpResponseMessage responseMessage = client.GetAsync(URL).Result;

            // How do we get the data from the response?

            var data = responseMessage.Content.ReadAsAsync<List<Order>>().Result;

            return View(data);
        }


        public IActionResult Details(Guid? orderId)
        {

            HttpClient client = new HttpClient(); // We can do get and post request using this

            string URI = "https://localhost:44343/api/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = orderId
            };

            // Send the ID of the order we want to see to the Post request

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(URI, content).Result;

            // How do we get the data from the response?

            var data = responseMessage.Content.ReadAsAsync<Order>().Result;

            return View(data);
        }


        // We can change it to return FileContentResult but IActionResult and FileStreamResult also work

        public FileContentResult CreateInvoice(Guid? orderId)
        {
            HttpClient client = new HttpClient(); // We can do get and post request using this

            string URI = "https://localhost:44343/api/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = orderId
            };

            // Send the ID of the order we want to see to the Post request

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(URI, content).Result;

            // How do we get the data from the response?

            var data = responseMessage.Content.ReadAsAsync<Order>().Result;


            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx"); // Get the template docx document

            var document = DocumentModel.Load(templatePath); // Use GemBox to load up this template

            document.Content.Replace("{{OrderNumber}}", data.Id.ToString());
            document.Content.Replace("{{UserName}}", data.User.UserName);

            // This is for the List of Products
            StringBuilder sb = new StringBuilder();

            var totalPrice = 0;

            foreach (var item in data.TicketOrders)
            {
                sb.AppendLine(item.OrderedTicket.MovieName + " with quantity of: " + item.Quantity + " and price of: $" + item.OrderedTicket.Price);
                totalPrice += item.Quantity * item.OrderedTicket.Price;
            }
            document.Content.Replace("{{TicketList}}", sb.ToString());


            document.Content.Replace("{{TotalPrice}}", "$" + totalPrice.ToString());


            // Time to save them. But how? We are gonna transform them to a stream.

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());



            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }


    }
}
