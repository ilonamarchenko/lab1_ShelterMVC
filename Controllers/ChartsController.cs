using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelterInfastructure;
using Microsoft.EntityFrameworkCore;

namespace ShelterInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private record CountByYearResponseItem(string Year, int Count);

        private readonly DbshelterContext _context;

        public ChartsController(DbshelterContext _context)
        {
            this._context = _context;
        }

        [HttpGet("countByYear")]
        public async Task<JsonResult> GetCountByYearAsync(CancellationToken cancellationToken)
        {
            var responseItems = await _context
                .Animals
                .GroupBy(movie => movie.DateOfBirth.Year)
                .Select(group => new CountByYearResponseItem(group.Key.ToString(), group.Count()))
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }

        [HttpGet("countByYear1")]
        public async Task<JsonResult> GetCountByYear1Async(CancellationToken cancellationToken)
        {
            var responseItems = await _context
                .MedicalCards
                .GroupBy(movie => movie.DateOfCreation.Year)
                .Select(group => new CountByYearResponseItem(group.Key.ToString(), group.Count()))
                .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }
    }
}