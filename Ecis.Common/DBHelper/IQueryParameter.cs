#region

using NLog;
using System.Data;

#endregion

namespace Ecis.Common
{
    public interface IQueryParameter
    {
        int CurrentPage { get; set; }
        int PageSize { get; set; }
        int RowCount { get; }
        int PageCount { get; }

        DataSet FillDataSet(string dbConnectionString, ILogger log);
    }
}