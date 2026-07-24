using Microsoft.AspNetCore.Mvc;
using BloodBank.Models;

namespace BloodBank.Controllers;

public class ReportsController : Controller
{
    private readonly BloodBankDbContext _context;

    public ReportsController(BloodBankDbContext context)
    {
        _context = context;
    }

    public IActionResult FilterByBloodGroup(string bloodGroup)
    {
        var donors = string.IsNullOrWhiteSpace(bloodGroup)
            ? new List<Donor>()
            : _context.Donors.Where(d => d.BloodGroup == bloodGroup).ToList();

        ViewData["SelectedGroup"] = bloodGroup;
        return View(donors);
    }

    public IActionResult SortByLastDonation()
    {
        var donors = _context.Donors
            .OrderByDescending(d => d.LastDonationDate)
            .ToList();

        return View(donors);
    }

    public IActionResult DonorDonationCounts()
    {
        var result = _context.Donors
            .Select(d => new DonorCountViewModel
            {
                FullName = d.FullName,
                DonationCount = d.Donations.Count()
            })
            .ToList();

        return View(result);
    }

    public IActionResult TotalVolume()
    {
        var total = _context.Donations.Sum(d => d.VolumeMl);
        ViewData["TotalVolume"] = total;
        return View();
    }
}