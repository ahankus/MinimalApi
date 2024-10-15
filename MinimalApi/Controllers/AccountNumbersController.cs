using Microsoft.AspNetCore.Mvc;
using MinimalApi.Interfaces;
using MinimalApi.Models;

namespace MinimalApi.Controllers
{
    [Route("api/accountNumbers")]
    [ApiController]
    public class AccountNumbersController : ControllerBase
    {
        private readonly IAccountNumberRepository _accountNumberRepository;

        public AccountNumbersController(IAccountNumberRepository accountNumberRepository)
        {
            _accountNumberRepository = accountNumberRepository;
        }

        [HttpGet]
        public IActionResult GetAccountNumbers()
        {
            var accountNumbers = _accountNumberRepository.GetAll();
            return Ok(accountNumbers);
        }

        [HttpPost]
        public IActionResult AddAccountNumber(AccountNumberModel accountNumber)
        {
            _accountNumberRepository.Add(accountNumber);
            return Created($"/api/accountNumbers/{accountNumber.Id}", accountNumber);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccountNumber(string id)
        {
            var existingAccountNumber = _accountNumberRepository.GetById(id);
            if (existingAccountNumber == null)
            {
                return NotFound();
            }

            _accountNumberRepository.Delete(id);
            return NoContent();
        }
    }
}