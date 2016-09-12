using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace HSKR.Identity.Commons
{
    public class MyRoleManager: RoleManager<MyRole,long>
    {
        public MyRoleManager(IRoleStore<MyRole, long> roleStore)
            : base(roleStore)
        {
          
        }
    }
}
