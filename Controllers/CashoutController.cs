using CashOut.Helpers;
using CashOut.Models;
using CashOut.Models.Http;
using CashOut.Models.ViewModels;
using CashOut.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CashOut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashoutController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IContactService _contactService;
        private readonly IWalletService _walletService;
        private readonly ICashOutService _cashOutService;

        public CashoutController(ITransactionService transactionService,
            IContactService contactService, 
            IWalletService walletService,
            ICashOutService cashOutService)
        {
            _transactionService = transactionService;
            _contactService = contactService;
            _walletService = walletService;
            _cashOutService = cashOutService;
        }

        [HttpPost]
        [Route("validate")]
        public IActionResult ValidateCashout([FromBody] CashOutRequest cashOutRequest)
        {
            //-- Validate system config
            var config = _cashOutService.GetSystemConfig(cashOutRequest.SystemConfigId);
            if (config.Id == 0)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.ConfigNotFound, data = default(object) });

            //-- Validate system rates
            var rates = _cashOutService.GetRates(config.RateId);
            if (!rates.Any())
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.RateNotConfigured, data = default(object) });

            //-- Validate contact
            var contact = _contactService.GetByPhone(cashOutRequest.ContactNo);
            if (contact.Id == 0)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.RateNotConfigured, data = default(object) });

            //-- Validate contact wallet
            var wallet = _walletService.GetByContact(contact.Id);
            if (wallet.Id == 0)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.WalletNotFound, data = default(object) });

            //-- Validate amount vs machine balance
            if (!_cashOutService.IsValidAmount(cashOutRequest.Amount, config.Balance))
                return BadRequest(new { code = HttpStatusCode.BadRequest, message = Constants.InsuficientMachineBalance, data = default(object) });

            //-- Validate amount vs contact balance
            if (!_cashOutService.IsValidAmount(cashOutRequest.Amount, wallet.Balance))
                return BadRequest(new { code = HttpStatusCode.BadRequest, message = Constants.InsuficientWalletBalance, data = default(object) });


            CashoutConfirmation confirmation = new()
            {
                ContactId = contact.Id,
                ContactName = contact.FullName,
                CashoutAmount = cashOutRequest.Amount,
                CashoutFee = _cashOutService.GetRateFee(cashOutRequest.Amount, rates)
            };
            return Ok(new { code = HttpStatusCode.OK, data = confirmation, message = Constants.Success });
        }
    }
}
