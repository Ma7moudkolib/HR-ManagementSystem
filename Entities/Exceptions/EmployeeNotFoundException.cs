namespace Entities.Exceptions
{
    public class EmployeeNotFoundException : NotFoundException
    {
        public EmployeeNotFoundException(int employeeId) 
            : base($"The Employee With id: {employeeId} doesn't exist in the database. ")
        {
        }
    }
}
