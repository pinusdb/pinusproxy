using System.Collections.Generic;
using System.Text;

namespace PinusProxy.Models
{
  public abstract class QueryParam
  {
    public abstract string GetTableName();
    public abstract string GetSql(TableItem tabItem);

    protected string BuildGroupTarget(List<AggregatorField> targetList)
    {
      if (targetList == null)
        throw new System.Exception("sql error");

      if (targetList.Count == 0)
        throw new System.Exception("sql error");

      StringBuilder targetBuilder = new StringBuilder();
      foreach (var field in targetList)
      {
        if (field.Aggregator.ToLower().Equals("none"))
        {
          targetBuilder.Append(field.Field);
        }
        else
        {
          targetBuilder.AppendFormat(" {0}({1})", field.Aggregator, field.Field);
        }

        if (!string.IsNullOrEmpty(field.Alias))
        {
          targetBuilder.AppendFormat(" AS {0}", field.Alias);
        }
        targetBuilder.Append(",");
      }

      return targetBuilder.ToString(0, targetBuilder.Length - 1);
    }
  
    protected string BuildFieldTarget(List<string> targetList)
    {
      if (targetList == null)
        throw new System.Exception("sql error");

      if (targetList.Count == 0)
        throw new System.Exception("sql error");

      StringBuilder targetBuilder = new StringBuilder();
      foreach (string target in targetList)
      {
        targetBuilder.Append(target);
        targetBuilder.Append(",");
      }

      return targetBuilder.ToString(0, targetBuilder.Length - 1);
    }

    protected string BuildCondition(List<ConditionItem> condiList)
    {
      if (condiList == null)
        return string.Empty;

      if (condiList.Count == 0)
        return string.Empty;

      StringBuilder conditionBuilder = new StringBuilder();
      conditionBuilder.Append(" WHERE ");
      foreach (var condition in condiList)
      {
        conditionBuilder.Append(condition.GetConditionSql());
        conditionBuilder.Append(" AND ");
      }

      return conditionBuilder.ToString(0, conditionBuilder.Length - 4);
    }
  }


}
