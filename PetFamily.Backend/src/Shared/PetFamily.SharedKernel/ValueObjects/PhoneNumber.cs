﻿using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public record PhoneNumber
{
    private const string PhoneRegex = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\-]?)?[\d\-]{7,10}$";
    public string Value { get; }
    private PhoneNumber(string value) => Value = value;

    public static Result<PhoneNumber, Error> Create(string number)
    {
        if (string.IsNullOrWhiteSpace(number?.Trim()))
            return Errors.General.ValueIsInvalid("PhoneNumber");

        number = number.Trim();

        if (Regex.IsMatch(number, PhoneRegex) == false)
            return Errors.General.ValueIsInvalid("PhoneNumber");

        return new PhoneNumber(number);
    }
}