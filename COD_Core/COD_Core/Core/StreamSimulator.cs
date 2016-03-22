using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COD_Base.Interface;

namespace COD_Base.Core
{
    class StreamSimulator : IStreamSimulator
    {

        //这些变量应该由Configuration处获得
        private int _curStreamStep;
        private bool _isProcessOver;
        private int _slideSpan;
        private double _streamRate;
        private int _windowSize;

        public int CurrentStep
        {
            get
            {
                return _curStreamStep;
            }
        }

        public bool IsProcessOver
        {
            get
            {
                return _isProcessOver;
            }
        }

        public int SliedSpan
        {
            get
            {
                return _slideSpan;
            }
        }

        public double StreamRate
        {
            get
            {
                return _streamRate;
            }

            set
            {
                _streamRate = value;
            }
        }

        public int WindowSize
        {
            get
            {
                return _windowSize;
            }
        }

        public void OnNewTupleArrive()
        {
            throw new NotImplementedException();
        }

        public void OnOldTupleDepart()
        {
            throw new NotImplementedException();
        }

        public void OnWindowSlide()
        {
            throw new NotImplementedException();
        }

        public void PerformAStep()
        {
            throw new NotImplementedException();
        }

        public void SendEvent()
        {
            throw new NotImplementedException();
        }

        public void SendEvent(IEvent anEvent)
        {
            throw new NotImplementedException();
        }

        public void StartStreamSimulate()
        {
            throw new NotImplementedException();
        }

        public void PauseSimulate()
        {

        }
    }
}
