﻿/* Copyright (c) 2017 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Collections.Generic;
using Gibbed.MassEffectAndromeda.FileFormats;
using Newtonsoft.Json;

namespace Gibbed.MassEffectAndromeda.SaveFormats.Items
{
    public class GearItemData : ItemData
    {
        #region Fields
        private bool _Unknown4;
        private readonly List<KeyValuePair<uint, ItemData>> _Mods;
        private readonly List<uint> _AugmentationItemHashes;
        private string _CustomName;
        #endregion

        public GearItemData()
        {
            this._Mods = new List<KeyValuePair<uint, ItemData>>();
            this._AugmentationItemHashes = new List<uint>();
        }

        #region Properties
        [JsonProperty("unknown4")]
        public bool Unknown4
        {
            get { return this._Unknown4; }
            set
            {
                this._Unknown4 = value;
                this.NotifyPropertyChanged("Unknown4");
            }
        }

        [JsonProperty("mods")]
        public List<KeyValuePair<uint, ItemData>> Mods
        {
            get { return this._Mods; }
        }

        [JsonProperty("augmentation_item_hashes")]
        public List<uint> AugmentationItemHashes
        {
            get { return this._AugmentationItemHashes; }
        }

        [JsonProperty("custom_name")]
        public string CustomName
        {
            get { return this._CustomName; }
            set
            {
                this._CustomName = value;
                this.NotifyPropertyChanged("CustomName");
            }
        }
        #endregion

        public override void Read(IBitReader reader, ushort version)
        {
            this._Unknown4 = reader.ReadBoolean();

            var modCount = Math.Min(5, (int)reader.ReadUInt16());
            for (int i = 0; i < modCount; i++)
            {
                reader.PushFrameLength(24);
                if (version >= 9)
                {
                    var modId = reader.ReadUInt32();
                    if (modId != 0)
                    {
                        reader.PushFrameLength(24);
                        var modItem = Components.InventoryComponent.ReadItemData(reader, version);
                        reader.PopFrameLength();
                        this._Mods.Add(new KeyValuePair<uint, ItemData>(modId, modItem));
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
                reader.PopFrameLength();
            }

            var augmentationCount = reader.ReadUInt16();
            for (int i = 0; i < augmentationCount; i++)
            {
                reader.PushFrameLength(24);
                var augmentationItemHash = reader.ReadUInt32();
                this._AugmentationItemHashes.Add(augmentationItemHash);
                reader.PopFrameLength();
            }

            this._CustomName = reader.ReadString();

            base.Read(reader, version);
        }

        public override void Write(IBitWriter writer)
        {
            writer.WriteBoolean(this._Unknown4);

            writer.WriteUInt16((ushort)Math.Min(5, this._Mods.Count));
            foreach (var kv in this._Mods)
            {
                writer.PushFrameLength(24);
                writer.WriteUInt32(kv.Key);
                if (kv.Key != 0)
                {
                    writer.PushFrameLength(24);
                    Components.InventoryComponent.WriteItemData(writer, kv.Value);
                    writer.PopFrameLength();
                }
                writer.PopFrameLength();
            }

            writer.WriteUInt16((ushort)this._AugmentationItemHashes.Count);
            foreach (var augmentationItemHash in this._AugmentationItemHashes)
            {
                writer.PushFrameLength(24);
                writer.WriteUInt32(augmentationItemHash);
                writer.PopFrameLength();
            }

            writer.WriteString(this._CustomName);

            base.Write(writer);
        }

        public override object Clone()
        {
            var instance = new GearItemData()
            {
                Unknown1 = this.Unknown1,
                PartitionName = this.PartitionName,
                Definition = this.Definition,
                Quantity = this.Quantity,
                IsNew = this.IsNew,
                Rarity = this.Rarity,
                Unknown4 = this.Unknown4,
                CustomName = this.CustomName,
            };
            foreach (var kv in this._Mods)
            {
                instance.Mods.Add(new KeyValuePair<uint, ItemData>(kv.Key, (ItemData)kv.Value.Clone()));
            }
            instance.AugmentationItemHashes.AddRange(this.AugmentationItemHashes);
            return instance;
        }
    }
}
