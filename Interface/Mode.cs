using System;
using System.Collections.Generic;
using Gohla.Shared;

namespace ReactiveIRC.Interface
{
    public class ModeChange
    {
        public bool Add { get; set; }
        public char Mode { get; set; }
    }

    public class Mode
    {
        public ObservableHashSet<char> Modes { get; private set; }
        public String ModesString { get { return Modes.ToString(""); } }

        public Mode()
        {
            Modes = new ObservableHashSet<char>();
        }

        public void AddMode(char mode)
        {
            Modes.Add(mode);
        }

        public void RemoveMode(char mode)
        {
            Modes.Remove(mode);
        }

        public void Apply(ModeChange change)
        {
            if(change.Add)
                AddMode(change.Mode);
            else
                RemoveMode(change.Mode);
        }

        public void Apply(params ModeChange[] changes)
        {
            foreach(ModeChange change in changes)
                Apply(change);
        }

        public void ParseAndApply(String modes)
        {
            Apply(Parse(modes));
        }

        public static ModeChange[] Parse(String modes)
        {
            List<ModeChange> changes = new List<ModeChange>();
            bool add = false;
            foreach(char c in modes)
            {
                switch(c)
                {
                    case '+':
                        add = true;
                        break;
                    case '-':
                        add = false;
                        break;
                    default:
                        changes.Add(new ModeChange { Add = add, Mode = c });
                        break;
                }
            }

            return changes.ToArray();
        }
    }
}
