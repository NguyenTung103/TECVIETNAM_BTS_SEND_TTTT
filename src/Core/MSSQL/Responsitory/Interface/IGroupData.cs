using BtsGetwayService.Core;
using BtsGetwayService.MSSQL.Entity;
using System.Collections.Generic;

namespace Core.MSSQL.Responsitory.Interface
{
    public interface IGroupData : IDapperBaseData<RegionalGroup>
    {
        IEnumerable<RegionalGroup> GetGroupSend();
        IEnumerable<RegionalGroup> GetAll();
    }
}
