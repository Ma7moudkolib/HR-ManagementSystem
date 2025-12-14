using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;


namespace CompanyEmployees.Profiles
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>().ForCtorParam("FullAddress", opt => opt.MapFrom(x=>$"{x.Address} {x.Country}"));
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<UserForRegistrationDto, User>();
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentCreateDto, Department>();
            CreateMap<DepartmentUpdateDto, Department>();
            CreateMap<Attendance, AttendanceDto>();
            CreateMap<LeaveBalanceDto,LeaveBalance>();
            CreateMap<LeaveRequestDto , LeaveRequest>();
            CreateMap<CreateLeaveRequestDto, LeaveRequest>();

        }
    }
}
