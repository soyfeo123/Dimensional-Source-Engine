using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DimensionalSourceEngine
{
    public static class DSEDebug
    {
        public static void InitDebug()
        {
            try
            {


                System.IO.File.Copy(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "latest_log.txt"), System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "previous_log.txt"), true);
                System.IO.File.Delete(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "latest_log.txt"));
                System.IO.File.Create(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "latest_log.txt"));
            }
            catch(Exception ex) { System.Diagnostics.Debug.WriteLine("Failed to init DSEDebug: " + ex.ToString()); }
        } 
        public static void Log(object value)
        {
            try
            {


                System.IO.File.AppendAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "latest_log.txt"), value.ToString() + "\n");
            }
            catch { }
        }
    }
}
