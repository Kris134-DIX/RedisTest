using Redis.Database.Models;
using Redis.Repositories;

namespace Redis.Services
{
    public class SubFundUnitPriceService : ISubFundUnitPriceService
    {
        private readonly ISubFundUnitPriceRepository _subFundUnitPriceRepository;

        public SubFundUnitPriceService(ISubFundUnitPriceRepository subFundUnitPriceRepository)
        {
            _subFundUnitPriceRepository = subFundUnitPriceRepository;
        }

        public async Task<IEnumerable<SubFundUnitPriceDTO>> GetBySubFundId(int id)
        {
            return await _subFundUnitPriceRepository.GetBySubfundId(id);
        }

        public async Task<IEnumerable<SubFundUnitPriceModel>> GetAll()
        {
            return await _subFundUnitPriceRepository.GetAll();
        }

        public async Task<IEnumerable<int>> GetSubFundIds()
        {
            return await _subFundUnitPriceRepository.GetSubFundIds();
        }
    }

    public interface ISubFundUnitPriceService
    {
        Task<IEnumerable<SubFundUnitPriceDTO>> GetBySubFundId(int id);
        Task<IEnumerable<SubFundUnitPriceModel>> GetAll();
        Task<IEnumerable<int>> GetSubFundIds();
    }
}
