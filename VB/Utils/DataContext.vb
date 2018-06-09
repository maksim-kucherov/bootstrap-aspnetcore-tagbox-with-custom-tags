Option Infer On

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.AspNetCore.Http
Imports TagBoxWithCustomTags.Models

Namespace TagBoxWithCustomTags
    ' This class simulates working with a database
    Public NotInheritable Class DataContext

        Private Sub New()
        End Sub
        Private Const RolesSessionKey As String = "25618B68-E085-4ECD-947D-BD3C8EC65477", EmployeesSessionKey As String = "A9E11C20-9121-4C77-880F-02C1C579A84E"

        Public Shared Function GetEmployees(ByVal httpContext As HttpContext) As IEnumerable(Of Employee)
            Return GetEmployeesFromSession(httpContext)
        End Function
        Public Shared Sub UpdateEmployee(ByVal employee As Employee, ByVal httpContext As HttpContext)
            Dim employees = GetEmployeesFromSession(httpContext)
            Dim storedEmployee = employees.First(Function(e) e.Id = employee.Id)
            storedEmployee.Roles = employee.Roles
            httpContext.Session.Set(EmployeesSessionKey, employees)
        End Sub
        Public Shared Function GetRoles(ByVal httpContext As HttpContext) As IEnumerable(Of Role)
            Return GetRolesFromSession(httpContext)
        End Function
        Public Shared Function ValidateAndStoreNewRoles(ByVal newCustomRoles As List(Of String), ByVal httpContext As HttpContext, <System.Runtime.InteropServices.Out()> ByRef newRolesIds As List(Of Role)) As Boolean
            newRolesIds = New List(Of Role)()
            If newCustomRoles.Any(Function(r) r.ToLower() = "admin") Then
                Return False
            End If

            For Each role In newCustomRoles
                newRolesIds.Add(InsertRole(role, httpContext))
            Next role
            Return True
        End Function

        Private Shared Function GetEmployeesFromSession(ByVal httpContext As HttpContext) As IEnumerable(Of Employee)
            Dim session = httpContext.Session
            If session.GetString(EmployeesSessionKey) Is Nothing Then
                session.Set(EmployeesSessionKey, GenerateEmployees(httpContext))
            End If
            Return session.Get(Of IEnumerable(Of Employee))(EmployeesSessionKey)
        End Function
        Private Shared Function GenerateEmployees(ByVal httpContext As HttpContext) As IEnumerable(Of Employee)
            Dim roles = GetRolesFromSession(httpContext)
            Return Enumerable.Range(0, roles.Count()).Select(Function(i) New Employee() With { _
                .Id = i, _
                .Roles = New List(Of Role)(roles.Take(i + 1)) _
            })
        End Function

        Private Shared Function InsertRole(ByVal roleText As String, ByVal httpContext As HttpContext) As Role
            Dim roles = GetRolesFromSession(httpContext).ToList()
            Dim newId = roles.Max(Function(r) r.Id) + 1
            Dim newRole = New Role() With { _
                .Id = newId, _
                .Text = roleText _
            }
            roles.Add(newRole)
            httpContext.Session.Set(RolesSessionKey, roles)
            Return newRole
        End Function
        Private Shared Function GetRolesFromSession(ByVal httpContext As HttpContext) As IEnumerable(Of Role)
            Dim session = httpContext.Session
            If session.GetString(RolesSessionKey) Is Nothing Then
                session.Set(RolesSessionKey, GenerateRoles())
            End If
            Return session.Get(Of IEnumerable(Of Role))(RolesSessionKey)
        End Function
        Private Shared Function GenerateRoles() As IEnumerable(Of Role)
            Return New List(Of Role)() From { _
                New Role() With { _
                    .Id = 0, _
                    .Text = "Developer" _
                }, _
                New Role() With { _
                    .Id = 1, _
                    .Text = "Support Developer" _
                }, _
                New Role() With { _
                    .Id = 2, _
                    .Text = "Team Leader" _
                } _
            }
        End Function
    End Class
End Namespace
