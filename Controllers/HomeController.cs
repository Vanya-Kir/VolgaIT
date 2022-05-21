using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VolgaIT.Data;
using VolgaIT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VolgaIT.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult Index()
    {
        ViewBag.Apps = _context.Application.Where(a => a.UserId == HttpContext.User.Identity.Name).Count();
        return View();
    }


    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Application application)
    {
        if (application.Name.Trim() != "" || application.Name != null)
        {
            try
            {
                application.Date = DateTime.UtcNow;
                application.UserId = HttpContext.User.Identity.Name;
                _context.Application.Add(application);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        // ModelState.AddModelError(string.Empty, "Some error");
        return View(application);
    }

    [Authorize]
    public IActionResult AppList()
    {
        return View(_context.Application.Where(i => i.UserId == HttpContext.User.Identity.Name).OrderBy(i => i.Name).ToList());
    }

    [Authorize]
    public IActionResult EventList(int id)
    {
        ViewBag.AppId = id;
        if (_context.Application.Find(id).UserId == HttpContext.User.Identity.Name)
        {
            return View(_context.Event.Where(i => i.AppId == id).OrderBy(i => i.Date).ToList());
        }
        return RedirectToAction("AppList");
    }

    [HttpGet]
    [Authorize]
    public IActionResult EventAdd()
    {
        List<SelectListItem> items = new List<SelectListItem>();
        var appList = _context.Application.Where(i => i.UserId == HttpContext.User.Identity.Name).ToList();
        foreach (var a in appList)
        {
            items.Add(new SelectListItem { Text = a.Name, Value = a.Id.ToString() });
        }
        ViewBag.AppId = items;
        return View();
    }

    [HttpPost]
    [Authorize]
    public IActionResult EventAdd(Event _event)
    {

        if (_context.Application.Find(_event.AppId).UserId == HttpContext.User.Identity.Name)
        {
            if (_event.Date == DateTime.MinValue)
            {
                _event.Date = DateTime.UtcNow;
            }
            else
            {
                _event.Date = _event.Date.ToUniversalTime();
            }

            try
            {
                _context.Event.Add(_event);
                _context.SaveChanges();
                return RedirectToAction("EventList", new { id = _event.AppId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        ModelState.AddModelError(string.Empty, "Some error");
        return View(_event);
    }

    [Authorize]
    public IActionResult Statistic()
    {
        var application = _context.Application.ToList();
        return View(application);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
