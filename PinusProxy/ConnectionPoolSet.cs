using PDB.DotNetSDK;
using PinusProxy.Models;
using System;
using System.Collections.Generic;

namespace PinusProxy
{
  public class ConnectionPoolSet
  {
    private ConnectionPoolSet()
    {
      connPoolDic_ = new Dictionary<string, ConnectionPool>();
    }

    public PDBConnection GetConnection(string serverName)
    {
      string tmpName = serverName.ToLower();
      if (!connPoolDic_.ContainsKey(tmpName))
      {
        ServerItem serItem = TableSet.GetImpl().GetServerInfo(tmpName);
        if (serItem == null)
        {
          throw new Exception("server not found");
        }

        string connStr = string.Format("server={0};port={1};username={2};password={3}",
          serItem.IP, serItem.Port, serItem.User, serItem.Pwd);

        lock (this)
        {
          if (!connPoolDic_.ContainsKey(tmpName))
          {
            connPoolDic_.Add(tmpName, new ConnectionPool(connStr, serItem.MaxConn));
          }
        }
      }
      
      return connPoolDic_[tmpName].GetConnection();
    }

    public void BackConnection(string serverName, PDBConnection conn, bool isErr)
    {
      string tmpName = serverName.ToLower();
      if (connPoolDic_.ContainsKey(tmpName))
      {
        connPoolDic_[tmpName].BackConnection(conn, isErr);
      }
    }

    public static ConnectionPoolSet GetImpl()
    {
      if (impl_ == null)
        impl_ = new ConnectionPoolSet();

      return impl_;
    }

    private static ConnectionPoolSet impl_;
    private Dictionary<string, ConnectionPool> connPoolDic_;
  }
}
