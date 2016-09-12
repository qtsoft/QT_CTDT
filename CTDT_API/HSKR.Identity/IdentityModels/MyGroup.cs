using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSKR.Identity.IdentityModels
{
    public class MyGroup
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<MyUserGroup> MyUsers { get; set; }
        public ICollection<MyGroupRole> MyRoles { get; set; }


        public MyGroup()
        {
            this.MyRoles = new List<MyGroupRole>();
            this.MyUsers = new List<MyUserGroup>();
        }
    }
}
