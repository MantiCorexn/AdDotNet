using BloodBank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Controllers;

public class ReportsController : Controller
{
    private readonly BloodBankDbContext _context;

    public ReportsController(BloodBankDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> FilterByBloodGroup(string? bloodGroup)
    {
        var donors = string.IsNullOrWhiteSpace(bloodGroup)
            ? new List<Donor>()
            : await _context.Donors
                .AsNoTracking()
                .Where(d => d.BloodGroup == bloodGroup)
                .OrderBy(d => d.FullName)
                .ToListAsync();

        ViewData["SelectedGroup"] = bloodGroup;
        return View(donors);
    }

    public async Task<IActionResult> SortByLastDonation()
    {
        var donors = await _context.Donors
            .AsNoTracking()
            .OrderByDescending(d => d.LastDonationDate)
            .ToListAsync();

        return View(donors);
    }

    public async Task<IActionResult> DonorDonationCounts()
    {
        var result = await _context.Donors
            .AsNoTracking()
            .Select(d => new DonorCountViewModel
            {
                FullName = d.FullName,
                DonationCount = d.Donations.Count()
            })
            .OrderByDescending(r => r.DonationCount)
            .ToListAsync();

        return View(result);
    }

    public async Task<IActionResult> TotalVolume()
    {
        var total = await _context.Donations.SumAsync(d => d.VolumeMl);
        ViewData["TotalVolume"] = total;
        return View();
    }
}
