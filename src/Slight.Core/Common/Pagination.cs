using System;
using System.Collections.Generic;

namespace Slight.Core
{
    /// <summary>
    /// struct of <see cref="Pagination{Target}"/>
    /// </summary>
    /// <typeparam name="Target"></typeparam>
    public struct Pagination<Target>
    {
        /// <summary>
        /// data 
        /// </summary>
        public IEnumerable<Target> Data { get; set; }

        /// <summary>
        /// total pages
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (TotalCount <= 0 || PageSize <= 0)
                {
                    return 1;
                }

                return ((int)Math.Ceiling((double)TotalCount / PageSize)).FromRange(1, int.MaxValue);
            }
        }

        /// <summary>
        /// total count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// current page
        /// </summary>
        public int CurrentPage { get; set; }

        
        /// <summary>
        /// page size
        /// </summary>
        public int PageSize { get; set; }


        /// <summary>
        /// condition
        /// </summary>
        public object Condition { get; set; }

        /// <summary>
        /// get <see cref="Pagination"/> not has data
        /// </summary>
        /// <returns></returns>
        public Pagination GetPagination()
        {
            return new Pagination()
            {
                TotalCount = TotalCount,
                CurrentPage = CurrentPage,
                PageSize = PageSize,
                Condition = Condition,
            };
        }
    }


    /// <summary>
    /// <see cref="Pagination"/>
    /// </summary>
    public struct Pagination
    {
        /// <summary>
        /// total pages
        /// </summary>
        public int TotalPage
        {
            get
            {
                if (TotalCount <= 0 || PageSize <= 0)
                {
                    return 1;
                }

                return ((int)Math.Ceiling((double)TotalCount / PageSize)).FromRange(1, int.MaxValue);
            }
        }

        /// <summary>
        /// total count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// current page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// condition
        /// </summary>
        public object Condition { get; set; }
    }
}
