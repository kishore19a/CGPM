using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CGPMFinal
{
    class Word
    {
        public Word() { }
        public string Text { get; set; }
        public string AttachedText { get; set; }
        public bool IsShellCommand { get; set; }
    }
}
