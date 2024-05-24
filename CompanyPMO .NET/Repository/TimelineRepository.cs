using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Hubs;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Timeline_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CompanyPMO_.NET.Repository
{
    public class TimelineRepository : ITimeline, ITimelineManagement
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<TimelineHub, ITimelineClient> _hubContext;
        public TimelineRepository(ApplicationDbContext context, IHubContext<TimelineHub, ITimelineClient> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public async Task<OperationResult> CreateTimelineEvent(TimelineDto timeline)
        {
            var newTimeline = new Timeline
            {
                Event = timeline.Event,
                EmployeeId = timeline.EmployeeId,
                ProjectId = timeline.ProjectId,
                TaskId = timeline.TaskId,
                IssueId = timeline.IssueId,
                Type = timeline.Type,
                Created = DateTime.UtcNow
            };

            await _context.Timelines.AddAsync(newTimeline);

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected > 0)
            {
                var newTimelineEvent = await _context.Timelines
                    .Where(x => x.TimelineId == newTimeline.TimelineId)
                    .Select(GetTimelinePredicate()).FirstOrDefaultAsync();

                await _hubContext.Clients.All.ReceiveTimelineEvent(newTimelineEvent);
            }

            return new OperationResult
            {
                Success = rowsAffected > 0,
                Message = rowsAffected > 0 ? "Timeline event created successfully." : "Something went wrong while creating timeline event."
            };
        }

        public async Task<OperationResult> DeleteTimelineEvents(int[] timelineIds)
        {
            var timelines = await _context.Timelines.Where(t => timelineIds.Contains(t.TimelineId)).ToListAsync();

            _context.Timelines.RemoveRange(timelines);

            int rowsAffected = await _context.SaveChangesAsync();

            return new OperationResult
            {
                Success = rowsAffected > 0,
                Message = rowsAffected > 0 ? "Timeline events deleted successfully." : "Something went wrong while deleting timeline events."
            };
        }

        public async Task<DataCountPages<TimelineShowcaseDto>> GetTimelineEvents(FilterParams filterParams)
        {
            int toSkip = (filterParams.Page - 1) * filterParams.PageSize;

            var timelines = await _context.Timelines
                .OrderByDescending(x => x.Created)
                .Select(GetTimelinePredicate())
                .Skip(toSkip)
                .Take(filterParams.PageSize)
                .ToListAsync();

            int totalTimelinesCount = await _context.Timelines.CountAsync();

            return new DataCountPages<TimelineShowcaseDto>
            {
                Data = timelines,
                Count = totalTimelinesCount,
                Pages = (int)Math.Ceiling((double)totalTimelinesCount / filterParams.PageSize)
            };
        }

        public async Task<DataCountPages<TimelineShowcaseDto>> GetTimelineEventsByEmployee(int employeeId, FilterParams filterParams)
        {
            int toSkip = (filterParams.Page - 1) * filterParams.PageSize;

            var timelines = await _context.Timelines
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.Created)
                .Select(GetTimelinePredicate())
                .Skip(toSkip)
                .Take(filterParams.PageSize)
                .ToListAsync();

            int totalTimelinesCount = await _context.Timelines.CountAsync();

            return new DataCountPages<TimelineShowcaseDto>
            {
                Data = timelines,
                Count = totalTimelinesCount,
                Pages = (int)Math.Ceiling((double)totalTimelinesCount / filterParams.PageSize)
            };
        }

        private static Expression<Func<Timeline, TimelineShowcaseDto>> GetTimelinePredicate()
        {
            return timeline => new TimelineShowcaseDto
            {
                TimelineId = timeline.TimelineId,
                Event = timeline.Event,
                Created = timeline.Created,
                Type = timeline.Type,
                EventText = $"{timeline.Employee.Username} {timeline.Event} {(timeline.ProjectId != null ? '#' + timeline.ProjectId.ToString() : null)} {(timeline.TaskId != null ? '#' + timeline.TaskId.ToString() : null)} {(timeline.IssueId != null ? '#' + timeline.IssueId.ToString() : null)}",
                Employee = new EmployeeShowcaseDto
                {
                    EmployeeId = timeline.Employee.EmployeeId,
                    Username = timeline.Employee.Username,
                    ProfilePicture = timeline.Employee.ProfilePicture,
                    LastLogin = timeline.Employee.LastLogin,
                },
                Project = timeline.Project != null ? new ProjectShowcaseDto
                {
                    ClientId = timeline.Project.CompanyId,
                    ProjectId = timeline.Project.ProjectId,
                    Name = timeline.Project.Name,
                } : null,
                Task = timeline.Task != null ? new TaskShowcaseDto
                {
                    ClientId = timeline.Task.Project.CompanyId,
                    ProjectId = timeline.Task.ProjectId,
                    TaskId = timeline.Task.TaskId,
                    Name = timeline.Task.Name
                } : null,
                Issue = timeline.Issue != null ? new IssueShowcaseDto
                {
                    ClientId = timeline.Issue.Task.Project.CompanyId,
                    ProjectId = timeline.Issue.Task.ProjectId,
                    TaskId = timeline.Issue.TaskId,
                    IssueId = timeline.Issue.IssueId,
                    Name = timeline.Issue.Name
                } : null
            };
        }

        public async Task<TimelineDto?> GetTimelineEvent(int timelineId)
        {
            return await _context.Timelines
                .Where(x => x.TimelineId == timelineId)
                .Select(x => new TimelineDto
                {
                    TimelineId = x.TimelineId,
                    Event = x.Event,
                    Created = x.Created,
                    EmployeeId = x.EmployeeId,
                    ProjectId = x.ProjectId,
                    TaskId = x.TaskId,
                    IssueId = x.IssueId,
                    Type = x.Type,
                    Employee = new EmployeeShowcaseDto
                    {
                        EmployeeId = x.Employee.EmployeeId,
                        Username = x.Employee.Username,
                        ProfilePicture = x.Employee.ProfilePicture,
                        LastLogin = x.Employee.LastLogin
                    },
                    Project = x.Project != null ? new ProjectDto
                    {
                        ProjectId = x.Project.ProjectId,
                        Name = x.Project.Name,
                        Description = x.Project.Description,
                        Created = x.Project.Created,
                        Finished = x.Project.Finished,
                        ExpectedDeliveryDate = x.Project.ExpectedDeliveryDate,
                        StartedWorking = x.Project.StartedWorking,
                        Lifecycle = x.Project.Lifecycle,
                        Priority = x.Project.Priority,
                        Company = new CompanyShowcaseDto
                        {
                            CompanyId = x.Project.CompanyId
                        },
                    } : null,
                    Task = x.Task != null ? new TaskDto
                    {
                        TaskId = x.Task.TaskId,
                        Name = x.Task.Name,
                        Description = x.Task.Description,
                        Created = x.Task.Created,
                        StartedWorking = x.Task.StartedWorking,
                        ExpectedDeliveryDate = x.Task.ExpectedDeliveryDate,
                        Finished = x.Task.Finished,
                        Project = new ProjectShowcaseDto
                        {
                            ClientId = x.Task.Project.CompanyId,
                            ProjectId = x.Task.ProjectId
                        }
                    } : null,
                    Issue = x.Issue != null ? new IssueDto
                    {
                        IssueId = x.Issue.IssueId,
                        Name = x.Issue.Name,
                        Description = x.Issue.Description,
                        Created = x.Issue.Created,
                        Finished = x.Issue.Finished,
                        ExpectedDeliveryDate = x.Issue.ExpectedDeliveryDate,
                        StartedWorking = x.Issue.StartedWorking,
                        Task = new TaskShowcaseDto
                        {
                            ClientId = x.Issue.Task.Project.CompanyId,
                            ProjectId = x.Issue.Task.ProjectId,
                            TaskId = x.Issue.TaskId
                        }
                    } : null
                })
                .FirstOrDefaultAsync();
        }
    }
}
