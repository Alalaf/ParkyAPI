using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nprepo;
        public NationalParkController(INationalParkRepository nprepo)
        {
            _nprepo = nprepo;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        public async Task<IActionResult> GetALLNationalPark()
        {
            return Json(new { data = await _nprepo.GetAllAsync(SD.NationalParkAPIPath) });
        }
    }
}