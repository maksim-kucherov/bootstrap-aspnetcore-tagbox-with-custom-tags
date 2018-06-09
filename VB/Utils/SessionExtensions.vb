Option Infer On

Imports Microsoft.AspNetCore.Http
Imports Newtonsoft.Json


Namespace TagBoxWithCustomTags
    Public Module SessionExtensions
        <System.Runtime.CompilerServices.Extension> _
        Public Sub [Set](Of T)(ByVal session As ISession, ByVal key As String, ByVal value As T)
            session.SetString(key, JsonConvert.SerializeObject(value))
        End Sub

        <System.Runtime.CompilerServices.Extension> _
        Public Function [Get](Of T)(ByVal session As ISession, ByVal key As String) As T
            Dim value = session.GetString(key)
            Return If(value Is Nothing, Nothing, JsonConvert.DeserializeObject(Of T)(value))
        End Function
    End Module
End Namespace