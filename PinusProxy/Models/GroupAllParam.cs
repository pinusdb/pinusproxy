using System.Collections.Generic;
using System.Text;

namespace PinusProxy.Models
{
  public class GroupAllParam : QueryParam
  {
    public string Table { get; set; }
    public List<AggregatorField> Fields { get; set; }
    public List<ConditionItem> Conditions { get; set; }

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
      sqlBuilder.AppendFormat(BuildCondition(Conditions));
      return sqlBuilder.ToString();
    }
  }
}
