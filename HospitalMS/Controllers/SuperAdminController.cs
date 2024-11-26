using AutoMapper;
using HospitalMS.Models;
using HospitalMS.Repository;
using HospitalMS.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Numerics;
using System.Security.Claims;

namespace HospitalMS.Controllers
{
    public class SuperAdminController : Controller
    {
        private readonly IAdminRepository adminRepository;
         readonly IDepartmentRepository departmentRepository;
        private readonly IDoctorRepository doctorRepository;
        private readonly IBookingRepository bookingRepository;
        private readonly IPatientRepository patientRepository;
        private readonly INurseRepository nurseRepository;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISuperAdminRepository superAdminRepository;

        public SuperAdminController
            (IAdminRepository _adminRepository, IDepartmentRepository _departmentRepository,
            IDoctorRepository _doctorRepository, IBookingRepository _bookingRepository,
            IPatientRepository patientRepository,
            INurseRepository _nurseRepository,IMapper mapper ,UserManager<ApplicationUser> userManager
            ,ISuperAdminRepository superAdminRepository)
        {
            adminRepository = _adminRepository;
            departmentRepository = _departmentRepository;
            doctorRepository = _doctorRepository;
            bookingRepository = _bookingRepository;
            this.patientRepository = patientRepository;
            nurseRepository = _nurseRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.superAdminRepository = superAdminRepository;
        }



        public IActionResult SuperAdminPage()
        {
            return View("SuperAdminPage");
        }
        public async Task<IActionResult> SuperAdminProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            return View("Profile", user);
        }

        //view all admins with their departments ------------------------------
        public IActionResult ViewAllAdmins()
        {
            return View("ViewAllAdmins", adminRepository.GetAll());
        }
        //AddNewSuperAdmin ---------------------------------------------------------
        public IActionResult AddNewSuperAdmin()
        {

            return View("AddSuperAdmin", new SuperAdminViewModel());
        }
        

        [HttpPost]
        public async Task<IActionResult> SaveSuperAdmin(SuperAdminViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                

                    SuperAdmin SuperAdmin = new ();
                    SuperAdmin.FName = ViewModel.FName;
                    SuperAdmin.LName = ViewModel.LName;
                    SuperAdmin.Username = ViewModel.Username;
                    SuperAdmin.Password = ViewModel.Password;
                    SuperAdmin.BirthDate = ViewModel.BirthDate;
                    SuperAdmin.Email = ViewModel.Email;
                    SuperAdmin.Phone = ViewModel.Phone;
                    SuperAdmin.Imag = ViewModel.Image;
                    SuperAdmin.Gender = ViewModel.Gender;
                    
                    var Userapp = mapper.Map<ApplicationUser>(SuperAdmin);
                    Userapp.Id = Guid.NewGuid().ToString();
                    IdentityResult result = await userManager.CreateAsync(Userapp, SuperAdmin.Password);
                    if (result.Succeeded)
                    {
                        IdentityResult result1 = await userManager.AddToRoleAsync(Userapp, "Super Admin");
                        if (result1.Succeeded)
                        {
                            superAdminRepository.Add(SuperAdmin);
                            superAdminRepository.Save();
                            return RedirectToAction("ViewAllSuperAdmins");
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

            return View("AddNewSuperAdmin", ViewModel);

        }
        public IActionResult ViewAllSuperAdmins()
        {
            return View("ViewAllSuperAdmins", superAdminRepository.GetAll());
        }
        //AddNewAdmin ---------------------------------------------------------
        public IActionResult AddNewAdmin()
        {
            ViewBag.DeptList = departmentRepository.GetAll();

            return View("AddNewAdmin", new SuperAdminAddAdminViewModel());
        }
        

        [HttpPost]
        public async Task<IActionResult> SaveAdmin(SuperAdminAddAdminViewModel ViewModel)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            int SuperAdminId=superAdminRepository.GetByUserName(username);
            if (ModelState.IsValid)
            {
                if (ViewModel.DepartmentId != 0)
                {

                    Admin admin = new Admin();
                    admin.DepartmentId = ViewModel.DepartmentId;
                    admin.FName = ViewModel.FName;
                    admin.LName = ViewModel.LName;
                    admin.Username = ViewModel.Username;
                    admin.Password = ViewModel.Password;
                    admin.BirthDate = ViewModel.BirthDate;
                    admin.Email = ViewModel.Email;
                    admin.Phone = ViewModel.Phone;
                    admin.Imag = ViewModel.Image;
                    admin.Gender = ViewModel.Gender;
                    admin.SuperAdminId = SuperAdminId;
                    var Userapp = mapper.Map<ApplicationUser>(admin);
                    Userapp.Id = Guid.NewGuid().ToString();
                    IdentityResult result = await userManager.CreateAsync(Userapp, admin.Password);
                    if (result.Succeeded)
                    {
                        IdentityResult result1 = await userManager.AddToRoleAsync(Userapp, "Admin");
                        if (result1.Succeeded)
                        {
                            adminRepository.Add(admin);
                            adminRepository.Save();
                            return RedirectToAction("ViewAllAdmins");
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

                ModelState.AddModelError("DepartmentId", "Please select a valid department.");
            }

            ViewData["DeptList"] = departmentRepository.GetAll();
            return View("AddNewAdmin", ViewModel);

        }

        //EditAdmin -----------------------------------------------------------------------------------------------
        public async Task<IActionResult> EditAdmin(int AdminId)
        {
            ViewData["DeptList"] = departmentRepository.GetAll();
            Admin admin = adminRepository.GetById(AdminId);
            SuperAdminAddAdminViewModel ViewModel = new();
            ViewModel.Id= AdminId;
            ViewModel.FName=admin.FName;
            ViewModel.LName=admin.LName;
            ViewModel.Username = admin.Username;
            ViewModel.Password = admin.Password;
            ViewModel.BirthDate = admin.BirthDate;
            ViewModel.Email = admin.Email;
            ViewModel.Phone = admin.Phone;
            ViewModel.Image = admin.Imag;
            ViewModel.Gender = admin.Gender;
            ViewModel.CurrentImage = admin.Imag;
            ViewModel.userid = (await userManager.FindByNameAsync(admin.Username)).Id;
            return View("EditAdmin", ViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SaveAdminEditAsync(SuperAdminAddAdminViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                if (ViewModel.DepartmentId != 0)
                {
                    var username = User.FindFirstValue(ClaimTypes.Name);
                    int SuperAdminId = superAdminRepository.GetByUserName(username);
                    Admin admin = new Admin();
                    admin.Id = (int)ViewModel.Id;
                    admin.DepartmentId = ViewModel.DepartmentId;
                    admin.FName = ViewModel.FName;
                    admin.LName = ViewModel.LName;
                    admin.Username = ViewModel.Username;
                    admin.Password = ViewModel.Password;
                    admin.BirthDate = ViewModel.BirthDate;
                    admin.Email = ViewModel.Email;
                    admin.Phone = ViewModel.Phone;
                    admin.Imag = ViewModel.Image ?? ViewModel.CurrentImage;
                    admin.Gender = ViewModel.Gender;
                    admin.SuperAdminId = SuperAdminId;
                    var user = await userManager.FindByIdAsync(ViewModel.userid);
                    if (user != null)
                    {
                        user.Id = ViewModel.userid;
                        user.FName = ViewModel.FName;
                        user.LName = ViewModel.LName;
                        user.Image = admin.Imag;
                        user.BirthDate = ViewModel.BirthDate;
                        user.Email = ViewModel.Email;
                        user.PhoneNumber = ViewModel.Phone;
                        user.UserName = ViewModel.Username;
                        await userManager.RemovePasswordAsync(user);
                        var passwordResult = await userManager.AddPasswordAsync(user, ViewModel.Password);
                        user.Gender = ViewModel.Gender;
                        IdentityResult result = await userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            adminRepository.Update(admin);
                            adminRepository.Save();
                            return RedirectToAction("ViewAllAdmins");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    ModelState.AddModelError("DepartmentId", "Please select a valid department.");
                }
            }
            ViewData["DeptList"] = departmentRepository.GetAll();
            return View("EditAdmin", ViewModel);

        }
        // Delete Admin--------------------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var admin = adminRepository.GetById(id);
            admin.SuperAdminId = null;
            if (id != 0)
            {
                var user = await userManager.FindByNameAsync(admin.Username);
                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Count > 0)
                    {
                        var removeRolesResult = await userManager.RemoveFromRolesAsync(user, roles);
                        await userManager.DeleteAsync(user);
                        adminRepository.Remove(id);
                        adminRepository.Save();
                        return RedirectToAction("ViewAllAdmins");
                    }
                }
            }
           return RedirectToAction("ViewAllAdmins");
    }

        //AddNewDepartment----------------------------------------------------------------------------------------------------------

        public IActionResult AddNewDepartment()
        {
            return View("AddNewDepartment");
        }

        public IActionResult SaveNewDepartment(Department department)
        {
          
            if (ModelState.IsValid)
            {
                departmentRepository.Add(department);
                departmentRepository.Save();
                return RedirectToAction("ViewAllDepartments");
            }
            return View("AddNewDepartment", department);
        }


        //view all departments---------------------------------------------------------------------------------------------------------
        public IActionResult ViewAllDepartments()
        {
            return View("ViewAllDepartments", departmentRepository.GetAll());
        }


        //EditDepartment--------------------------------------------------------------------------------------------------------------
        public IActionResult EditDepartment(int DeptId, string depName)
        {
            Department department = departmentRepository.GetById(DeptId);
            ViewBag.currentImage = department.image;
            ViewBag.depName = depName;
            return View("EditDepartment",department);
        }

        public IActionResult SaveDepartmentEdit(Department department,string currentimage)
        {
            if (ModelState.IsValid)
            {
                if (department.image == null) { department.image = currentimage; }
                departmentRepository.Update(department);
                departmentRepository.Save();
                return RedirectToAction("ViewAllDepartments");
            }
            return View("EditDepartment", department);
        }


        ///////////////////////////////////////Edit Patient ///////////////////////////////////////////
        public async Task<IActionResult> EditPatient(int PatientId)
        {
            ViewData["DeptList"] = departmentRepository.GetAll();
            Patient patient = patientRepository.GetById(PatientId);
            PatientViewModel ViewModel = new();
            ViewModel.Id = PatientId;
            ViewModel.FName = patient.FName;
            ViewModel.LName = patient.LName;
            ViewModel.Username = patient.Username;
            ViewModel.Password = patient.Password;
            ViewModel.BirthDate = patient.BirthDate;
            ViewModel.Email = patient.Email;
            ViewModel.Phone = patient.Phone;
            ViewModel.Imag = patient.Imag;
            ViewModel.Gender = patient.Gender;
            ViewModel.CurrentImage = patient.Imag;
            ViewModel.userid = (await userManager.FindByNameAsync(patient.Username)).Id;
            return View("Editpatient", ViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SavePatientEdit(PatientViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                
                    var username = User.FindFirstValue(ClaimTypes.Name);
                    int SuperAdminId = superAdminRepository.GetByUserName(username);
                    Patient patient = new Patient();
                    patient.Id = ViewModel.Id;
                    patient.FName = ViewModel.FName;
                    patient.LName = ViewModel.LName;
                    patient.Username = ViewModel.Username;
                    patient.Password = ViewModel.Password;
                    patient.BirthDate = ViewModel.BirthDate;
                    patient.Email = ViewModel.Email;
                    patient.Phone = ViewModel.Phone;
                    patient.Imag = ViewModel.Imag ?? ViewModel.CurrentImage;
                    patient.Gender = ViewModel.Gender;
                    var user = await userManager.FindByIdAsync(ViewModel.userid);
                    if (user != null)
                    {
                        user.Id = ViewModel.userid;
                        user.FName = ViewModel.FName;
                        user.LName = ViewModel.LName;
                        user.Image = patient.Imag;
                        user.BirthDate = ViewModel.BirthDate;
                        user.Email = ViewModel.Email;
                        user.PhoneNumber = ViewModel.Phone;
                        user.UserName = ViewModel.Username;
                        await userManager.RemovePasswordAsync(user);
                        var passwordResult = await userManager.AddPasswordAsync(user, ViewModel.Password);
                        user.Gender = ViewModel.Gender;
                        IdentityResult result = await userManager.UpdateAsync(user);

                        if (result.Succeeded)
                        {
                            patientRepository.Update(patient);
                            patientRepository.Save();
                            return RedirectToAction("AllPatients","Admin");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
            }
            return View("EditPatient", ViewModel);

        }
        //-----------------------------Delete Patient -------------------------------------------------------------
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = patientRepository.GetById(id);
            if (id != 0)
            {
                var user = await userManager.FindByNameAsync(patient.Username);
                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Count > 0)
                    {
                        bookingRepository.DeletePatientAppointmentList(id);
                        patientRepository.RemoveById(id);
                        patientRepository.Save();
                        var removeRolesResult = await userManager.RemoveFromRolesAsync(user, roles);
                        await userManager.DeleteAsync(user);
                        return RedirectToAction("AllPatients","Admin");
                    }
                }
            }
            return RedirectToAction("AllPatients", "Patient");
         }

        //view all doctors with their department------------------------------------------------------------------------------------------
        public ActionResult ViewAllDeptDoctors(int DeptId,string depName)
        {
            if (DeptId == 0)
            {
                return View("ViewAllDeptDoctors", doctorRepository.GetAll());
            }
            ViewBag.depId = DeptId;
            ViewBag.depName = depName;
            return View("ViewAllDeptDoctors", doctorRepository.GetListByDeptId(DeptId));
        }

        //view all nurses with their department-------------------------------------------------------------------------------------------
        public ActionResult ViewAllDeptNurses(int DeptId, string depName)
        {
            if (DeptId == 0)
            {
                return View("ViewAllDeptNurses", nurseRepository.GetAll());
            }
            ViewBag.depId = DeptId;
            ViewBag.depName = depName;
            return View("ViewAllDeptNurses", nurseRepository.GetByDeptId(DeptId));
        }

        //view all recorded appointments--
        public ActionResult ViewDoctorBookedAppointments(int DoctId, string docname)
        {
            ViewBag.doctorname = docname;
            return View("ViewDoctorBookedAppointments", bookingRepository.GetDocBookingListWithPatients(DoctId));
        }
     
        public ActionResult CustomBirthDateValidation(DateOnly BirthDate)
        {
            if (BirthDate.Year < 2005 && BirthDate.Year > 1960)
                return Json(true);
            else return Json(false);
        }

    }
}
