using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListForm.Model
{
    class Task
    {
        public int Id { get; set; } //id of task
        public string Name { get; set; } //name of task
        public DateTime? DueDate { get; set; } //due date of task

        public int StatusId { get; set; } //foreign key naming convention "name of class + property"

        public Status Status { get; set; } //navigation property (does not appear in database table)
    }
}
