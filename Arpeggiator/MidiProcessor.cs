using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Host;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using Jacobi.Vst.Framework.Common;

namespace Arpeggiator
{

    public enum Directions { up, down, cyclic_up_down, cyclic_down_up, terraced, random }

    public enum NoteLengths { quarter = 1, eighth_eighth = 2, rest = 3, eighth_rest = 4, rest_eighth = 5, triplet = 6 };


    public struct NoteLengthSamples
    {
        public bool note;          // true: note, false: rest
        public double frames;     // for how many samples

        public NoteLengthSamples(bool note, double frames)
        {
            this.note = note;
            this.frames = frames;
        }
    }

    public enum Accents { none = 1, downbeat, upbeat } 


    // Implements the incoming Midi event handling for the plugin.
    class MidiProcessor : IVstMidiProcessor
    {
        private Plugin _plugin;

        // Gets the midi events that should be processed in the current cycle. 
        public VstEventCollection Events { get; private set; }

        // Gets or sets a value indicating wether non-mapped midi events should be passed to the output.    
        public bool MidiThru { get; set; }

        // The raw note on note numbers
        public Queue<byte> NoteOnNumbers { get; private set; }

        // eredetileg milyen hangokat adott be a user éppen
        private List<VstMidiEvent> _noteOnEvents { get; set; }

        private List<VstMidiEvent> _octavesAddedOrderedNoteOnEvents { get; set; }

        // The raw note off note numbers
        public Queue<byte> NoteOffNumbers { get; private set; }

        private VstTimeInfo _timeInfo;

        public Directions Direction { get; set; }

        public int Octave { get; set; }

        public NoteLengths[] NoteLengthsArray { get; set; }



        public MidiProcessor(Plugin plugin)
        {
            _plugin = plugin;
            Events = new VstEventCollection();
            NoteOnNumbers = new Queue<byte>();
            NoteOffNumbers = new Queue<byte>();
            _noteOnEvents = new List<VstMidiEvent>();
            _octavesAddedOrderedNoteOnEvents = new List<VstMidiEvent>();
            _rythm = new List<NoteLengthSamples>();
            NoteLengthsArray = new NoteLengths[4];

            for (int i = 0; i < 4; i++)
            {
                NoteLengthsArray[i] = NoteLengths.quarter;
            }
            CountRythm();
        }


        public int ChannelCount
        {
            get { return _plugin.ChannelCount; }
        }


        public void Process(VstEventCollection events)
        {
            foreach (VstEvent evnt in events)
            {
                if (evnt.EventType != VstEventTypes.MidiEvent) continue;

                VstMidiEvent midiEvent = (VstMidiEvent)evnt;


                //  note on     
                if ((midiEvent.Data[0] & 0xF0) == 0x90)
                {
                    // note off
                    // You can treat note-on midi events with a velocity of 0 (zero) as a note-off midi event.             
                    if (midiEvent.Data[2] == 0)
                    {
                        lock (((ICollection)NoteOffNumbers).SyncRoot)
                        {
                            lock (((ICollection)_noteOnEvents).SyncRoot)
                            {
                                lock (((ICollection)_octavesAddedOrderedNoteOnEvents).SyncRoot)
                                {
                                    NoteOffNumbers.Enqueue(midiEvent.Data[1]);

                                    /* FOR TESTING ADDOCTAVES()
                                    string notes = "";
                                    foreach (VstMidiEvent e in _noteOnEvents)
                                        notes += e.Data[1] + " ";

                                    MessageBox.Show("Process method: \nNoteOnEvents: " + notes);
                                    */

                                    _noteOnEvents.RemoveAll(n => n.Data[1] == midiEvent.Data[1]);

                                    /* FOR TESTING ADDOCTAVES()
                                    notes = "";
                                    foreach (VstMidiEvent e in _noteOnEvents)
                                        notes += e.Data[1] + " ";
                                    MessageBox.Show("Process method: \nNoteOnEvents after removing Note Off event: " + notes);
                                    */

                                    // AddOctaves();
                                    _octavesAddedOrderedNoteOnEvents = makeCopy(_noteOnEvents);

                                    OrderNoteOnEventsWithOctaves();
                                }
                            }

                        }
                    }

                    // note on
                    else
                    {
                        lock (((ICollection)NoteOnNumbers).SyncRoot)
                        {
                            lock (((ICollection)_noteOnEvents).SyncRoot)
                            {
                                lock (((ICollection)_octavesAddedOrderedNoteOnEvents).SyncRoot)
                                {
                                    NoteOnNumbers.Enqueue(midiEvent.Data[1]);

                                    /* FOR TESTING ADDOCTAVES()
                                    string notes = "";
                                    foreach (VstMidiEvent e in _noteOnEvents)
                                        notes += e.Data[1] + " ";

                                    MessageBox.Show("Process method: \nNoteOnEvents: " + notes);
                                    */

                                    _noteOnEvents.Add(midiEvent);

                                    /* FOR TESTING ADDOCTAVES()
                                    notes = "";
                                    foreach (VstMidiEvent e in _noteOnEvents)
                                        notes += e.Data[1] + " ";
                                    MessageBox.Show("Process method: \nNoteOnEvents after adding Note On event: " + notes);
                                    */

                                    // AddOctaves();
                                    _octavesAddedOrderedNoteOnEvents = makeCopy(_noteOnEvents);

                                    OrderNoteOnEventsWithOctaves();

                                }
                            }
                        }
                    }
                }

                // note off
                else if ((midiEvent.Data[0] & 0xF0) == 0x80)
                {
                    lock (((ICollection)NoteOffNumbers).SyncRoot)
                    {
                        lock (((ICollection)_noteOnEvents).SyncRoot)
                        {
                            lock (((ICollection)_octavesAddedOrderedNoteOnEvents).SyncRoot)
                            {
                                NoteOffNumbers.Enqueue(midiEvent.Data[1]);

                                /* FOR TESTING ADDOCTAVES()
                                string notes = "";
                                foreach (VstMidiEvent e in _noteOnEvents)
                                    notes += e.Data[1] + " ";

                                MessageBox.Show("Process method: \nNoteOnEvents: " + notes);

                                */

                                // mivel a kisebb oktávú hang (2x...) vhogy belekerült a listába az eredeti helyett, eltávolítani se tudja

                                _noteOnEvents.RemoveAll(n => n.Data[1] == midiEvent.Data[1]);

                                /* FOR TESTING ADDOCTAVES()
                                notes = "";
                                foreach (VstMidiEvent e in _noteOnEvents)
                                    notes += e.Data[1] + " ";
                                MessageBox.Show("Process method: \nNoteOnEvents after removing Note Off event: " + notes);
                                */

                                // AddOctaves();
                                _octavesAddedOrderedNoteOnEvents = makeCopy(_noteOnEvents);

                                OrderNoteOnEventsWithOctaves();
                            }
                        }
                    }
                }

            }
        }




        #region rythm

        public void setTimeInfo(VstTimeInfo timeInfo)
        {
            _timeInfo = timeInfo;
        }

        List<NoteLengthSamples> _rythm;
        private double quarterSampleNo;

        // a Tempotól függően kiszámolja, hogy egy negyed hang hány sample
        private void countQuarter()
        {
            double beatsPerSec = _timeInfo.Tempo / 60;
            quarterSampleNo = 44100 / beatsPerSec;
        }

        // based on the length of a quarter, counts the sample numbers of the rythm
        public void CountRythm()
        {
            _rythm = new List<NoteLengthSamples>();

            foreach (NoteLengths n in NoteLengthsArray)
            {
                switch (Convert.ToInt32(n))
                {
                    case 1: // quarter
                        _rythm.Add(new NoteLengthSamples(true, quarterSampleNo));
                        break;
                    case 2: // eighth-eighth
                        _rythm.Add(new NoteLengthSamples(true, quarterSampleNo / 2));
                        _rythm.Add(new NoteLengthSamples(true, quarterSampleNo / 2));
                        break;
                    case 3: // rest
                        _rythm.Add(new NoteLengthSamples(false, quarterSampleNo));
                        break;
                    case 4: // eighth-rest
                        _rythm.Add(new NoteLengthSamples(true, quarterSampleNo / 2));
                        _rythm.Add(new NoteLengthSamples(false, quarterSampleNo / 2));
                        break;
                    case 5: // rest-eighth
                        _rythm.Add(new NoteLengthSamples(false, quarterSampleNo / 2));
                        _rythm.Add(new NoteLengthSamples(true, quarterSampleNo / 2));
                        break;
                    case 6: // triplet
                        _rythm.Add(new NoteLengthSamples(true, quarterSampleNo / 3));
                        _rythm.Add(new NoteLengthSamples(true, quarterSampleNo / 3));
                        _rythm.Add(new NoteLengthSamples(true, quarterSampleNo / 3));
                        break;
                }
            }

            MessageBox.Show(rythmToString());
        }

        private string rythmToString()
        {
            string result = "";
            foreach (NoteLengthSamples s in _rythm)
            {
                result += s.note.ToString() + " " + s.frames.ToString() + "\n";
            }
            return result;
        }


        private const int _deltaFrames = 44100;
        private const int _blockSize = 512;
        private int _processedFrames = 0;   // ez a _rythm lista egy tagján belül értendő
        private VstMidiEvent _actualMidiEvent;
        private int _counter = 0;   // a _octavesAddedOrderedNoteOnEvents listen belül hol tart az arp 
        private int _remainder = 0; // FrameDelay 

        private int _rythmCounter = 0;  // hol tartunk a _rythm listben

        // 44100 sample frame = 1 sec
        // az_octavesAddedOrderedNoteOnEventset dolgozza fel
        public void Arpeggiate()
        {
            countQuarter(); // minden körben változhat a Tempo...

            VstMidiEvent newMidiEvent;


            // egy kör = _rythm lista elejétől a végéig - egyelőre
            // vagy: _octavesAddedOrdered lófasz lista számít egy körnek?

            // processedFrames: minden _rythm lista taggal újraindul

            if (_processedFrames == 0 && _octavesAddedOrderedNoteOnEvents.Count > 0) // kör eleje, van lejátszandó hang
            {
                _actualMidiEvent = makeCopy(_octavesAddedOrderedNoteOnEvents[_counter]);

                if (_rythm[_rythmCounter].note) // ha nem szünet
                {
                    // note on - 1. hang a listából            
                    newMidiEvent = new VstMidiEvent(_remainder, _actualMidiEvent.NoteLength, _actualMidiEvent.NoteOffset, _actualMidiEvent.Data, _actualMidiEvent.Detune, _actualMidiEvent.NoteOffVelocity);
                    Events.Add(newMidiEvent);
                    _processedFrames += 512;
                }
                else // ha szünet
                {
                    _processedFrames += 512;
                }
            }

            // ha a még legalább egy kör van hátra addig, amíg a hangot ki kell játszani
            else if (_processedFrames < _rythm[_rythmCounter].frames - 512 && _actualMidiEvent != null) // 43588 + 512 = 44100
            {
                _processedFrames += 512;
            }

            else if (_actualMidiEvent != null) // note off. a processedFrames belépett a kritikus szakaszba
            {

                if (_rythm[_rythmCounter].note) // ha nem szünet, akkor note off az előző hangra
                {
                    // create note off event
                    byte[] midiData = new byte[4];
                    midiData[0] = 0x80;
                    midiData[1] = _actualMidiEvent.Data[1];
                    midiData[2] = _actualMidiEvent.Data[2];
                    midiData[3] = 0;
                    newMidiEvent = new VstMidiEvent(Convert.ToInt32(_rythm[_rythmCounter].frames) - _processedFrames, _actualMidiEvent.NoteLength, _actualMidiEvent.NoteOffset, midiData, _actualMidiEvent.Detune, _actualMidiEvent.NoteOffVelocity);
                    Events.Add(newMidiEvent);
                }

                if ((_counter + 1) < _octavesAddedOrderedNoteOnEvents.Count)    // note on a következő hangra az _octavesAddedOrderedNoteOnEvents listából
                {
                    if (_rythm[_rythmCounter].note) // ha nem szünet volt, akkor lépthethetem az events listát
                    {
                        _counter++;
                    }

                    // rythm
                    if ((_rythmCounter + 1) < _rythm.Count)
                        _rythmCounter++;
                    else
                    {
                        _rythmCounter = 0;
                        _counter = 0;        // ha a ritmusképlet körbeért, a hangok is kezdődjenek elölről
                    }

                    _actualMidiEvent = makeCopy(_octavesAddedOrderedNoteOnEvents[_counter]);

                    // következő hang 
                    if (_rythm[_rythmCounter].note) // ha nem szünet, jöhet a következő hang note on-ja
                    {                 
                        // a note off-fal egy időben jöhet a köv. hang note on-ja is
                        newMidiEvent = new VstMidiEvent(Convert.ToInt32(_rythm[_rythmCounter].frames) - _processedFrames, _actualMidiEvent.NoteLength, _actualMidiEvent.NoteOffset, _actualMidiEvent.Data, _actualMidiEvent.Detune, _actualMidiEvent.NoteOffVelocity);
                        Events.Add(newMidiEvent);

                        _remainder = 512 - (Convert.ToInt32(_rythm[_rythmCounter].frames) - _processedFrames);
                        _processedFrames = 0;
                    }
                    else // ha a következő hang szünet
                    {
                        _remainder = 512 - (Convert.ToInt32(_rythm[_rythmCounter].frames) - _processedFrames);
                        _processedFrames = 0;
                    }
            

                }
                else // végigért az _octavesAddedOrderedNoteOnEvents listán, kör eleje
                {
                    _counter = 0;
                    _actualMidiEvent = null;

                    _remainder = 0;
                    _processedFrames = 0;


                    // rythm
                    if ((_rythmCounter + 1) < _rythm.Count)
                        _rythmCounter++;
                    else
                    {
                        _rythmCounter = 0;
                        _counter = 0;        // ha a ritmusképlet körbeért, a hangok is kezdődjenek elölről
                    }
                }
            }

        }

        #endregion




        private void OrderNoteOnEventsWithOctaves()
        {
            // mindegyik Note On eventből csak egy legyen (az AddOctaves() miatt kell)
            List<VstMidiEvent> distinctList = makeCopy(_octavesAddedOrderedNoteOnEvents);
            distinctList = distinctList.GroupBy(e => e.Data[1])
                   .Select(grp => grp.First())
                   .ToList();

            switch (Direction)
            {
                case Directions.up:
                    _octavesAddedOrderedNoteOnEvents = distinctList.OrderBy(midiEvent => midiEvent.Data[1]).ToList();
                    break;

                case Directions.down:
                    _octavesAddedOrderedNoteOnEvents = distinctList.OrderByDescending(midiEvent => midiEvent.Data[1]).ToList();
                    break;

                case Directions.cyclic_up_down:
                    _octavesAddedOrderedNoteOnEvents = distinctList.OrderBy(midiEvent => midiEvent.Data[1]).ToList();
                    List<VstMidiEvent> descendingList = distinctList.OrderByDescending(midiEvent => midiEvent.Data[1]).ToList();
                    if (descendingList.Count != 0)
                        descendingList.RemoveAt(0);                         // first item
                    if (descendingList.Count != 0)
                        descendingList.RemoveAt(descendingList.Count - 1);  // last item
                    _octavesAddedOrderedNoteOnEvents.AddRange(descendingList);
                    break;


                case Directions.cyclic_down_up:
                    _octavesAddedOrderedNoteOnEvents = distinctList.OrderByDescending(midiEvent => midiEvent.Data[1]).ToList();
                    List<VstMidiEvent> ascendingList = distinctList.OrderBy(midiEvent => midiEvent.Data[1]).ToList();
                    if (ascendingList.Count != 0)
                        ascendingList.RemoveAt(0);                         // first item
                    if (ascendingList.Count != 0)
                        ascendingList.RemoveAt(ascendingList.Count - 1);  // last item
                    _octavesAddedOrderedNoteOnEvents.AddRange(ascendingList);
                    break;

                /* TODO
            case Directions.terraced:
                orderedList = NoteOnEvents.OrderBy(midiEvent => midiEvent.Data[1]).ToList();
                break;
                */

                case Directions.random:
                    Random rand = new Random();
                    _octavesAddedOrderedNoteOnEvents = distinctList.ToList();
                    int n = _octavesAddedOrderedNoteOnEvents.Count;
                    while (n > 1)
                    {
                        n--;
                        int k = rand.Next(n + 1);
                        VstMidiEvent e = _octavesAddedOrderedNoteOnEvents[k];
                        _octavesAddedOrderedNoteOnEvents[k] = distinctList[n];
                        _octavesAddedOrderedNoteOnEvents[n] = e;
                    }
                    break;

                default:
                    break;
            }
        }

        public void AddOctaves()
        {

            // az eredeti listából induljunk ki
            _octavesAddedOrderedNoteOnEvents = makeCopy(_noteOnEvents);
            List<VstMidiEvent> noteOnEventsCopy = makeCopy(_noteOnEvents);

            switch (Octave)
            {
                case -2:

                    break;

                case -1:
                    /*
                    lock (((ICollection)_noteOnEvents).SyncRoot)
                    {
                        lock (((ICollection)_octavesAddedOrderedNoteOnEvents).SyncRoot)
                        {
                        */
                    string notes = "";
                    string extraNotes = "";

                    foreach (VstMidiEvent e in noteOnEventsCopy)
                    {
                        notes += e.Data[1] + " ";

                        byte[] midiData = e.Data;
                        if (midiData[1] >= 0xC)         // 0x21 = 33
                        {
                            midiData[1] -= 0xC;         // -12
                            extraNotes += midiData[1] + " ";

                            VstMidiEvent newMidiEvent = new VstMidiEvent(e.DeltaFrames, e.NoteLength, e.NoteOffset, midiData, e.Detune, e.NoteOffVelocity);

                            // ez miért adja hozzá az új hangot a _noteOnEventshez is?! mert ugyanarra az objectre mutatnak... deep copy kell...
                            _octavesAddedOrderedNoteOnEvents.Add(newMidiEvent);

                        }
                    }

                    MessageBox.Show("notes: " + notes + "\nadded notes: " + extraNotes);



                    break;

                case 0:
                    // do nothing
                    break;

                case 1:

                    break;

                case 2:

                    break;

                default:
                    // do nothing
                    break;

            }
        }





        // * HELPER FUNCTIONS *

        // deep copy (reference type)
        private List<VstMidiEvent> makeCopy(List<VstMidiEvent> originalList)
        {

            List<VstMidiEvent> newList = new List<VstMidiEvent>();

            foreach (VstMidiEvent e in originalList)
            {

                VstMidiEvent newEvent = makeCopy(e);
                newList.Add(newEvent);
            }

            return newList;
        }

        // deep copy (reference type)
        private VstMidiEvent makeCopy(VstMidiEvent e)
        {
            VstMidiEvent newEvent = new VstMidiEvent(e.DeltaFrames, e.NoteLength, e.NoteOffset, e.Data, e.Detune, e.NoteOffVelocity);
            return newEvent;
        }

        public void TestMakeCopy()
        {
            // test makeCopy(List<VstMidiEvent> l)

            byte[] midiData = new byte[4];
            midiData[0] = 0x90;
            midiData[1] = 0x67;
            midiData[2] = 0;
            midiData[3] = 0;
            VstMidiEvent event1 = new VstMidiEvent(0, 0, 0, midiData, 0, 0, false);

            byte[] midiData2 = new byte[4];
            midiData[0] = 0x90;
            midiData[1] = 0x67;
            midiData[2] = 0;
            midiData[3] = 0;
            VstMidiEvent event2 = new VstMidiEvent(1, 1, 1, midiData2, 1, 1, false);

            List<VstMidiEvent> list1 = new List<VstMidiEvent>();
            List<VstMidiEvent> list2 = makeCopy(list1);

            list1.Add(event1);
            list2.Add(event2);

            string s = "";
            foreach (VstMidiEvent e in list2)
            {
                s += e.DeltaFrames;
            }

            MessageBox.Show(s);  // expected: 1


            // test makeCopy(VstMidiEvent e)

            VstMidiEvent event3 = makeCopy(event1);

            event1 = new VstMidiEvent(1, 1, 1, midiData2, 1, 1, false);

            MessageBox.Show(event3.DeltaFrames.ToString()); // expected: 0

        }






    }
}

