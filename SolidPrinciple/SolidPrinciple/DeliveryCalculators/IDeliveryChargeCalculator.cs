using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCourierApp.DeliveryCalculators
{
    public interface IDeliveryChargeCalculator
    {
        double CalculateCharge(double weight);
    }
}