using Characters;
using System;
using System.Reflection;

namespace SkulPatcher.Mods.SpecialStats
{
    public abstract class SpecialStat
    {
        // Slider value
        public virtual double Value { get; set; }

        public SpecialStat(double value)
        {
            Value = value;
        }

        // Used as primary keys for StatMenuFuncs.stats, should just return a static value
        public abstract Stat.Kind Kind { get; }
        public abstract Stat.Category Category { get; }

        // Slider limits/information for StatMenu
        public abstract string Title { get; }
        public abstract double MinValue { get; }
        public abstract double MaxValue { get; }
        public abstract double DefaultValue { get; }
        public abstract string Abbreviation { get; }

        // Called on applying
        public abstract void Attach();

        // Called on unapplying, reapplying and resetting
        public abstract void Detach();

        // Creating static values for Kind and Category properties
        public static Stat.Kind CreateKind(string name)
        {
            return (Stat.Kind)Activator.CreateInstance(typeof(Stat.Kind), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { name, Stat.Kind.ValueForm.Constant }, null, null);
        }

        public static Stat.Category CreateCategory(string name)
        {
            return (Stat.Category)Activator.CreateInstance(typeof(Stat.Category), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { name, "" }, null, null);
        }
    }
}
