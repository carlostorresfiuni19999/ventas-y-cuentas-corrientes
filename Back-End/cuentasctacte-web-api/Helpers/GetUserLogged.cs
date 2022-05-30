using cuentasctacte_web_api.Models;
using cuentasctacte_web_api.Models.Entities;
using System.Data;
using System.Linq;

namespace cuentasctacte_web_api.Helpers
{
    public class GetUserLogged
    {
        public static Persona GetUser(ApplicationDbContext db,
             string Id)
        {
            var user = db.Users
                .FirstOrDefault(u => u.Id.Equals(Id));

            return db.Personas
                .Where(p => !p.Deleted)
                .FirstOrDefault(p => p.UserName.Equals(user.Email));
        }
    }
}
