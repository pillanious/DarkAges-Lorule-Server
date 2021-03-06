﻿#region

using Darkages.Network.ServerFormats;
using Darkages.Scripting;
using Darkages.Storage.locales.debuffs;
using Darkages.Types;
using System;
using System.Linq;

#endregion

namespace Darkages.Storage.locales.Scripts.Skills
{
    [Script("beag suain", "Wren")]
    public class beagsuain : SkillScript
    {
        public Random rand = new Random();
        public Sprite Target;

        public beagsuain(Skill skill) : base(skill)
        {
        }

        public override void OnFailed(Sprite sprite)
        {
            if (sprite is Aisling)
            {
                var client = (sprite as Aisling).Client;
                client.Aisling.Show(Scope.NearbyAislings,
                    new ServerFormat29(Skill.Template.MissAnimation, (ushort)sprite.XPos, (ushort)sprite.YPos));

                client.SendMessage(0x02, "The enemy has made it through.");
            }
        }

        public override void OnSuccess(Sprite sprite)
        {
            var a = sprite.GetInfront().OfType<Monster>().ToList();
            var b = sprite.GetInfront().OfType<Aisling>().ToList();

            var i = a.Concat<Sprite>(b);

            if (i == null || !i.Any())
                if (sprite is Aisling)
                {
                    var client = (sprite as Aisling).Client;
                    client.SendMessage(0x02, "The enemy has made it through.");
                    return;
                }

            var debuff = new debuff_beagsuain();

            if (debuff == null)
                return;

            foreach (var target in i)
                if (!target.HasDebuff(debuff.Name))
                    if (sprite is Aisling)
                    {
                        var client = (sprite as Aisling).Client;
                        var action = new ServerFormat1A
                        {
                            Serial = client.Aisling.Serial,
                            Number = 0x81,
                            Speed = 20
                        };

                        client.Aisling.Show(Scope.NearbyAislings, action);
                        {
                            target.ApplyDamage(client.Aisling, 0, false, Skill.Template.Sound);
                            debuff.OnApplied(target, debuff);
                        }
                        return;
                    }
        }

        public override void OnUse(Sprite sprite)
        {
            if (sprite is Aisling)
            {
                var client = (sprite as Aisling).Client;

                if (Skill.Ready)
                {
                    client.TrainSkill(Skill);

                    if (client.Aisling.Invisible && Skill.Template.PostQualifers == PostQualifer.BreakInvisible)
                    {
                        client.Aisling.Invisible = false;
                        client.Refresh();
                    }

                    if (rand.Next(1, 101) >= 5)
                        OnSuccess(sprite);
                    else
                        OnFailed(sprite);
                }
            }
            else
            {
                OnSuccess(sprite);
            }
        }
    }
}