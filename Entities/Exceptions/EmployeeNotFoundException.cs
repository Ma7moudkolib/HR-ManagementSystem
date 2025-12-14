namespace Entities.Exceptions
{
    public class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(Guid employeeId) 
            : base($"The Employee With id: {employeeId} doesn't exist in the database. ")
        {
        }
    }
}
