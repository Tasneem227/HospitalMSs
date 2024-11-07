using HospitalMS.Data;
using HospitalMS.Models;
using HospitalMS.Repository;
using HospitalMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace HospitalMS.Controllers
{
    public class PatientController : Controller
    {

        private IPatientRepository patientRepository;
        private IDepartmentRepository departmentRepository;
        private IDoctorRepository doctorRepository;
        private IBookingRepository bookingRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public PatientController(IPatientRepository _patientRepository, 
            IDepartmentRepository _departmentRepository, IDoctorRepository _doctorRepository,
            IBookingRepository _bookingRepository
            , UserManager<ApplicationUser> userManager  
            )
        {
            patientRepository = _patientRepository;
            departmentRepository = _departmentRepository;
            doctorRepository = _doctorRepository;
            bookingRepository = _bookingRepository;
            this.userManager = userManager;
        }

    

        public async Task<IActionResult> PatientProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user=await userManager.FindByIdAsync(userId);
            return View("Profile", user);
        }

        public IActionResult ShowDepartments()
        {
            return View("ShowDepartments", departmentRepository.GetAll());
        }

        public IActionResult ShowDepartmentDoctors(int DeptId)
        {
            return View("ShowDepartmentDoctors", doctorRepository.GetListByDeptId(DeptId));
        }

        public IActionResult ShowAllDoctors()
        {
            return View("ShowAllDoctors", doctorRepository.GetAll()); ;
        }

        public IActionResult BookAnAppoinment(int DocId)
        {
            if (User.IsInRole("Patient")) { 
            ViewData["BookingLst"] = bookingRepository.GetDoctorBookingList(DocId);
            ViewData["DocId"] = DocId;
            return View("BookAnAppoinment");
        }
            
              return  RedirectToAction("Login", "Account");
            
        }

        public IActionResult SaveAppointment(DocDateTimeViewModel booking)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var patient = patientRepository.SearchByUserName(username);
            int PID = patient.Id;
            TimeOnly Time = TimeOnly.FromDateTime(booking.DateTimeAppointment);
            DateOnly Date = DateOnly.FromDateTime(booking.DateTimeAppointment);

            Booking appointment= bookingRepository.GetAppointment(booking.DocId, Date, Time);

            if (appointment != null)
            {
                appointment.PatientId = PID;//////
                bookingRepository.Save();
            }

            return RedirectToAction("ShowAppointments","Patient");
        }

        public IActionResult ShowAppointments()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var patient = patientRepository.SearchByUserName(username);
            int PatientId = patient.Id;
            return View("ShowAppointments", bookingRepository.GetPatientAppointmentList(PatientId));
        }

        public IActionResult CancelAppointment(int DocId, DateOnly dateOnly, TimeOnly timeOnly)
        {
            Booking? CancelAppointment = bookingRepository.GetAppointment(DocId, dateOnly, timeOnly);
            if (CancelAppointment != null)
            {
                CancelAppointment.PatientId = null;
                bookingRepository.Save();
            }
           
            return RedirectToAction("ShowAppointments", "Patient");
        }
    }
}