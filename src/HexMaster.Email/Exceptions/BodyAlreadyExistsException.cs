﻿using System;

namespace HexMaster.Email.Exceptions
{
    public class BodyAlreadyExistsException : Exception
    {
        internal BodyAlreadyExistsException(string name) : base(
            $"A body with name {name} is already present")
        {

        }
    }
}