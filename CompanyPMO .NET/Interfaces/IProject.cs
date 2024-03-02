﻿using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Models;

namespace CompanyPMO_.NET.Interfaces
{
    public interface IProject
    {
        Task<OperationResult<int>> CreateProject(Project project, int employeeSupervisorId, List<IFormFile>? images, int companyId, List<int>? employees);
        Task<(bool updated, ProjectDto)> UpdateProject(int employeeId, int projectId, ProjectDto projectDto, List<IFormFile>? images);
        Task<ProjectDto> GetProjectById(int projectId);
        Task<ProjectSomeInfoDto> GetProjectNameCreatorLifecyclePriorityAndTeam(int projectId);
        Task<Project> GetProjectEntityById(int projectId);
        Task<DataCountPages<ProjectDto>> GetProjectsByCompanyName(int companyId, FilterParams filterParams);
        Task<bool> SetProjectFinalized(int projectId);
        Task<bool> DoesProjectExist(int projectId);
        Task<(string status, IEnumerable<ImageDto>)> AddImagesToExistingProject(int projectId, List<IFormFile>? images);
        Task<DataCountPages<ProjectDto>> GetAllProjects(FilterParams filterParams);
        Task<Dictionary<string, List<ProjectDto>>> GetProjectsGroupedByCompany(int page, int pageSize);
        ICollection<Image> SelectImages(ICollection<Image> images);
        ICollection<ProjectDto> ProjectSelectQuery(ICollection<Project> projects);
        Task<(string status, IEnumerable<EmployeeShowcaseDto>)> AddEmployeesToProject(int projectId, List<int> employees);
        Task<bool> IsEmployeeAlreadyInProject(int employeeId, int projectId);
        Task<DataCountPages<ProjectDto>> GetProjectsByEmployeeUsername(string username, FilterParams filterParams);
        Task<DataCountPages<ProjectShowcaseDto>> GetProjectsShowcaseByEmployeeUsername(string username, int page, int pageSize);
        Task<DataCountPages<ProjectShowcaseDto>> GetAllProjectsShowcase(int page, int pageSize);
        Task<ProjectShowcaseDto> GetProjectShowcase(int projectId);
        Task<bool> IsParticipant(int projectId, int employeeId); // Used for checking if the employee is a participant in the project
        Task<bool> IsOwner(int projectId, int employeeId); // Used for checking if the employee is the owner of the project

    }
}
