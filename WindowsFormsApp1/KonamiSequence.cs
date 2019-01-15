using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class KonamiSequence
    {
        private static readonly Keys[] KonamiCode = { Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right, Keys.B, Keys.A };

        private readonly Queue<Keys> _inputKeys = new Queue<Keys>();

        public bool IsCompletedBy(Keys inputKey)
        {
            _inputKeys.Enqueue(inputKey);

            while (_inputKeys.Count > KonamiCode.Length)
                _inputKeys.Dequeue();

            return _inputKeys.SequenceEqual(KonamiCode);
        }
    }
}
