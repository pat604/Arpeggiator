﻿namespace Arpeggiator
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
            _uiWrapper.SafeInstance.AccentSelected += AccentSelected;
            _uiWrapper.SafeInstance.SwingSelected += SwingSelected;
            _uiWrapper.SafeInstance.ProbabilitySelected += ProbabilitySelected;
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
            _uiWrapper.SafeInstance.NoteOnNumbers = _plugin.GetInstance<MidiProcessor>().NoteOnNumbers;
            _uiWrapper.SafeInstance.NoteOffNumbers = _plugin.GetInstance<MidiProcessor>().NoteOffNumbers;
          
            _uiWrapper.Open(hWnd);              
    }

        private void DirectionSelected(object sender, DirectionEventArgs e)
        {
            _plugin.GetInstance<MidiProcessor>().Direction = e.Direction;
            _plugin.GetInstance<MidiProcessor>().OrderNoteOnEventsWithOctaves();
        }

        private void OctaveSelected(object sender, OctaveEventArgs o)
        {
            _plugin.GetInstance<MidiProcessor>().Octave = o.Octave;
            _plugin.GetInstance<MidiProcessor>().AddOctaves();
            _plugin.GetInstance<MidiProcessor>().OrderNoteOnEventsWithOctaves();

        }

        private void RythmSelected(object sender, RythmEventArgs r)
        {
            _plugin.GetInstance<MidiProcessor>().NoteLengthsArray = r.NoteLengthsArray;
            _plugin.GetInstance<MidiProcessor>().CountRythm();
        }

        private void AccentSelected(object sender, AccentEventArgs a)
        {
            _plugin.GetInstance<MidiProcessor>().AccentsArray = a.AccentsArray;
            _plugin.GetInstance<MidiProcessor>().CountRythm();
        }

        private void SwingSelected(object sender, SwingEventArgs s)
        {
            _plugin.GetInstance<MidiProcessor>().Swing = s.Swing;
            _plugin.GetInstance<MidiProcessor>().CountRythm();
        }

        private void ProbabilitySelected(object sender, ProbabilityEventArgs p)
        {
          _plugin.GetInstance<MidiProcessor>().Probability = p.Probability;
        }


        public void ProcessIdle()
        {
            _uiWrapper.SafeInstance.ProcessIdle();
        }

        #endregion
    }
}
