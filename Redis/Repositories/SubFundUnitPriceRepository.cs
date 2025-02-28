using Microsoft.EntityFrameworkCore;
using Redis.Database;
using Redis.Database.Models;

namespace Redis.Repositories
{
    public class SubFundUnitPriceRepository : ISubFundUnitPriceRepository
    {
        private readonly PlatformaDbContext _dbContext;
        public SubFundUnitPriceRepository(PlatformaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SubFundUnitPriceDTO>> GetBySubfundId(int id)
        {
            return await _dbContext.SubFundUnitPrices
                .Where(s => s.SubFundId == id)
                .Select(_ => new SubFundUnitPriceDTO
                {
                    UnitPrice = _.UnitPrice,
                    UnitPriceDate = _.UnitPriceDate,
                })
                .ToListAsync();
        }
        public async Task<List<SubFundUnitPriceModel>> GetAll()
        {
            return _dbContext.SubFundUnitPrices.ToList();
        }

        public async Task<List<int>> GetSubFundIds()
        {
            var getSubFundIds = _dbContext.SubFundUnitPrices
                .Select(s => s.SubFundId)
                .Distinct()
                .Order()
                .ToList();
            return getSubFundIds;
        }
    }

    public interface ISubFundUnitPriceRepository
    {
        Task<List<SubFundUnitPriceDTO>> GetBySubfundId(int id);
        Task<List<SubFundUnitPriceModel>> GetAll();
        Task<List<int>> GetSubFundIds();
    }
}
