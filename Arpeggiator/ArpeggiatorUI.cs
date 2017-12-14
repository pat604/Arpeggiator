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

            this.comboBoxRythm1.SelectedIndex = 0;
            this.comboBoxRythm2.SelectedIndex = 0;
            this.comboBoxRythm3.SelectedIndex = 0;
            this.comboBoxRythm4.SelectedIndex = 0;
            this.comboBox5.SelectedIndex = 0;
            this.comboBox6.SelectedIndex = 0;
            this.comboBox7.SelectedIndex = 0;
            this.comboBox8.SelectedIndex = 0;
            this.comboBoxProbability.SelectedIndex = 0;

            Init();
        }

        // ennek az osztálynak a láthatóságok miatt nem lehet Plugin tagja


        private void Init()
        {
            _notesOn = new List<byte>();

            BindingSource bs = new BindingSource();
            bs.DataSource = Enum.GetNames(typeof(Directions));
            comboBoxDirection.DataSource = bs.DataSource;

            bs.DataSource = _octaves;
            comboBoxOctaves.DataSource = bs.DataSource;

            for (int i = 0; i < 4; i++)
            {
                _noteLengths[i] = NoteLengths.quarter;              
            }
        }

        #region InformationPanel

        public Queue<byte> NoteOnNumbers { get; set; }
        public Queue<byte> NoteOffNumbers { get; set; }
        private List<byte> _notesOn { get; set; }


        // Updates the UI with the NoteOnEvents
        public void ProcessIdle()
        {

            if (NoteOnNumbers.Count > 0)
            {
                byte noteNo;

                lock (_notesOn)
                {
                    noteNo = NoteOnNumbers.Dequeue();
                    _notesOn.Add(noteNo);
                }
                displayNotes();
            }


            if (NoteOffNumbers.Count > 0)
            {
                byte noteNo;

                lock (_notesOn)
                {
                    noteNo = NoteOffNumbers.Dequeue();
                    _notesOn.RemoveAll(n => n.Equals(noteNo));
                }
                displayNotes();
            }
        }


        private void displayNotes()
        {
            labelNotesOn.Text = "";
            foreach (byte note in _notesOn)
            {
                labelNotesOn.Text += convertNote(note) + "  ";
            }
        }

        private string convertNote(byte note)
        {
            string result = "";
          
            switch ((int) (note % 12))
            {
                case 0:
                    result += "C";
                    break;
                case 1:
                    result += "C#";
                    break;
                case 2:
                    result += "D";
                    break;
                case 3:
                    result += "D#";
                    break;
                case 4:
                    result += "E";
                    break;
                case 5:
                    result += "F";
                    break;
                case 6:
                    result += "F#";
                    break;
                case 7:
                    result += "G";
                    break;
                case 8:
                    result += "G#";
                    break;
                case 9:
                    result += "A";
                    break;
                case 10:
                    result += "A#";
                    break;
                case 11:
                    result += "B";
                    break;
            }

            int octave = (int)note / 12;
            result += octave;

            return result;
        }

        #endregion

        #region Direction

        public event EventHandler<DirectionEventArgs> DirectionSelected;

        private void comboBoxDirection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Directions result;
            Enum.TryParse<Directions>(comboBoxDirection.SelectedValue.ToString(), out result);
            DirectionEventArgs d = new DirectionEventArgs();
            d.Direction = result;

            if (DirectionSelected != null)
            {
                DirectionSelected(this, d);
            }
        }

        #endregion

        #region Octaves 
        private static int[] _octaves = new int[] { 0, -1, +1 };

        public event EventHandler<OctaveEventArgs> OctaveSelected;

        private void comboBoxOctaves_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int selectedOctave = (int)comboBoxOctaves.SelectedValue;
            OctaveEventArgs o = new OctaveEventArgs();
            o.Octave = selectedOctave;

            if (OctaveSelected != null)
            {
                OctaveSelected(this, o);
            }

        }

        #endregion

        #region Rythm

        private void comboBox1_DrawItem_1(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index > -1 && imageList1.Images.Count >= e.Index)
                e.Graphics.DrawImage(imageList1.Images[e.Index], new PointF(e.Bounds.X, e.Bounds.Y));
        }

        private void comboBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index > -1 && imageList1.Images.Count >= e.Index)
                e.Graphics.DrawImage(imageList1.Images[e.Index], new PointF(e.Bounds.X, e.Bounds.Y));
        }

        private void comboBox3_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index > -1 && imageList1.Images.Count >= e.Index)
                e.Graphics.DrawImage(imageList1.Images[e.Index], new PointF(e.Bounds.X, e.Bounds.Y));
        }

        private void comboBox4_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index > -1 && imageList1.Images.Count >= e.Index)
                e.Graphics.DrawImage(imageList1.Images[e.Index], new PointF(e.Bounds.X, e.Bounds.Y));
        }


        private NoteLengths[] _noteLengths = new NoteLengths[4];


        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // MessageBox.Show(Convert.ToInt32(comboBoxRythm1.SelectedItem).ToString());

            _noteLengths[0] = (NoteLengths) Convert.ToInt32(comboBoxRythm1.SelectedItem);

            // MessageBox.Show(_noteLengths[0].ToString());

            sendRythmEvent(sender, e);
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _noteLengths[1] = (NoteLengths)Convert.ToInt32(comboBoxRythm2.SelectedItem);

            sendRythmEvent(sender, e);

        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _noteLengths[2] = (NoteLengths)Convert.ToInt32(comboBoxRythm3.SelectedItem);

            sendRythmEvent(sender, e);
        }

        private void comboBox4_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _noteLengths[3] = (NoteLengths)Convert.ToInt32(comboBoxRythm4.SelectedItem);

            sendRythmEvent(sender, e);
        }

        public event EventHandler<RythmEventArgs> RythmSelected;

        private void sendRythmEvent(object sender, EventArgs e)
        {
            RythmEventArgs r = new RythmEventArgs();
            r.NoteLengthsArray = _noteLengths;

            if (RythmSelected != null)
            {
                RythmSelected(this, r);
            }
               
        }

        #endregion

        #region Accent

        Accents[] _accents = new Accents[4];

        private void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Accents result;
            Enum.TryParse<Accents>(comboBox5.SelectedItem.ToString(), out result);
            _accents[0] = result;

           // MessageBox.Show(_accents[0].ToString());

            sendAccentEvent(sender, e);

        }

        private void comboBox6_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Accents result;
            Enum.TryParse<Accents>(comboBox6.SelectedItem.ToString(), out result);
            _accents[1] = result;

            sendAccentEvent(sender, e);

        }

        private void comboBox7_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Accents result;
            Enum.TryParse<Accents>(comboBox7.SelectedItem.ToString(), out result);
            _accents[2] = result;

            sendAccentEvent(sender, e);

        }

        private void comboBox8_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Accents result;
            Enum.TryParse<Accents>(comboBox8.SelectedItem.ToString(), out result);
            _accents[3] = result;

            sendAccentEvent(sender, e);
        }


        public event EventHandler<AccentEventArgs> AccentSelected;

        private void sendAccentEvent(object sender, EventArgs e)
        {
            AccentEventArgs a = new AccentEventArgs();
            a.AccentsArray = _accents;

            if (AccentSelected != null)
            {
                AccentSelected(this, a);
            }
        }



        #endregion


        #region Swing

        public event EventHandler<SwingEventArgs> SwingSelected;


        private void checkBoxSwing_CheckedChanged(object sender, EventArgs e)
        {
            bool selectedSwing = checkBoxSwing.Checked;
            SwingEventArgs s = new SwingEventArgs();
            s.Swing = selectedSwing;

            if (SwingSelected != null)
            {
                SwingSelected(this, s);
            }
        }

        #endregion

        #region Note Probability

        public event EventHandler<ProbabilityEventArgs> ProbabilitySelected;

        private void comboBoxProbability_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int selectedProbability = Convert.ToInt32(comboBoxProbability.SelectedItem);
            ProbabilityEventArgs p = new ProbabilityEventArgs();
            p.Probability = selectedProbability;

            if (ProbabilitySelected != null)
            {
                ProbabilitySelected(this, p);
            }
        }

        #endregion
    }





    public class DirectionEventArgs : EventArgs
    {
        public Directions Direction { get; set; }
    }

    public class OctaveEventArgs : EventArgs
    {
        public int Octave { get; set; }
    }

    public class RythmEventArgs : EventArgs
    {
        public NoteLengths[] NoteLengthsArray { get; set; }
    }

    public class AccentEventArgs : EventArgs
    {
        public Accents[] AccentsArray { get; set; }
    }

    public class SwingEventArgs : EventArgs
    {
        public bool Swing { get; set; }
    }

    public class ProbabilityEventArgs : EventArgs
    {
        public int Probability { get; set; }
    }


}
