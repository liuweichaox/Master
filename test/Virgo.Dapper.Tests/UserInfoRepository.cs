using Virgo.Domain.Uow;

namespace Virgo.Dapper.Tests
{
    public class UserInfoRepository : DapperRepository<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
