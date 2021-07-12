using System.Linq;
using System.Threading.Tasks;
using SimplCommerce.Module.Core.Models;

namespace SimplCommerce.Module.Core.Services
{
    public interface IWidgetInstanceService
    {
        IQueryable<WidgetInstance> GetPublished();
    }
}
