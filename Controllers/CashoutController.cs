using CashOut.Helpers;
using CashOut.Models;
using CashOut.Models.Http;
using CashOut.Models.ViewModels;
using CashOut.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

        [EnableCors]
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
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.ContactNotFound, data = default(object) });

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
                ConfigId = config.Id,
                RateRangeId = _cashOutService.GetRateRangeIdByAmount(cashOutRequest.Amount, rates),
                CashoutFee = _cashOutService.GetRateFee(cashOutRequest.Amount, rates)
            };
            return Ok(new { code = HttpStatusCode.OK, data = confirmation, message = Constants.Success });
        }

        [EnableCors]
        [HttpPost]
        [Route("confirm")]
        public IActionResult ConfirmCashout([FromBody] CashoutConfirmation confirmation)
        {
            //-- Create Transaction
            Transaction transaction = new()
            {
                SystemId = confirmation.ConfigId,
                ContactId = confirmation.ContactId,
                RateRangeId = confirmation.RateRangeId,
                Amount = confirmation.CashoutAmount
            };
            var trans = _transactionService.Add(transaction);
            if(trans.Id == transaction.Id)
                return BadRequest(new { code = HttpStatusCode.BadRequest, message = Constants.FailedToCreate + " Transaction", data = default(object) });

            return Ok(new { code = HttpStatusCode.OK, data = trans, message = Constants.Success });
        }

        [HttpPost]
        [Route("finalize")]
        public IActionResult UpdateKioskBalance([FromBody] Transaction transaction)
        {
            //-- Update wallet balance
            var wallet = _walletService.GetByContact(transaction.ContactId);
            WalletBalance walletBalance = new()
            {
                WalletId = wallet.Id,
                Balance = wallet.Balance - transaction.Amount
            };
            var walletUpd = _walletService.UpdateBalance(walletBalance);
            if (walletUpd == 0)
                return BadRequest(new { code = HttpStatusCode.BadRequest, message = Constants.FailedToUpdate + " Wallet", data = default(object) });

            //-- Update system accumulated amount
            var systemConfig = _cashOutService.GetSystemConfig(transaction.SystemId);
            decimal kioskBalance = systemConfig.Balance - transaction.Amount;
            decimal kioskAccumulatedAmount = systemConfig.AccumulatedAmount + transaction.Amount;
            var sysUpd = _cashOutService.UpdateKioskBalanceAndAccumulatedAmount(systemConfig.Id, kioskBalance, kioskAccumulatedAmount);
            if (sysUpd == 0)
                return BadRequest(new { code = HttpStatusCode.BadRequest, message = Constants.FailedToUpdate + " Kiosk amounts", data = default(object) });

            //-- TODO: Revert previous update when failed update
            
            return Ok(new { code = HttpStatusCode.OK, data = default(object), message = Constants.Success });
        }
    }
}
