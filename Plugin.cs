using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jacobi.Vst.Core;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;

namespace Arpeggiator
{
    // The Plugin root class that implements the interface manager and the plugin midi source.
    class Plugin : VstPluginWithInterfaceManagerBase, IVstPluginMidiSource
    {

        public Plugin()
            : base("LiveAggiator", new VstProductInfo("VST.NET Arpeggiator", "Patricia", 1000),
                VstPluginCategory.Effect, VstPluginCapabilities.NoSoundInStop, 0, 0x30313233)
        {
        }

  

        // Creates a default instance and reuses that for all threads.
        protected override IVstPluginAudioProcessor CreateAudioProcessor(IVstPluginAudioProcessor instance)
        {
            if (instance == null) return new AudioProcessor(this);

            return instance;
        }

        /*
        protected override IVstPluginEditor CreateEditor(IVstPluginEditor instance)
        {
            if (instance == null) return new PluginEditor(this);

            return instance;
        }

        protected override IVstMidiProcessor CreateMidiProcessor(IVstMidiProcessor instance)
        {
            if (instance == null) return new MidiProcessor(this);

            return instance;
        }

        protected override IVstPluginPersistence CreatePersistence(IVstPluginPersistence instance)
        {
            if (instance == null) return new PluginPersistence(this);

            return instance;
        }
        */


        // Always returns 'this'
        protected override IVstPluginMidiSource CreateMidiSource(IVstPluginMidiSource instance)
        {
            return this;
        }


        #region IVstPluginMidiSource Members

        // Returns the channel count as reported by the host
        public int ChannelCount
        {
            get
            {
                IVstMidiProcessor midiProcessor = null;

                if (Host != null)
                {
                    midiProcessor = Host.GetInstance<IVstMidiProcessor>();
                }

                if (midiProcessor != null)
                {
                    return midiProcessor.ChannelCount;
                }

                return 0;
            }
        }

        #endregion

    
    }
 }

