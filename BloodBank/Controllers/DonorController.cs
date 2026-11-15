using BloodBank.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Controllers;

public class DonorController : Controller
{
    private readonly BloodBankDbContext _context;

    public DonorController(BloodBankDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var donors = await _context.Donors
            .AsNoTracking()
            .OrderBy(d => d.FullName)
            .ToListAsync();

        return View(donors);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(nameof(Donor.FullName), nameof(Donor.BloodGroup), nameof(Donor.ContactNo),
              nameof(Donor.City), nameof(Donor.LastDonationDate))] Donor donor)
    {
        if (!ModelState.IsValid)
            return View(donor);

        _context.Donors.Add(donor);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var donor = await _context.Donors.FindAsync(id);
        if (donor == null) return NotFound();
        return View(donor);
    }

    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditPost(int id)
    {
        // Load the existing row and copy over only the fields the form is allowed to change.
        // This prevents overposting: extra fields in the request are ignored.
        var donor = await _context.Donors.FindAsync(id);
        if (donor == null) return NotFound();

        var updated = await TryUpdateModelAsync(donor, prefix: "",
            d => d.FullName, d => d.BloodGroup, d => d.ContactNo,
            d => d.City, d => d.LastDonationDate);

        if (!updated || !ModelState.IsValid)
            return View(donor);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var donor = await _context.Donors.AsNoTracking().FirstOrDefaultAsync(d => d.DonorId == id);
        if (donor == null) return NotFound();
        return View(donor);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var donor = await _context.Donors.FindAsync(id);
        if (donor == null) return RedirectToAction(nameof(Index));

        // Donations reference the donor, so deleting a donor with history would violate the foreign key.
        if (await _context.Donations.AnyAsync(d => d.DonorId == id))
        {
            ModelState.AddModelError(string.Empty,
                "This donor has recorded donations and cannot be deleted.");
            return View("Delete", donor);
        }

        _context.Donors.Remove(donor);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
