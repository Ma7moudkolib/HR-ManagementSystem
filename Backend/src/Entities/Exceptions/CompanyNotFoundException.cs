namespace Entities.Exceptions
{
    public class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException(int companyId) : base($"The Company With id: {companyId} doesn't exist in the database. ")
        {
        }
    }
}
