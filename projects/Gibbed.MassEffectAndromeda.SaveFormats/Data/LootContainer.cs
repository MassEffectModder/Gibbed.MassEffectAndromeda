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

using System.Collections.Generic;
using Gibbed.MassEffectAndromeda.FileFormats;
using Newtonsoft.Json;

namespace Gibbed.MassEffectAndromeda.SaveFormats.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LootContainer
    {
        #region Fields
        private uint _Unknown1;
        private readonly List<LootObject> _Loot;
        #endregion

        public LootContainer()
        {
            this._Loot = new List<LootObject>();
        }

        #region Properties
        [JsonProperty("unknown1")]
        public uint Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }

        [JsonProperty("loot")]
        public List<LootObject> Loot
        {
            get { return this._Loot; }
        }
        #endregion

        internal void Read(IBitReader reader)
        {
            reader.PushFrameLength(24);
            this._Unknown1 = reader.ReadUInt32();
            this._Loot.Clear();
            var lootCount = reader.ReadUInt16();
            for (int i = 0; i < lootCount; i++)
            {
                var lootObject = new LootObject();
                lootObject.Read(reader);
                this._Loot.Add(lootObject);
            }
            reader.PopFrameLength();
        }

        internal void Write(IBitWriter writer)
        {
            writer.PushFrameLength(24);
            writer.WriteUInt32(this._Unknown1);
            writer.WriteUInt16((ushort)this._Loot.Count);
            foreach (var lootObject in this._Loot)
            {
                lootObject.Write(writer);
            }
            writer.PopFrameLength();
        }
    }
}
