﻿using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}