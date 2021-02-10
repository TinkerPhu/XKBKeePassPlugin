using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace XKBKeePassPlugin
{
    /// <summary>
    /// https://stackoverflow.com/questions/2837985/getting-serial-port-information
    /// </summary>
    public class ComPortFinder
    {
        /// <summary>
        /// COM32 - Silicon Labs CP210x USB to UART Bridge (COM32)
        /// </summary>
        /// <returns></returns>
        public static string CollectAllComPortsToString() => CollectAllComPortsToString(CollectAllComPortProps());
        public static string CollectAllComPortsToString(List<Dictionary<string, object>> comPortProps) => string.Join("\n\n", comPortProps.Select(d => string.Join("\n", d.Select(p => $"{p.Key}:{p.Value}"))));

        public static List<Dictionary<string, object>> CollectAllComPortProps()
        {
            var comPortProps = new List<Dictionary<string, object>>();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort")) //"Win32_USBControllerDevice", "Win32_PnPEntity"
            {
                //string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();

                foreach (var p in ports)
                {
                    var dic = new Dictionary<string, object>();

                    dic["DeviceID"]         = p["DeviceID"];
                    dic["Caption"]          = p["Caption"];
                    dic["ProviderType"]     = p["ProviderType"];
                    dic["PNPDeviceID"]      = p["PNPDeviceID"];
                    dic["MaxBaudRate"]      = p["MaxBaudRate"];
                    dic["LastErrorCode"]    = p["LastErrorCode"];
                    dic["ErrorDescription"] = p["ErrorDescription"];
                    dic["Binary"]           = p["Binary"];
                    //same for all items! p.Qualifiers["UUID"]
                    comPortProps.Add(dic);
                }
            }

            return comPortProps;
        }

        public static string FindComPortWithCaption(string caption)
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                //string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();

                var port = ports.Find(p => p["Caption"].ToString().Contains(caption));
                if (null == port)
                {
                    return null;
                }

                return port["DeviceID"].ToString();
            }
        }
        public static string FindComPortWithPNPDeviceID(string pnpDeviceID)
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                //string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();

                var port = ports.Find(p => p["PNPDeviceID"].ToString().Contains(pnpDeviceID));
                if (null == port)
                {
                    return null;
                }

                return port["DeviceID"].ToString();
            }
        }
    }
}