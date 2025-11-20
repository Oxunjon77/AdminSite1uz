using DataAsseccLayer.Repostory.Interfase;
using DataAsseccLayer.Repostory;
using EntityLayer.Concreat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAsseccLayer.Concreat;


namespace DataAsseccLayer.EntityFramework
{
    public class EfImageFileDal : GenericRepostory<ImageFile>, IImageFileDal
    {

        public EfImageFileDal(AppDbContext ctx) : base(ctx)
        {
        }
    }
}
