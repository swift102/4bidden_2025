
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
//using Backend.Data;
using Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AlertsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAlerts()
    {
        var alerts = _context.Alerts.OrderByDescending(a => a.Timestamp).ToList();
        return Ok(alerts);
    }

    [HttpPost]
    public IActionResult CreateAlert(Alert alert)
    {
        var southAfricaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");
        alert.Timestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, southAfricaTimeZone);

        _context.Alerts.Add(alert);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetAlerts), new { id = alert.Id }, alert);
    }

}
