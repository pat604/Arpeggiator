using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;


namespace Arpeggiator
{

    // Represents one note mapping.
    class MapNoteItem
    {

        public string KeyName { get; set; }

        // Gets or sets the note number that triggers this mapping item.
        public byte TriggerNoteNumber { get; set; }

        // Gets or sets the note number that is sent to the output when this item is triggered.
        public byte OutputNoteNumber { get; set; }
    }



    class MapNoteItemList : KeyedCollection<byte, MapNoteItem>
    {

        protected override byte GetKeyForItem(MapNoteItem item)
        {
            return item.TriggerNoteNumber;
        }
    }


}

