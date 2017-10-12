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

namespace Arpeggiator
{

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

        public List<VstMidiEvent> NoteOnEvents { get; private set; }

        // The raw note off note numbers
        public Queue<byte> NoteOffNumbers { get; private set; }

        private VstTimeInfo _timeInfo;



        public MidiProcessor(Plugin plugin)
        {
            _plugin = plugin;
            Events = new VstEventCollection();
            NoteOnNumbers = new Queue<byte>();
            NoteOffNumbers = new Queue<byte>();
            NoteOnEvents = new List<VstMidiEvent>();



        }

        #region IVstMidiProcessor Members

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
                // VstMidiEvent mappedEvent = midiEvent;


                //  note on     
                if ((midiEvent.Data[0] & 0xF0) == 0x90)
                {
                    // You can treat note-on midi events with a velocity of 0 (zero) as a note-off midi event.             
                    if (midiEvent.Data[2] == 0)
                    {
                        lock (((ICollection)NoteOffNumbers).SyncRoot)
                        {
                            NoteOffNumbers.Enqueue(midiEvent.Data[1]);
                            NoteOnEvents.RemoveAll(n => n.Data[1] == midiEvent.Data[1]);
                        }
                    }

                    else
                    {
                        lock (((ICollection)NoteOnNumbers).SyncRoot)
                        {
                            lock (((ICollection)NoteOnEvents).SyncRoot)
                            {
                                NoteOnNumbers.Enqueue(midiEvent.Data[1]);
                                NoteOnEvents.Add(midiEvent);
                                // MessageBox.Show(NoteOnEvents.Count.ToString());

                            }
                        }
                    }
                }

                // note off
                else if ((midiEvent.Data[0] & 0xF0) == 0x80)
                {
                    lock (((ICollection)NoteOffNumbers).SyncRoot)
                    {
                        lock (((ICollection)NoteOnEvents).SyncRoot)
                        {
                            NoteOffNumbers.Enqueue(midiEvent.Data[1]);
                            NoteOnEvents.RemoveAll(n => n.Data[1] == midiEvent.Data[1]);
                            // MessageBox.Show("note removed from the NoteOnEvents list");
                        }
                    }
                }

            }
        }

        #endregion

        private const int _deltaFrames = 44100;
        private const int _blockSize = 512;
        private int _processedFrames = 0;
        private VstMidiEvent _actualMidiEvent;
        private int _counter = 0;   // a NoteOnEvents listen belül hol tart az arp 

        // 44100 sample frame = 1 sec
        public void Arpeggiate()
        {        
            // NoteOnEvents.Count == 1

            VstMidiEvent newMidiEvent;

            if (_processedFrames == 0 && NoteOnEvents.Count > 0)
            {
                // note on
                _actualMidiEvent = NoteOnEvents[_counter];
                newMidiEvent = new VstMidiEvent(0, NoteOnEvents[_counter].NoteLength, NoteOnEvents[_counter].NoteOffset, NoteOnEvents[_counter].Data, NoteOnEvents[_counter].Detune, NoteOnEvents[_counter].NoteOffVelocity);
                Events.Add(newMidiEvent);
                _processedFrames += 512;
                
            }

            else if (_processedFrames < 44000 && _actualMidiEvent != null) // 86x512=44000
            {
                _processedFrames += 512;             
            }
         
            else if (_actualMidiEvent != null)
            {
                // note off 
                byte[] midiData = new byte[4];
                midiData[0] = 0x80;
                midiData[1] = _actualMidiEvent.Data[1];
                midiData[2] = _actualMidiEvent.Data[2];
                midiData[3] = 0;
                newMidiEvent = new VstMidiEvent(68, _actualMidiEvent.NoteLength, _actualMidiEvent.NoteOffset, midiData, _actualMidiEvent.Detune, _actualMidiEvent.NoteOffVelocity);
                Events.Add(newMidiEvent);

                _processedFrames = 0;
                if (NoteOnEvents[_counter + 1] != null)
                {
                    _counter++;
                    _actualMidiEvent = NoteOnEvents[_counter];
                }
                else
                {
                    _counter = 0;
                    _actualMidiEvent = null;
                }
            }
            



            // note on 
            // newMidiEvent = new VstMidiEvent(NoteOnEvents[i].DeltaFrames, NoteOnEvents[i].NoteLength, NoteOnEvents[i].NoteOffset, NoteOnEvents[i].Data, NoteOnEvents[i].Detune, NoteOnEvents[i].NoteOffVelocity);         
            //MessageBox.Show("note added, delta frames: " + newMidiEvent.DeltaFrames + "\nnote length: " + newMidiEvent.NoteLength);


            /*
            // note off 
            byte[] midiData = new byte[4];
            midiData[0] = 0x80;
            midiData[1] = NoteOnEvents[i].Data[1];
            midiData[2] = NoteOnEvents[i].Data[2];
            midiData[3] = 0;
            newMidiEvent = new VstMidiEvent(90000, NoteOnEvents[i].NoteLength, 44000, midiData, NoteOnEvents[i].Detune, NoteOnEvents[i].NoteOffVelocity);
            Events.Add(newMidiEvent);
            //MessageBox.Show("note removed, deltaframes: " + newMidiEvent.DeltaFrames + "\nnote length: " + newMidiEvent.NoteLength);           
            */
        }





        public void setTimeInfo(VstTimeInfo timeInfo)
        {
            _timeInfo = timeInfo;
        }

        /*
    public void setBlockSize(int BlockSize)
    {
        _blockSize = BlockSize;
    }
    */

    }
}

