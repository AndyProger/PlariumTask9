using System;
using LetterSpace;

namespace VariantB
{
    class SendLetterEventArgs : EventArgs
    {
        public Letter Letter { get; set; }
    }

    delegate void SendLetterHandler(object source, SendLetterEventArgs arg);
}
