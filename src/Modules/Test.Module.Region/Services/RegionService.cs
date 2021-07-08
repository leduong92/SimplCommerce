using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.Core.Models;
using SimplCommerce.Module.Core.Services;

namespace Test.Module.Region.Services
{
    public class RegionService : IRegionService
    {
        private const string RegionEntityTypeId = "Region";
        private readonly IRepository<UserRegion> _userRegionRepository;
        private readonly IEntityService _entityService;


        public RegionService(IRepository<UserRegion> userRegionRepository, IEntityService entityService)
        {
            _userRegionRepository = userRegionRepository;
            _entityService = entityService;
        }

        public async Task Create(UserRegion region)
        {
            using (var transection = _userRegionRepository.BeginTransaction())
            {
                
            }
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(UserRegion region)
        {
            throw new NotImplementedException();
        }

        public Task Update(UserRegion region)
        {
            throw new NotImplementedException();
        }
    }
}
