using HospitalMS.AutoMapper;
using AutoMapper;
using HospitalMS.Models.Identity;
using HospitalMS.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HospitalMS.Models;

namespace HospitalMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IMapper Mapper;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;
       
        private readonly IMapper mapper;

        public AccountController(UserManager<ApplicationUser> userManager,IMapper mappingProfile,SignInManager<ApplicationUser> signInManager
          , ApplicationDbContext context, IMapper mapper)
        {
            this.UserManager = userManager;
            this.Mapper = mappingProfile;
            this.signInManager = signInManager;
            this.context = context;
            
            this.mapper = mapper;
        }
      
        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View("Register",registerViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> saveRegister(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                //Mapping
                var Userapp = Mapper.Map<ApplicationUser>(registerViewModel);
                //Save to DB
                Patient patient = new Patient();
                patient.FName = Userapp.FName;
                patient.LName = Userapp.LName;
                patient.Imag = Userapp.Image;
                patient.BirthDate = Userapp.BirthDate;
                patient.Email = Userapp.Email;
                patient.Phone = Userapp.PhoneNumber;
                patient.Username = Userapp.UserName;
                patient.Password = Userapp.PasswordHash;
                patient.Gender = Userapp.Gender;
                context.Patients.Add(patient);
                context.SaveChanges();
                IdentityResult result = await UserManager.CreateAsync(Userapp, registerViewModel.Password);
                if (result.Succeeded)
                {
                    //Assign to role 
                    IdentityResult result1 = await UserManager.AddToRoleAsync(Userapp, "Patient");
                    if (result1.Succeeded)
                    {
                        //create cookie
                        await signInManager.SignInAsync(Userapp, false);
                       
                        return RedirectToAction("Index", "Home");
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
            return View("Register", registerViewModel);
        }
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
        
        public IActionResult Login()
        {
            LoginUserViewModel loginViewModel = new LoginUserViewModel();
            return View("Login",loginViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
               ApplicationUser appuser= await UserManager.FindByNameAsync(loginUser.UserName);
                if (appuser != null)
                {
                    bool check = await UserManager.CheckPasswordAsync(appuser, loginUser.Password);
                    if (check)
                    {

                        await signInManager.SignInAsync(appuser, loginUser.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                        ModelState.AddModelError("", "Wrong Password ");
                }
                else
                {
                    ModelState.AddModelError("", "User Name or Password May Be Wrong");
                }
            }
            return View("Login",loginUser);
        }











    }
}
