using System.Collections.Generic;
using System.Linq;

namespace HexMaster.Email.DomainModels
{
    public sealed class Substitution
    {
        public string Key { get; }
        public string Value { get;  }

        public static List<Substitution> FromDictionary(Dictionary<string, string> substitutions)
        {
            return substitutions
                .Select(s => new Substitution(s.Key, s.Value))
                .ToList();
        }

        public Substitution(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}