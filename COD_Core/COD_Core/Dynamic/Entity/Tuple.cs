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
        protected List<double> _data;
        protected int _departStep;
        protected bool _isOutlier;
        protected int _dimension;

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

        public int Dimension
        {
            get
            {
                return _dimension;
            }

            set
            {
                _dimension = value;
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

        public int ID
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            _data.Clear();
            _data = null;
        }

        public override bool Equals(object obj)
        {
            if(obj is Tuple)
            {
                Tuple that = (Tuple)obj;
                if(this.Dimension != that.Dimension)
                {
                    return false;
                }
                for (int i = 0; i < Dimension; i++)
                {
                    if(Data[i] != that.Data[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
