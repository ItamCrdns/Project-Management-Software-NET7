using CompanyPMO_.NET.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    public class EmployeeTask : IEmployeeEntity
    {
        // Relations table
        [Column("relation_id")]
        public int RelationId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; } // Employee key ref
        [Column("task_id")]
        public int TaskId { get; set; } // Task key ref
    }
}
