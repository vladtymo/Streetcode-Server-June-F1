using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.XUnitTest.MediatRTests.Term_Testing.Mocker
{
    public class MockTermRepo : ITermRepository
    {
        public static IEnumerable<Term> Terms = new List<Term>()
        {
            new Term(){ Id = 1 },
            new Term(){ Id = 2 },
            new Term(){ Id = 3 },
        };

        public void Attach(Term entity)
        {
            throw new NotImplementedException();
        }

        public Term Create(Term entity)
        {
            throw new NotImplementedException();
        }

        public Task<Term> CreateAsync(Term entity)
        {
            throw new NotImplementedException();
        }

        public Task CreateRangeAsync(IEnumerable<Term> items)
        {
            throw new NotImplementedException();
        }

        public void Delete(Term entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange(IEnumerable<Term> items)
        {
            throw new NotImplementedException();
        }

        public void Detach(Term entity)
        {
            throw new NotImplementedException();
        }

        public EntityEntry<Term> Entry(Term entity)
        {
            throw new NotImplementedException();
        }

        public Task ExecuteSqlRaw(string query)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Term> FindAll(Expression<Func<Term, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Term>> GetAllAsync(Expression<Func<Term, bool>>? predicate = null, Func<IQueryable<Term>, IIncludableQueryable<Term, object>>? include = null)
        {
            return await Task.FromResult(Terms);
        }

        public async Task<IEnumerable<Term>?> GetAllAsync(Expression<Func<Term, Term>> selector, Expression<Func<Term, bool>>? predicate = null, Func<IQueryable<Term>, IIncludableQueryable<Term, object>>? include = null)
        {
            return await Task.FromResult(Terms);
        }

        public async Task<Term?> GetFirstOrDefaultAsync(Expression<Func<Term, bool>>? predicate = null, Func<IQueryable<Term>, IIncludableQueryable<Term, object>>? include = null)
        {
            var query = Terms.BuildMock<Term>();

            if (predicate is not null)
                query = query.Where(predicate);

            if (include is not null)
                query = include(query);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Term?> GetFirstOrDefaultAsync(Expression<Func<Term, Term>> selector, Expression<Func<Term, bool>>? predicate = null, Func<IQueryable<Term>, IIncludableQueryable<Term, object>>? include = null)
        {
            var query = Terms.BuildMock<Term>();

            if (predicate is not null)
                query = query.Where(predicate);

            if (include is not null)
                query = include(query);

            return await query.FirstOrDefaultAsync();
        }

        public Task<Term?> GetSingleOrDefaultAsync(Expression<Func<Term, bool>>? predicate = null, Func<IQueryable<Term>, IIncludableQueryable<Term, object>>? include = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Term> Include(params Expression<Func<Term, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public EntityEntry<Term> Update(Term entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<Term> items)
        {
            throw new NotImplementedException();
        }
    }
}
