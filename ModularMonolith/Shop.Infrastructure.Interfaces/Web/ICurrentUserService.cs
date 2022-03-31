namespace Shop.Infrastructure.Interfaces.Web;

public interface ICurrentUserService
{
    int Id { get; }
    string Email { get; }
}