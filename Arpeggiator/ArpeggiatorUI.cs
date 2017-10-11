using Jacobi.Vst.Core;
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
            notesOn = new List<byte>();
        }

        public Queue<byte> NoteOnNumbers { get; set; }
        public Queue<byte> NoteOffNumbers { get; set; }
        private List<byte> notesOn { get; set;  } 



        // Updates the UI with the NoteOnEvents
        public void ProcessIdle()
        {
            
            if (NoteOnNumbers.Count > 0)
            {
                byte noteNo;

                lock (((ICollection)NoteOnNumbers).SyncRoot)
                {
                    noteNo = NoteOnNumbers.Dequeue();
                    notesOn.Add(noteNo);
                    DisplayNotes();
                }               
            }


            if (NoteOffNumbers.Count > 0)
            {
                byte noteNo;

                lock (((ICollection)NoteOffNumbers).SyncRoot)
                {
                    noteNo = NoteOffNumbers.Dequeue();
                    notesOn.Remove(noteNo);
                    DisplayNotes();
                }         
            }
        }


        private void DisplayNotes()
        {
            labelNotesOn.Text = "";
            foreach (byte note in notesOn)
            {
                labelNotesOn.Text += note.ToString() + " ";
            }
        }

    }
}
