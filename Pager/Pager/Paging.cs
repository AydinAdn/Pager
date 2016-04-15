using System;
using System.Collections.Generic;
using System.Linq;

namespace Pager
{
    public class Paging<T> : IPaging<T>
    {
        private readonly IQueryable<T> queryable;
        private int page;
        private int itemsPerPage;

        /// <summary>
        ///     Pages the result of a query.
        /// </summary>
        /// <param name="query">The query to page</param>
        /// <param name="page">The page number</param>
        /// <param name="itemsPerPage">The total number of items per page</param>
        public Paging(IQueryable<T> query, int page, int itemsPerPage) : this(query, page, itemsPerPage, false)
        {
        }

        /// <summary>
        ///     Pages the result of a query.
        /// </summary>
        /// <param name="query">The query to page</param>
        /// <param name="page">The page number</param>
        /// <param name="itemsPerPage">The total number of items per page</param>
        /// <param name="disablePageCount">Disables the total page count, resulting in one less query to the underlying data store</param>
        public Paging(IQueryable<T> query, int page, int itemsPerPage, bool disablePageCount)
        {
            if (query == null) throw new ArgumentNullException("query");

            this.ItemsPerPage = itemsPerPage;
            this.Page = page;
            this.TotalPages = disablePageCount ? -1 : 0;
            this.queryable = query;
            this.GetPagedResult();
        }

        private void GetPagedResult()
        {
            if (this.TotalPages != -1)
            {
                int itemCount = this.queryable.Count();
                int pages = itemCount / this.ItemsPerPage;
                this.TotalPages = itemCount % this.ItemsPerPage > 0 ? pages + 1 : pages;
                this.TotalPages = this.TotalPages == 0 ? 1 : this.TotalPages;
            }

            List<T> result = this.queryable.Skip((this.Page - 1) * this.ItemsPerPage)
                                  .Take(this.ItemsPerPage + 1)
                                  .ToList();

            this.HasMorePages = result.Count() == this.ItemsPerPage + 1;
            this.PagedResult = result.Take(this.ItemsPerPage).ToList();
        }

        public int ItemsPerPage
        {
            get { return this.itemsPerPage; }
            private set { this.itemsPerPage = value < 1 ? 1 : value; }
        }

        public int Page
        {
            get { return this.page; }
            private set { this.page = value < 1 ? 1 : value; }
        }

        public int TotalPages { get; private set; }
        public bool HasMorePages { get; private set; }


        public IList<T> PagedResult { get; private set; }

        public IPaging<T> NextPage()
        {
            if (this.HasMorePages == false) return this;

            this.Page++;
            this.GetPagedResult();

            return this;
        }

        public IPaging<T> PreviousPage()
        {
            if (this.Page == 1) return this;

            this.Page--;
            this.GetPagedResult();
            return this;
        }
    }
}

