using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;

namespace COD_Base.Util
{
    class TupleComputation
    {
        public static bool IsInRange(ITuple xTuple, ITuple yTuple, double range, out double result)
        {
            double distance = 0;
            if(xTuple.Dimension != yTuple.Dimension)
            {
                throw new Exception("Util.Compute.GetDistance : dimension of respecting tuple not consistent with each other");
            }
            for(int i = 0; i < xTuple.Dimension; i++)
            {
                distance += (xTuple.Data[i] * xTuple.Data[i]) + (yTuple.Data[i] * yTuple.Data[i]);
                if(distance > range)
                {
                    result = 0;
                    return false;
                }
            }
            result = distance;
            return true;
        }
    }
}
