using ImgShareDemo.BO.Entities;
using ImgShareDemo.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShareDemo.DAL.Repositories.Concrete
{
    internal class LinkedInUserRepository : GenericRepository<LinkedInUser>, ILinkedInUserRepository
    {
        public LinkedInUserRepository(ImgShareDemoContext context) : base(context) { }
    }
}
