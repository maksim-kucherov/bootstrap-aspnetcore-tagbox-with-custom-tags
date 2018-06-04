# BootstrapTagBox for ASP.NET Core - How to use the BootstrapTagBoxValues class.
This example demonstrates how to use the [`BootstrapTagBoxValues<TKey>`](http://newdoc.devexpress.devx/ASPNETCoreBootstrap/DevExpress.AspNetCore.Bootstrap.BootstrapTagBoxValues-1?tabs=tabid-csharp) class with a Tag Box that allows custom tags.

## Preview
<p align="center">
  <img src="https://github.com/maksim-kucherov/bootstrap-aspnetcore-tagbox-with-custom-tags/blob/18.1.3+/Media/preview.gif?raw=true">
</p>

## Required steps:
1. Clone this repository.
2. Change the working directory to the ASP.NET Core project folder within the cloned repository.
```cmd
cd cs
```
3. Install npm packages.
```cmd
npm i
```
4. Build the project.
```cmd
dotnet build
```
5. Run the project.
```cmd
dotnet run
```

## The most important part of the example is devoted to how to manipulate the Tag Box's values on the controller side:
```cs
public class EditController : Controller
{
    public IActionResult Employee(int employeeID)
    {
        // Getting an Employee from database.
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
        // Getting custom tags(custom roles) filled into a TagBox control.
        List<string> newCustomRoles = employee.Roles.CustomTagTexts;

        if (DataContext.ValidateAndStoreNewRoles(newCustomRoles, HttpContext, out List<Role> newRoles))
        {
            // Getting selected predefined tag's ids.
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
```
