using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplCommerce.Module.Core.Models;

namespace Test.Module.Region.Services
{
    public interface IRegionService
    {
        Task Create(UserRegion region);
        Task Update(UserRegion region);
        Task Delete(long id);
        Task Delete(UserRegion region);
    }
}
