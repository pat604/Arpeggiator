using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Jacobi.Vst.Core.Plugin;
using Jacobi.Vst.Framework;
using Jacobi.Vst.Framework.Plugin;


namespace Arpeggiator
{
    public class PluginCommandStub : StdPluginDeprecatedCommandStub, IVstPluginCommandStub
    {

        // Called by the framework to create the plugin root class.
        protected override IVstPlugin CreatePluginInstance()
        {
            return new Plugin();
        }
    }
}
