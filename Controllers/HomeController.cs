using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TerminDoc.Models;

namespace TerminDoc.Controllers
{
    // [ApiController]
    // [Route("products")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
        public IActionResult Index()
        {
            ViewData["Write"] = "Writing text from Viewdata";
            return View();
        }

        public IActionResult About()
        {
            List<holidayModel> holidays = new List<holidayModel>();
            var request = (HttpWebRequest)WebRequest.Create("https://holidayapi.com/v1/holidays?pretty&key=bb4b77e1-0cc2-47d9-872e-483b2449ba44&country=DE&year=2020");
            request.Method = "GET";
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        var content = sr.ReadToEnd();
                        
                        dynamic holidayInfo = JsonConvert.DeserializeObject(content);
                        var holidayName = holidayInfo.holidays[0].name;
                       
                        foreach (var holiday in holidayInfo.holidays)
                        {
                            

                            holidayModel hm = new holidayModel();
                            hm.Name = holiday.name;
                            hm.Date = holiday.date;
                            hm.Observed = holiday.observed;
                            // hm.Weekday = holiday.weekday.date.name;
                            holidays.Add(hm);
                           
                        }
                    }
                }
            }
        ViewBag.h = holidays;
            return View();
        }

        // public IActionResult About(holidayModel model){
        //     //List<holidayInfo> holidays = new List<holidayInfo>();
        //     var request = (HttpWebRequest)WebRequest.Create("https://date.nager.at/api/v2/PublicHolidays/2021/DE");
        //     request.Method = "GET";
        //     using (var response = (HttpWebResponse)request.GetResponse()){
        //         using (var stream = response.GetResponseStream())
        //         {
        //             using (var st = new StreamReader(stream))
        //             {
        //                 var content = st.ReadToEnd();
        //                 var holidayinfo = JsonConvert.DeserializeObject<dynamic>(content);
        //                 foreach (var document in holidayinfo.Document)
        //                 {
        //                         model.LocalName = document.localName;
        //                         model.Name = document.name;
        //                         model.LaunchYear = document.launchYear;
        //                         //return View(model);
        //                 }                        
        //             }
        //         }
        //     }
            

        //     //return View("Views/Home/User.cshtml");
        //     // return View("../Home/Appointment");
        //     return View(model);
            
        // }
        // [HttpGet]
        public IActionResult Profile(){
            return View();
            // return RedirectToAction("Appointment");
            
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AppointmentViewModel(){
            return View();
        }
    }
}
