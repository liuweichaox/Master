using Autofac.Extras.IocManager;

namespace Virgo.Presentation.Models
{
    public class Test:ITest, ILifetimeScopeDependency
    {
        public string Get()
        {
            return "Virgo";
        }
    }
}