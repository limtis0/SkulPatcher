using Characters;
using System;
using System.Reflection;

namespace SkulPatcher
{
    public abstract class SpecialStat
    {
        protected Character Owner { get; }
        protected double Value { get; set; }

        public SpecialStat(Character owner, double value)
        {
            Owner = owner;
            Value = value;
        }

        public abstract Stat.Kind Kind { get; }

        public abstract Stat.Category Category { get; }

        public abstract void Attach();

        public abstract void Detach();

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
