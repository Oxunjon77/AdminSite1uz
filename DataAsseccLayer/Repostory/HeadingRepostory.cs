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
    public class HeadingRepostory : IHeadingDl
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Heading>? _heading;

        public HeadingRepostory(AppDbContext context)
        {
            _context = context;
            _heading = _context.Set<Heading>();
        }

        public void Delete(Heading p)
        {
            _heading?.Remove(p);
            _context.SaveChanges();
        }

        public Heading Get(Expression<Func<Heading, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Heading> GetAll()
        {

           return _heading.ToList();
            _context.SaveChanges();
        }

        public void Insert(Heading p)
        {
             _heading.Add(p);
            _context.SaveChanges();
        }

        public List<Heading> List(Expression<Func<Heading, bool>> filter)
        {
            return _heading.Where(filter).ToList();
        }

        public void Update(Heading p)
        {
            _context.SaveChanges();
        }
    }
}
