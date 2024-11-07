using AutoMapper;
using HospitalMS.AutoMapper;
using HospitalMS.Data;
using HospitalMS.Models;
using HospitalMS.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;


namespace HospitalMS.Controllers
{
    public class NurseController : Controller
    {

        IDoctorNurseRepository MapedNurseNurseRepository;
        IDepartmentRepository DepartmentRepository;
        IDoctorRepository DoctorRepository;
        INurseRepository NurseRepository;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        IBookingRepository bookingRepository;
        IMedicalRecordRepository MedicalRecordRepository;
        public NurseController(IMedicalRecordRepository _MedicalRecordRepository,
            IBookingRepository _bookingRepository, IDoctorNurseRepository _MapedNurseNurseRepository,
            IDepartmentRepository departmentRepository, IDoctorRepository MapedNurseRepository,
            INurseRepository nurseRepository, IMapper mapper, UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager)
        {
            MedicalRecordRepository = _MedicalRecordRepository;
            bookingRepository = _bookingRepository;
            MapedNurseNurseRepository = _MapedNurseNurseRepository;
            this.DepartmentRepository = departmentRepository;
            DoctorRepository = MapedNurseRepository;
            NurseRepository = nurseRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        


        public IActionResult Index()
        {
            return View("Index");
        }
        public async Task<IActionResult> NurseProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            ViewBag.role =await userManager.GetRolesAsync(user);
            return View("Profile", user);
        }

        public async Task<IActionResult> ViewAppointment(int NrsId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            NrsId = NurseRepository.SearchByUserName(username).Id;
            List<BookingNurseViewModel>? bookings = await bookingRepository
                .GetDepartmentAppointments(NurseRepository.GetById(NrsId).DepartmentId);
            return View("ViewPatientAppointment", bookings);
        }

        public async Task<IActionResult> ViewPatientMedicalRecord(int NrsId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            NrsId = NurseRepository.SearchByUserName(username).Id;
            List<MedicalRecordNurseViewModel>? bookings = await bookingRepository
                .GetDepartmenMedicalRecord(NurseRepository.GetById(NrsId).DepartmentId);
            return View("ViewPatientMedicalRecord", bookings );
        }

       
        public async Task<IActionResult> NurseEditView(int NrsId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            NrsId = NurseRepository.SearchByUserName(username).Id;
            Nurse nurse = NurseRepository.GetById(NrsId);
            var nurseModel = mapper.Map<AdminNurseDoctorViewModel>(nurse);
            nurseModel.CurrentImage = nurse.Imag;
            nurseModel.userid = (await userManager.FindByNameAsync(username)).Id;
            nurseModel.OldPassword = nurse.Password;
            nurseModel.OldUserName = nurse.Username;
            return View("Edit", nurseModel);
        }


        public async Task<ActionResult> SaveEdit(AdminNurseDoctorViewModel Nrs)
        {
           
            if (ModelState.IsValid)
            {
                var MapedNurse = mapper.Map<Nurse>(Nrs);
                if (MapedNurse.Imag == null)
                {
                    MapedNurse.Imag = Nrs.CurrentImage;
                }
                var user = await userManager.FindByIdAsync(Nrs.userid);
                if (user != null)
                {
                    user.Id = Nrs.userid;
                    user.FName = MapedNurse.FName;
                    user.LName = MapedNurse.LName;
                    user.Image = MapedNurse.Imag;
                    user.BirthDate = MapedNurse.BirthDate;
                    user.Email = MapedNurse.Email;
                    user.PhoneNumber = MapedNurse.Phone;
                    user.UserName = MapedNurse.Username;
                    user.Gender = MapedNurse.Gender;
                    if (Nrs.OldPassword != Nrs.Password)
                    {
                        await userManager.RemovePasswordAsync(user);
                        var passwordResult = await userManager.AddPasswordAsync(user, Nrs.Password);
                    }
                    IdentityResult result = await userManager.UpdateAsync(user);
                    
                    if (result.Succeeded)
                    {
                        NurseRepository.Update(MapedNurse);
                        NurseRepository.Save();
                        if (Nrs.OldUserName != Nrs.Username)
                        {
                            var currentNameClaim = (await userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == ClaimTypes.Name);
                            if (currentNameClaim != null)
                            {
                                await userManager.RemoveClaimAsync(user, currentNameClaim);
                            }
                            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, MapedNurse.Username));
                            await signInManager.RefreshSignInAsync(user);
                        }
                        return RedirectToAction("NurseProfile");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            var viewModel = mapper.Map<AdminNurseDoctorViewModel>(Nrs);
            return View("Edit", viewModel);
        }


    }
}