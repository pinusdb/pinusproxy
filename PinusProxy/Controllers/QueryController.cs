using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PDB.DotNetSDK;
using PinusProxy.Models;
using PinusProxy.Utils;

namespace PinusProxy.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class QueryController : ControllerBase
  {
    [HttpGet("tables")]
    public string QueryTablesGet()
    {
      
      List<TableItem> tabList = TableSet.GetImpl().GetTableList();
      return JsonConvert.SerializeObject(tabList);
    }

    [HttpPost("tables")]
    public string QueryTables()
    {
      List<TableItem> tabList = TableSet.GetImpl().GetTableList();
      return JsonConvert.SerializeObject(tabList);
    }

    [HttpPost("raw")]
    public string QueryRaw([FromBody] RawQueryParam queryParam)
    {
      try
      {
        return ExecuteQuery(queryParam);
      }
      catch (Exception ex)
      {
        return "{ \"err\":1, \"errmsg\": \"" + ex.Message + "\"}";
      }
    }

    [HttpPost("group_devid")]
    public string QueryGroupDevId([FromBody] GroupDevIDParam queryParam)
    {
      try
      {
        return ExecuteQuery(queryParam);
      }
      catch (Exception ex)
      {
        return "{ \"err\":1, \"errmsg\": \"" + ex.Message + "\"}";
      }
    }

    [HttpPost("group_tstamp")]
    public string QueryGroupTstamp([FromBody] GroupTstampParam queryParam)
    {
      try
      {
        return ExecuteQuery(queryParam);
      }
      catch (Exception ex)
      {
        return "{ \"err\":1, \"errmsg\": \"" + ex.Message + "\"}";
      }
    }

    [HttpPost("group_all")]
    public string QueryGroupAll([FromBody] GroupAllParam queryParam)
    {
      try
      {
        return ExecuteQuery(queryParam);
      }
      catch (Exception ex)
      {
        return "{ \"err\":1, \"errmsg\": \"" + ex.Message + "\"}";
      }
    }

    private string ExecuteQuery(QueryParam queryParam)
    {
      TableItem tabItem  = TableSet.GetImpl().GetTableInfo(queryParam.GetTableName());
      if (tabItem == null)
        throw new Exception("table not found");

      string querySql = queryParam.GetSql(tabItem);
      if (string.IsNullOrEmpty(querySql))
        throw new Exception("invalid query param");

      string errMsg = string.Empty;
      DataTable resultTable = null;
      PDBConnection conn = ConnectionPoolSet.GetImpl().GetConnection(tabItem.ServerName);
      try
      {
        PDBCommand cmd = conn.CreateCommand();
        resultTable = cmd.ExecuteQuery(querySql);
      }
      catch (Exception ex)
      {
        errMsg = ex.Message;
      }

      ConnectionPoolSet.GetImpl().BackConnection(tabItem.ServerName, conn, errMsg != string.Empty);
      if (errMsg != string.Empty)
        throw new Exception(errMsg);

      JsonConverter[] pinusConverter = { new DateTime2LongConverter(), new BlobConverter() };
      return "{ \"err\":0, \"data\": " + JsonConvert.SerializeObject(resultTable, pinusConverter) + "}";
    }

  }
}