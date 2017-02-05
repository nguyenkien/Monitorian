﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace StartupBridge
{
	public static class PlatformInfo
	{
		/// <summary>
		/// Whether this assembly is packaged in AppX package
		/// </summary>
		public static bool IsPackaged => _isPackaged.Value;
		private static readonly Lazy<bool> _isPackaged = new Lazy<bool>(() => IsPackagedBase());

		private static bool IsPackagedBase()
		{
			try
			{
				var package = Package.Current;
				return !string.IsNullOrEmpty(package.Id.FamilyName);
			}
			catch (InvalidOperationException)
			{
				// Message: The process has no package identity. (Exception from HRESULT: 0x80073D54)
				// This error code means 3D54 -> 15700 -> APPMODEL_ERROR_NO_PACKAGE
				return false;
			}
			catch (AggregateException ex) when (ex.InnerException is ArgumentException)
			{
				// Message: The parameter is incorrect.
				// This error occurs when StartupTask TaskId is not defined in AppxManifest.xml.
				throw;
			}
		}
	}
}