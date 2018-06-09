Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.AspNetCore.Bootstrap
Imports Microsoft.AspNetCore.Mvc
Imports TagBoxWithCustomTags.Models

Namespace TagBoxWithCustomTags.Controllers
    Public Class EditController
        Inherits Controller

        Public Function Employee(ByVal employeeID As Integer) As IActionResult
            ' Getting an Employee from database;
            Dim employeeDataObject As Employee = DataContext.GetEmployees(HttpContext).First(Function(e) e.Id = employeeID)

            ' Create and fill a view model with data obtained from database.
            Dim model As New EmployeeViewModel() With { _
                .Id = employeeDataObject.Id, _
                .Roles = New BootstrapTagBoxValues(Of Integer)() With {.TagValues = employeeDataObject.Roles.Select(Function(r) r.Id).ToList()} _
            }
            Return View(model)
        End Function


        <HttpPost> _
        Public Function Employee(ByVal employee_Renamed As EmployeeViewModel) As IActionResult
            ' Getting custom tags(custom roles) filled into a TagBox control
            Dim newCustomRoles As List(Of String) = employee_Renamed.Roles.CustomTagTexts

            Dim newRoles As List(Of Role)
            If DataContext.ValidateAndStoreNewRoles(newCustomRoles, HttpContext, newRoles) Then
                ' Getting selected predefined tag's ids
                Dim selectedPredefinedRolesIds As List(Of Integer) = employee_Renamed.Roles.TagValues

                Dim employeeRoles As List(Of Role) = DataContext.GetRoles(HttpContext).Where(Function(r) selectedPredefinedRolesIds.Contains(r.Id)).ToList()
                employeeRoles.AddRange(newRoles)

                Dim employeeDataObject As Employee = DataContext.GetEmployees(HttpContext).First(Function(e) e.Id = employee_Renamed.Id)
                employeeDataObject.Roles = employeeRoles

                DataContext.UpdateEmployee(employeeDataObject, HttpContext)
                TempData("Success") = $"The Employee #{employee_Renamed.Id} has been updated"
                Return RedirectToAction("Index", "Home")
            End If

            TempData("Error") = "Some roles are incorrect. Please fix them and submit a form again."
            Return View(employee_Renamed)
        End Function
    End Class
End Namespace