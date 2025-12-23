using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DatabaseContext;

namespace Repository
{
    public class LeaveTypeRepository : RepositoryBase<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

        public void CreateLeaveType(LeaveType leaveType) =>
            Create(leaveType);

        public void DeleteLeaveType(LeaveType leaveType) =>
            Delete(leaveType);

        public async Task<IEnumerable<LeaveType>> GetAllLeaveTypeAsync(bool trackChange) =>
            await FindAll(trackChange).ToListAsync();
        public async Task<LeaveType> GetLeaveTypeByIdAsync(int id, bool trackChange) =>
            await FindByCondition(lt => lt.Id == id, trackChange).FirstOrDefaultAsync();

    }

}
