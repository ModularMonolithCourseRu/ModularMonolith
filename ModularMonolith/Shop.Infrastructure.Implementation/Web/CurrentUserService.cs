using Shop.Infrastructure.Interfaces.Web;

namespace Shop.Infrastructure.Implementation.Web;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService()
    {
        Id = 1;
        Email = "test@user.com";
    }

    public int Id { get; }
    public string Email { get; }
}

