using System.Collections.Generic;
using System.Text;

namespace PinusProxy.Models
{
  public class GroupDevIDParam : QueryParam
  {
    public string Table { get; set; }
    public List<AggregatorField> Fields { get; set; }
    public List<ConditionItem> Conditions { get; set; }
    public int Offset { get; set; }
    public int Limit { get; set; }

    public override string GetTableName()
    {
      return Table;
    }

    public override string GetSql(TableItem tabItem)
    {
      StringBuilder sqlBuilder = new StringBuilder();
      sqlBuilder.Append("SELECT ");
      sqlBuilder.Append(BuildGroupTarget(Fields));
      sqlBuilder.AppendFormat(" FROM {0} ", tabItem.TableName);
      sqlBuilder.Append(BuildCondition(Conditions));
      sqlBuilder.Append(" GROUP BY devid ");
      if (Offset > 0 || Limit > 0)
      {
        sqlBuilder.AppendFormat(" LIMIT {0},{1} ",
          Offset > 0 ? Offset : 0,
          Limit > 0 ? Limit : 1000);
      }

      return sqlBuilder.ToString();
    }
  }
}
