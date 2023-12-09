using Microsoft.AspNetCore.Mvc;
using ms_travel_easy.src.Models;
using ms_travel_easy.src.Services;

namespace ms_travel_easy.src.Controllers
{
    [ApiController]
    [Route("api/v1/Customer/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountsService;

        public AccountsController(IAccountService productsService) =>
            _accountsService = productsService;

        [HttpGet]
        public async Task<List<Account>> Get() =>
            await _accountsService.GetAllAccountsAsync();

        [HttpGet("{email}")]
        public async Task<ActionResult<Account>> Get(string email)
        {
            var account = await _accountsService.GetAccountByEmailAsync(email);

            if (account is null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> Post(AccountRequest accountRequest)
        {
            Account newAccount = new Account
            {
                AccountId = Guid.NewGuid().ToString("D"),
                Name = accountRequest.Name,
                LastName = accountRequest.LastName,
                Email = accountRequest.Email,
                PhoneNumber = accountRequest.PhoneNumber
            };
            await _accountsService.CreateAccountAsync(newAccount);
            return Created("", newAccount);

        }

        [HttpPatch("{email}")]
        public async Task<ActionResult<Account>> UpdateAccount(string email, [FromBody] AccountUpdateRequest updatePayload)
        {
            var existingAccount = await _accountsService.GetAccountByEmailAsync(email);

            if (existingAccount == null)
            {
                return NotFound();
            }

            // Update only the specified fields
            existingAccount.LastName = updatePayload.LastName ?? existingAccount.LastName;
            existingAccount.PhoneNumber = updatePayload.PhoneNumber ?? existingAccount.PhoneNumber;

            await _accountsService.UpdateAccountAsync(existingAccount);

            return Ok(existingAccount);
        }
    }
}

