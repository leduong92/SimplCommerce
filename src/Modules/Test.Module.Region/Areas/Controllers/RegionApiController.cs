using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Module.Core.Models;
using Test.Module.Region.Services;

namespace Test.Module.Region.Areas.Controllers
{
    [Area("Regions")]
    [Authorize(Roles = "admin")]
    [Route("api/regions")]
    public class RegionApiController : Controller
    {

        private readonly IRepository<UserRegion> _userRegionRepository;
        private readonly IRegionService _regionService;

        public RegionApiController(IRepository<UserRegion> userRegionRepository, IRegionService regionService)
        {
            _userRegionRepository = userRegionRepository;
            _regionService = regionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var regions = await _userRegionRepository.Query().Select(x => new
            {
                UserId = x.UserId,
                User = x.User,
                CountryId = x.CountryId,
                Country = x.Country,
                CreateOn = x.CreateOn
            }).ToListAsync();

            return Json(regions);
        }

    }
}
