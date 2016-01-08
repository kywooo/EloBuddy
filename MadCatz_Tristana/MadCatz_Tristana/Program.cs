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
            if(Player.ChampionName != ChampName)
            {
                return;
            }

            Q = new Spell.Active(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 1170, SkillShotType.Circular, 450, int.MaxValue, 180);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Targeted(SpellSlot.R, 600);

            menu = MainMenu.AddMenu("MadCatz_Tristana", "MadCatz");
            menu.AddGroupLabel("MadCatz_Tristana");

            ComboMenu = menu.AddSubMenu("Combo", "Combo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("Q", new CheckBox("Use Q", true));
            ComboMenu.Add("W", new CheckBox("Use W", false));
            ComboMenu.Add("E", new CheckBox("Use E", true));
            ComboMenu.Add("R", new CheckBox("Use R", true));
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
                    
            }
        }

        static void Combo(bool UseQ, bool UseE, bool UseR)
        {

        }

    }
}
