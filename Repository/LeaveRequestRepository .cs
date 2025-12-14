using Contracts;
using Entities.Enums;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;

namespace Repository
{
    public class LeaveRequestRepository : RepositoryBase<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(RepositoryContext repositoryContext):base(repositoryContext) { }

        public void CreateLeaveRequestAsync(LeaveRequest request) => Create(request);
        public void DeleteLeaveRequest(LeaveRequest request) => Delete(request);

        public async Task<IEnumerable<LeaveRequest>> GetByEmployeeAsync(Guid employeeId, bool trackChange) =>
            await FindByCondition(l=> l.EmployeeId == employeeId, trackChange).ToListAsync();

        public async Task<IEnumerable<LeaveRequest>> GetByStatusAsync(LeaveStatus status, bool trackChange)=>
            await FindByCondition(l=> l.Status == status,trackChange).ToListAsync();

        public async Task<LeaveRequest?> GetLeaveRequestByIdAsync(Guid id, bool trackChanges) => 
            await FindByCondition(l=>l.Equals(id),trackChanges).FirstOrDefaultAsync();
    }

}
