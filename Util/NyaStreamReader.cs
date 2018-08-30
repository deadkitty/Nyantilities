using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyantilities.Util
{
    public class NyaStreamReader : StreamReader
    {
        private int currentLine = 0;

        public int CurrentLine
        {
            get { return currentLine; }
        }

        public NyaStreamReader(Stream stream)
            : base(stream)
        {

        }

        public override string ReadLine()
        {
            ++currentLine;
            return base.ReadLine();
        }
    }
}
