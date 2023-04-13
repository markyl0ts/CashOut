using CashOut.Helpers;
using CashOut.Models;
using CashOut.Models.Http;
using CashOut.Services;
using CashOut.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CashOut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        [Route("{walletId}")]
        public IActionResult Get(long walletId)
        {
            var wallet = _walletService.GetById(walletId);
            if (wallet == null)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.RecordNotFound, data = default(object) });

            return Ok(new { code = HttpStatusCode.OK, data = wallet, message = Constants.Success });
        }

        [HttpGet]
        [Route("contact/{contactId}")]
        public IActionResult GetByContact(long contactId)
        {
            var wallet = _walletService.GetByContact(contactId);
            if (wallet == null)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.RecordNotFound, data = default(object) });

            return Ok(new { code = HttpStatusCode.OK, data = wallet, message = Constants.Success });
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create([FromBody] Wallet wallet)
        {
            var obj = _walletService.Add(wallet);
            if (obj == null)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.FailedToCreate, data = default(object) });

            return Ok(new { code = HttpStatusCode.OK, data = obj, message = Constants.Success });
        }

        [HttpPost]
        [Route("balance")]
        public IActionResult UpdateBalance([FromBody] WalletBalance walletBalance)
        {
            var res = _walletService.UpdateBalance(walletBalance);
            if (res == 0)
                return NotFound(new { code = HttpStatusCode.NotFound, message = Constants.FailedToUpdate, data = default(object) });

            return Ok(new { code = HttpStatusCode.OK, data = res, message = Constants.Success });
        }
    }
}
