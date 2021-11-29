using Application.Interfaces;
using Model;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Models.Base;

namespace Web.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Transaction")]
    public class TransactionController : ApiController
    {
        private ITransactionLogic TransactionLogic { get; }

        public TransactionController(ITransactionLogic logic)
        {
            TransactionLogic = logic;
        }

        private TransactionalModel CreateModel(TransactionViewModel model)
        {
            return new TransactionalModel()
            {
                UserId = model.UserId,
                AccountId = model.AccountId,
                Amount = model.Amount,
                TransactionFee = model.TransactionFee,
                Note = model.Note
            };
        }

        [Route("Debit")]
        public async Task<IHttpActionResult> Debit(TransactionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = TransactionLogic.Debit(CreateModel(model));

                if (result.IsSuccess)
                    return Ok();
                else
                    return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [Route("Credit")]
        public async Task<IHttpActionResult> Credit(TransactionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = TransactionLogic.Credit(CreateModel(model));

                if (result.IsSuccess)
                    return Ok();
                else
                    return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet]
        [Route("PerAccount")]
        public async Task<IHttpActionResult> GetAllPerAccount(GetAccountTransactions model)
        {
            try
            {
                var list = TransactionLogic.GetAllPerAccount(new GetAllAccountTransactionsModel()
                {
                    UserId = model.UserId,
                    AccountId = model.AccountId
                });
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
    }
}