﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyPMO_.NET.Models
{
    [Table("projects")]
    public class Project
    {
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("started_working")]
        public DateTime? StartedWorking { get; set; }
        [Column("finished")]
        public DateTime? Finished { get; set;}
        [Column("expected_delivery_date")]
        public DateTime? ExpectedDeliveryDate { get; set; }
        [Column("project_creator_id")]
        public int ProjectCreatorId { get; set; }
        [Column("company_id")]
        public int CompanyId { get; set; }
        [Column("priority")]
        public int Priority { get; set; }
        [Column("lifecycle")]
        public string? Lifecycle { get; set; }
        [Column("latest_task_creation")]
        public DateTime LatestTaskCreation { get; set; } // Used to order projects based on their latest task creation.

        // Navigation properties

        public ICollection<Image>? Images { get; set; }
        public List<Employee>? Employees { get; set; }
        public Company? Company { get; set; }
        public Employee? ProjectCreator { get; set; }
    }
}
