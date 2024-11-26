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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AutoMapper;

namespace HospitalMS.Controllers
{
    public class PatientController : Controller
    {

        private IPatientRepository patientRepository;
        private IDepartmentRepository departmentRepository;
        private IDoctorRepository doctorRepository;
        private IBookingRepository bookingRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly SignInManager<ApplicationUser> signInManager;

        public PatientController(IPatientRepository _patientRepository, 
            IDepartmentRepository _departmentRepository, IDoctorRepository _doctorRepository,
            IBookingRepository _bookingRepository
            , UserManager<ApplicationUser> userManager ,
            IMapper mapper,SignInManager<ApplicationUser> signInManager
            )
        {
            patientRepository = _patientRepository;
            departmentRepository = _departmentRepository;
            doctorRepository = _doctorRepository;
            bookingRepository = _bookingRepository;
            this.userManager = userManager;
            this.mapper = mapper;
            this.signInManager = signInManager;
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

        public IActionResult BookAnAppoinment(int DocId , string DocName)
        {
            if (User.IsInRole("Patient")) { 
                ViewData["BookingLst"] = bookingRepository.GetDoctorBookingList(DocId);
                ViewData["DocId"] = DocId;
                ViewBag.DoctorName = DocName;
                return View("BookAnAppoinment");
             }
            
              return  RedirectToAction("Login", "Account");
            
        }

        public IActionResult SaveAppointment(DocDateTimeViewModel booking)
        {
            if (ModelState.IsValid&&booking.DateTimeAppointment.Year!=1) { 
            var username = User.Identity.Name;
            var patient = patientRepository.SearchByUserName(username);
            int PID = patient.Id;
            TimeOnly Time = TimeOnly.FromDateTime(booking.DateTimeAppointment);
            DateOnly Date = DateOnly.FromDateTime(booking.DateTimeAppointment);

            Booking appointment = bookingRepository.GetAppointment(booking.DocId, Date, Time);

            if (appointment != null)
            {
                appointment.PatientId = PID;//////
                bookingRepository.Save();
            }
                return RedirectToAction("ShowAppointments", "Patient");
            }
            return RedirectToAction("BookAnAppoinment", new { DocId=booking.DocId, DocName=booking.DocName });
        }

        public IActionResult ShowAppointments()
        {
            var username = User.Identity.Name;
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
        ///////////////////////////////////Profile//////////////////////////////////////////////////////////
        public async Task<IActionResult> PatientEditView()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            Patient patient = patientRepository.SearchByUserName(username);
            var PatientModel = mapper.Map<PatientViewModel>(patient);
            PatientModel.CurrentImage = patient.Imag;
            PatientModel.userid = (await userManager.FindByNameAsync(username)).Id;
            PatientModel.OldPassword = patient.Password;
            PatientModel.OldUserName = patient.Username;
            return View("PatientProfileEdit", PatientModel);
        }


        public async Task<ActionResult> SaveEdit(PatientViewModel patientViewModel)
        {
            if (ModelState.IsValid)
            {
                var MappedPatient = mapper.Map<Patient>(patientViewModel);
                if (MappedPatient.Imag == null)
                {
                    MappedPatient.Imag = patientViewModel.CurrentImage;
                }
                var user = await userManager.FindByIdAsync(patientViewModel.userid);
                if (user != null)
                {
                    user.Id = patientViewModel.userid;
                    user.FName = MappedPatient.FName;
                    user.LName = MappedPatient.LName;
                    user.Image = MappedPatient.Imag;
                    user.BirthDate = MappedPatient.BirthDate;
                    user.Email = MappedPatient.Email;
                    user.PhoneNumber = MappedPatient.Phone;
                    user.UserName = MappedPatient.Username;
                    user.Gender = MappedPatient.Gender;

                    if (patientViewModel.OldPassword != patientViewModel.Password)
                    {
                        await userManager.RemovePasswordAsync(user);
                        var passwordResult = await userManager.AddPasswordAsync(user, patientViewModel.Password);
                    }
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        patientRepository.Update(MappedPatient);
                        patientRepository.Save();
                        if (patientViewModel.OldUserName != patientViewModel.Username)
                        {
                            var currentNameClaim = (await userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == ClaimTypes.Name);
                            if (currentNameClaim != null)
                            {
                                await userManager.RemoveClaimAsync(user, currentNameClaim);
                            }
                            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, patientViewModel.Username));
                            await signInManager.RefreshSignInAsync(user);
                        }
                        return RedirectToAction("PatientProfile");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View("PatientProfileEdit", patientViewModel);
        }

    }
}