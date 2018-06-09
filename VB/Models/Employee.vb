Imports System.Collections.Generic

Namespace TagBoxWithCustomTags.Models
    Public Class Employee
        Public Property Id() As Integer
        Public Property Roles() As List(Of Role)
        ' ... another Employee's data fields.
    End Class
End Namespace
