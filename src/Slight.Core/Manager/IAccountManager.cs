using System.Threading.Tasks;

namespace Slight.Core.Manager
{
    public interface IAccountManager
    {
        Task<object> LoginInAsync(params object[] loginInfos);

        Task<object> LoginOutAsync(params object[] loginInfos);
    }
}
