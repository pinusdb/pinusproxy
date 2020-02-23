using System;

namespace PinusProxy.Models
{
  public class ConditionItem
  {
    public string Field { get; set; }
    public string Op { get; set; }
    public object Value { get; set; }

    public string GetConditionSql()
    {
      if (string.IsNullOrEmpty(Field) || string.IsNullOrEmpty(Op))
        return string.Empty;

      string op = Op.ToLower();
      if (op == "isnull" || op == "isnotnull")
      {
        return string.Format(" {0} {1} ", Field, op == "isnull" ? "IS NULL" : "IS NOT NULL");
      }

      if (Value == null)
        throw new Exception("condition error");

      System.Text.Json.JsonElement jsonVal = (System.Text.Json.JsonElement)Value;
      string valStr = jsonVal.ToString();
      if (jsonVal.ValueKind == System.Text.Json.JsonValueKind.String)
      {
        valStr = "'" + valStr.Replace("'", "''") + "'";
      }
      else if (jsonVal.ValueKind == System.Text.Json.JsonValueKind.Array)
      {
        valStr = valStr.Substring(1, valStr.Length - 2);
      }

      switch (Op.ToLower())
      {
        case "eq":
          return string.Format(" {0} = {1} ", Field, valStr);
        case "ne":
          return string.Format(" {0} <> {1} ", Field, valStr);
        case "lt":
          return string.Format(" {0} < {1} ", Field, valStr);
        case "le":
          return string.Format(" {0} <= {1} ", Field, valStr);
        case "gt":
          return string.Format(" {0} > {1} ", Field, valStr);
        case "ge":
          return string.Format(" {0} >= {1} ", Field, valStr);
        case "like":
          return string.Format(" {0} like {1} ", Field, valStr);
        case "in":
          return string.Format(" {0} in ({1}) ", Field, valStr);
        case "notin":
          return string.Format(" {0} not in ({1})", Field, valStr);
      }

      throw new Exception("condition error");
    }
  }
}
