using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;
using System.Windows;

namespace Arpeggiator
{

    // A dummy audio processor only used for the timing of midi processing.
    class AudioProcessor : VstPluginAudioProcessorBase
    {
        private Plugin _plugin;
        private MidiProcessor _midiProcessor;
        private IVstMidiProcessor _hostProcessor;

        private VstTimeInfo timeInfo;




        public AudioProcessor(Plugin plugin)
            : base(0, 0, 0)
        {
            _plugin = plugin;
            _midiProcessor = plugin.GetInstance<MidiProcessor>();
        }

        // MidiProcessor's Process is called before this
        // This method is used to push midi events to the host.
        public override void Process(VstAudioBuffer[] inChannels, VstAudioBuffer[] outChannels)
        {          
            timeInfo = _plugin.Host.GetInstance<IVstHostCommands20>().GetTimeInfo(
            VstTimeInfoFlags.PpqPositionValid | VstTimeInfoFlags.BarStartPositionValid | VstTimeInfoFlags.TempoValid);

            // MessageBox.Show("tempo:" + timeInfo.Tempo.ToString() + "\n sample rate" + timeInfo.SampleRate.ToString()
            //     + "\n sign. numerator: " + timeInfo.TimeSignatureNumerator.ToString());            

            if (_hostProcessor == null)
            {
                _hostProcessor = _plugin.Host.GetInstance<IVstMidiProcessor>();
            }

            if (_midiProcessor != null && _hostProcessor != null)
            {

                _midiProcessor.setTimeInfo(timeInfo);
                _midiProcessor.Arpeggiate();

                if (_midiProcessor.Events.Count > 0)
                {
                    _hostProcessor.Process(_midiProcessor.Events);
                    _midiProcessor.Events.Clear();
                }
            }

            // perform audio-through
            base.Process(inChannels, outChannels);
        }
    }

}