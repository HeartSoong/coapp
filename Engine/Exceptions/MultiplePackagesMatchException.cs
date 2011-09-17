﻿//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Copyright (c) 2011 Garrett Serack . All rights reserved.
// </copyright>
// <license>
//     The software is licensed under the Apache 2.0 License (the "License")
//     You may not use the software except in compliance with the License. 
// </license>
//-----------------------------------------------------------------------

namespace CoApp.Toolkit.Engine.Exceptions {
    using System;
    using System.Collections.Generic;

    internal class MultiplePackagesMatchException : Exception {
        internal string PackageMask;
        internal IEnumerable<Package> PackageMatches;

        internal MultiplePackagesMatchException(string packageMask, IEnumerable<Package> packageMatch) {
            PackageMask = packageMask;
            PackageMatches = packageMatch;
        }
    }
}