namespace Entities.Exceptions
{
    public class CompanyNotFoundException : NotFoundException
    {
        public CompanyNotFoundException(Guid companyId) : base($"The Company With id: {companyId} doesn't exist in the database. ")
        {
        }
    }
}
