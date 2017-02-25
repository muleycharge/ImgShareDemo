using ImgShareDemo.BO.Entities;
using ImgShareDemo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShareDemo.DAL.Repositories.Concrete
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ImgShareDemoContext context) : base(context) { }
    }
}
