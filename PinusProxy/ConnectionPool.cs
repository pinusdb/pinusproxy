using PDB.DotNetSDK;
using System.Collections.Generic;
using System.Threading;

namespace PinusProxy
{
  public class ConnectionPool
  {
    public ConnectionPool(string connStr, int maxConnCnt)
    {
      connEvent_ = new ManualResetEvent(false);
      connStr_ = connStr;
      curConnCnt_ = 0;
      maxConnCnt_ = maxConnCnt;
      connList_ = new List<PDBConnection>();
    }

    public PDBConnection GetConnection()
    {
      do
      {
        lock (this)
        {
          while (connList_.Count > 0)
          {
            int idx = connList_.Count - 1;
            PDBConnection tmpConn = connList_[idx];
            connList_.RemoveAt(idx);

            if (tmpConn.IsValid())
            {
              return tmpConn;
            }
            else
            {
              tmpConn.Dispose();
              Interlocked.Decrement(ref curConnCnt_);
              connEvent_.Set();
            }
          }

          if (curConnCnt_ < maxConnCnt_)
          {
            PDBConnection conn = new PDBConnection(connStr_);
            conn.Open();
            Interlocked.Increment(ref curConnCnt_);
            return conn;
          }
          else
          {
            connEvent_.Reset();
          }
        }

        connEvent_.WaitOne();
      } while (true);
    }

    public void BackConnection(PDBConnection conn, bool isErr)
    {
      if (!isErr && conn.IsValid())
      {
        lock(this)
        {
          connList_.Add(conn);
        }
      }
      else
      {
        conn.Dispose();
        Interlocked.Decrement(ref curConnCnt_);
      }

      connEvent_.Set();
    }

    private ManualResetEvent connEvent_;
    private string connStr_;
    private int maxConnCnt_;
    private int curConnCnt_;
    private List<PDBConnection> connList_;
  }
}
