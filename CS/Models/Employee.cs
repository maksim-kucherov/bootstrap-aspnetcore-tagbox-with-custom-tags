using System.Collections.Generic;

namespace TagBoxWithCustomTags.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public List<Role> Roles { get; set; }
        // ... another Employee's data fields.
    }
}
