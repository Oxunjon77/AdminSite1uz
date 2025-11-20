using DataAsseccLayer.Concreat;
using DataAsseccLayer.Repostory;
using DataAsseccLayer.Repostory.Interfase;
using EntityLayer.Concreat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAsseccLayer.EntityFramework
{
    public class EfAboutDl : GenericRepostory<About>,IAboutDl
    {
        public EfAboutDl(AppDbContext ctx) : base(ctx)
        {
        }
    }
}
