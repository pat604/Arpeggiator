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

    public enum Directions { up, down, updown, downup, random }

    public enum NoteLengths { quarter = 1, eighth_eighth = 2, rest = 3, eighth_rest = 4, rest_eighth = 5, triplet = 6 };


    public struct NoteLengthFrames
    {
        public bool note;           // true: note, false: rest
        public double frames;       // for how many samples
        public bool accent;        // accent: 120, no-accent: 100

        public NoteLengthFrames(bool note, double frames, bool accent)
        {
            this.note = note;
            this.frames = frames;
            this.accent = accent;
        }
    }

    // default velocity: 100
    // 1-127 (pppp-ffff)
    public enum Accents { none, downbeat, upbeat }


    // Implements the incoming Midi event handling for the plugin.
    class MidiProcessor : IVstMidiProcessor
    {
        private Plugin _plugin;

        // Gets the midi events that should be processed in the current cycle. 
        public VstEventCollection Events { get; private set; }

        // Gets or sets a value indicating wether non-mapped midi events should be passed to the output.    
        public bool MidiThru { get; set; }

        public Queue<byte> NoteOnNumbers { get; private set; }

        private List<VstMidiEvent> _noteOnEvents { get; set; }

        private List<VstMidiEvent> _octavesAddedOrderedNoteOnEvents { get; set; }

        // The raw note off note numbers
        public Queue<byte> NoteOffNumbers { get; private set; }

        private VstTimeInfo _timeInfo;

        public Directions Direction { get; set; }

        public int Octave { get; set; }

        public NoteLengths[] NoteLengthsArray { get; set; }

        public Accents[] AccentsArray { get; set; }


        public MidiProcessor(Plugin plugin)
        {
            _plugin = plugin;
            Events = new VstEventCollection();
            NoteOnNumbers = new Queue<byte>();
            NoteOffNumbers = new Queue<byte>();
            _noteOnEvents = new List<VstMidiEvent>();
            _octavesAddedOrderedNoteOnEvents = new List<VstMidiEvent>();
            _rythm = new List<NoteLengthFrames>();
            NoteLengthsArray = new NoteLengths[4];
            AccentsArray = new Accents[4];

            for (int i = 0; i < 4; i++)
            {
                NoteLengthsArray[i] = NoteLengths.quarter;
                AccentsArray[i] = Accents.none;
            }

            CountRythm();
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
                                    _noteOnEvents.RemoveAll(n => n.Data[1] == midiEvent.Data[1]);

                                    AddOctaves();
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
                                    _noteOnEvents.Add(midiEvent);

                                    AddOctaves();
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

                                _noteOnEvents.RemoveAll(n => n.Data[1] == midiEvent.Data[1]);

                                AddOctaves();
                                OrderNoteOnEventsWithOctaves();
                            }
                        }
                    }
                }

            }
        }


        #region Rythm & Accent

        public void setTimeInfo(VstTimeInfo timeInfo)
        {
            _timeInfo = timeInfo;
        }

        List<NoteLengthFrames> _rythm;
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
            _rythm = new List<NoteLengthFrames>();

            for (int i = 0; i < 4; i++)
            {
                switch (Convert.ToInt32(NoteLengthsArray[i]))
                {
                    case 1: // quarter
                        if (AccentsArray[i] == Accents.downbeat)
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo, true));
                        else
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo, false));
                        break;

                    case 2: // eighth-eighth
                        if (AccentsArray[i] == Accents.downbeat)
                        {
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 2, true));
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 2, false));
                        }
                        if (AccentsArray[i] == Accents.upbeat)
                        {
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 2, false));
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 2, true));
                        }
                        if (AccentsArray[i] == Accents.none)
                        {
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 2, false));
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 2, false));
                        }
                        break;

                    case 3: // rest
                        _rythm.Add(new NoteLengthFrames(false, quarterSampleNo, false));
                        break;

                    case 4: // eighth-rest
                        if (AccentsArray[i] == Accents.downbeat)
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 2, true));
                        else
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 2, false));
                        _rythm.Add(new NoteLengthFrames(false, quarterSampleNo / 2, false));
                        break;

                    case 5: // rest-eighth
                        _rythm.Add(new NoteLengthFrames(false, quarterSampleNo / 2, false));
                        if (AccentsArray[i] == Accents.upbeat)
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 2, true));
                        else
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 2, false));
                        break;

                    case 6: // triplet
                        if (AccentsArray[i] == Accents.downbeat)
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 3, true));
                        else
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 3, false));

                        _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 3, false));

                        if (AccentsArray[i] == Accents.upbeat)
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 3, true));
                        else
                            _rythm.Add(new NoteLengthFrames(true, quarterSampleNo / 3, false));
                        break;
                }
            }

            // MessageBox.Show(rythmToString());
        }

        private string rythmToString()
        {
            string result = "";
            foreach (NoteLengthFrames s in _rythm)
            {
                result += s.note.ToString() + " " + s.frames.ToString() + " " + s.accent.ToString() + "\n";
            }
            return result;
        }


        private const int _deltaFrames = 44100;
        private const int _blockSize = 512;
        private int _processedFrames = 0;   // ez a _rythm lista egy tagján belül értendő. minden taggal újraindul
        private VstMidiEvent _actualMidiEvent;
        private int _counter = 0;   // a _octavesAddedOrderedNoteOnEvents listen belül hol tart az arp 
        private int _remainder = 0; // FrameDelay 
        private int _rythmCounter = 0;  // hol tartunk a _rythm listben

        // 44100 sample frame = 1 sec
        // az_octavesAddedOrderedNoteOnEventset dolgozza fel
        public void Arpeggiate()
        {
            countQuarter(); // if new Tempo was set

            VstMidiEvent newMidiEvent;

            if (_octavesAddedOrderedNoteOnEvents.Count == 0)
                _rythmCounter = 0;

            if (_processedFrames == 0 && _octavesAddedOrderedNoteOnEvents.Count > 0) // kör eleje, van lejátszandó hang
            {
                _actualMidiEvent = makeCopy(_octavesAddedOrderedNoteOnEvents[_counter]);

                if (_rythm[_rythmCounter].note) // ha nem szünet
                {
                    byte[] midiData = new byte[4];
                    midiData[0] = 0x90;
                    midiData[1] = _actualMidiEvent.Data[1];
                    if (_rythm[_rythmCounter].accent)
                        midiData[2] = 0x7F;
                    else
                        midiData[2] = _actualMidiEvent.Data[2];
                    midiData[3] = 0;

                    newMidiEvent = new VstMidiEvent(_remainder, _actualMidiEvent.NoteLength, _actualMidiEvent.NoteOffset, midiData, _actualMidiEvent.Detune, _actualMidiEvent.NoteOffVelocity);
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
                    // rythm
                    if (_rythm[_rythmCounter].note) // ha nem szünet volt, akkor lépthethetem az events listát
                        _counter++;
                                   
                    if ((_rythmCounter + 1) < _rythm.Count)
                        _rythmCounter++;
                    else
                    {
                        _rythmCounter = 0;
                        _counter = 0;        // ha a ritmusképlet körbeért, a hangok is kezdődjenek elölről
                    }

                    _actualMidiEvent = makeCopy(_octavesAddedOrderedNoteOnEvents[_counter]);

                    // következő hang 
                    if (_rythm[_rythmCounter].note) // ha nem szünet, jöhet a következő hang note on-ja is, a note off-fal egyidőben
                    {
                        byte[] midiData = new byte[4];
                        midiData[0] = 0x90;
                        midiData[1] = _actualMidiEvent.Data[1];
                        if (_rythm[_rythmCounter].accent)
                            midiData[2] = 0x7F;
                        else
                            midiData[2] = _actualMidiEvent.Data[2];
                        midiData[3] = 0;

                        newMidiEvent = new VstMidiEvent(Convert.ToInt32(_rythm[_rythmCounter].frames) - _processedFrames, _actualMidiEvent.NoteLength, _actualMidiEvent.NoteOffset, midiData, _actualMidiEvent.Detune, _actualMidiEvent.NoteOffVelocity);
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


        #region Order and Octaves

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

                case Directions.updown:
                    _octavesAddedOrderedNoteOnEvents = distinctList.OrderBy(midiEvent => midiEvent.Data[1]).ToList();
                    List<VstMidiEvent> descendingList = distinctList.OrderByDescending(midiEvent => midiEvent.Data[1]).ToList();
                    if (descendingList.Count != 0)
                        descendingList.RemoveAt(0);                         // first item
                    if (descendingList.Count != 0)
                        descendingList.RemoveAt(descendingList.Count - 1);  // last item
                    _octavesAddedOrderedNoteOnEvents.AddRange(descendingList);
                    break;


                case Directions.downup:
                    _octavesAddedOrderedNoteOnEvents = distinctList.OrderByDescending(midiEvent => midiEvent.Data[1]).ToList();
                    List<VstMidiEvent> ascendingList = distinctList.OrderBy(midiEvent => midiEvent.Data[1]).ToList();
                    if (ascendingList.Count != 0)
                        ascendingList.RemoveAt(0);                         // first item
                    if (ascendingList.Count != 0)
                        ascendingList.RemoveAt(ascendingList.Count - 1);  // last item
                    _octavesAddedOrderedNoteOnEvents.AddRange(ascendingList);
                    break;

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
                case -1:

                    //string notes = "";
                    //string extraNotes = "";

                    foreach (VstMidiEvent e in noteOnEventsCopy)
                    {
                        //notes += e.Data[1] + " ";

                        byte[] midiData = new byte[4];
                        midiData[0] = e.Data[0];
                        midiData[1] = e.Data[1];
                        midiData[2] = e.Data[2];
                        midiData[3] = 0;

                        if (midiData[1] >= 0xC)         // 0x21 = 33
                        {
                            midiData[1] -= 0xC;         // -12
                            //extraNotes += midiData[1] + " ";
                            VstMidiEvent newMidiEvent = new VstMidiEvent(e.DeltaFrames, e.NoteLength, e.NoteOffset, midiData, e.Detune, e.NoteOffVelocity);               
                            _octavesAddedOrderedNoteOnEvents.Add(newMidiEvent);
                        }
                    }
                    //MessageBox.Show("notes: " + notes + "\nadded notes: " + extraNotes);
                    break;

                case 0:
                    // do nothing
                    break;

                case 1:
                    foreach (VstMidiEvent e in noteOnEventsCopy)
                    {
                        byte[] midiData = new byte[4];
                        midiData[0] = e.Data[0];
                        midiData[1] = e.Data[1];
                        midiData[2] = e.Data[2];
                        midiData[3] = 0;

                        if (midiData[1] <= 0x73)         // 0x21 = 33
                        {
                            midiData[1] += 0xC;         // -12
                            VstMidiEvent newMidiEvent = new VstMidiEvent(e.DeltaFrames, e.NoteLength, e.NoteOffset, midiData, e.Detune, e.NoteOffVelocity);
                            _octavesAddedOrderedNoteOnEvents.Add(newMidiEvent);
                        }
                    }
                    break;

            }
        }

        #endregion



        public int ChannelCount
        {
            get { return _plugin.ChannelCount; }
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

