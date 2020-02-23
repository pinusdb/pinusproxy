using System.Collections.Generic;
using System.Text;

namespace PinusProxy.Models
{
  public class GroupTstampParam : QueryParam
  {
    public string Table { get; set; }
    public List<AggregatorField> Fields { get; set; }
    public List<ConditionItem> Conditions { get; set; }
    public long GroupTstamp { get; set; }
    public long Offset { get; set; }
    public long Limit { get; set; }

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
      sqlBuilder.AppendFormat(" GROUP BY tstamp {0}s ", GroupTstamp);
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
