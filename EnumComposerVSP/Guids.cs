// Guids.cs
// MUST match guids.h
using System;

namespace Uriah65.EnumComposerVSP
{
    static class GuidList
    {
        public const string guidEnumComposerVSPPkgString = "a9d36138-b3cc-4978-802d-208260eefcef";
        public const string guidEnumComposerVSPCmdSetString = "bb88bc3d-07e2-4964-8f06-b59cfb296ae5";

        public static readonly Guid guidEnumComposerVSPCmdSet = new Guid(guidEnumComposerVSPCmdSetString);
    };
}