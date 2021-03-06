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

namespace Gibbed.MassEffectAndromeda.SaveFormats.Agents
{
    [JsonObject(MemberSerialization.OptIn)]
    [Agent(_AgentName)]
    public class WorldMapAgent : Agent
    {
        private const string _AgentName = "WorldMapSaveGameAgent";

        internal override string AgentName
        {
            get { return _AgentName; }
        }

        #region Fields
        private readonly List<Data.WorldMapUnknown0> _Unknown1;
        private readonly List<Data.WorldMapUnknown2> _Unknown2;
        #endregion

        public WorldMapAgent()
            : base(4)
        {
            this._Unknown1 = new List<Data.WorldMapUnknown0>();
            this._Unknown2 = new List<Data.WorldMapUnknown2>();
        }

        #region Properties
        [JsonProperty("unknown1")]
        public List<Data.WorldMapUnknown0> Unknown1
        {
            get { return this._Unknown1; }
        }

        [JsonProperty("unknown2")]
        public List<Data.WorldMapUnknown2> Unknown2
        {
            get { return this._Unknown2; }
        }
        #endregion

        internal override void Read2(IBitReader reader)
        {
            base.Read2(reader);

            this._Unknown1.Clear();
            var unknown1Count = reader.ReadUInt16();
            for (int i = 0; i < unknown1Count; i++)
            {
                var unknown1 = new Data.WorldMapUnknown0();
                unknown1.Read(reader);
                this._Unknown1.Add(unknown1);
            }

            this._Unknown2.Clear();
            if (this.ReadVersion >= 4)
            {
                var unknown2Count = reader.ReadUInt16();
                for (int i = 0; i < unknown2Count; i++)
                {
                    var unknown2 = new Data.WorldMapUnknown2();
                    unknown2.Read(reader);
                    this._Unknown2.Add(unknown2);
                }
            }
        }

        internal override void Write2(IBitWriter writer)
        {
            base.Write2(writer);

            writer.WriteUInt16((ushort)this._Unknown1.Count);
            foreach (var unknown1 in this._Unknown1)
            {
                unknown1.Write(writer);
            }

            writer.WriteUInt16((ushort)this._Unknown2.Count);
            foreach (var unknown2 in this._Unknown2)
            {
                unknown2.Write(writer);
            }
        }
    }
}
