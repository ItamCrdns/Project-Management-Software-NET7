﻿using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Dto
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? StartedWorking { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? Finished { get; set; }
        public EmployeeShowcaseDto? TaskCreator { get; set; }
        public List<EmployeeShowcaseDto>? Employees { get; set; }
        public int EmployeeCount { get; set; }
        public ProjectShowcaseDto? Project { get; set; } // Not nullable. All tasks should have a project
    }
}
