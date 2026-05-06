using PyroCloud.Core.Domain.Common;

namespace PyroCloud.Core.Domain.Entities.Identity
{
    public class UserCompany : BaseEntity<int>, ICompanyEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid? CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
