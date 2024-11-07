using AutoMapper;
using HospitalMS.Data;
using HospitalMS.Models;
using HospitalMS.Repository;
using HospitalMS.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HospitalMS.Controllers
{
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public class DoctorController : Controller
    {

        IPatientRepository patientRepository;
        IDoctorRepository DoctorRepository;
        IMedicalRecordRepository MedicalRecordRepository;
        IDepartmentRepository departmentRepository;
        IBookingRepository bookingRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DoctorController(IDoctorRepository DoctorRepository, IPatientRepository patientRepository, 
            IMedicalRecordRepository MedicalRecordRepository, IDepartmentRepository departmentRepository,
            IBookingRepository bookingRepository, UserManager<ApplicationUser> userManager , 
            IMapper mapper,SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            this.DoctorRepository = DoctorRepository;
            this.patientRepository = patientRepository;
            this.MedicalRecordRepository = MedicalRecordRepository;
            this.departmentRepository = departmentRepository;
            this.bookingRepository = bookingRepository;
            this.userManager = userManager;
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View("Index");
        }
        public async Task<IActionResult> DoctorProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            ViewBag.role=await userManager.GetRolesAsync(user);
            return View("Profile", user);
        }
        //<-------------------------------------------------Add-------------------------------------------------->
        public async Task<IActionResult> AddMedicalRecordAsync(int DocId)
{
    var username = User.FindFirstValue(ClaimTypes.Name);
    DocId=DoctorRepository.SearchByUserName(username).Id;
    MedicalRecordWithPatientIdList viewModel = new MedicalRecordWithPatientIdList();
    viewModel.patientlist = patientRepository.GetAllPatientByDocId(DocId);
    viewModel.DoctorId = DocId;
    return View("AddPatientMedicalRecord", viewModel);
}

public IActionResult AddPatientMedicalRecord(MedicalRecord record)
{
    ModelState.Remove("Doctor");
    ModelState.Remove("Patient");

    if (ModelState.IsValid)
    {
        MedicalRecordRepository.Add(record);
        MedicalRecordRepository.Save();
        return View("Index");
    }
   
        MedicalRecordWithPatientIdList viewModel = new MedicalRecordWithPatientIdList();
        viewModel.Id = record.Id;
        viewModel.DoctorId = record.DoctorId;
        viewModel.Date = record.Date;
        viewModel.PatientId = record.PatientId;
        viewModel.Note = record.Note;
        viewModel.patientlist = patientRepository.GetAllPatientByDocId(record.DoctorId);
        return View ("AddPatientMedicalRecord",viewModel);
    
}
       


        //--------------------------------------Delete---------------------------------------------------------/

        public IActionResult DeleteMedicalRecord(int DocId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            DocId = DoctorRepository.SearchByUserName(username).Id;
            MedicalRecordWithPatientIdList medical = new MedicalRecordWithPatientIdList();
            medical.patientlist = patientRepository.GetAllPatientByDocId(DocId) ?? new List<Patient>();
            medical.MedicalRecords = MedicalRecordRepository.GetMedicalRecordsByDocId(DocId) ?? new List<MedicalRecord>();
            return View("DeletePatientMedicalRecord", medical);
        }

        public IActionResult DeletePatientMedicalRecord(int id, int medicalrecordId)
        {

            MedicalRecordWithPatientIdList medical = new MedicalRecordWithPatientIdList();
            List<MedicalRecord> medicallist = MedicalRecordRepository.GetListByMedicalRecordId(medicalrecordId);
            medical.MedicalRecords = medicallist;

            var obj = MedicalRecordRepository.GetById(id);
            MedicalRecordRepository.Delete(obj);
            MedicalRecordRepository.Save();
            return RedirectToAction("DeleteMedicalRecord" ,new { DocId =4});
        }
        //--------------------------------------ViewAppointment--------------------------------------------------------/
        public IActionResult ViewAppointment(int DocId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            DocId = DoctorRepository.SearchByUserName(username).Id;
            List<Booking> BookingLst = bookingRepository.GetBookingListByDocId(DocId);
            return View("ViewPatientAppointment", BookingLst);
        }

        //--------------------------------------------------Edit Profile-----------------------------------------------------------/

       
        public async Task<IActionResult> DoctorEditView(int DocId)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            Doctor doctor = DoctorRepository.SearchByUserName(username);
            var DoctorModel = mapper.Map<AdminNurseDoctorViewModel>(doctor);
            DoctorModel.CurrentImage = doctor.Imag;
            DoctorModel.userid = (await userManager.FindByNameAsync(username)).Id;
            DoctorModel.OldPassword = doctor.Password;
            DoctorModel.OldUserName = doctor.Username;
            return View("EditDoctor", DoctorModel);
        }


        public async Task<ActionResult> SaveEdit(AdminNurseDoctorViewModel doctorViewModel)
        {
            if (ModelState.IsValid)
            {
                var MapedDoctor = mapper.Map<Doctor>(doctorViewModel);
                if (MapedDoctor.Imag == null)
                {
                    MapedDoctor.Imag = doctorViewModel.CurrentImage;
                }
                var user = await userManager.FindByIdAsync(doctorViewModel.userid);
                if (user != null)
                {
                    user.Id = doctorViewModel.userid;
                    user.FName = MapedDoctor.FName;
                    user.LName = MapedDoctor.LName;
                    user.Image = MapedDoctor.Imag;
                    user.BirthDate = MapedDoctor.BirthDate;
                    user.Email = MapedDoctor.Email;
                    user.PhoneNumber = MapedDoctor.Phone;
                    user.UserName = MapedDoctor.Username;
                    user.Gender = MapedDoctor.Gender;
                    
                    if (doctorViewModel.OldPassword != doctorViewModel.Password) {
                        await userManager.RemovePasswordAsync(user);
                        var passwordResult = await userManager.AddPasswordAsync(user, doctorViewModel.Password);
                    }
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        DoctorRepository.Update(MapedDoctor);
                        DoctorRepository.Save();
                        if (doctorViewModel.OldUserName != doctorViewModel.Username)
                        {
                            var currentNameClaim = (await userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == ClaimTypes.Name);
                            if (currentNameClaim != null)
                            {
                                await userManager.RemoveClaimAsync(user, currentNameClaim);
                            }
                            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, doctorViewModel.Username));
                            await signInManager.RefreshSignInAsync(user);
                        }
                        return RedirectToAction("DoctorProfile");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            var viewModel = mapper.Map<AdminNurseDoctorViewModel>(doctorViewModel);
            return View("EditDoctor", viewModel);
        }

    }
}