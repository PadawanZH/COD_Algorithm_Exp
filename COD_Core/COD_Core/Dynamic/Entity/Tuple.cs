using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;

namespace COD_Base.Dynamic.Entity
{
    class Tuple : ITuple
    {
        protected int _arrivalStep;
        protected bool _canBeQueried;
        protected List<double> _data;
        protected int _departStep;
        protected bool _isOutlier;

        public int ArrivalStep
        {
            get
            {
                return _arrivalStep;
            }

            set
            {
                _arrivalStep = value;
            }
        }

        public bool CanBeRangeQueried
        {
            get
            {
                return _canBeQueried;
            }

            set
            {
                _canBeQueried = value;
            }
        }

        public List<double> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public int DepartStep
        {
            get
            {
                return _departStep;
            }

            set
            {
                _departStep = value;
            }
        }

        public bool IsOutlier
        {
            get
            {
                return _isOutlier;
            }

            set
            {
                _isOutlier = value;
            }
        }
    }
}
