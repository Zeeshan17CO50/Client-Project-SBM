﻿using Client.Application.Features.SubContractor.Dtos;
using Client_WebApp.Models;
using Client_WebApp.Services.Master;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Client_WebApp.Controllers.Master
{
    public class SubContractorController : BaseController
    {
        private readonly SubContractorService _service;

        public SubContractorController(SubContractorService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(string searchText = null)
        {
            try
            {
                var companyId = CurrentCompanyId;
                var subcontractors = await _service.GetAllSubContractorAsync(companyId);

                // Apply filtering by name if searchText is provided
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    subcontractors = subcontractors
                        .Where(s => s.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                var viewModel = new SubContractorViewModel
                {
                    CompanyId = companyId,
                    SubContractors = subcontractors
                };

                // Keep search text in ViewData so input retains value
                ViewData["searchText"] = searchText;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to load sub-contractors. " + ex.Message;
                return View(new SubContractorViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(SubContractor model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Validation failed. Please check the input fields.";
                return RedirectToAction("Index");
            }

            try
            {
                if (model.Id != null && model.Id > 0)
                {
                    // Update existing subcontractor
                    var updateDto = new UpdateSubContractorDto
                    {
                        Id = model.Id,
                        CompanyId = CurrentCompanyId,
                        Name = model.Name,
                        UpdatedBy = CurrentUserId
                    };

                    await _service.UpdateSubContractorAsync(updateDto);
                    TempData["SuccessMessage"] = "Sub-Contractor updated successfully!";
                }
                else
                {
                    // Create new subcontractor
                    var createDto = new CreateSubContractorDto
                    {
                        CompanyId = CurrentCompanyId,
                        Name = model.Name,
                        CreatedBy = CurrentUserId
                    };

                    await _service.CreateSubContractorAsync(createDto);
                    TempData["SuccessMessage"] = "Sub-Contractor added successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Operation failed. " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int companyId = CurrentCompanyId;
                int updatedBy = CurrentUserId;

                await _service.DeleteSubContractorAsync(id, updatedBy, companyId);
                TempData["SuccessMessage"] = "Sub-Contractor deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to delete sub-contractor. " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetSubcontractor(int id)
        {
            try
            {
                int companyId = CurrentCompanyId;

                // Call service to get subcontractor by id
                var subcontractors = await _service.GetAllSubContractorAsync(companyId, id);

                // Get the first item (should be only one)
                var sub = subcontractors.FirstOrDefault();

                if (sub == null)
                {
                    return NotFound(new { message = "Subcontractor not found." });
                }

                // Return JSON data for AJAX
                return Json(new
                {
                    id = sub.Id,
                    name = sub.Name
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to fetch subcontractor data. " + ex.Message });
            }
        }

    }
}
