using cuentasctacte_web_api.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
namespace cuentasctacte_web_api.Helpers
{
    public class GetRole
    {
        public static List<string> GetRoles(ApplicationDbContext db, string email)
        {
            var query = db.Roles
                .Include(r => r.Users);

            List<string> results = new List<string>();
            foreach (var role in query)
            {
                foreach (var item in role.Users)
                {
                    var User = db.Users
                        .Where(u => u.Email.Equals(email))
                        .Where(u => u.UserName.Equals(email))
                        .Select(u => u.Id)
                        .FirstOrDefault();

                    if (User != null && User.Equals(item.UserId))
                    {
                        results.Add(role.Name);
                    }
                }

            }

            return results;
        }

        public static bool HasRole(ApplicationDbContext db, string email, string role)
        {
            List<string> Roles = GetRoles(db, email);
            if (Roles.Count(r => r.Equals(role)) > 0) return true;
            return false;
        }


    }
}


