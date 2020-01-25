using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(400)]
    public class NationalParksV2Controller : Controller
    {
        private INationalParkRepository _nprepo;
        private readonly IMapper _mapper;

        public NationalParksV2Controller(INationalParkRepository nprepo, IMapper mapper)
        {
            _nprepo = nprepo;
            _mapper = mapper;
        }


        /// <summary>
        /// Get list of national parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type =typeof(List<NationalParkDtos>))]
        public IActionResult GetNationalPark(object objlistdtos)
        {
            var obj = _nprepo.GetNationalParks().FirstOrDefault();
           
            return Ok(_mapper.Map<NationalParkDtos>(obj));
            
           
        }

    }
}