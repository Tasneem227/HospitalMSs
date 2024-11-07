using AutoMapper;
using HospitalMS.Data;
using HospitalMS.Models;
using HospitalMS.Repository;
using HospitalMS.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.Numerics;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HospitalMS.Controllers
{
    public class AdminController : Controller
    {
        IAdminRepository AdminRepository;
        IDepartmentRepository DepartmentRepository;
        IDoctorRepository DoctorRepository;
        INurseRepository NurseRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IPatientRepository patientRepository;

        public AdminController(IAdminRepository adminRepository, IDepartmentRepository departmentRepository, 
            IDoctorRepository doctorRepository, INurseRepository nurseRepository, UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,IMapper mapper,SignInManager<ApplicationUser> signInManager,
            IPatientRepository patientRepository)
        {
            this.AdminRepository = adminRepository;
            this.DepartmentRepository = departmentRepository;
            DoctorRepository = doctorRepository;
            NurseRepository = nurseRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.patientRepository = patientRepository;
        }
        public IActionResult AdminPage()
        {
            return View("Index");
        }
        public async Task<IActionResult> AdminProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            return View("Profile", user);
        }


        //<-------------------------------------------Doctor--------------------------------------------------------->
        //add Doctor
        public IActionResult AddDoctor()
        {
            AdminNurseDoctorViewModel viewModel = new AdminNurseDoctorViewModel();
            return View("AddDoctor", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDoctorData(Doctor doctor)
        {
            ModelState.Remove("Bookings");
            ModelState.Remove("Admins");
            ModelState.Remove("Nurses");
            ModelState.Remove("MedicalRecords");
            ModelState.Remove("Department");
            ModelState.Remove("DoctorNurse");
            ModelState.Remove("DoctorNurse");
            ModelState.Remove("DepartmentId");

            if (ModelState.IsValid)
            {
                //Save to DB
                var username = User.FindFirstValue(ClaimTypes.Name);
                var Userapp = mapper.Map<ApplicationUser>(doctor);
                doctor.DepartmentId = AdminRepository.GetByUserName(username).DepartmentId;
                Userapp.Id = Guid.NewGuid().ToString();
                IdentityResult result = await userManager.CreateAsync(Userapp, doctor.Password);
                if (result.Succeeded)
                {
                    DoctorRepository.Add(doctor);
                    DoctorRepository.Save();
                    //add Role
                    IdentityResult result1 =await userManager.AddToRoleAsync(Userapp, "Doctor");
                    if (result1.Succeeded)
                    {
                        
                        return View("Index");
                    }
                    foreach (var item in result1.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            var view = mapper.Map<AdminNurseDoctorViewModel>(doctor);
            return View("AddDoctor", view);
        }
       
        public async Task<IActionResult> EditDoctor(int id)
        {
            var doctor = DoctorRepository.GetById(id);
            var viewModel = mapper.Map<AdminNurseDoctorViewModel>(doctor);
            viewModel.Id = doctor.Id;
            viewModel.CurrentImage = doctor.Imag;
            
            viewModel.userid = (await userManager.FindByNameAsync(doctor.Username)).Id;

            return View("EditDoctor", viewModel);

        }
        [HttpPost]
        public async Task<IActionResult> SaveDoctorEdit(AdminNurseDoctorViewModel doctor)
        {
           
            if (ModelState.IsValid)
            {

                var MapedDoctor=mapper.Map<Doctor>(doctor);
                if (MapedDoctor.Imag == null) {
                    MapedDoctor.Imag = doctor.CurrentImage;
                }
                var username = User.FindFirstValue(ClaimTypes.Name);
                MapedDoctor.DepartmentId = AdminRepository.GetByUserName(username).DepartmentId;
                var user = await userManager.FindByIdAsync(doctor.userid);
                if (user != null) {
                    user.Id = doctor.userid;
                    user.FName = doctor.FName;
                    user.LName = doctor.LName;
                    user.Image = MapedDoctor.Imag;
                    user.BirthDate = doctor.BirthDate;
                    user.Email = doctor.Email;
                    user.PhoneNumber = doctor.Phone;
                    user.UserName = doctor.Username;
                    if (doctor.OldPassword != doctor.Password)
                    {
                        await userManager.RemovePasswordAsync(user);
                        var passwordResult = await userManager.AddPasswordAsync(user, doctor.Password);
                    }
                    user.Gender = doctor.Gender;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    
                    if (result.Succeeded)
                    {
                        DoctorRepository.Update(MapedDoctor);
                        DoctorRepository.Save();
                        return RedirectToAction("DoctorEditView");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            var viewModel = mapper.Map<AdminNurseDoctorViewModel>(doctor);
            return View("EditDoctor",viewModel);
        }

        public ActionResult DoctorEditView()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            List<Doctor> doctorList = DoctorRepository.GetListByDeptId(AdminRepository.GetByUserName(username).DepartmentId);
            return View("EditDoctor1", doctorList);
        }
        public IActionResult DeleteDoctor()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            List<Doctor> doctorList = DoctorRepository.GetListByDeptId(AdminRepository.GetByUserName(username).DepartmentId);
            return View("DeleteDoctor", doctorList);
        }
        public async Task<IActionResult> EnsureDoctorDelete(int id)
        {
            var doc = DoctorRepository.GetByIdWithMedicalRcord(id);
            if (id != 0)
            {
                var user= await userManager.FindByNameAsync(doc.Username);
                if (user != null && !doc.Bookings.Any(d=>d.DoctorId==id) )
                {
                    if (!doc.MedicalRecords.Any(d => d.DoctorId == id))
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        if (roles.Count > 0)
                        {
                            var removeRolesResult = await userManager.RemoveFromRolesAsync(user, roles);
                            await userManager.DeleteAsync(user);
                            DoctorRepository.Remove(id);
                            DoctorRepository.Save();
                            return RedirectToAction("DeleteDoctor");
                        }
                    }
                    ViewBag.errorMedical = "true";
                }                
            }
            if(doc.Bookings.Any()) { ViewBag.error = "true"; }
            ViewBag.doctorname = doc.FName+" "+doc.LName;
            var userId = User.FindFirstValue(ClaimTypes.Name);
            List<Doctor> doctorList = DoctorRepository.GetListByDeptId(AdminRepository.GetByUserName(userId).DepartmentId);
            return View("DeleteDoctor", doctorList);
        }


        //<-------------------------------------------Nurse--------------------------------------------------------->

        public IActionResult AddNurse()
        {
            AdminNurseDoctorViewModel viewModel = new AdminNurseDoctorViewModel();
            return View("AddNurse", viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SaveNurseData (Nurse nurse)

        {
            ModelState.Remove("Admins");
            ModelState.Remove("Doctors");
            ModelState.Remove("Patients");
            ModelState.Remove("Department");
            ModelState.Remove("DoctorNurse");

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.Name);
                nurse.DepartmentId = AdminRepository.GetByUserName(userId).DepartmentId;
                var Userapp = mapper.Map<ApplicationUser>(nurse);
                Userapp.Id = Guid.NewGuid().ToString(); 
                IdentityResult result = await userManager.CreateAsync(Userapp, nurse.Password);
                NurseRepository.Add(nurse);
                NurseRepository.Save();
                if (result.Succeeded)
                {
                    //add Role
                    IdentityResult result1 = await userManager.AddToRoleAsync(Userapp, "Nurse");
                    if (result1.Succeeded)
                    {

                        return View("Index");
                    }
                    foreach (var item in result1.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                
            }
            
                AdminNurseDoctorViewModel viewModel = new AdminNurseDoctorViewModel();
            ModelState.AddModelError("DepartmentId", "Select Department");
                viewModel.FName = nurse.FName;
                viewModel.LName = nurse.LName;
                viewModel.Salary = nurse.Salary;
                viewModel.Imag = nurse.Imag;
                viewModel.BirthDate = nurse.BirthDate;
                viewModel.Email = nurse.Email;
                viewModel.Phone = nurse.Phone;
                viewModel.Username = nurse.Username;
                viewModel.Password = nurse.Password;
                viewModel.Gender = nurse.Gender;
                return View("AddNurse", viewModel);
         //_______________________________________________________________________________   
        }
         public async Task<IActionResult> EditNurse(int id)
        {
            var nurse = NurseRepository.GetById(id);
            var viewModel = mapper.Map<AdminNurseDoctorViewModel>(nurse);
            viewModel.Id = nurse.Id;
            viewModel.CurrentImage = nurse.Imag;
            viewModel.userid = (await userManager.FindByNameAsync(nurse.Username)).Id;

            return View("EditNurse", viewModel);

        }
        [HttpPost]
        public async Task<IActionResult> SaveNurseEdit(AdminNurseDoctorViewModel nurse)
        {
           
            if (ModelState.IsValid)
            {

                var MapedNurse=mapper.Map<Nurse>(nurse);
                if (MapedNurse.Imag == null) {
                    MapedNurse.Imag = nurse.CurrentImage;
                }
                var userId = User.FindFirstValue(ClaimTypes.Name);
                MapedNurse.DepartmentId = AdminRepository.GetByUserName(userId).DepartmentId;
                var user = await userManager.FindByIdAsync(nurse.userid);
                if (user != null) {
                    user.Id = nurse.userid;
                    user.FName = nurse.FName;
                    user.LName = nurse.LName;
                    user.Image = MapedNurse.Imag;
                    user.BirthDate = nurse.BirthDate;
                    user.Email = nurse.Email;
                    user.PhoneNumber = nurse.Phone;
                    user.UserName = nurse.Username;
                    if (nurse.OldPassword != nurse.Password)
                    {
                        await userManager.RemovePasswordAsync(user);
                        var passwordResult = await userManager.AddPasswordAsync(user, nurse.Password);
                    }
                    user.Gender = nurse.Gender;
                    IdentityResult result = await userManager.UpdateAsync(user);
                    
                    if (result.Succeeded)
                    {
                        NurseRepository.Update(MapedNurse);
                        NurseRepository.Save();
                        return RedirectToAction("NurseEditView");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            var viewModel = mapper.Map<AdminNurseDoctorViewModel>(nurse);
            return View("EditNurse", viewModel);
        }
      
        public ActionResult NurseEditView()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            List<Nurse> NurseList = NurseRepository.GetByDeptId(AdminRepository.GetByUserName(userId).DepartmentId);
            return View("EditNurse1", NurseList);
        }

        public IActionResult DeleteNurse()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            List<Nurse> NurseList = NurseRepository.GetByDeptId(AdminRepository.GetByUserName(userId).DepartmentId);
            return View("DeleteNurse", NurseList);
        }


        
        public async Task<IActionResult> EnsureNurseDelete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            List<Nurse> NurseList = NurseRepository.GetByDeptId(AdminRepository.GetByUserName(userId).DepartmentId);
            if (id != 0)
            {
                var nurse = NurseRepository.GetById(id);
                var user = await userManager.FindByNameAsync(nurse.Username);

                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Count > 0)
                    {
                        var removeRolesResult = await userManager.RemoveFromRolesAsync(user, roles);
                        await userManager.DeleteAsync(user);
                        NurseRepository.RemoveById(id);
                        NurseRepository.Save();
                        return RedirectToAction("DeleteNurse");
                    }
                }
            }
            List<Nurse> nurseList = NurseRepository.GetByDeptId(AdminRepository.GetByUserName(userId).DepartmentId);
            return View("DeleteNurse", nurseList);
        }


       

        //-------------------------------Validation---------------------------------------
        public async Task<ActionResult> CheckUserName(string UserName, int Id)
        {

            var found = await userManager.FindByNameAsync(UserName);
            if (found!=null)
            {
               
                
                return Json(false);
            }
            return Json(true);

        }
        public ActionResult CustomBirthDateValidation(DateOnly BirthDate)
        {
            if (BirthDate.Year < 2005 && BirthDate.Year > 1960)
                return Json(true);
            else return Json(false);


        }
        public ActionResult ConfirmPassword(string ConfirmPassword, string Password)
        {

            if (!string.IsNullOrEmpty(ConfirmPassword))
            {
                if (ConfirmPassword.Equals(Password))
                    return Json(true);
            }
            return Json(false);
        }

        //---------------------------------------Patient------------------------------------------
        public ActionResult AllPatients()
        {
            List<Patient> patients = patientRepository.GetAll();
            return View("AllPatients", patients);
        }

    }
   


}

