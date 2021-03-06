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
    public class ActivePlayTimerAgent : Agent
    {
        private const string _AgentName = "ServerActivePlayTimerSaveGameAgent";

        internal override string AgentName
        {
            get { return _AgentName; }
        }

        #region Fields
        private ushort _Unknown1;
        private readonly List<KeyValuePair<uint, uint>> _Unknown2;
        #endregion

        public ActivePlayTimerAgent()
            : base(1)
        {
            this._Unknown2 = new List<KeyValuePair<uint, uint>>();
        }

        #region Properties
        [JsonProperty("unknown1")]
        public ushort Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }

        [JsonProperty("unknown2")]
        public List<KeyValuePair<uint, uint>> Unknown2
        {
            get { return this._Unknown2; }
        }
        #endregion

        internal override void Read4(IBitReader reader)
        {
            base.Read4(reader);
            this._Unknown1 = reader.ReadUInt16();
            reader.PushFrameLength(24);
            var unknown2Count = reader.ReadUInt16();
            this._Unknown2.Clear();
            for (int i = 0; i < unknown2Count; i++)
            {
                reader.PushFrameLength(24);
                var unknown2Key = reader.ReadUInt32();
                var unknown2Value = reader.ReadUInt32();
                reader.PopFrameLength();
                this._Unknown2.Add(new KeyValuePair<uint, uint>(unknown2Key, unknown2Value));
            }
            reader.PopFrameLength();
        }

        internal override void Write4(IBitWriter writer)
        {
            base.Write4(writer);
            writer.WriteUInt16(this._Unknown1);
            writer.PushFrameLength(24);
            writer.WriteUInt16((ushort)this._Unknown2.Count);
            foreach (var kv in this._Unknown2)
            {
                writer.PushFrameLength(24);
                writer.WriteUInt32(kv.Key);
                writer.WriteUInt32(kv.Value);
                writer.PopFrameLength();
            }
            writer.PopFrameLength();
        }
    }
}
