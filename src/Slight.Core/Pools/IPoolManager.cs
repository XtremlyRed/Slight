using System.Threading.Tasks;

using Slight.Core.Pools;

namespace Slight.Core
{
    /// <summary>
    /// pool manager
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    public interface IPoolManager<Target>
    {
        /// <summary>
        /// rent an  object
        /// </summary>
        /// <returns></returns>
        PoolItem<Target> Rent();


        /// <summary>
        /// rent an  object
        /// </summary>
        /// <returns></returns>
        Task<PoolItem<Target>> RentAsync(int waitTimeout_Ms);


        /// <summary>
        /// return  an  object
        /// </summary>
        /// <param name="poolItem"></param>
        /// <returns></returns>
        bool Return(PoolItem<Target> poolItem);
    }
}
