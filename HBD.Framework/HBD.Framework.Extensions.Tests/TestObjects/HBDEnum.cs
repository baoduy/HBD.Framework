﻿using System.ComponentModel.DataAnnotations;

namespace HBD.Framework.Extensions.Tests.TestObjects
{
    public enum HBDEnum
    {
        [Display(Name = "HBD")]
        DescriptionEnum = 1,

        [Display(Name = "NamedEnum")]
        NamedEnum = 2,

        Enum = 3,
    }
}
