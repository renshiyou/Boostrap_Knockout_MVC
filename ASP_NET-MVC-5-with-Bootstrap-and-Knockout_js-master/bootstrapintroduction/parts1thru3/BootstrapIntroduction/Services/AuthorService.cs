using BootstrapIntroduction.Behaviors;
using BootstrapIntroduction.DAL;
using BootstrapIntroduction.Models;
using BootstrapIntroduction.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BootstrapIntroduction.Services
{
    public class AuthorService : IDisposable
    {
        private BookContext db = new BookContext();

        public List<Author> Get(QueryOptions queryOptions)
        {
            var start = QueryOptionsCalculator.CalculateStart(queryOptions);

            var authors = db.Authors.
                OrderBy(queryOptions.Sort).
                Skip(start).
                Take(queryOptions.PageSize);

            queryOptions.TotalPages = QueryOptionsCalculator.CaclulateTotalPages(
                db.Authors.Count(), queryOptions.PageSize);

            return authors.ToList();
        }

        public Author GetById(long id)
        {
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                throw new System.Data.Entity.Core.ObjectNotFoundException
                    (string.Format("Unable to find author with id {0}", id));
            }

            return author;
        }

        public Author GetByName(string name)
        {
            Author author = db.Authors
                .Where(a => a.FirstName + ' ' + a.LastName == name)
                .SingleOrDefault();
            if (author == null)
            {
                throw new System.Data.Entity.Core.ObjectNotFoundException
                    (string.Format("Unable to find author with name {0}", name));
            }

            return author;
        }

        public void Insert(Author author)
        {
            db.Authors.Add(author);

            db.SaveChanges();
        }

        public void Update(Author author)
        {
            db.Entry(author).State = EntityState.Modified;

            db.SaveChanges();
        }

        public void Delete(Author author)
        {
            db.Authors.Remove(author);

            db.SaveChanges();
        }

        public void Dispose() {
            db.Dispose();
        }
    }
}