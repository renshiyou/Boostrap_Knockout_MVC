using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using BootstrapIntroduction.Models;
using BootstrapIntroduction.ViewModels;
using BootstrapIntroduction.Filters;
using BootstrapIntroduction.Services;

namespace BootstrapIntroduction.Controllers
{
    [RoutePrefix("Writer")]
    public class AuthorsController : Controller
    {
        private AuthorService authorService;

        public AuthorsController()
        {
            authorService = new AuthorService();

            AutoMapper.Mapper.CreateMap<Author, AuthorViewModel>();
        }

        // GET: Authors
        [GenerateResultListFilterAttribute(typeof(Author), typeof(AuthorViewModel))]
        [Route("~/Writers")]
        public ActionResult Index([Form] QueryOptions queryOptions)
        {
            var authors = authorService.Get(queryOptions);

            ViewData["QueryOptions"] = queryOptions;

            return View(authors);
        }

        // GET: Authors/Details/5
        [Route("Details/{id:int:min(0)?}")]
        public ActionResult GetById(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var author = authorService.GetById(id.Value);

            return View(AutoMapper.Mapper.Map<Author, AuthorViewModel>(author));
        }

        // GET: Authors/Details/Jamie Munro
        [Route("Details/{name}")]
        public ActionResult GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var author = authorService.GetByName(name);

            return View(AutoMapper.Mapper.Map<Author, AuthorViewModel>(author));
        }

        // GET: Authors/Create
        [BasicAuthorization]
        public ActionResult Create()
        {
            return View("Form", new AuthorViewModel());
        }

        // GET: Authors/Edit/5
        [BasicAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var author = authorService.GetById(id.Value);

            return View("Form", AutoMapper.Mapper.Map<Author, AuthorViewModel>(author));
        }

        // GET: Authors/Delete/5
        [BasicAuthorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var author = authorService.GetById(id.Value);

            return View(AutoMapper.Mapper.Map<Author, AuthorViewModel>(author));
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [BasicAuthorization]
        public ActionResult DeleteConfirmed(int id)
        {
            var author = authorService.GetById(id);

            authorService.Delete(author);
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                authorService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
