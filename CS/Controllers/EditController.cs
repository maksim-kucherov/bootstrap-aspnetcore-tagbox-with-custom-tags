using System.Collections.Generic;
using System.Linq;
using DevExpress.AspNetCore.Bootstrap;
using Microsoft.AspNetCore.Mvc;
using TagBoxWithCustomTags.Models;

namespace TagBoxWithCustomTags.Controllers
{
    public class EditController : Controller
    {
        public IActionResult Employee(int employeeID)
        {
            // Getting an Employee from database;
            Employee employeeDataObject = DataContext.GetEmployees(HttpContext)
                .First(e => e.Id == employeeID);

            // Create and fill a view model with data obtained from database.
            EmployeeViewModel model = new EmployeeViewModel()
            {
                Id = employeeDataObject.Id,
                Roles = new BootstrapTagBoxValues<int>()
                {
                    TagValues = employeeDataObject.Roles
                        .Select(r => r.Id).ToList()
                }
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Employee(EmployeeViewModel employee)
        {
            // Getting custom tags(custom roles) filled into a TagBox control
            List<string> newCustomRoles = employee.Roles.CustomTagTexts;

            if (DataContext.ValidateAndStoreNewRoles(newCustomRoles, HttpContext, out List<Role> newRoles))
            {
                // Getting selected predefined tag's ids
                List<int> selectedPredefinedRolesIds = employee.Roles.TagValues;

                List<Role> employeeRoles = DataContext.GetRoles(HttpContext)
                    .Where(r => selectedPredefinedRolesIds.Contains(r.Id)).ToList();
                employeeRoles.AddRange(newRoles);

                Employee employeeDataObject = DataContext.GetEmployees(HttpContext)
                    .First(e => e.Id == employee.Id);
                employeeDataObject.Roles = employeeRoles;

                DataContext.UpdateEmployee(employeeDataObject, HttpContext);
                TempData["Success"] = $"The Employee #{ employee.Id } has been updated";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Some roles are incorrect. Please fix them and submit a form again.";
            return View(employee);
        }
    }
}