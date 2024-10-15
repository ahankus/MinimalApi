using System.Collections.Generic;
using MinimalApi.Interfaces;
using MinimalApi.Models;

namespace MinimalApi.Services
{
    public class AccountNumberService
    {
        private readonly IAccountNumberRepository _accountNumberRepository;

        public AccountNumberService(IAccountNumberRepository accountNumberRepository)
        {
            _accountNumberRepository = accountNumberRepository;
        }

        public IEnumerable<AccountNumberModel> GetAccountNumbers()
        {
            return _accountNumberRepository.GetAll();
        }
        
        public AccountNumberModel GetAccountNumberByNumber(string number)    
        {
            var existingAccountNumber = _accountNumberRepository.GetByNumber(number);
            if (existingAccountNumber != null)
            {
                _accountNumberRepository.GetByNumber(number);
                return _accountNumberRepository.GetByNumber(existingAccountNumber.Number);
            }
            return null;
        }

        public void AddAccountNumber(AccountNumberModel accountNumber)
        {
            _accountNumberRepository.Add(accountNumber);
        }

        public bool DeleteAccountNumber(string id)
        {
            var existingAccountNumber = _accountNumberRepository.GetById(id);
            if (existingAccountNumber != null)
            {
                _accountNumberRepository.Delete(id);
                return true;
            }
            return false;
        }
        
        public bool DeleteAll()
        {
            var allAccountNumbers = _accountNumberRepository.GetAll(); // Assuming this method returns all account numbers

            if (allAccountNumbers != null && allAccountNumbers.Any())
            {
                foreach (var accountNumber in allAccountNumbers)
                {
                    _accountNumberRepository.Delete(accountNumber.Id); // Assuming account number has an Id property
                }
                return true;
            }
            return false;
        }
    }
}
