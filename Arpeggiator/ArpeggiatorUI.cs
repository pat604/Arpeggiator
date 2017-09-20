using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arpeggiator
{
    public partial class ArpeggiatorUI : Form
    {
        public ArpeggiatorUI()
        {
            InitializeComponent();
        }

        public Queue<byte> NoteOnEvents { get; set; }

        
        // Updates the UI with the NoteOnEvents
        public void ProcessIdle()
        {
            if (NoteOnEvents.Count > 0)
            {
                byte noteNo;

                lock (((ICollection)NoteOnEvents).SyncRoot)
                {
                    noteNo = NoteOnEvents.Dequeue();
                }

                // SelectNoteMapItem(noteNo);
                DisplayNote();
                
            }
        }

        private void DisplayNote()
        {


        }

    }
}
