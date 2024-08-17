﻿using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Models;

public record class SocialLink
{
    public string Name { get; }
    public string Url { get; }

    private SocialLink(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public static Result<SocialLink, Error> Create(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MAX_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Name url cannot be empty or more then {Constants.MAX_TEXT_LENGTH}");

        if (string.IsNullOrWhiteSpace(url) || url.Length > Constants.EXTRA_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Path url cannot be empty or more then {Constants.EXTRA_TEXT_LENGTH}");

        return new SocialLink(name, url);
    }
}