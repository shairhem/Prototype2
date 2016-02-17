using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype2
{
    public class frameInfo
    {
        private int _frameNum;
        private bool _boobDetected;
        private bool _pussDetected;
        private bool _penDetected;
        
        public frameInfo()
        {}

        public frameInfo(int frame, bool boob, bool puss, bool pen)
        {
            _frameNum = frame;
            _boobDetected = boob;
            _pussDetected = puss;
            _penDetected = pen;
        }

        public int frameNum
        {
            get
            {
                return _frameNum;
            }
            set
            {
                _frameNum = value;
            }
        }

        public bool boobDetected
        {
            get
            {
                return _boobDetected;
            }
            set
            {
                _boobDetected = value;
            }
        }

        public bool pussDetected
        {
            get
            {
                return _pussDetected;
            }
            set
            {
                _pussDetected = value;
            }
        }

        public bool penDetected
        {
            get
            {
                return _penDetected;
            }
            set
            {
                _penDetected = value;
            }
        }

    }
}
