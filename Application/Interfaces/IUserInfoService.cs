namespace Application.Interfaces
{
    public interface IUserInfoService
    {
        string GetEmail();
        string GetUsername();
        string GetGivenName();
        string GetFamilyName();
        string GetDisplayName();
    }
}
