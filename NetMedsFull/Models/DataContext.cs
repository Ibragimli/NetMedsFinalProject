using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetMedsFull.Models
{
    public class DataContext:IdentityDbContext
    {
        public DataContext(DbContextOptions options  ):base(options)
        {
        }

    }
}
