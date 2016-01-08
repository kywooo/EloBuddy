using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace MadCatz_Katarina
{
    class Program
    {
        public const string ChampName = "Katarina";

        static Spell.Targeted Q;
        static Spell.Active W;
        static Spell.Targeted E;
        static Spell.Active R;

        public static Menu menu, ComboMenu, HarassMenu, LaneClearMenu, Misc;

        public static AIHeroClient Player
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
            if (Player.ChampionName != ChampName)
            {
                return;
            }

            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Active(SpellSlot.R, 550);

            menu = MainMenu.AddMenu("MadCatz_Katarina", "MadCatz");
            menu.AddGroupLabel("MadCatz_Katarina");

            ComboMenu = menu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("Q", new CheckBox("Use Q", true));
            ComboMenu.Add("W", new CheckBox("Use W", true));
            ComboMenu.Add("E", new CheckBox("Use E", true));
            ComboMenu.Add("R", new CheckBox("Use R", true));
            ComboMenu.AddSeparator();

            HarassMenu = menu.AddSubMenu("Harass", "Harass");
            HarassMenu.AddGroupLabel("Harass");
            HarassMenu.Add("Q", new CheckBox("Use Q", true));
            HarassMenu.Add("W", new CheckBox("Use W", true));
            HarassMenu.Add("E", new CheckBox("Use E", false));
            HarassMenu.AddSeparator();

            LaneClearMenu = menu.AddSubMenu("LanClear", "LanClear");
            LaneClearMenu.AddGroupLabel("LaneClear");
            LaneClearMenu.Add("Q", new CheckBox("Use Q", false));
            LaneClearMenu.Add("W", new CheckBox("Use W", false));
            LaneClearMenu.AddSeparator();

            Misc = menu.AddSubMenu("Misc", "Misc");
            Misc.AddGroupLabel("Misc");
            Misc.Add("Q", new CheckBox("KillSteal Q", true));
            Misc.Add("E", new CheckBox("KillSteal E", true));
            Misc.Add("AutoKill", new CheckBox("Enable E for Kill(minion ally, Target)", true));
            Misc.AddSeparator();


            Game.OnTick += Update;

            Chat.Print("MadCatz" + ChampName + "MadCatz_Load");
            Chat.Print("Korean Developer Good Luck!");
        }

        static void Update(EventArgs args)
        {
            if(Player.IsDead)
            {
                return;
            }

            switch(Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    Combo(ComboMenu["Q"].Cast<CheckBox>().CurrentValue, ComboMenu["W"].Cast<CheckBox>().CurrentValue, ComboMenu["E"].Cast<CheckBox>().CurrentValue, ComboMenu["R"].Cast<CheckBox>().CurrentValue);
                    break;
                case Orbwalker.ActiveModes.Harass:
                    Harass(HarassMenu["Q"].Cast<CheckBox>().CurrentValue, HarassMenu["W"].Cast<CheckBox>().CurrentValue);
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    LaneClear(LaneClearMenu["Q"].Cast<CheckBox>().CurrentValue, LaneClearMenu["W"].Cast<CheckBox>().CurrentValue);
                    break;
                    
            }
        }

        static void Combo(bool UseQ, bool UseW, bool UseE, bool UseR)
        {
            var _target = TargetSelector.GetTarget(E.Range, DamageType.Magical);

            if (_target == null || !_target.IsValidTarget())
                return;

            if(Q.IsReady() && _target.IsValidTarget(Q.Range))
            {
                Q.Cast(_target);
            }
            if(W.IsReady() && _target.IsValidTarget(W.Range))
            {
                W.Cast();
            }
            if(E.IsReady() && _target.IsValidTarget(E.Range) && !Q.IsReady())
            {
                E.Cast(_target);
            }

            if(R.IsReady() && !E.IsReady() && !Q.IsReady() && !W.IsReady() && 
                _target.IsValidTarget(R.Range))
            {
                Orbwalker.DisableMovement = false;
                Orbwalker.DisableAttacking = false;

                R.Cast();
            }
        }

        static void Harass(bool UseQ, bool UseW)
        {
            var _target = TargetSelector.GetTarget(Q.Range, DamageType.Magical);

            if (Q.IsReady() && _target.IsValidTarget(Q.Range))
            {
                Q.Cast(_target);
            }
            if (W.IsReady() && _target.IsValidTarget(W.Range))
            {
                W.Cast();
            }
        }

        static void LaneClear(bool UseQ, bool UseW)
        {
        }

        static void AutoKill()
        {

        }

        public float ComboDamage(AIHeroClient enemy)
        {
            var Damage = 0d;

            if(Q.IsReady())
            {
                Damage += Player.GetSpellDamage(enemy, SpellSlot.Q);
            }
            if(E.IsReady())
            {
                Damage += Player.GetSpellDamage(enemy, SpellSlot.E);
            }
            if(R.IsReady() || R.State == SpellState.Surpressed && R.Level > 0)
            {
                Damage += Player.GetSpellDamage(enemy, SpellSlot.R) * 8;
            }

            return (float)Damage;
        }
    }
}
