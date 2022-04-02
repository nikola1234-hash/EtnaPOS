
namespace EtnaPOS.BLL.Services.Authorization
{
    public enum LoginStatus
    {
        Success,
        Fail
    }
    public interface IAuthentication
    {
        LoginStatus Login();
    }
}
