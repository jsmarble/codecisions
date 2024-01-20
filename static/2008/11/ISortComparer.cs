using System;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;

namespace BindingSample
{
    public interface ISortComparer<T> : IComparer<T>
    {
        PropertyDescriptor SortProperty { get; set; }
        ListSortDirection SortDirection { get; set; }
    }
}
