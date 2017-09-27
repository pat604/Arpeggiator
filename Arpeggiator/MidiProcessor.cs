using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;


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
        public Queue<byte> NoteOnEvents { get; private set; }

        // The raw note off note numbers
        public Queue<byte> NoteOffEvents { get; private set; }


        public MidiProcessor(Plugin plugin)
        {
            _plugin = plugin;
            Events = new VstEventCollection();
            NoteOnEvents = new Queue<byte>();
            NoteOffEvents = new Queue<byte>();

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
                VstMidiEvent mappedEvent = midiEvent;

                // 0x80: 1-es Channelen küldött Note OFF message (a channel igazából mindegy, kinullázva)
                // 0x90: 1-es Channelen küldött Note ON message 
                // & 0xF0: bitenkénti és; a két hexa karakter első tagját kapjuk meg, a másodikat kinullázza
                // Data[0]: első bájt: két hexadecimális karakter
                // a midiEvent Note off vagy Note on típusú
                if (((midiEvent.Data[0] & 0xF0) == 0x80 || (midiEvent.Data[0] & 0xF0) == 0x90))
                {

                    //  ha Note On message, akkor csak a kimeneti hangmagasságot (.Data[1]) hozzáadom NoteOnEventshez
                    // add raw note-on note numbers to the queue
                    if ((midiEvent.Data[0] & 0xF0) == 0x90)
                    {
                        // You can treat note-on midi events with a velocity of 0 (zero) as a note-off midi event.             
                        if (midiEvent.Data[2] == 0)
                        {
                            lock (((ICollection)NoteOffEvents).SyncRoot)
                            {
                                NoteOffEvents.Enqueue(midiEvent.Data[1]);
                            }
                        }

                        else
                        {
                            lock (((ICollection)NoteOnEvents).SyncRoot)
                            {
                                /* itt kéne machinálni a time-mal
                                VstTimeInfo timeInfo = _plugin.Host.GetInstance<VstTimeInfo>();
                                double tempo = timeInfo.Tempo;
                                //timeInfo.
                                */
                                /*
                                byte[] midiData = new byte[4];
                                // midiData[0]: Note On v Note Off marad
                                midiData[0] = midiEvent.Data[0];
                                // midiData[1]: a Pitch-et átállítjuk
                                midiData[1] = _plugin.NoteMap[midiEvent.Data[1]].OutputNoteNumber;
                                // midiData[2]: Velocity marad
                                midiData[2] = midiEvent.Data[2];
                                */

                                // int noteLength = midiEvent.NoteLength;


                                // 1-es noteLength
                                mappedEvent = new VstMidiEvent(midiEvent.DeltaFrames,
                                    100,
                                    midiEvent.NoteOffset,
                                    midiEvent.Data,
                                    midiEvent.Detune,
                                    midiEvent.NoteOffVelocity);
                                

                                // itt a mappedEventet kéne sorolni, és vhol megnézni, hogy ha egyszerre több noteOnEvent van, akkor a noteLength lejárta után loopolni a következőt
                               NoteOnEvents.Enqueue(midiEvent.Data[1]);
                            }
                        }
                    }

                    // note off
                    else if ((midiEvent.Data[0] & 0xF0) == 0x80)
                    {
                        lock (((ICollection)NoteOffEvents).SyncRoot)
                        {
                            NoteOffEvents.Enqueue(midiEvent.Data[1]);
                        }
                    }

                    Events.Add(mappedEvent);
                }
            }
        }

        #endregion
    }
}

