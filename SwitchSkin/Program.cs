﻿using System;
using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;

namespace SwitchSkin
{
    class Program
    {
        private static Menu menu;
        private static Dictionary<String, int> ChampSkins = new Dictionary<String, int>();
  

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += GameLoad;
            Obj_AI_Hero.OnFloatPropertyChange += FloatPropertyChange;
        }


        static void GameLoad(EventArgs argss)
        {
             menu = new Menu("SwitchSkins", "Skinswitcher", true);

             menu.AddItem(new MenuItem("forall", "Enable for all (lags menu)").SetValue(false));

            foreach (var hero in HeroManager.AllHeroes)
            {
                if (!menu.Item("forall").GetValue<bool>() && hero.Name != ObjectManager.Player.Name)
                {
                    continue;
                } 

                var currenthero = hero;

                var newselect = menu.AddItem(new MenuItem("skin." + hero.ChampionName, hero.ChampionName + " (" + hero.Name + ")").SetValue(new StringList(new[] { "Skin 0", "Skin 1", "Skin 2", "Skin 3", "Skin 4", "Skin 5", "Skin 6", "Skin 7", "Skin 8", "Skin 9", "Skin 10" }, 0)));

                ChampSkins.Add(hero.Name, newselect.GetValue<StringList>().SelectedIndex);

                hero.SetSkin(hero.ChampionName, ChampSkins[hero.Name]);

                newselect.ValueChanged += delegate(Object sender, OnValueChangeEventArgs args)
                {
                   ChampSkins[currenthero.Name] = args.GetNewValue<StringList>().SelectedIndex;
                   currenthero.SetSkin(currenthero.ChampionName, ChampSkins[currenthero.Name]);
                };

            }
            menu.AddToMainMenu();
        }


        static void FloatPropertyChange(GameObject sender, GameObjectFloatPropertyChangeEventArgs args)
        {
            if (!(sender is Obj_AI_Hero) || args.Property != "mHP")
            {
                return; 
            }
            var hero = (Obj_AI_Hero) sender;
 
            if (args.OldValue.Equals(args.NewValue) && args.NewValue.Equals(hero.MaxHealth) && !hero.IsDead)
            {
                hero.SetSkin(hero.ChampionName, ChampSkins[hero.Name]);
            }
        }
    }
}

