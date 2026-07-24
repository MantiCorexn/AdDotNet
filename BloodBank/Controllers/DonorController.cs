using Microsoft.AspNetCore.Mvc;
using BloodBank.Models;

namespace BloodBank.Controllers;

public class DonorController : Controller
{
    private readonly BloodBankDbContext _context;

    public DonorController(BloodBankDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var donors = _context.Donors.ToList();
        return View(donors);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Donor donor)
    {
        if (string.IsNullOrWhiteSpace(donor.FullName))
            ModelState.AddModelError("FullName", "Full name is required.");

        if (string.IsNullOrWhiteSpace(donor.BloodGroup))
            ModelState.AddModelError("BloodGroup", "Blood group is required.");

        if (!ModelState.IsValid)
            return View(donor);

        _context.Donors.Add(donor);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var donor = _context.Donors.Find(id);
        if (donor == null) return NotFound();
        return View(donor);
    }

    [HttpPost]
    public IActionResult Edit(int id, Donor donor)
    {
        if (id != donor.DonorId) return NotFound();

        if (string.IsNullOrWhiteSpace(donor.FullName))
            ModelState.AddModelError("FullName", "Full name is required.");

        if (string.IsNullOrWhiteSpace(donor.BloodGroup))
            ModelState.AddModelError("BloodGroup", "Blood group is required.");

        if (!ModelState.IsValid)
            return View(donor);

        _context.Donors.Update(donor);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var donor = _context.Donors.Find(id);
        if (donor == null) return NotFound();
        return View(donor);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var donor = _context.Donors.Find(id);
        if (donor != null)
        {
            _context.Donors.Remove(donor);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}