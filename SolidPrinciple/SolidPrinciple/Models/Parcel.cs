using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCourierApp.Models
{
    public class Parcel
    {
        public double Weight { get; set; }
        public string SourceCity { get; set; }
        public string DestinationCity { get; set; }
    }
}