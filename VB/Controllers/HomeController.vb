Imports Microsoft.AspNetCore.Mvc

Namespace TagBoxWithCustomTags.Controllers
    Public Class HomeController
        Inherits Controller

        Public Function Index() As IActionResult
            Return View(DataContext.GetEmployees(HttpContext))
        End Function
    End Class
End Namespace
