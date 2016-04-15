using System.Collections.Generic;

namespace Pager
{
    public interface IPaging<T>
    {
        /// <summary>
        ///     Total number of pages
        /// </summary>
        int TotalPages { get; }

        int ItemsPerPage { get; }

        /// <summary>
        ///     The current page number
        /// </summary>
        int Page { get; }

        /// <summary>
        ///     Returns whether any more pages exist
        /// </summary>
        bool HasMorePages { get; }

        IList<T> PagedResult { get; }

        IPaging<T> NextPage();

        IPaging<T> PreviousPage();
    }
}