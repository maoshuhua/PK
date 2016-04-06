using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tuhui.Common45.Utility
{
    [Serializable]
    public class PagedList
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页个数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总个数
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount { get; protected set; }

        /// <summary>
        /// 开始序号
        /// </summary>
        public int StartRecordIndex { get; protected set; }

        /// <summary>
        /// 结束序号
        /// </summary>
        public int EndRecordIndex { get; protected set; }
    }

    /// <summary>
    /// 分页类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PagedList<T> : PagedList
    {
        /// <summary>
        /// 数据集合
        /// </summary>
        public List<T> PageData { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PagedList()
        {
            PageData = new List<T>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagedList(IQueryable<T> items, int pageIndex, int pageSize)
        {
            if (pageIndex < 1) throw new ArgumentException("pageIndex参数不可以小于1");
            TotalItemCount = items.Count();
            TotalPageCount = TotalItemCount == 0 ? 0 : (int)Math.Ceiling(TotalItemCount / (double)pageSize);
            if (pageIndex > TotalPageCount && TotalPageCount != 0)
            {
                pageIndex = TotalPageCount;
            }
            PageData = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            if (PageData.Count > 0)
            {
                PageData.ForEach(model =>
                {
                    //茅书华
                    //利用反射对string赋值默认值空
                    Type type = typeof(T);
                    PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    if (props.Length > 0)
                    {
                        foreach (PropertyInfo prop in props)
                        {
                            string propType = prop.PropertyType.Name;
                            object obj = prop.GetValue(model, null);
                            if (propType is string)
                            {
                                if (obj == null)
                                {
                                    prop.SetValue(model, "");
                                }
                            }
                        }
                    }
                });
            }

            PageIndex = pageIndex;
            PageSize = pageSize;
            StartRecordIndex = (pageIndex - 1) * pageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : TotalItemCount;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="count"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PagedList(int count, int pageIndex, int pageSize)
        {
            if (pageIndex < 1) throw new ArgumentException("pageIndex参数不可以小于1");
            TotalItemCount = count;
            TotalPageCount = TotalItemCount == 0 ? 0 : (int)Math.Ceiling(TotalItemCount / (double)pageSize);
            if (pageIndex > TotalPageCount && TotalPageCount != 0)
            {
                pageIndex = TotalPageCount;
            }
            
            PageIndex = pageIndex;
            PageSize = pageSize;
            StartRecordIndex = (pageIndex - 1) * pageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : TotalItemCount;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="items"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalItemCount"></param>
        /// <returns></returns>
        public static PagedList<T> CreatePagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            if (pageIndex < 1) throw new ArgumentException("pageIndex参数不可以小于1");
            var _pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            if (pageIndex > _pageCount && _pageCount != 0)
            {
                pageIndex = _pageCount;
            }
            return new PagedList<T>
            {
                TotalItemCount = totalItemCount,
                TotalPageCount = _pageCount,
                PageData = (items ?? new List<T>()).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                StartRecordIndex = (pageIndex - 1) * pageSize + 1,
                EndRecordIndex = totalItemCount > pageIndex * pageSize ? pageIndex * pageSize : totalItemCount
            };
        }

        /// <summary>
        /// 转换类型
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public PagedList<S> ChangeType<S>(Func<T, S> func)
        {
            if (this.PageData == null)
                throw new ArgumentException("PagedList对象不能为空");
            IEnumerable<S> list = this.PageData.Select(p => func(p)).ToList();
            return PagedList<S>.CreatePagedList(list, this.PageIndex, this.PageSize, this.TotalItemCount);
        }

        /// <summary>
        /// 循环处理逻辑
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<T> action)
        {
            this.PageData.ForEach(action);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        public void Add(T model)
        {
            PageData.Add(model);
        }
    }
}