using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Model;
using Web.Models.Base;
using Application.Interfaces;

namespace Web.Controllers
{
    public class AppUserController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private IAppUserLogic AppUserLogic { get; }

        public AppUserController(IAppUserLogic userLogic)
        {
            AppUserLogic = userLogic;
        }

        public IQueryable<AppUserModel> Get()
        {
            IQueryable<AppUserModel> appUserModels = (IQueryable<AppUserModel>)AppUserLogic.GetData();

            return appUserModels;
        }


        #region OldLogic
        //// GET: api/AppUser
        public IQueryable<AppUserModel> GetAppUserModels()
        {
            return db.AppUserModels;
        }

        //// GET: api/AppUser/5
        //[ResponseType(typeof(AppUserModel))]
        //public async Task<IHttpActionResult> GetAppUserModel(int id)
        //{
        //    AppUserModel appUserModel = await db.AppUserModels.FindAsync(id);
        //    if (appUserModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(appUserModel);
        //}

        //// PUT: api/AppUser/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutAppUserModel(int id, AppUserModel appUserModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != appUserModel.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(appUserModel).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AppUserModelExists(id))
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

        //// POST: api/AppUser
        //[ResponseType(typeof(AppUserModel))]
        //public async Task<IHttpActionResult> PostAppUserModel(AppUserModel appUserModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.AppUserModels.Add(appUserModel);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = appUserModel.Id }, appUserModel);
        //}

        //// DELETE: api/AppUser/5
        //[ResponseType(typeof(AppUserModel))]
        //public async Task<IHttpActionResult> DeleteAppUserModel(int id)
        //{
        //    AppUserModel appUserModel = await db.AppUserModels.FindAsync(id);
        //    if (appUserModel == null)
        //    {
        //        return NotFound();
        //    }

        //    db.AppUserModels.Remove(appUserModel);
        //    await db.SaveChangesAsync();

        //    return Ok(appUserModel);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool AppUserModelExists(int id)
        //{
        //    return db.AppUserModels.Count(e => e.Id == id) > 0;
        //}
        #endregion
    }
}