using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;
using System.Linq;

namespace StarChart.Controllers
{
    [Route(""), ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(co => co.Id == id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            else
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == id).ToList();
                return Ok(celestialObject);
            }

        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(co => co.Name.Equals(name)).ToList();
            if (celestialObjects.Any())
            {
                foreach (var celestialObject in celestialObjects)
                {
                    celestialObject.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == celestialObject.Id).ToList();
                }
                return Ok(celestialObjects);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(co => co.OrbitedObjectId == celestialObject.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var oldObject = _context.CelestialObjects.FirstOrDefault(co => co.Id == id);
            if (oldObject == null)
            {
                return NotFound();
            }
            else
            {
                oldObject.Name = celestialObject.Name;
                oldObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
                oldObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
                _context.CelestialObjects.Update(oldObject);
                _context.SaveChanges();
                return NoContent();
            }
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var oldObject = _context.CelestialObjects.FirstOrDefault(co => co.Id == id);
            if (oldObject == null)
            {
                return NotFound();
            }
            else
            {
                oldObject.Name = name;
                _context.CelestialObjects.Update(oldObject);
                _context.SaveChanges();
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var connetedObjects = _context.CelestialObjects.Where(co => co.Id == id ||co.OrbitedObjectId ==id).ToList();
            if (connetedObjects.Any())
            {
                _context.CelestialObjects.RemoveRange(connetedObjects);
                _context.SaveChanges();
                return NoContent();
            }
            return NotFound();
        }
    }
 }
