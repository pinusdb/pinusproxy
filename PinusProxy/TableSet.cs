using PinusProxy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace PinusProxy
{
  public class TableSet
  {
    private TableSet()
    {
      TableDic = new Dictionary<string, TableItem>();
      ServerDic = new Dictionary<string, ServerItem>();
    }

    public void LoadCfg()
    {
      XmlDocument xmlCfg = new XmlDocument();
      xmlCfg.Load(Directory.GetCurrentDirectory() + "/proxy.xml");

      var serverList = xmlCfg.SelectSingleNode("Proxy/ServerSet").ChildNodes;
      foreach (XmlNode node in serverList)
      {
        string serverName = node.Attributes["name"].Value.ToLower();
        string ip = node.Attributes["ip"].Value;
        int port = Convert.ToInt32(node.Attributes["port"].Value);
        string user = node.Attributes["username"].Value;
        string pwd = node.Attributes["password"].Value;
        int maxConn = Convert.ToInt32(node.Attributes["maxconn"].Value);
        if (!ServerDic.ContainsKey(serverName))
        {
          ServerDic.Add(serverName, new ServerItem(serverName, ip, port, user, pwd, maxConn));
        }
      }

      var tableList = xmlCfg.SelectSingleNode("Proxy/TableSet").ChildNodes;
      foreach (XmlNode node in tableList)
      {
        string aliasName = node.Attributes["aliasName"].Value.ToLower();
        string tableName = node.Attributes["tableName"].Value;
        string server = node.Attributes["server"].Value.ToLower();

        if (ServerDic.ContainsKey(server) && !TableDic.ContainsKey(aliasName))
        {
          TableDic.Add(aliasName, new TableItem(aliasName, tableName, server));
        }
      }
    }

    public List<TableItem> GetTableList()
    {
      List<TableItem> tabList = new List<TableItem>();
      foreach (var tabIt in TableDic)
      {
        tabList.Add(tabIt.Value);
      }

      return tabList;
    }

    public TableItem GetTableInfo(string tableName)
    {
      string tmpName = tableName.ToLower();
      if (TableDic.ContainsKey(tmpName))
      {
        return TableDic[tmpName];
      }
      else if (tmpName.EndsWith(".snapshot"))
      {
        tmpName = tmpName.Substring(0, tmpName.Length - 9);
        if (TableDic.ContainsKey(tmpName))
        {
          return new TableItem(tmpName, TableDic[tmpName].TableName + ".snapshot",
            TableDic[tmpName].ServerName);
        }
      }

      return null;
    }

    public ServerItem GetServerInfo(string serverName)
    {
      string tmpName = serverName.ToLower();
      if (ServerDic.ContainsKey(tmpName))
      {
        return ServerDic[tmpName];
      }

      return null;
    }

    public static TableSet GetImpl()
    {
      if (impl_ == null)
        impl_ = new TableSet();

      return impl_;
    }

    private static TableSet impl_;

    private Dictionary<string, TableItem> TableDic;
    private Dictionary<string, ServerItem> ServerDic;
  }
}
