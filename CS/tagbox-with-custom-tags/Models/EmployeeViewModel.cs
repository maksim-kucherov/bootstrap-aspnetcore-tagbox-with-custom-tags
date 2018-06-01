using System.Collections.Generic;
using DevExpress.AspNetCore.Bootstrap;

namespace TagBoxWithCustomTags.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        // Selected TagBox' values (both predefined and custom). 
        public BootstrapTagBoxValues<int> Roles { get; set; }
        public IEnumerable<Role> AllRoles { get; set; }
    }
}
