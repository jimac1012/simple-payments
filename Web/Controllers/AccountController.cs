using Application.Interfaces;
using Model;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Models.Base;

namespace Web.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private IAccountLogic AccountLogic { get; }

        public AccountController(IAccountLogic logic)
        {
            AccountLogic = logic;
        }

        [Route("Create")]
        public async Task<IHttpActionResult> Create(AccountCreationBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = AccountLogic.Save(new AccountModel()
                {
                    UserId = model.UserId,
                    AccountName = model.AccountName,
                    Type = "Savings"
                });

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
        [Route("List")]
        public async Task<IHttpActionResult> GetList(GetUserAccountList model)
        {
            try
            {
                var list = AccountLogic.GetUserAccounts(model.UserId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }


        #region OldControllerLogic
        //private WebDBContext db = new WebDBContext();

        //// GET: api/Accounts
        //public IQueryable<Account> GetAccounts()
        //{
        //    return db.Accounts;
        //}

        //// GET: api/Accounts/5
        //[ResponseType(typeof(Account))]
        //public async Task<IHttpActionResult> GetAccount(int id)
        //{
        //    Account account = await db.Accounts.FindAsync(id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(account);
        //}

        //// PUT: api/Accounts/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutAccount(int id, Account account)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != account.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(account).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AccountExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Accounts
        //[ResponseType(typeof(Account))]
        //public async Task<IHttpActionResult> PostAccount(Account account)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Accounts.Add(account);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = account.Id }, account);
        //}

        //// DELETE: api/Accounts/5
        //[ResponseType(typeof(Account))]
        //public async Task<IHttpActionResult> DeleteAccount(int id)
        //{
        //    Account account = await db.Accounts.FindAsync(id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Accounts.Remove(account);
        //    await db.SaveChangesAsync();

        //    return Ok(account);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool AccountExists(int id)
        //{
        //    return db.Accounts.Count(e => e.Id == id) > 0;
        //}
        #endregion
    }
}