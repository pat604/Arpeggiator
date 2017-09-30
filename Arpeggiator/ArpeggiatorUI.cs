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

        public Queue<byte> NoteOnEvents { get; set; }
        public Queue<byte> NoteOffEvents { get; set; }
        private List<byte> notesOn { get; set;  } 



        // Updates the UI with the NoteOnEvents
        public void ProcessIdle()
        {

            // ez itt mind logikailag a midiprocessorhoz tartozik

            

            if (NoteOnEvents.Count > 0)
            {
                MessageBox.Show(NoteOnEvents.Count.ToString());
                byte noteNo;

                lock (((ICollection)NoteOnEvents).SyncRoot)
                {
                    noteNo = NoteOnEvents.Dequeue();
                    notesOn.Add(noteNo);             
                }
                DisplayNotes();
            }


            /*
            if (NoteOffEvents.Count > 0)
            {
                byte noteNo;

                lock (((ICollection)NoteOffEvents).SyncRoot)
                {
                    noteNo = NoteOffEvents.Dequeue();
                    notesOn.Remove(noteNo);                    
                }
                DisplayNotes();
            }
            */
        }

        private void DisplayNotes()
        {
            labelNotesOn.Text = "";
            foreach (byte note in notesOn)
            {
                System.Diagnostics.Debug.WriteLine("note is on: " + note.ToString());
                labelNotesOn.Text += note.ToString() + " ";
            }   
        }

        private void ArpeggiatorUI_Load(object sender, EventArgs e)
        {
            // labelNotesOn.Text += " test";
        }
    }
}
