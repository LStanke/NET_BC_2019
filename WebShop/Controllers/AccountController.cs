﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebShop.Logic;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(RegisteredUserModel model)
        {
            //Ja izmanto registracijas modeli:
            //ModelState.Remove("PasswordRepeat");
            if (ModelState.IsValid)
            {
                var manager = new UserManager();
                var user = manager.GetByEmailAndPassword(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("error", "Invalid email/password");
                }                
                else
                {
                    HttpContext.Session.SetUserId(user.Id);
                    HttpContext.Session.SetUserEmail( user.Email);
                    TempData["message"] = "Login succesful";
                    return RedirectToAction("Index", "Item");
                }             
                
            }
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                UserManager manager = new UserManager();
                if (manager.GetByEmail(model.Email) !=null)
                {
                    ModelState.AddModelError("error", "Email already exists!");
                }
                else
                {
                    manager.Create(new Logic.User()
                    {
                        Email=model.Email,
                        Password=model.Password
                    });

                    TempData["message"] = "Account created!";
                    return RedirectToAction("SignIn");
                }                
            }            
            
            return View();
        }
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Item");
        }

    }
}