﻿#region

using Darkages.Network.ServerFormats;
using Darkages.Scripting;
using Darkages.Storage.locales.debuffs;
using Darkages.Types;
using System;
using System.Linq;

#endregion

namespace Darkages.Storage.locales.Scripts.Spells
{
    [Script("pramh", "Dean")]
    public class pramh : SpellScript
    {
        private readonly Random rand = new Random();

        public pramh(Spell spell) : base(spell)
        {
        }

        public override void OnFailed(Sprite sprite, Sprite target)
        {
            if (sprite is Aisling)
            {
                (sprite as Aisling).Client.SendMessage(0x02, "Your spell has been deflected.");
                (sprite as Aisling).Client.SendAnimation(33, target, sprite);
            }
            else if (sprite is Monster)
            {
                (sprite.Target as Aisling)?.Client.SendAnimation(33, sprite, target);
            }
        }

        public override void OnSuccess(Sprite sprite, Sprite target)
        {
            if (sprite is Aisling)
            {
                var client = (sprite as Aisling).Client;

                client.TrainSpell(Spell);

                var debuff = new debuff_sleep();

                target.RemoveDebuff("frozen");

                if (!target.HasDebuff(debuff.Name))
                {
                    debuff.OnApplied(target, debuff);

                    if (target is Aisling)
                        (target as Aisling).Client.SendMessage(0x02,
                            $"{client.Aisling.Username} Attacks you with {Spell.Template.Name}.");

                    client.SendMessage(0x02, $"you cast {Spell.Template.Name}");
                    client.SendAnimation(32, target, sprite);

                    var action = new ServerFormat1A
                    {
                        Serial = sprite.Serial,
                        Number = (byte)(client.Aisling.Path == Class.Priest ? 0x80 :
                            client.Aisling.Path == Class.Wizard ? 0x88 : 0x06),
                        Speed = 30
                    };

                    var hpbar = new ServerFormat13
                    {
                        Serial = client.Aisling.Serial,
                        Health = 255,
                        Sound = 8
                    };

                    client.Aisling.Show(Scope.NearbyAislings, action);
                    client.Aisling.Show(Scope.NearbyAislings, hpbar);
                }
                else
                {
                    client.SendMessage(0x02, "They are sleeping already.");
                }
            }
            else
            {
                var debuff = new debuff_sleep();
                var curses = target.Debuffs.OfType<debuff_cursed>().ToList();

                if (curses.Count == 0)
                    if (!target.HasDebuff(debuff.Name))
                    {
                        debuff.OnApplied(target, debuff);

                        if (target is Aisling)
                            (target as Aisling).Client
                                .SendMessage(0x02,
                                    $"{(sprite is Monster ? (sprite as Monster).Template.Name : (sprite as Mundane).Template.Name) ?? "Monster"} Attacks you with {Spell.Template.Name}.");

                        target.SendAnimation(32, target, sprite);

                        var action = new ServerFormat1A
                        {
                            Serial = sprite.Serial,
                            Number = 1,
                            Speed = 30
                        };

                        var hpbar = new ServerFormat13
                        {
                            Serial = target.Serial,
                            Health = 255,
                            Sound = 8
                        };

                        sprite.Show(Scope.NearbyAislings, action);
                        target.Show(Scope.NearbyAislings, hpbar);
                    }
            }
        }

        public override void OnUse(Sprite sprite, Sprite target)
        {
            if (sprite.CurrentMp - Spell.Template.ManaCost > 0)
            {
                sprite.CurrentMp -= Spell.Template.ManaCost;
            }
            else
            {
                if (sprite is Aisling)
                    (sprite as Aisling).Client.SendMessage(0x02, ServerContextBase.Config.NoManaMessage);
                return;
            }

            if (sprite.CurrentMp < 0)
                sprite.CurrentMp = 0;

            if (rand.Next(0, 100) > target.Mr)
                OnSuccess(sprite, target);
            else
                OnFailed(sprite, target);

            if (sprite is Aisling)
                (sprite as Aisling)
                    .Client
                    .SendStats(StatusFlags.StructB);
        }
    }
}