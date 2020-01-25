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
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    [ProducesResponseType(400)]
    public class NationalParksController : Controller
    {
        private INationalParkRepository _nprepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository nprepo, IMapper mapper)
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
        public IActionResult GetNationalPark()
        {
            var objlist = _nprepo.GetNationalParks();
            var objlistdtos = new List<NationalParkDtos>();
            foreach(var obj in objlist)
            {
                objlistdtos.Add(_mapper.Map<NationalParkDtos>(obj));
            }
            return Ok(objlistdtos);
        }


        /// <summary>
        /// Get indivisual national park
        /// </summary>
        /// <param name="nationalparkid">the Id of national park</param>
        /// <returns></returns>
        [HttpGet("{nationalparkid:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDtos>))]       
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalparkid)
        {
            var obj = _nprepo.GetNationalPark(nationalparkid);
            if(obj == null)
            {
                return NotFound();
            }
            var objdtos = _mapper.Map<NationalParkDtos>(obj);
            return Ok(objdtos);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<NationalParkDtos>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDtos nationalparkdtos)
        {
            if (nationalparkdtos == null)
            {
                return BadRequest(ModelState);
            }

            

            if (_nprepo.NationalParkExists(nationalparkdtos.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);
            }

            var nationalparkobj = _mapper.Map<NationalPark>(nationalparkdtos);

            if(!_nprepo.CreateNationalPark(nationalparkobj))
            {
                ModelState.AddModelError("", $"something wrong with {nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark", new { nationalparkid = nationalparkobj.Id }, nationalparkobj);
        }

        [HttpPatch("{nationalparkid:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]       
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalparkid,[FromBody] NationalParkDtos nationalparkdtos)
        {
            if (nationalparkdtos == null || nationalparkid != nationalparkdtos.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalparkobj = _mapper.Map<NationalPark>(nationalparkdtos);

            if (!_nprepo.UpdateNationalPark(nationalparkobj))
            {
                ModelState.AddModelError("", $"something wrong with {nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{nationalparkid:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalparkid)
        {
            if (!_nprepo.NationalParkExists(nationalparkid))
            {
                return NotFound();
            }

            var nationalparkobj = _nprepo.GetNationalPark(nationalparkid);

            if (!_nprepo.DeleteNationalPark(nationalparkobj))
            {
                ModelState.AddModelError("", $"something wrong with {nationalparkobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


    }
}