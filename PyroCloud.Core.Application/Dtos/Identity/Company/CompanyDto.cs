using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyroCloud.Core.Application.Dtos.Identity.Company
{
    public class CompanyResponseDto
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public string? ImageUrl { get; set; }
    }

    //public class TenantResponseDto
    //{
    //    public Guid? Id { get; set; }
    //    public string? Name { get; set; }
    //    //public string Code { get; set; }
    //    public string? Email { get; set; }
    //    //public string? ShowName { get; set; }
    //    public string? Description { get; set; }
    //    public string? PhoneNumber { get; set; }
    //    public string? ImageUrl { get; set; }

    //}
}
