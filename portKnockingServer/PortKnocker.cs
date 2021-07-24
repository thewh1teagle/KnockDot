using System;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portKnockingServer
{
    class PortKnocker
    {
        private int period;
        private List<int> sequence;
        private int sequenceIndex;
        private Stopwatch firstKnockTimeWatch;

        public PortKnocker(List<int> sequence, int period)
            // period in seconds
        {
            this.period = period;
            this.sequence = sequence;
            this.sequenceIndex = 0;
            // firstKnockTimeWatch = 0;
        }

        public bool check(int port)
        {
            if (this.sequenceIndex == 0 && port == this.sequence[this.sequenceIndex]) // first correct knocking
            {   
                this.firstKnockTimeWatch = System.Diagnostics.Stopwatch.StartNew();
                this.sequenceIndex = 1;
                return false;
            }
            else if (port == this.sequence[this.sequenceIndex] &&  firstKnockTimeWatch.ElapsedMilliseconds / 1000.0 <  this.period )
            {
                this.sequenceIndex += 1;

                // Check if it's the winning knock
                if (this.sequenceIndex >= this.sequence.Count)
                {
                    this.sequenceIndex = 0;
                    return true;
                }
                return false;
            }
            this.sequenceIndex = 0;
            return false;   
        }
    }
}
