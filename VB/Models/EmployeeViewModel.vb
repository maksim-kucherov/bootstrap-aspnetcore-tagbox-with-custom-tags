Imports System.Collections.Generic
Imports DevExpress.AspNetCore.Bootstrap

Namespace TagBoxWithCustomTags.Models
    Public Class EmployeeViewModel
        Public Property Id() As Integer

        ' Selected TagBox' values (both predefined and custom). 
        Public Property Roles() As BootstrapTagBoxValues(Of Integer)
        Public Property AllRoles() As IEnumerable(Of Role)
    End Class
End Namespace
