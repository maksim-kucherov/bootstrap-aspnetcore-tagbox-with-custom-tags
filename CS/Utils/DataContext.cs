using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TagBoxWithCustomTags.Models;

namespace TagBoxWithCustomTags
{
    // This class simulates working with a database
    public static class DataContext
    {
        const string
            RolesSessionKey = "25618B68-E085-4ECD-947D-BD3C8EC65477",
            EmployeesSessionKey = "A9E11C20-9121-4C77-880F-02C1C579A84E";

        public static IEnumerable<Employee> GetEmployees(HttpContext httpContext)
        {
            return GetEmployeesFromSession(httpContext);
        }
        public static void UpdateEmployee(Employee employee, HttpContext httpContext)
        {
            var employees = GetEmployeesFromSession(httpContext);
            var storedEmployee = employees.First(e => e.Id == employee.Id);
            storedEmployee.Roles = employee.Roles;
            httpContext.Session.Set(EmployeesSessionKey, employees);
        }
        public static IEnumerable<Role> GetRoles(HttpContext httpContext)
        {
            return GetRolesFromSession(httpContext);
        }
        public static bool ValidateAndStoreNewRoles(List<string> newCustomRoles, HttpContext httpContext, out List<Role> newRolesIds)
        {
            newRolesIds = new List<Role>();
            if (newCustomRoles.Any(r => r.ToLower() == "admin"))
                return false;

            foreach (var role in newCustomRoles)
            {
                newRolesIds.Add(InsertRole(role, httpContext));
            }
            return true;
        }

        static IEnumerable<Employee> GetEmployeesFromSession(HttpContext httpContext)
        {
            var session = httpContext.Session;
            if (session.GetString(EmployeesSessionKey) == null)
                session.Set(EmployeesSessionKey, GenerateEmployees(httpContext));
            return session.Get<IEnumerable<Employee>>(EmployeesSessionKey);
        }
        static IEnumerable<Employee> GenerateEmployees(HttpContext httpContext)
        {
            var roles = GetRolesFromSession(httpContext);
            return Enumerable.Range(0, roles.Count()).Select(i => new Employee()
            {
                Id = i,
                Roles = new List<Role>(roles.Take(i + 1))
            });
        }

        static Role InsertRole(string roleText, HttpContext httpContext)
        {
            var roles = GetRolesFromSession(httpContext).ToList();
            var newId = roles.Max(r => r.Id) + 1;
            var newRole = new Role() { Id = newId, Text = roleText };
            roles.Add(newRole);
            httpContext.Session.Set(RolesSessionKey, roles);
            return newRole;
        }
        static IEnumerable<Role> GetRolesFromSession(HttpContext httpContext)
        {
            var session = httpContext.Session;
            if (session.GetString(RolesSessionKey) == null)
                session.Set(RolesSessionKey, GenerateRoles());
            return session.Get<IEnumerable<Role>>(RolesSessionKey);
        }
        static IEnumerable<Role> GenerateRoles()
        {
            return new List<Role>() {
                new Role() { Id = 0, Text = "Developer" },
                new Role() { Id = 1, Text = "Support Developer" },
                new Role() { Id = 2, Text = "Team Leader" }
            };
        }
    }
}
