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

    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/Trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]
    [ProducesResponseType(400)]
    public class TrailsController : Controller
    {
        private ITrailRepository _trailrepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailrepo, IMapper mapper)
        {
            _trailrepo = trailrepo;
            _mapper = mapper;
        }


        /// <summary>
        /// Get list of national parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type =typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objlist = _trailrepo.GetTrails();
            var objlistdtos = new List<TrailDto>();
            foreach(var obj in objlist)
            {
                objlistdtos.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objlistdtos);
        }


        /// <summary>
        /// Get indivisual national park
        /// </summary>
        /// <param name="trailsid">the Id of national park</param>
        /// <returns></returns>
        [HttpGet("{trailsid:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]       
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrails(int trailsid)
        {
            var obj = _trailrepo.GetTrail(trailsid);
            if(obj == null)
            {
                return NotFound();
            }
            var objdtos = _mapper.Map<TrailDto>(obj);
            return Ok(objdtos);
        }


        [HttpGet("[action]/{nationalparkid:int}")]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalparkid)
        {
            var objlist = _trailrepo.GetTrailsInNationalPark(nationalparkid);
            if (objlist == null)
            {
                return NotFound();
            }
            var objdtos = new List<TrailDto>();
            foreach(var obj in objlist)
            {
                objdtos.Add(_mapper.Map<TrailDto>(obj));
            }
             
            return Ok(objdtos);
        }



        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrails([FromBody] TrailCreateDto trailsdtos)
        {
            if (trailsdtos == null)
            {
                return BadRequest(ModelState);
            }

            

            if (_trailrepo.TrailExists(trailsdtos.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);
            }

            var trailsobj = _mapper.Map<Trail>(trailsdtos);

            if(!_trailrepo.CreateTrail(trailsobj))
            {
                ModelState.AddModelError("", $"something wrong with {trailsobj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrails", new { trailsid = trailsobj.Id }, trailsobj);
        }

        [HttpPatch("{trailsid:int}", Name = "UpdateTrails")]
        [ProducesResponseType(204)]       
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrails(int trailsid,[FromBody] TrailUpdateDto trailsdtos)
        {
            if (trailsdtos == null || trailsid != trailsdtos.Id)
            {
                return BadRequest(ModelState);
            }

            var trailsobj = _mapper.Map<Trail>(trailsdtos);

            if (!_trailrepo.UpdateTrail(trailsobj))
            {
                ModelState.AddModelError("", $"something wrong with {trailsobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{trailsid:int}", Name = "DeleteTrails")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrails(int trailsid)
        {
            if (!_trailrepo.TrailExists(trailsid))
            {
                return NotFound();
            }

            var trailsobj = _trailrepo.GetTrail(trailsid);

            if (!_trailrepo.DeleteTrail(trailsobj))
            {
                ModelState.AddModelError("", $"something wrong with {trailsobj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


    }
}