﻿using System;
using System.IO;
using System.Windows.Forms;
using UELib;

namespace UEExplorer.UI.Tabs
{
	public partial class UC_Default : UserControl_Tab
	{
		/// <summary>
		/// Called when the Tab is added to the chain.
		/// </summary>
		protected override void TabCreated()
		{
			// ...

			DefaultPage.Navigate( Path.Combine( Application.StartupPath, "default.htm" ) );
			base.TabCreated();

			Dock = System.Windows.Forms.DockStyle.Fill;
		}
	}
}
