using System.Net.NetworkInformation;
using System.Text;

namespace TheIdealShip.Net;

public static class PingUtils
{
    public static PingInfo Ping(string url)
    {
        log.Msg($"ping {url}", Utils.MethodUtils.GetVoidName(), Utils.MethodUtils.GetClassName());

        Ping ping = new Ping();
        PingReply reply = ping.Send(url);

        StringBuilder stringB = new StringBuilder();
        string status;
        status = reply.Status switch
        {
            IPStatus.Success => "成功",
            IPStatus.TimedOut =>  "超时",
            _ => "失败",
        };
        stringB.AppendLine("状态：" + status);

        if (reply.Status == IPStatus.Success)
        {
            stringB.AppendLine(string.Format("Ip地址: {0} ", reply.Address.ToString()));
            stringB.AppendLine(string.Format("ping时间: {0} ", reply.Options.Ttl));
            stringB.AppendLine(string.Format("ping包大小: {0} ", reply.Buffer.Length));
            stringB.AppendLine(string.Format("往返时间: {0} ", reply.RoundtripTime));
        }

        return new PingInfo(reply.Address.ToString(), reply.Options.Ttl.ToString(), reply.Buffer.Length.ToString(), reply.RoundtripTime.ToString(), stringB);
    }
}

public class PingInfo
{
    public string ip;
    public string pingTime;
    public string size;
    public string roundTripTime;
    public StringBuilder stringB;

    public PingInfo
    (string ip, string pingTime, string size = "", string roundTripTime = "", StringBuilder stringB = null)
    {
        this.ip = ip;
        this.pingTime = pingTime;
        this.size = size;
        this.roundTripTime = roundTripTime;
        this.stringB = stringB;
    }
}