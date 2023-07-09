using CinemaTicketSystemAdminApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTicketSystemAdminApp.Controllers
{
    public class OrderController : Controller
    {
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

            string URL = "https://localhost:44343/api/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = orderId
            };

            // Send the ID of the order we want to see to the Post request

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(URL, content).Result;

            // How do we get the data from the response?

            var data = responseMessage.Content.ReadAsAsync<Order>().Result;

            return View(data);
        }


    }


}
