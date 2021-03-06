//-----------------------------------------------------------------------
// <copyright company="CoApp Project">
//     Copyright (c) 2010-2012 Garrett Serack and CoApp Contributors. 
//     Contributors can be discovered using the 'git log' command.
//     All rights reserved.
// </copyright>
// <license>
//     The software is licensed under the Apache 2.0 License (the "License")
//     You may not use the software except in compliance with the License. 
// </license>
//-----------------------------------------------------------------------

namespace CoApp.Packaging.Service {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Feeds;

    /// <summary>
    ///   This stores information that is really only relevant to the currently running request, not between sessions. The instance of this is bound to the Session.
    /// </summary>
    internal class PackageRequestData : NotifiesPackageManager {
        private Package _package;
        internal readonly Lazy<IPackage> SatisfiedBy;

        internal readonly Lazy<IEnumerable<IPackage>> TrimablePackages;
        internal readonly Lazy<IEnumerable<IPackage>> InstalledPackages;
        internal readonly Lazy<IEnumerable<IPackage>> OtherVersions;
        internal readonly Lazy<IEnumerable<IPackage>> AvailableVersions;
        
        internal readonly Lazy<IEnumerable<IPackage>> NewerPackages;
        internal readonly Lazy<IEnumerable<IPackage>> UpdatePackages;
        internal readonly Lazy<IEnumerable<IPackage>> UpgradePackages;

        internal readonly Lazy<IPackage> InstalledNewest;
        internal readonly Lazy<IPackage> InstalledNewestUpdate;
        internal readonly Lazy<IPackage> InstalledNewestUpgrade;

        internal readonly Lazy<IPackage> LatestInstalledThatUpdatesToThis;
        internal readonly Lazy<IPackage> LatestInstalledThatUpgradesToThis;
        internal readonly Lazy<IPackage> AvailableNewest;
        internal readonly Lazy<IPackage> AvailableNewestUpdate;
        internal readonly Lazy<IPackage> AvailableNewestUpgrade;
        internal readonly Lazy<Package> ActivePackage;
        internal readonly Lazy<PackageState> State;

        internal bool NotifiedClientThisSupercedes;

        internal PackageRequestData(Package package) {
            _package = package;
            InstalledPackages = new Lazy<IEnumerable<IPackage>>(() => InstalledPackageFeed.Instance.FindPackages(_package.CanonicalName.OtherVersionFilter).OrderByDescending(each => each.Version).ToArray());
            OtherVersions = new Lazy<IEnumerable<IPackage>>(() => PackageManagerImpl.Instance.SearchForPackages(_package.CanonicalName.OtherVersionFilter).OrderByDescending(each => each.Version).ToArray());

            NewerPackages = new Lazy<IEnumerable<IPackage>>(() => OtherVersions.Value.TakeWhile(each => each.IsNewerThan(_package)).ToArray());
            AvailableVersions = new Lazy<IEnumerable<IPackage>>(() => OtherVersions.Value.Where(each => each.IsInstalled == false).ToArray());

            UpdatePackages = new Lazy<IEnumerable<IPackage>>(() => State.Value >= PackageState.Updatable ? NewerPackages.Value.Where(each => !each.IsInstalled && each.IsAnUpdateFor(_package)).ToArray() : Enumerable.Empty<IPackage>());
            UpgradePackages = new Lazy<IEnumerable<IPackage>>(() => State.Value >= PackageState.Upgradable ? NewerPackages.Value.Where(each => !each.IsInstalled && each.IsAnUpgradeFor(_package)).ToArray() : Enumerable.Empty<IPackage>());

            InstalledNewest = new Lazy<IPackage>(() => InstalledPackages.Value.FirstOrDefault());
            InstalledNewestUpdate = new Lazy<IPackage>(() => InstalledPackages.Value.FirstOrDefault(each => each.IsAnUpdateFor(_package)));
            InstalledNewestUpgrade = new Lazy<IPackage>(() => InstalledPackages.Value.FirstOrDefault(each => each.IsAnUpgradeFor(_package)));
            SatisfiedBy = new Lazy<IPackage>(() => InstalledNewestUpdate.Value ?? (_package.IsInstalled ? _package : null));

            LatestInstalledThatUpdatesToThis = new Lazy<IPackage>(() => InstalledPackages.Value.FirstOrDefault(each => each.IsAnUpdateFor(_package)));
            LatestInstalledThatUpgradesToThis = new Lazy<IPackage>(() => InstalledPackages.Value.FirstOrDefault(each => each.IsAnUpgradeFor(_package)));

            AvailableNewest = new Lazy<IPackage>(() =>  AvailableVersions.Value.FirstOrDefault());
            AvailableNewestUpdate = new Lazy<IPackage>(() => {
                var result = UpdatePackages.Value.FirstOrDefault();
                return result != null && InstalledNewestUpdate.Value != null && InstalledNewestUpdate.Value.CanonicalName.Version >= result.CanonicalName.Version ? null : result;
            });
            AvailableNewestUpgrade = new Lazy<IPackage>(() => {
                var result = UpgradePackages.Value.FirstOrDefault();
                return result != null && InstalledNewestUpgrade.Value != null && InstalledNewestUpgrade.Value.CanonicalName.Version >= result.CanonicalName.Version ? null : result;
            });

            TrimablePackages = new Lazy<IEnumerable<IPackage>>(() => InstalledPackages.Value.Where(each => each.IsTrimable));

            ActivePackage = new Lazy<Package>(() => InstalledPackages.Value.Select(each =>
                (Package)each).OrderBy(each => each, new Toolkit.Extensions.Comparer<Package>((packageA, packageB) => GeneralPackageSettings.Instance.WhoWins(packageA, packageB))).FirstOrDefault());

            State = new Lazy<PackageState>(() => {
                PackageState state;
                return Enum.TryParse(GeneralPackageSettings.Instance.GetValue(package.CanonicalName, "state"), true, out state) ? state : (package.IsWanted ? PackageState.Upgradable : PackageState.Updatable);
            });
        }

    }
}