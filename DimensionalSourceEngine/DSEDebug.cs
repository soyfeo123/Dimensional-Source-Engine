using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DimensionalSourceEngine
{
    public static class DSEDebug
    {
        public static void Log(object value)
        {
            System.IO.File.AppendAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "latest_log.txt"), value.ToString() + "\n");
        }
    }
}
