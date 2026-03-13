using AttendanceApp.Domain.Entities;

namespace AttendanceApp.Application.Interfaces
{
    public interface IJwtProvider
    {
        string Generate(Student student);
    }
}