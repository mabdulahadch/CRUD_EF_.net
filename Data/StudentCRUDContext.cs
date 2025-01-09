using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentCRUD.Models;

namespace StudentCRUD.Data
{
    public class StudentCRUDContext : DbContext
    {
        public StudentCRUDContext (DbContextOptions<StudentCRUDContext> options)
            : base(options)
        {
        }

        public DbSet<StudentCRUD.Models.Student> Student { get; set; } = default!;
    }
}
