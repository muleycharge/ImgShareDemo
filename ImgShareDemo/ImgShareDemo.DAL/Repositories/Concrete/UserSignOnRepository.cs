using ImgShareDemo.BO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShareDemo.DAL.Repositories.Concrete
{
    internal class UserSignOnRepository : GenericRepository<UserSignOn>, IUserSignOnRepository
    {
        public UserSignOnRepository(ImgShareDemoContext context) : base(context) { }

        public UserSignOn GenerateNew(string returnUrl)
        {
            UserSignOn suo = new UserSignOn
            {
                State = Guid.NewGuid().ToString("N"),
                RedirectUri = returnUrl
            };
            Insert(suo);
            return suo;
        }
    }
}
