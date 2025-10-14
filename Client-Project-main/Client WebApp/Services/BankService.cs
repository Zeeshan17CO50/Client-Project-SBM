using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Application.Features.Bank.Dtos;
using Client.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Client_WebApp.Services
{
    public class BankService
    {
        private readonly IBankMasterRepository _bankRepo;
        private readonly ILogger<BankService> _logger;

        public BankService(IBankMasterRepository bankRepo, ILogger<BankService> logger)
        {
            _bankRepo = bankRepo;
            _logger = logger;
        }

        public async Task<List<BankMasterDto>> GetAllBanksAsync()
        {
            try
            {
                return await _bankRepo.GetBanksAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching bank list");
                throw;
            }
        }

        public async Task<BankMasterDto> GetBankByIdAsync(int id)
        {
            try
            {
                var banks = await _bankRepo.GetBanksAsync(id);
                return banks.Count > 0 ? banks[0] : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching bank by Id {id}");
                throw;
            }
        }

        public async Task<List<BankMasterDto>> CreateBankAsync(CreateBankMasterDto dto)
        {
            try
            {
                return await _bankRepo.CreateBankAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating bank");
                throw;
            }
        }

        public async Task<List<BankMasterDto>> UpdateBankAsync(UpdateBankMasterDto dto)
        {
            try
            {
                return await _bankRepo.UpdateBankAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating bank");
                throw;
            }
        }

        public async Task<List<BankMasterDto>> DeleteBankAsync(DeleteBankMasterDto dto)
        {
            try
            {
                return await _bankRepo.DeleteBankAsync(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting bank");
                throw;
            }
        }
    }
}
