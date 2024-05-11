using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BeireMKit.Domain.BaseModels
{
    public class QueryCriteria<T>
    {
        public Expression<Func<T, bool>> Expression { get; set; }
        public List<string> Navigation { get; set; } = new List<string>();
        public bool IgnoreNavigation { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public bool Ascending { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }

        public QueryCriteria() 
        {
        }

        public QueryCriteria(Page? pager)
        {
            if (pager != null)
            {
                Limit = pager.Limit;
                Offset = pager.Offset;
            }
        }

        public QueryCriteria<T> WithExpression(Expression<Func<T, bool>> expression)
        {
            Expression = expression;
            return this;
        }

        public QueryCriteria<T> WithNavigation(IEnumerable<string> navigationProperties)
        {
            Navigation.AddRange(navigationProperties);
            return this;
        }

        public QueryCriteria<T> WithIgnoreNavigation(bool ignoreNavigation = true)
        {
            IgnoreNavigation = ignoreNavigation;
            return this;
        }

        public QueryCriteria<T> WithOrderBy(Expression<Func<T, object>> orderBy, bool ascending = true)
        {
            OrderBy = orderBy;
            Ascending = ascending;
            return this;
        }

        public QueryCriteria<T> WithPaging(int limit, int offset)
        {
            Limit = limit;
            Offset = offset;
            return this;
        }
    }
}
