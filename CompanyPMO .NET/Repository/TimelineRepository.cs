using CompanyPMO_.NET.Common;
using CompanyPMO_.NET.Data;
using CompanyPMO_.NET.Dto;
using CompanyPMO_.NET.Interfaces;
using CompanyPMO_.NET.Interfaces.Timeline_interfaces;
using CompanyPMO_.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyPMO_.NET.Repository
{
    public class TimelineRepository : ITimeline, ITimelineManagement
    {
        private readonly ApplicationDbContext _context;
        public TimelineRepository(ApplicationDbContext context)
        {
            _context = context;
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

        public async Task<DataCountPages<TimelineDto>> GetTimelineEvents(FilterParams filterParams)
        {
            int toSkip = (filterParams.Page - 1) * filterParams.PageSize;

            var timelines = await _context.Timelines
                .Select(x => new TimelineDto
                {
                    Event = x.Event,
                    EmployeeId = x.EmployeeId,
                    Type = x.Type,
                    Created = x.Created,
                    Employee = new EmployeeShowcaseDto
                    {
                        EmployeeId = x.Employee.EmployeeId,
                        Username = x.Employee.Username,
                        ProfilePicture = x.Employee.ProfilePicture,
                        LastLogin = x.Employee.LastLogin,
                    }
                })
                .Skip(toSkip)
                .Take(filterParams.PageSize)
                .ToListAsync();

            int totalTimelinesCount = await _context.Timelines.CountAsync();

            return new DataCountPages<TimelineDto>
            {
                Data = timelines,
                Count = totalTimelinesCount,
                Pages = (int)Math.Ceiling((double)totalTimelinesCount / filterParams.PageSize)
            };
        }

        public async Task<DataCountPages<TimelineDto>> GetTimelineEventsByEmployee(int employeeId, FilterParams filterParams)
        {
            int toSkip = (filterParams.Page - 1) * filterParams.PageSize;

            var timelines = await _context.Timelines
                .Where(x => x.EmployeeId == employeeId)
                .Select(x => new TimelineDto
                {
                    Event = x.Event,
                    EmployeeId = x.EmployeeId,
                    Type = x.Type,
                    Created = x.Created,
                    Employee = new EmployeeShowcaseDto
                    {
                        EmployeeId = x.Employee.EmployeeId,
                        Username = x.Employee.Username,
                        ProfilePicture = x.Employee.ProfilePicture,
                        LastLogin = x.Employee.LastLogin,
                    }
                })
                .Skip(toSkip)
                .Take(filterParams.PageSize)
                .ToListAsync();

            int totalTimelinesCount = await _context.Timelines.CountAsync();

            return new DataCountPages<TimelineDto>
            {
                Data = timelines,
                Count = totalTimelinesCount,
                Pages = (int)Math.Ceiling((double)totalTimelinesCount / filterParams.PageSize)
            };
        }
    }
}
