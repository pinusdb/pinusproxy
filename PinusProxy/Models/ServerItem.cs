namespace PinusProxy.Models
{
  public class ServerItem
  {
    public ServerItem(string serverName, string ip, int port, string user, string pwd, int maxConn)
    {
      this.ServerName = serverName;
      this.IP = ip;
      this.Port = port;
      this.User = user;
      this.Pwd = pwd;
      this.MaxConn = maxConn;
    }

    /// <summary>
    /// 服务名
    /// </summary>
    public string ServerName { get; private set; }

    /// <summary>
    /// IP地址
    /// </summary>
    public string IP { get; private set; }

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; private set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string User { get; private set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Pwd { get; private set; }

    /// <summary>
    /// 最大数据库连接
    /// </summary>
    public int MaxConn { get; private set; }
  }
}
