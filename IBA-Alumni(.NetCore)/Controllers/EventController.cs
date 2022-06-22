using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IBA_Alumni_.NetCore_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly DataContext _context;
        public EventController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Event>>> GetEvents()
        {
            return Ok(await _context.Events.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<Event>>> CreateEvents(Event events)
        {
            _context.Events.Add(events);
            await _context.SaveChangesAsync();

            return Ok(events);
        }

        [HttpPut]
        public async Task<ActionResult<List<Event>>> UpdateEvents(Event events)
        {
            var dbEvent = await _context.Events.FindAsync(events.Id);
            if (dbEvent == null)
            {
                return BadRequest("Events not Found");
            }
            else
            {
                dbEvent.EventHeading = events.EventHeading;
                dbEvent.EventDescription = events.EventDescription;
                dbEvent.EventLocation = events.EventLocation;
                dbEvent.EventDate = events.EventDate;
                dbEvent.EventTime = events.EventTime;

                await _context.SaveChangesAsync();
            }
            return Ok(await _context.Events.ToListAsync());
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Event>>> DeleteEvents(int id)
        {
            var dbEvent = await _context.Events.FindAsync(id);
            if(dbEvent == null)
            {
                return BadRequest("Events not Found");
            }
            else
            {
                _context.Events.Remove(dbEvent);
                await _context.SaveChangesAsync();
            }
            return Ok(await _context.Events.ToListAsync());
        }
    }
}
