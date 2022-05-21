using Microsoft.AspNetCore.Mvc;
using VolgaIT.Data;
using VolgaIT.Models;
using Newtonsoft.Json;

namespace VolgaIT.Controllers;

//контроллер, отвечающий за отображение графиков
public class ChartController : Controller
{
    private readonly ILogger<ChartController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChartController(ILogger<ChartController> logger, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public JsonResult Week(int appId)
    {
        var sevenDays = DateTime.UtcNow.AddDays(-7);
        var query = (_context.Event
               .Where(i => i.AppId == appId && i.Date >= sevenDays)
               .GroupBy(o => o.Date.DayOfWeek)
               .Select(grp => new { key = grp.Key, cnt = grp.Count() })).ToList();


        var chart = new ChartData()
        {
            data = new List<Int32>(),
            fill = false,
            borderColor = "red"
        };

        var dataLabel = new DataLabel()
        {
            labels = new List<String>(),
            datasets = new List<ChartData>() { chart }
        };

        foreach (var item in query)
        {
            dataLabel.labels.Add(item.key.ToString());
            chart.data.Add(item.cnt);
        }

        var answer = new ChartList()
        {
            type = "line",
            data = dataLabel
        };

        var t = JsonConvert.SerializeObject(answer);
        return Json(t);
    }

    public JsonResult Month(int appId)
    {
        var monthDays = DateTime.UtcNow.AddMonths(-1);
        var query = (_context.Event
               .Where(i => i.AppId == appId && i.Date >= monthDays)
               .GroupBy(o => o.Date.Day)
               .Select(grp => new { key = grp.Key, cnt = grp.Count() })).ToList();


        var chart = new ChartData()
        {
            data = new List<Int32>(),
            fill = false,
            borderColor = "yellow"
        };

        var dataLabel = new DataLabel()
        {
            labels = new List<String>(),
            datasets = new List<ChartData>() { chart }
        };

        foreach (var item in query)
        {
            dataLabel.labels.Add(item.key.ToString());
            chart.data.Add(item.cnt);
        }

        var answer = new ChartList()
        {
            type = "line",
            data = dataLabel
        };

        var t = JsonConvert.SerializeObject(answer);
        return Json(t);
    }

    public JsonResult Year(int appId = 1)
    {
        var query = (_context.Event
                    .Where(i => i.AppId == appId)
                    .GroupBy(o => o.Date.Year)
                    .Select(grp => new { key = grp.Key, cnt = grp.Count() })).ToList();
        var chart = new ChartData()
        {
            data = new List<Int32>(),
            fill = false,
            borderColor = "blue"
        };

        var dataLabel = new DataLabel()
        {
            labels = new List<String>(),
            datasets = new List<ChartData>() { chart }
        };

        foreach (var item in query)
        {
            dataLabel.labels.Add(item.key.ToString());
            chart.data.Add(item.cnt);
        }

        var answer = new ChartList()
        {
            type = "line",
            data = dataLabel
        };

        var t = JsonConvert.SerializeObject(answer);
        return Json(t);
    }
}
