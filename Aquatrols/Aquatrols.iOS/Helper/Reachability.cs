﻿using CoreFoundation;
using System;
using System.Net;
using SystemConfiguration;

namespace Aquatrols.iOS.Helper
{
    /// <summary>
    /// Network status.
    /// </summary>
	public enum NetworkStatus
	{
		NotReachable,
		ReachableViaCarrierDataNetwork,
		ReachableViaWiFiNetwork
	}
    /// <summary>
    /// Reachability.
    /// </summary>
	public static class Reachability
	{
        public static string HostName = AppResources.NetworkUrl;

		public static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
		{
			// Is it reachable with the current network configuration?
			bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

			// Do we need a connection to reach it?
			bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0
				|| (flags & NetworkReachabilityFlags.IsWWAN) != 0;

			return isReachable && noConnectionRequired;
		}
        // Is the host reachable with the current network configuration
		public static bool IsHostReachable(string host)
		{
			if (string.IsNullOrEmpty(host))
				return false;

			using (var r = new NetworkReachability(host))
			{
				NetworkReachabilityFlags flags;

				if (r.TryGetFlags(out flags))
					return IsReachableWithoutRequiringConnection(flags);
			}
			return false;
		}
		// Raised every time there is an interesting reachable event,
		// we do not even pass the info as to what changed, and
		// we lump all three status we probe into one
        public static event EventHandler ReachabilityChanged;

		static void OnChange(NetworkReachabilityFlags flags)
		{
			ReachabilityChanged?.Invoke(null, EventArgs.Empty);
		}
		// Returns true if it is possible to reach the AdHoc WiFi network
		// and optionally provides extra network reachability flags as the
		// out parameter
		//
		static NetworkReachability adHocWiFiNetworkReachability;
        /// <summary>
        /// Ises the ad hoc wi fi network available.
        /// </summary>
        /// <returns><c>true</c>, if ad hoc wi fi network available was ised, <c>false</c> otherwise.</returns>
        /// <param name="flags">Flags.</param>
		public static bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags)
		{
			if (adHocWiFiNetworkReachability == null)
			{
				var ipAddress = new IPAddress(new byte[] { 169, 254, 0, 0 });
				adHocWiFiNetworkReachability = new NetworkReachability(ipAddress.MapToIPv6());
				adHocWiFiNetworkReachability.SetNotification(OnChange);
				adHocWiFiNetworkReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
			}

			return adHocWiFiNetworkReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
		}

		static NetworkReachability defaultRouteReachability;
        /// <summary>
        /// Ises the network available.
        /// </summary>
        /// <returns><c>true</c>, if network available was ised, <c>false</c> otherwise.</returns>
        /// <param name="flags">Flags.</param>
		static bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
		{
			if (defaultRouteReachability == null)
			{
				var ipAddress = new IPAddress(0);
				defaultRouteReachability = new NetworkReachability(ipAddress.MapToIPv6());
				defaultRouteReachability.SetNotification(OnChange);
				defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
			}
			return defaultRouteReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
		}
        static NetworkReachability remoteHostReachability;
        /// <summary>
        /// Remotes the host status.
        /// </summary>
        /// <returns>The host status.</returns>
		public static NetworkStatus RemoteHostStatus()
		{
			NetworkReachabilityFlags flags;
			bool reachable;

			if (remoteHostReachability == null)
			{
				remoteHostReachability = new NetworkReachability(HostName);

				// Need to probe before we queue, or we wont get any meaningful values
				// this only happens when you create NetworkReachability from a hostname
				reachable = remoteHostReachability.TryGetFlags(out flags);

				remoteHostReachability.SetNotification(OnChange);
				remoteHostReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
			}
			else
			{
				reachable = remoteHostReachability.TryGetFlags(out flags);
			}
            if (!reachable)
				return NetworkStatus.NotReachable;

			if (!IsReachableWithoutRequiringConnection(flags))
				return NetworkStatus.NotReachable;

			return (flags & NetworkReachabilityFlags.IsWWAN) != 0 ?
				NetworkStatus.ReachableViaCarrierDataNetwork : NetworkStatus.ReachableViaWiFiNetwork;
		}
        /// <summary>
        /// Internets the connection status.
        /// </summary>
        /// <returns>The connection status.</returns>
		public static NetworkStatus InternetConnectionStatus()
		{
			NetworkReachabilityFlags flags;
			bool defaultNetworkAvailable = IsNetworkAvailable(out flags);

			if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
				return NetworkStatus.NotReachable;

			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				return NetworkStatus.ReachableViaCarrierDataNetwork;

			if (flags == 0)
				return NetworkStatus.NotReachable;

			return NetworkStatus.ReachableViaWiFiNetwork;
		}
        /// <summary>
        /// Locals the wifi connection status.
        /// </summary>
        /// <returns>The wifi connection status.</returns>
		public static NetworkStatus LocalWifiConnectionStatus()
		{
			NetworkReachabilityFlags flags;
			if (IsAdHocWiFiNetworkAvailable(out flags))
				if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
					return NetworkStatus.ReachableViaWiFiNetwork;

			return NetworkStatus.NotReachable;
		}
	}
}
