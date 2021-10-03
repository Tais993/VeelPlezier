using System;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable 414

namespace VeelPlezier.scr
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal static class ProjectInfo
    {
        internal const string GithubLink = "https://github.com/Tais993/VeelPlezier";

        internal const string GithubProjectLink = "https://github.com/Tais993/VeelPlezier/projects/2";

        internal const string Creator = "Tijs Beek";

        internal const string Email = "tijs@familiebeek.eu";

        internal const string Phone = "+31622458154";
        internal static readonly Uri GithubUri = new(GithubLink, UriKind.Absolute);
        internal static readonly Uri GithubProjectUri = new(GithubProjectLink, UriKind.Absolute);
        internal static readonly Uri EmailUri = new("mailto:" + Email, UriKind.Absolute);
        internal static readonly Uri PhoneUri = new("tel:" + Phone, UriKind.Absolute);

        internal static readonly string Deadline = "05/10/2021";
    }
}