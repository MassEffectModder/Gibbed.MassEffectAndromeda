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
using Gibbed.MassEffectAndromeda.FileFormats;
using Newtonsoft.Json;

namespace Gibbed.MassEffectAndromeda.SaveFormats.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class NotificationsUnknown2
    {
        #region Fields
        private uint _Unknown1;
        private uint _Unknown2;
        private readonly NotificationsUnknown5 _Unknown3;
        private readonly NotificationsUnknown5 _Unknown4;
        private uint _Unknown5;
        private uint _Unknown6;
        private uint _Unknown7;
        #endregion

        public NotificationsUnknown2()
        {
            this._Unknown3 = new NotificationsUnknown5();
            this._Unknown4 = new NotificationsUnknown5();
        }

        #region Properties
        [JsonProperty("unknown1")]
        public uint Unknown1
        {
            get { return this._Unknown1; }
            set { this._Unknown1 = value; }
        }

        [JsonProperty("unknown2")]
        public uint Unknown2
        {
            get { return this._Unknown2; }
            set { this._Unknown2 = value; }
        }

        [JsonProperty("unknown3")]
        public NotificationsUnknown5 Unknown3
        {
            get { return this._Unknown3; }
        }

        [JsonProperty("unknown4")]
        public NotificationsUnknown5 Unknown4
        {
            get { return this._Unknown4; }
        }

        [JsonProperty("unknown5")]
        public uint Unknown5
        {
            get { return this._Unknown5; }
            set { this._Unknown5 = value; }
        }

        [JsonProperty("unknown6")]
        public uint Unknown6
        {
            get { return this._Unknown6; }
            set { this._Unknown6 = value; }
        }

        [JsonProperty("unknown7")]
        public uint Unknown7
        {
            get { return this._Unknown7; }
            set { this._Unknown7 = value; }
        }
        #endregion

        internal void Read(IBitReader reader, ushort version)
        {
            reader.PushFrameLength(24);
            this._Unknown1 = reader.ReadUInt32();
            this._Unknown2 = reader.ReadUInt32();
            reader.PushFrameLength(24);
            this._Unknown3.Read(reader, version);
            reader.PopFrameLength();
            reader.PushFrameLength(24);
            this._Unknown4.Read(reader, version);
            reader.PopFrameLength();
            this._Unknown5 = reader.ReadUInt32();
            this._Unknown6 = reader.ReadUInt32();
            this._Unknown7 = reader.ReadUInt32();
            reader.PopFrameLength();
        }

        internal void Write(IBitWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
