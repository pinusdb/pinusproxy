namespace PinusProxy.Models
{
  public class TableItem
  {
    public TableItem(string aliasName, string tableName, string serverName)
    {
      this.AliasName = aliasName;
      this.TableName = tableName;
      this.ServerName = serverName;
    }

    /// <summary>
    /// Restful API 使用的表名
    /// </summary>
    public string AliasName { get; private set; }

    /// <summary>
    /// 数据库上的表名
    /// </summary>
    public string TableName { get; private set; }

    /// <summary>
    /// 服务名
    /// </summary>
    public string ServerName { get; private set; }
  }
}
