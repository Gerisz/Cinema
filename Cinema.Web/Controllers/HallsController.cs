using Cinema.Data.Models.DTOs;
using Cinema.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cinema.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HallsController : ControllerBase
    {
        private readonly ICinemaService _service;

        public HallsController(ICinemaService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<HallIndex>> GetHalls()
        {
            return Ok(/*_service.GetHalls().Select(HallIndex.Projection)*/);
        }
    }
}
