using GYMProgram.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GYMProgram.BusinessFunctional
{
    public class MaxValue<T> where T:class
    {
        ApplicationDbContext _context;
        DbSet<T> table = null;
        public MaxValue(ApplicationDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }
        public int GetMaxValue(T obj)
        {
            return 0;
            //return (table.Max(t => (int?)t.GetType(obj).n) ?? 0) + 1;
        }
    }

 
}
