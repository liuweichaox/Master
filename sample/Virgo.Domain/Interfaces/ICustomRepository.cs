using Virgo.Domain.Models;

namespace Virgo.Domain.Interfaces
{
    /// <summary>
    /// 定义泛型仓储接口，并继承IDisposable，显式释放资源
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICustomRepository
    {
        bool Call(CustomEntity entity);
    }
}
