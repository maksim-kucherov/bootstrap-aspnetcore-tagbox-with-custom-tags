# BootstrapTagBox for ASP.NET Core - How to use the BootstrapTagBoxValues class.
This example demonstrates how to use the [`BootstrapTagBoxValues<TKey>`](http://newdoc.devexpress.devx/ASPNETCoreBootstrap/DevExpress.AspNetCore.Bootstrap.BootstrapTagBoxValues-1?tabs=tabid-csharp) class when a TagBox control allows custom tags.

## Preview
![Preview](https://github.com/maksim-kucherov/bootstrap-aspnetcore-tagbox-with-custom-tags/blob/18.1.3+/Media/preview.gif?raw=true)

## How to run
1. Clone this repository.
2. Set the current directory in the cloned repository.
```cmd
cd cs\tagbox-with-custom-tags\
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

## The most important part of the example is setting and getting the TagBox' values on a controller:
```cs
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
```
