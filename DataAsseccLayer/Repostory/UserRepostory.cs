using DataAsseccLayer.Concreat;
using DataAsseccLayer.Repostory.Interfase;
using EntityLayer.Concreat;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAsseccLayer.Repostory
{
    public class UserRepostory : IUserDl
    {
        private readonly AppDbContext dbContext;
        private readonly DbSet<Users> _object;

        public UserRepostory(AppDbContext context)
        {
            dbContext = context;
            _object = dbContext.Set<Users>();
        }

        public void Delete(Users p)
        {
            _object.Remove(p);
            dbContext.SaveChanges();
        }

        public Users Get(Expression<Func<Users, bool>> filter)
        {
            return _object.SingleOrDefault(filter);
        }

        public List<Users> GetAll()
        {
            return _object.ToList();
        }

        public void Insert(Users p)
        {
            _object?.Add(p);
            dbContext.SaveChanges();
        }

        public List<Users> List(Expression<Func<Users, bool>> filter)
        {
            return _object.Where(filter).ToList();
        }

        public void Update(Users p)
        {
            dbContext.SaveChanges();
        }
    }
}
