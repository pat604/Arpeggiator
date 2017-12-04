namespace Arpeggiator
{
    using System;
    using Jacobi.Vst.Core;
    using Jacobi.Vst.Framework;
    using Jacobi.Vst.Framework.Common;
    using System.Windows;
    // Implements the custom UI editor for the plugin.
    class PluginEditor : IVstPluginEditor
    {
        private Plugin _plugin;
        private WinFormsControlWrapper<ArpeggiatorUI> _uiWrapper = new WinFormsControlWrapper<ArpeggiatorUI>();


        public PluginEditor(Plugin plugin)
        {
            _plugin = plugin;
            _uiWrapper.SafeInstance.DirectionSelected += DirectionSelected;     
            _uiWrapper.SafeInstance.OctaveSelected += OctaveSelected;
            _uiWrapper.SafeInstance.RythmSelected += RythmSelected;
        }

        #region IVstPluginEditor Members

        public System.Drawing.Rectangle Bounds
        {
            get { return _uiWrapper.Bounds; }
        }

        public void Close()
        {
            _uiWrapper.Close();
        }

        public bool KeyDown(byte ascii, VstVirtualKey virtualKey, VstModifierKeys modifers)
        {
            // no-op
            return false;
        }

        public bool KeyUp(byte ascii, VstVirtualKey virtualKey, VstModifierKeys modifers)
        {
            // no-op
            return false;
        }

        public VstKnobMode KnobMode { get; set; }

        
        public void Open(IntPtr hWnd)
        {
           //  _uiWrapper.SafeInstance.SetPlugin(_plugin); // ???

            _uiWrapper.SafeInstance.NoteOnNumbers = _plugin.GetInstance<MidiProcessor>().NoteOnNumbers;
            _uiWrapper.SafeInstance.NoteOffNumbers = _plugin.GetInstance<MidiProcessor>().NoteOffNumbers;
          
            _uiWrapper.Open(hWnd);              
    }

        private void DirectionSelected(object sender, DirectionEventArgs e)
        {
            _plugin.GetInstance<MidiProcessor>().Direction = e.Direction;
        }

        private void OctaveSelected(object sender, OctaveEventArgs o)
        {
            _plugin.GetInstance<MidiProcessor>().Octave = o.Octave;
            // _plugin.GetInstance<MidiProcessor>().AddOctaves();     
        }

        private void RythmSelected(object sender, RythmEventArgs r)
        {
            _plugin.GetInstance<MidiProcessor>().NoteLengthsArray = r.NoteLengths;
            _plugin.GetInstance<MidiProcessor>().CountRythm();
        }

        public void ProcessIdle()
        {
            _uiWrapper.SafeInstance.ProcessIdle();
        }

        #endregion
    }
}
