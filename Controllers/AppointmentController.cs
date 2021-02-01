using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TerminDoc.Data;
using TerminDoc.Models;

namespace TerminDoc.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppointmentDbContext _context;

        public AppointmentController(AppointmentDbContext context)
        {
            _context = context;
        }

        // GET: Appointment
        public async Task<IActionResult> Index(string sortOrder,string searchString)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;
            var patients = from p in _context.Appointment
                            select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.Name.Contains(searchString)
                                    || p.Address.Contains(searchString));
            }
            switch (sortOrder){
                case "name_desc":
                patients = patients.OrderByDescending(p => p.Name);
                break;
                case "Date":
                patients = patients.OrderBy(p => p.appointmentDate);
                break;
                case "date_desc":
                patients = patients.OrderByDescending(p => p.appointmentDate);
                break;
                default:
                patients = patients.OrderBy(p => p.Name);
                break;
            }
            return View(await patients.AsNoTracking().ToListAsync());
        }

        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentViewModel = await _context.Appointment
                .FirstOrDefaultAsync(m => m.id == id);
            if (appointmentViewModel == null)
            {
                return NotFound();
            }

            return View(appointmentViewModel);
        }

        // GET: Appointment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appointment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,appointmentDate,Name,Address,BloodGroup,Age,Gender")] AppointmentViewModel appointmentViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentViewModel);
        }

        // GET: Appointment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentViewModel = await _context.Appointment.FindAsync(id);
            if (appointmentViewModel == null)
            {
                return NotFound();
            }
            return View(appointmentViewModel);
        }

        // POST: Appointment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,appointmentDate,Name,Address,BloodGroup,Age,Gender")] AppointmentViewModel appointmentViewModel)
        {
            if (id != appointmentViewModel.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentViewModelExists(appointmentViewModel.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentViewModel);
        }

        // GET: Appointment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentViewModel = await _context.Appointment
                .FirstOrDefaultAsync(m => m.id == id);
            if (appointmentViewModel == null)
            {
                return NotFound();
            }

            return View(appointmentViewModel);
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentViewModel = await _context.Appointment.FindAsync(id);
            _context.Appointment.Remove(appointmentViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentViewModelExists(int id)
        {
            return _context.Appointment.Any(e => e.id == id);
        }      
        
        public IActionResult UpcomingAppointments() {
            return View();
        }
        


        
        
        //     var cronofy = new CronofyAccountClient("jZ8rMQLrfkx4Bc42uTHuorlqustKiA60");
        //     var calendars = cronofy.GetCalendars();



        //     //read events
        //     var cronofy = new CronofyAccountClient("jZ8rMQLrfkx4Bc42uTHuorlqustKiA60");
        //     var events = cronofy.GetEvents();




        //     //create events
        //     var eventBuilder = new UpsertEventRequestBuilder()
        //     .EventId("uniq-id")
        //     .Summary("Event summary")
        //     .Description("Event description")
        //     .Start(2015, 10, 20, 17, 00)
        //     .End(2015, 10, 20, 17, 30)
        //     .TimeZoneId("Europe/London")
        //     .Location("Meeting room");

        // cronofy.UpsertEvent(calendarId, eventBuilder);
        // }


        // //delete event
        // var cronofy = new CronofyAccountClient(accessToken);
        // cronofy.DeleteEvent(calendarId, "uniq-id");

        // [GoogleScopedAuthorize(CalendarService.ScopeConstants.Calendar)]
        // public async Task<IActionResult> LinkGoogleCalendar([FromServices]IGoogleAuthProvider auth, int? id)
        // {
        //     GoogleCredential cred = await auth.GetCredentialAsync();
        //     var service = new CalendarService(new BaseClientService.Initializer {
        //         HttpClientInitializer = cred
        //     });
        //     var events = await service.Events.List("primary").ExecuteAsync();
        //     var calendars = await service.CalendarList.List().ExecuteAsync();
        //     IList<String> existingCalendars = new List<String>();
        //     foreach (var item in calendars.Items)
        //     {
        //         existingCalendars.Add(item.Summary);
        //     }
        //     ViewData["existingCalendars"] = existingCalendars;

        //     return View();
        // }
    }
}
