using System.Web;
using HSKR.Identity.IdentityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace HSKR.Identity.Commons
{
    public class MyGroupManager
    {
       
        private GroupStore _groupStore;
        private MyDbContext _db;
        private MyUserManager _userManager;
        private MyRoleManager _roleManager;

        public MyGroupManager()
        {
            _db = HttpContext.Current.GetOwinContext().Get<MyDbContext>();
            _userManager = HttpContext.Current.GetOwinContext().GetUserManager<MyUserManager>();
            _roleManager = HttpContext.Current.GetOwinContext().Get<MyRoleManager>();
            _groupStore = new GroupStore(_db);
        }


        public IQueryable<MyGroup> Groups
        {
            get
            {
                return _groupStore.Groups;
            }
        }


        public async Task<IdentityResult> CreateGroupAsync(MyGroup group)
        {
            await _groupStore.CreateAsync(group);
            return IdentityResult.Success;
        }


        public IdentityResult CreateGroup(MyGroup group)
        {
            _groupStore.Create(group);
            return IdentityResult.Success;
        }


        public IdentityResult SetGroupRoles(long groupId, params string[] roleNames)
        {
            // Clear all the roles associated with this group:
            var thisGroup = this.FindById(groupId);
            thisGroup.MyRoles.Clear();
            _db.SaveChanges();

            // Add the new roles passed in:
            var newRoles = _roleManager.Roles.Where(r => roleNames.Any(n => n == r.Name));
            foreach (var role in newRoles)
            {
                thisGroup.MyRoles.Add(new MyGroupRole { GroupId = groupId, RoleId = role.Id });
            }
            _db.SaveChanges();

            // Reset the roles for all affected users:
            foreach (var groupUser in thisGroup.MyUsers)
            {
                this.RefreshUserGroupRoles(groupUser.UserId);
            }
            return IdentityResult.Success;
        }


        public async Task<IdentityResult> SetGroupRolesAsync(long groupId, params string[] roleNames)
        {
            // Clear all the roles associated with this group:
            var thisGroup = await this.FindByIdAsync(groupId);
            thisGroup.MyRoles.Clear();
            await _db.SaveChangesAsync();

            // Add the new roles passed in:
            var newRoles = _roleManager.Roles.Where(r => roleNames.Any(n => n == r.Name));
            foreach (var role in newRoles)
            {
                thisGroup.MyRoles.Add(new MyGroupRole { GroupId = groupId, RoleId = role.Id });
            }
            await _db.SaveChangesAsync();

            // Reset the roles for all affected users:
            foreach (var groupUser in thisGroup.MyUsers)
            {
                await this.RefreshUserGroupRolesAsync(groupUser.UserId);
            }
            return IdentityResult.Success;
        }


        public async Task<IdentityResult> SetUserGroupsAsync(long userId, params long[] groupIds)
        {
            // Clear current group membership:
            var currentGroups = await this.GetUserGroupsAsync(userId);
            foreach (var group in currentGroups)
            {
                group.MyUsers
                    .Remove(group.MyUsers.FirstOrDefault(gr => gr.UserId == userId));
            }
            await _db.SaveChangesAsync();

            // Add the user to the new groups:
            foreach (long groupId in groupIds)
            {
                var newGroup = await this.FindByIdAsync(groupId);
                newGroup.MyUsers.Add(new MyUserGroup { UserId = userId, GroupId = groupId });
            }
            await _db.SaveChangesAsync();

            await this.RefreshUserGroupRolesAsync(userId);
            return IdentityResult.Success;
        }


        public IdentityResult SetUserGroups(long userId, params long[] groupIds)
        {
            // Clear current group membership:
            var currentGroups = this.GetUserGroups(userId);
            foreach (var group in currentGroups)
            {
                group.MyUsers
                    .Remove(group.MyUsers.FirstOrDefault(gr => gr.UserId == userId));
            }
            _db.SaveChanges();

            // Add the user to the new groups:
            foreach (long groupId in groupIds)
            {
                var newGroup = this.FindById(groupId);
                newGroup.MyUsers.Add(new MyUserGroup { UserId = userId, GroupId = groupId });
            }
            _db.SaveChanges();

            this.RefreshUserGroupRoles(userId);
            return IdentityResult.Success;
        }


        public IdentityResult RefreshUserGroupRoles(long userId)
        {
            var user = _userManager.FindById(userId);
            if (user == null)
            {
                throw new ArgumentNullException("User");
            }
            // Remove user from previous roles:
            var oldUserRoles = _userManager.GetRoles(userId);
            if (oldUserRoles.Count > 0)
            {
               // _userManager.RemoveFromRoles(userId, oldUserRoles.ToArray());
            }

            // Find teh roles this user is entitled to from group membership:
            var newGroupRoles = this.GetUserGroupRoles(userId);

            // Get the damn role names:
            var allRoles = _roleManager.Roles.ToList();
            var addTheseRoles = allRoles.Where(r => newGroupRoles.Any(gr => gr.RoleId == r.Id));
            var roleNames = addTheseRoles.Select(n => n.Name).ToArray();

            // Add the user to the proper roles
            //_userManager.AddToRoles(userId, roleNames);
            
            return IdentityResult.Success;
        }


        public async Task<IdentityResult> RefreshUserGroupRolesAsync(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentNullException("User");
            }
            // Remove user from previous roles:
            var oldUserRoles = await _userManager.GetRolesAsync(userId);
            if (oldUserRoles.Count > 0)
            {
                //await _userManager.RemoveFromRolesAsync(userId, oldUserRoles.ToArray());
            }

            // Find the roles this user is entitled to from group membership:
            var newGroupRoles = await this.GetUserGroupRolesAsync(userId);

            // Get the damn role names:
            var allRoles = await _roleManager.Roles.ToListAsync();
            var addTheseRoles = allRoles.Where(r => newGroupRoles.Any(gr => gr.RoleId == r.Id));
            var roleNames = addTheseRoles.Select(n => n.Name).ToArray();

            // Add the user to the proper roles
            //await _userManager.AddToRolesAsync(userId, roleNames);

            return IdentityResult.Success;
        }


        public async Task<IdentityResult> DeleteGroupAsync(long groupId)
        {
            var group = await this.FindByIdAsync(groupId);
            if (group == null)
            {
                throw new ArgumentNullException("User");
            }

            var currentGroupMembers = (await this.GetGroupUsersAsync(groupId)).ToList();
            // remove the roles from the group:
            group.MyRoles.Clear();

            // Remove all the users:
            group.MyUsers.Clear();

            // Remove the group itself:
            _db.Groups.Remove(group);

            await _db.SaveChangesAsync();

            // Reset all the user roles:
            foreach (var user in currentGroupMembers)
            {
                await this.RefreshUserGroupRolesAsync(user.Id);
            }
            return IdentityResult.Success;
        }


        public IdentityResult DeleteGroup(long groupId)
        {
            var group = this.FindById(groupId);
            if (group == null)
            {
                throw new ArgumentNullException("User");
            }

            var currentGroupMembers = this.GetGroupUsers(groupId).ToList();
            // remove the roles from the group:
            group.MyRoles.Clear();

            // Remove all the users:
            group.MyUsers.Clear();

            // Remove the group itself:
            _db.Groups.Remove(group);

            _db.SaveChanges();

            // Reset all the user roles:
            foreach (var user in currentGroupMembers)
            {
                this.RefreshUserGroupRoles(user.Id);
            }
            return IdentityResult.Success;
        }


        public async Task<IdentityResult> UpdateGroupAsync(MyGroup group)
        {
            await _groupStore.UpdateAsync(group);
            foreach (var groupUser in group.MyUsers)
            {
                await this.RefreshUserGroupRolesAsync(groupUser.UserId);
            }
            return IdentityResult.Success;
        }


        public IdentityResult UpdateGroup(MyGroup group)
        {
            _groupStore.Update(group);
            foreach (var groupUser in group.MyUsers)
            {
                this.RefreshUserGroupRoles(groupUser.UserId);
            }
            return IdentityResult.Success;
        }


        public IdentityResult ClearUserGroups(long userId)
        {
            return this.SetUserGroups(userId, new long[] { });
        }


        public async Task<IdentityResult> ClearUserGroupsAsync(long userId)
        {
            return await this.SetUserGroupsAsync(userId, new long[] { });
        }


        public async Task<IEnumerable<MyGroup>> GetUserGroupsAsync(long userId)
        {
            var result = new List<MyGroup>();
            var userGroups = (from g in this.Groups
                              where g.MyUsers.Any(u => u.UserId == userId)
                              select g).ToListAsync();
            return await userGroups;
        }


        public IEnumerable<MyGroup> GetUserGroups(long userId)
        {
            var result = new List<MyGroup>();
            var userGroups = (from g in this.Groups
                              where g.MyUsers.Any(u => u.UserId == userId)
                              select g).ToList();
            return userGroups;
        }


        public async Task<IEnumerable<MyRole>> GetGroupRolesAsync(long groupId)
        {
            var grp = await _db.Groups.FirstOrDefaultAsync(g => g.Id == groupId);
            var roles = await _roleManager.Roles.ToListAsync();
            var groupRoles = (from r in roles
                              where grp.MyRoles.Any(ap => ap.RoleId == r.Id)
                              select r).ToList();
            return groupRoles;
        }


        public IEnumerable<MyRole> GetGroupRoles(long groupId)
        {
            var grp = _db.Groups.FirstOrDefault(g => g.Id == groupId);
            var roles = _roleManager.Roles.ToList();
            var groupRoles = from r in roles
                             where grp.MyRoles.Any(ap => ap.RoleId == r.Id)
                             select r;
            return groupRoles;
        }


        public IEnumerable<MyUser> GetGroupUsers(long groupId)
        {
            var group = this.FindById(groupId);
            var users = new List<MyUser>();
            foreach (var groupUser in group.MyUsers)
            {
                var user = _db.Users.Find(groupUser.UserId);
                users.Add(user);
            }
            return users;
        }


        public async Task<IEnumerable<MyUser>> GetGroupUsersAsync(long groupId)
        {
            var group = await this.FindByIdAsync(groupId);
            var users = new List<MyUser>();
            foreach (var groupUser in group.MyUsers)
            {
                var user =  _db.Users.SingleOrDefault(u => u.Id == groupUser.UserId);
                users.Add(user);
            }
            return users;
        }


        public IEnumerable<MyGroupRole> GetUserGroupRoles(long userId)
        {
            var userGroups = this.GetUserGroups(userId);
            var userGroupRoles = new List<MyGroupRole>();
            foreach (var group in userGroups)
            {
                userGroupRoles.AddRange(group.MyRoles.ToArray());
            }
            return userGroupRoles;
        }


        public async Task<IEnumerable<MyGroupRole>> GetUserGroupRolesAsync(long userId)
        {
            var userGroups = await this.GetUserGroupsAsync(userId);
            var userGroupRoles = new List<MyGroupRole>();
            foreach (var group in userGroups)
            {
                userGroupRoles.AddRange(group.MyRoles.ToArray());
            }
            return userGroupRoles;
        }


        public async Task<MyGroup> FindByIdAsync(long id)
        {
            return await _groupStore.FindByIdAsync(id);
        }


        public MyGroup FindById(long id)
        {
            return _groupStore.FindById(id);
        }
    }
}
