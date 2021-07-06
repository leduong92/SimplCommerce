using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SimplCommerce.Infrastructure.Data;
using SimplCommerce.Infrastructure.Web.SmartTable;
using SimplCommerce.Module.Core.Areas.Core.ViewModels;
using SimplCommerce.Module.Core.Models;


namespace SimplCommerce.Module.Core.Areas.Core.Controllers
{
    [Area("Core")]
    [Route("api/countries")]
    public class CountryApiController : Controller
    {
        private readonly IRepositoryWithTypedId<Country, string> _countryRepository;
        private IHostingEnvironment _hostingEnvironment;

        public CountryApiController(IRepositoryWithTypedId<Country, string> countryRepository, IHostingEnvironment hostingEnvironment)
        {
            _countryRepository = countryRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] bool? shippingEnabled)
        {
            var query = _countryRepository.Query();
            if (shippingEnabled.HasValue)
            {
                query = query.Where(x => x.IsShippingEnabled == shippingEnabled.Value);
            }
            var countries = await query
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return Json(countries);
        }

        [HttpPost("grid")]
        public IActionResult List([FromBody] SmartTableParam param)
        {
            var query = _countryRepository.Query();

            if (param.Search.PredicateObject != null)
            {
                dynamic search = param.Search.PredicateObject;

                if (search.Name != null)
                {
                    string name = search.Name;
                    query = query.Where(x => x.Name.Contains(name));
                }
            }

            var countries = query.ToSmartTableResult(
                param,
                c => new
                {
                    c.Id,
                    c.Name,
                    c.Code3,
                    c.IsShippingEnabled,
                    c.IsBillingEnabled,
                    c.IsCityEnabled,
                    c.IsZipCodeEnabled,
                    c.IsDistrictEnabled
                });

            return Json(countries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var country = await _countryRepository.Query().FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            var model = new CountryForm
            {
                Id = country.Id,
                Name = country.Name,
                Code3 = country.Code3,
                IsBillingEnabled = country.IsBillingEnabled,
                IsShippingEnabled = country.IsShippingEnabled,
                IsCityEnabled = country.IsCityEnabled,
                IsZipCodeEnabled = country.IsZipCodeEnabled,
                IsDistrictEnabled = country.IsDistrictEnabled
            };

            return Json(model);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Put(string id, [FromBody] CountryForm model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var country = await _countryRepository.Query().FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            country.Name = model.Name;
            country.Code3 = model.Code3;
            country.IsShippingEnabled = model.IsShippingEnabled;
            country.IsBillingEnabled = model.IsBillingEnabled;
            country.IsCityEnabled = model.IsCityEnabled;
            country.IsZipCodeEnabled = model.IsZipCodeEnabled;
            country.IsDistrictEnabled = model.IsDistrictEnabled;

            await _countryRepository.SaveChangesAsync();

            return Accepted();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] CountryForm model)
        {
            if (ModelState.IsValid)
            {
                var country = new Country(model.Id)
                {
                    Name = model.Name,
                    Code3 = model.Code3,
                    IsBillingEnabled = model.IsBillingEnabled,
                    IsShippingEnabled = model.IsShippingEnabled,
                    IsCityEnabled = model.IsCityEnabled,
                    IsZipCodeEnabled = model.IsZipCodeEnabled,
                    IsDistrictEnabled = model.IsDistrictEnabled
                };

                _countryRepository.Add(country);
                await _countryRepository.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), new { id = country.Id }, null);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var country = await _countryRepository.Query().FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            try
            {
                _countryRepository.Remove(country);
                await _countryRepository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { Error = $"The country {country.Name} can't not be deleted because it is referenced by other tables" });
            }

            return NoContent();
        }


        [Route("import")]
        [HttpPost]
        public async Task<IActionResult> Import()
        {
            IFormFile file = Request.Form.Files[0];

            if (file == null || file.Length <= 0)
            {
                return BadRequest(new { Error = $"File is empty" });
            }

            if (!Path.GetExtension(file.FileName).Equals(".xltx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { Error = $"Not Support file extension" });
            }

            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);

            DirectoryInfo di = new DirectoryInfo(newPath);
            foreach (FileInfo filesDelete in di.GetFiles())
            {
                filesDelete.Delete();
            }

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            var fiName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            using (var fileStream = new FileStream(Path.Combine(newPath, fiName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            string rootFolder = _hostingEnvironment.WebRootPath;
            string fileName = @"Upload/" + fiName;
            FileInfo fileInf = new FileInfo(Path.Combine(rootFolder, fileName));

            var listCountry = this.ReadProductFromExcel(fileInf);

            if (listCountry.Count < 0)
            {
                return BadRequest(new { Error = $"No data in Excel file" });
            }

            foreach (var country in listCountry)
            {
                _countryRepository.Add(country);
            }
            await _countryRepository.SaveChangesAsync();

            return Ok();
        }
        private List<Country> ReadProductFromExcel(FileInfo str)
        {
            using (var package = new ExcelPackage(str))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                List<Country> listProduct = new List<Country>();
                CountryForm countryVm;

                bool isShippingEnabled = false;
                bool isBillingEnabled = false;
                bool isDisplayCity = false;
                bool isDisplayPostalCode = false;
                bool isDisplayDistrict = false;

                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    countryVm = new CountryForm();

                    countryVm.Id = workSheet.Cells[i, 1].Value.ToString();

                    countryVm.Name = workSheet.Cells[i, 2].Value.ToString();

                    bool.TryParse(workSheet.Cells[i, 3].Value.ToString(), out isShippingEnabled);
                    countryVm.IsShippingEnabled = isShippingEnabled;

                    bool.TryParse(workSheet.Cells[i, 4].Value.ToString(), out isBillingEnabled);
                    countryVm.IsBillingEnabled = isBillingEnabled;

                    bool.TryParse(workSheet.Cells[i, 5].Value.ToString(), out isDisplayCity);
                    countryVm.IsCityEnabled = isDisplayCity;

                    bool.TryParse(workSheet.Cells[i, 6].Value.ToString(), out isDisplayPostalCode);
                    countryVm.IsZipCodeEnabled = isDisplayPostalCode;

                    bool.TryParse(workSheet.Cells[i, 7].Value.ToString(), out isDisplayDistrict);
                    countryVm.IsDistrictEnabled = isDisplayDistrict;

                    var country = new Country(countryVm.Id)
                    {
                        Name = countryVm.Name,
                        Code3 = countryVm.Code3,
                        IsBillingEnabled = countryVm.IsBillingEnabled,
                        IsShippingEnabled = countryVm.IsShippingEnabled,
                        IsCityEnabled = countryVm.IsCityEnabled,
                        IsZipCodeEnabled = countryVm.IsZipCodeEnabled,
                        IsDistrictEnabled = countryVm.IsDistrictEnabled
                    };

                    listProduct.Add(country);
                }
                return listProduct;
            }
        }
       
    }
}
