using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BootstrapIntroduction.Models;
using BootstrapIntroduction.ViewModels;
using BootstrapIntroduction.Services;

namespace BootstrapIntroduction.Controllers.Api
{
    public class AuthorsController : ApiController
    {
        private AuthorService authorService;

        public AuthorsController()
        {
            authorService = new AuthorService();

            AutoMapper.Mapper.CreateMap<Author, AuthorViewModel>();
            AutoMapper.Mapper.CreateMap<AuthorViewModel, Author>();
        }

        // GET: api/Authors
        public ResultList<AuthorViewModel> Get([FromUri]QueryOptions queryOptions)
        {
            var authors = authorService.Get(queryOptions);

            return new ResultList<AuthorViewModel>(
                AutoMapper.Mapper.Map<List<Author>, List<AuthorViewModel>>(authors), queryOptions);
        }

        // GET: api/Authors/5
        [ResponseType(typeof(AuthorViewModel))]
        public IHttpActionResult Get(int id)
        {
            var author = authorService.GetById(id);

            return Ok(AutoMapper.Mapper.Map<Author, AuthorViewModel>(author));
        }

        // PUT: api/Authors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(AuthorViewModel author)
        {
            var model = AutoMapper.Mapper.Map<AuthorViewModel, Author>(author);

            authorService.Update(model);

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Authors
        [ResponseType(typeof(AuthorViewModel))]
        public IHttpActionResult Post(AuthorViewModel author)
        {
            var model = AutoMapper.Mapper.Map<AuthorViewModel, Author>(author);

            authorService.Insert(model);

            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [ResponseType(typeof(Author))]
        public IHttpActionResult DeleteAuthor(int id)
        {
            var author = authorService.GetById(id);

            authorService.Delete(author);

            return Ok(author);
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