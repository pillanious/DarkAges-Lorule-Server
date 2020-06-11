﻿///************************************************************************
//Project Lorule: A Dark Ages Client (http://darkages.creatorlink.net/index/)
//Copyright(C) 2018 TrippyInc Pty Ltd
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
//*************************************************************************/

using Darkages.Network.Object;
using Darkages.Types;

namespace Darkages.Scripting
{
    public abstract class ItemScript : ObjectManager
    {
        public ItemScript(Item item)
        {
            Item = item;
        }

        public Item Item { get; set; }

        public abstract void OnUse(Sprite sprite, byte slot);
        public abstract void Equipped(Sprite sprite, byte displayslot);
        public abstract void UnEquipped(Sprite sprite, byte displayslot);

        public virtual void OnDropped(Sprite sprite, Position dropped_position, Area map)
        {
        }

        public virtual void OnPickedUp(Sprite sprite, Position picked_position, Area map)
        {
        }
    }
}