﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using HexMaster.Email.Exceptions;

namespace HexMaster.Email.DomainModels
{
    public sealed class Recipient
    {

        private List<Substitution> _substitutions;
        public string? Name { get; private set; }
        public string EmailAddress { get; private set; } = default!;
        public IReadOnlyCollection<Substitution> Substitutions => _substitutions.AsReadOnly();
        public void SetEmailAddress(string value)
        {
            if (!Regex.IsMatch(value, RegularExpression.Email))
            {
                throw new InvalidEmailAddressException(value);
            }
            EmailAddress = value;
        }
        public void SetName(string value)
        {
            Name = value;
        }
        public void AddSubstitution(string key, string value)
        {
            AddSubstitution(new Substitution(key, value));
        }
        public void AddSubstitution(Substitution value)
        {
            if (_substitutions.Any(x => x.Key.Equals(value.Key, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception("error");
            }
            _substitutions.Add(value);
        }

public Recipient(string emailAddress)
{
    _substitutions = new List<Substitution>();
    SetEmailAddress(emailAddress);
}

[JsonConstructor]
public Recipient(string name, string emailAddress, IReadOnlyCollection<Substitution>? substitutions)
{
    EmailAddress = emailAddress;
    Name = name;
    _substitutions = substitutions != null ? substitutions.ToList() : new List<Substitution>();
}

        public static Recipient Create(string email, string? name = null, Dictionary<string, string>? substitutions = null)
        {
            var recipient = new Recipient(email, name, null);
            recipient.SetEmailAddress(email);
            recipient.SetName(name ?? email);
            var substitutionsDictionary = substitutions ?? new Dictionary<string, string>();
            recipient._substitutions = Substitution.FromDictionary(substitutionsDictionary);
            return recipient;
        }
    }
}