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
        private readonly INurseRepository nurseRepository;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ISuperAdminRepository superAdminRepository;

        public SuperAdminController
            (IAdminRepository _adminRepository, IDepartmentRepository _departmentRepository,
            IDoctorRepository _doctorRepository, IBookingRepository _bookingRepository,
            INurseRepository _nurseRepository,IMapper mapper ,UserManager<ApplicationUser> userManager
            ,ISuperAdminRepository superAdminRepository)
        {
            adminRepository = _adminRepository;
            departmentRepository = _departmentRepository;
            doctorRepository = _doctorRepository;
            bookingRepository = _bookingRepository;
            nurseRepository = _nurseRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.superAdminRepository = superAdminRepository;
        }



        public IActionResult SuperAdminPage()
        {
            return View("SuperAdminPage");
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

        //EditAdmin --
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
                    admin.Id = ViewModel.Id;
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

        public IActionResult SaveNewDepartment(string Name, string Description)
        {
            Department department = new Department();
            department.Name = Name;
            department.Description = Description;
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
        public IActionResult EditDepartment(int DeptId)
        {
            return View("EditDepartment", departmentRepository.GetById(DeptId));
        }

        public IActionResult SaveDepartmentEdit(Department department)
        {
            if (ModelState.IsValid)
            {
                departmentRepository.Update(department);
                departmentRepository.Save();
                return RedirectToAction("ViewAllDepartments");
            }
            return View("EditDepartment", department);
        }


        //view all doctors with their department------------------------------------------------------------------------------------------
        public ActionResult ViewAllDeptDoctors(int DeptId)
        {
            if (DeptId == 0)
            {
                return View("ViewAllDeptDoctors", doctorRepository.GetAll());
            }
            return View("ViewAllDeptDoctors", doctorRepository.GetListByDeptId(DeptId));
        }

        //view all nurses with their department-------------------------------------------------------------------------------------------
        public ActionResult ViewAllDeptNurses(int DeptId)
        {
            if (DeptId == 0)
            {
                return View("ViewAllDeptNurses", nurseRepository.GetAll());
            }
            return View("ViewAllDeptNurses", nurseRepository.GetByDeptId(DeptId));
        }

        //view all recorded appointments--
        public ActionResult ViewDoctorBookedAppointments(int DocId)
        {
            return View("ViewDoctorBookedAppointments", bookingRepository.GetDocBookingListWithPatients(DocId));
        }
        

       

        public ActionResult CheckUserName(string Username, int Id)
        {
            if (adminRepository.GetByUserNameAndId(Username, Id) == null)
                return Json(true);
            return Json(false);
        }
        public ActionResult CustomBirthDateValidation(DateOnly BirthDate)
        {
            if (BirthDate.Year < 2005 && BirthDate.Year > 1960)
                return Json(true);
            else return Json(false);
        }

    }
}
