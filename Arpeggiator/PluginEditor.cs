﻿namespace Arpeggiator
{
    using System;
    using Jacobi.Vst.Core;
    using Jacobi.Vst.Framework;
    using Jacobi.Vst.Framework.Common;
   
    // Implements the custom UI editor for the plugin.
    class PluginEditor : IVstPluginEditor
    {
        private Plugin _plugin;
        private WinFormsControlWrapper<ArpeggiatorUI> _uiWrapper = new WinFormsControlWrapper<ArpeggiatorUI>();


        public PluginEditor(Plugin plugin)
        {
            _plugin = plugin;
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
            
            // _uiWrapper.SafeInstance.NoteMap = _plugin.NoteMap;
            _uiWrapper.SafeInstance.NoteOnEvents = _plugin.GetInstance<MidiProcessor>().NoteOnEvents;
            _uiWrapper.Open(hWnd);
                
    }

        public void ProcessIdle()
        {

            _uiWrapper.SafeInstance.ProcessIdle();
        }

        #endregion
    }
}