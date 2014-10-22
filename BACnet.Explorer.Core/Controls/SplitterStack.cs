using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto;
using Eto.Forms;

namespace BACnet.Explorer.Core.Controls
{
    public class SplitterStack : Panel
    {
        public const int MaxWidth = 10;

        /// <summary>
        /// The orientation of the stack
        /// </summary>
        public SplitterOrientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }
        private SplitterOrientation _orientation;

        private TableLayout _layout;
        private List<Entry> _entries;

        public SplitterStack()
        {
            Orientation = SplitterOrientation.Horizontal;

            _layout = new TableLayout(MaxWidth, 1);
            for (int i = 0; i < MaxWidth; i++)
                _layout.Add(null, i, 0);

            _entries = new List<Entry>();
            this.Content = _layout;
        }

        /// <summary>
        /// Pushes a new control onto the stack
        /// </summary>
        /// <param name="control"></param>
        public void Push(Control control, bool scaleIfLast = true)
        {
            var entry = new Entry(control, scaleIfLast);
            _entries.Add(entry);

            if (_entries.Count > 1)
                _layout.Rows[0].Cells[_entries.Count - 2].ScaleWidth = false;

            _layout.Add(
                control,
                _entries.Count - 1,
                0,
                scaleIfLast,
                false);
        }

        /// <summary>
        /// Pops a control off of the splitter stack
        /// </summary>
        public void Pop()
        {
            if (_entries.Count > 0)
            {
                _layout.Add(
                    new Panel(),
                    _entries.Count - 1,
                    0,
                    false,
                    false);

                _entries.RemoveAt(_entries.Count - 1);
            }
        }

        /// <summary>
        /// Pops controls off the stack until
        /// a control with a certain type is reached
        /// </summary>
        /// <typeparam name="T">The control type to search for</typeparam>
        public void PopUntil<T>() where T : Control
        {
            while(_entries.Count > 0)
            {
                var last = _entries[_entries.Count - 1];
                if(last.Control is T)
                {
                    break;
                }
                else
                {
                    Pop();
                }
            }
        }

        public struct Entry
        {
            public Control Control { get; private set; }

            public bool ScaleIfLast { get; private set; }

            public Entry(Control control, bool scaleIfLast) : this()
            {
                this.Control = control;
                this.ScaleIfLast = scaleIfLast;
            }
        }

    }
}
