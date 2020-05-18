using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity; //required

namespace ToDoListForm.Model
{
    class ToDoListDBContext : DbContext
    {
        public DbSet<Status> Statuses { get; set; } //property of type Status (table in the database)

        public DbSet<Task> Tasks { get; set; } //property of type Task (table in the database)
    }
}