using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace MadCatz_Tristana
{
    class Program
    {
        public const string ChampName = "Tristana";

        static Spell.Active Q;
        static Spell.Skillshot W;
        static Spell.Targeted E;
        static Spell.Targeted R;

        public static Menu menu, ComboMenu, HarassMenu, Misc, Drawing;

        public static AIHeroClient _target;

        public static AIHeroClient _Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }

        static void OnLoad(EventArgs args)
        {
            if(_Player.ChampionName != ChampName)
            {
                return;
            }

            Bootstrap.Init(null);

            uint level = (uint)Player.Instance.Level;

            Q = new Spell.Active(SpellSlot.Q, 543 + level * 7);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 450, int.MaxValue, 180);
            E = new Spell.Targeted(SpellSlot.E, 543 + level * 7);
            R = new Spell.Targeted(SpellSlot.R, 543 + level * 7);

            menu = MainMenu.AddMenu("MadCatz_Tristana", "MadCatz");
            menu.AddGroupLabel("MadCatz_Tristana");

            ComboMenu = menu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("Q", new CheckBox("Use Q", true));
            ComboMenu.Add("W", new CheckBox("Use W", false));
            ComboMenu.Add("E", new CheckBox("Use E", true));
            ComboMenu.Add("R", new CheckBox("Use R", true));
            ComboMenu.AddLabel("Self W,R");
            ComboMenu.AddSeparator();

            HarassMenu = menu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Harass");
            HarassMenu.Add("Q", new CheckBox("Use Q", true));
            HarassMenu.Add("E", new CheckBox("Use E", false));
            HarassMenu.AddSeparator();

            Misc = menu.AddSubMenu("Misc", "Misc");
            Misc.AddGroupLabel("Misc");
            Misc.Add("R", new CheckBox("Killsteal R", true));
            Misc.AddLabel("Killsteal,R");
            Misc.AddSeparator();

            Game.OnTick += Update;

            Chat.Print("MadCatz" + ChampName + "MadCatz_Load");
            Chat.Print("Korean Developer Good Luck!");
        }

        static void Update(EventArgs args)
        {
            if(_Player.IsDead)
            {
                return;
            }

            switch(Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Combo(ComboMenu["Q"].Cast<CheckBox>().CurrentValue, ComboMenu["E"].Cast<CheckBox>().CurrentValue, ComboMenu["R"].Cast<CheckBox>().CurrentValue);
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Harass(HarassMenu["Q"].Cast<CheckBox>().CurrentValue, HarassMenu["E"].Cast<CheckBox>().CurrentValue);
                    break;
            }
        }

        static void Combo(bool UseQ, bool UseE, bool UseR)
        {
            if(Q.IsReady() && _target != null && _target.IsEnemy && _target.IsValidTarget(Q.Range))
            {
                Q.Cast();
            }

            if(E.IsReady() && _target != null && _target.IsEnemy && _target.IsValidTarget(E.Range))
            {
                E.Cast(_target);
            }

        }

        static void Harass(bool UseQ, bool UseE)
        {
            if(Q.IsReady() && _target != null && _target.IsValidTarget(Q.Range) && _target.IsEnemy)
            {
                Q.Cast();
            }
            if(E.IsReady() && _target != null && _target.IsEnemy && _target.IsValidTarget(E.Range))
            {
                E.Cast(_target);
            }
        }

        static void Killsteal()
        {
            var useR = Misc["R"].Cast<CheckBox>().CurrentValue;

            foreach (var Target in HeroManager.Enemies.Where(x => x.IsValidTarget(R.Range) && !x.HasBuffOfType(BuffType.Invulnerability) && !x.IsZombie && x.HasBuffOfType(BuffType.SpellShield)))
            {
                if (useR && R.IsReady() && Target.Health + Target.AttackShield < Player.Instance.GetSpellDamage(Target, SpellSlot.R, DamageLibrary.SpellStages.Default))
                {
                    R.Cast(Target);
                    break;
                }
            }
        }

    }
}
