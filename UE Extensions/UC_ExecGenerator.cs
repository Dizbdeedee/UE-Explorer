﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace UE_Extensions
{
	using UEExplorer.UI;
	using UELib;
	using UELib.Core;
	using UELib.Flags;

	[System.Runtime.InteropServices.ComVisible( false )]
	public partial class UC_ExecGenerator : UserControl_Tab
	{
		private readonly List<UnrealPackage> Packages = new List<UnrealPackage>();

		private void Button_Add_Click( object sender, EventArgs e )
		{
			using( var ofd = new OpenFileDialog() )
			{
				ofd.DefaultExt = "u";
				ofd.Filter = "UnrealScript(*.u)|*.u";
				ofd.FilterIndex = 1;
				ofd.Title = "File Dialog";
				ofd.Multiselect = true;
				var dr = ofd.ShowDialog( this );
				if( dr != DialogResult.OK )
				{
					return;
				}

				// Load every selected file from the file dialog
				foreach( string fileName in ofd.FileNames )
				{
					Packages.Add( UnrealLoader.LoadPackage( fileName ) );
					TreeView_Packages.Nodes.Add( fileName );
				}
			}
		}

		private void Button_Save_Click( object sender, EventArgs e )
		{
			string gameprefix = TextBox_GamePrefix.Text;
			string engineprefix = TextBox_EnginePrefix.Text;

			string buffer = "{{autogenerated}}\r\nEvery [[console command]] declared in [[UnrealScript]] of [[" + gameprefix + "]] is listed here.\r\n";

			Packages.Sort( (P1, P2) => String.Compare( P1.PackageName, P2.PackageName ) );
			foreach( UnrealPackage NPkg in Packages )
			{
				NPkg.InitializePackage( UnrealPackage.InitFlags.Construct | UnrealPackage.InitFlags.Link | UnrealPackage.InitFlags.RegisterClasses | UnrealPackage.InitFlags.Deserialize );

				string klasbuffer = String.Empty;
				foreach( UClass klas in NPkg.ObjectsList.OfType<UClass>() )
				{
					if( klas.ChildFunctions.Count == 0 )
						continue;

					var execfunc = new List<UFunction>();
					foreach( UFunction Func in klas.ChildFunctions )
					{
						if( Func.HasFunctionFlag( FunctionFlags.Exec ) )
						{
							execfunc.Add( Func );
						}
					}

					if( execfunc.Count > 0 )
					{
						execfunc.Sort
						( 
							delegate( UFunction P1, UFunction P2 )
							{
								return String.Compare( P1.Name, P2.Name );
							}
						);

						klasbuffer += "===" + klas.Name + " Commands===\r\n{{main|" + engineprefix + ":" + klas.Name + " (" + gameprefix + ")}}\r\n";
						foreach( UFunction func in execfunc )
						{
							klasbuffer += "\r\n;" + func.Name + " - '''???.'''";
							if( func.ChildParams.Count > 0 )
							{
								foreach( UProperty prop in func.ChildParams )
								{
									string typetext = (prop.Type.ToString() == "Str" ? "string" : (string)prop.Type.ToString()).ToLower( System.Globalization.CultureInfo.CurrentCulture );
									string friendlytypetext = prop.GetFriendlyType();
									klasbuffer += "\r\n:#'''''" + (prop.HasPropertyFlag( PropertyFlagsLO.OptionalParm ) ? "[[optional]] " : "") + "[[" + typetext + (typetext != friendlytypetext ? ("|" + friendlytypetext) : "") + "]]''''' '''" + prop.Name + "''' - '''???.'''";
								}
							}
							klasbuffer += "\r\n";
						}
					}
				}
				if( !String.IsNullOrEmpty( klasbuffer ) )
				{
					buffer += "==" + NPkg.PackageName + " Commands==\r\n" + klasbuffer + "\r\n";
				}
				NPkg.Stream.Close();
			}
			buffer += "\r\n==External links==\r\n* [[udn2:ConsoleCommands|Native Console Commands]]\r\n\r\n[[Category:Console Commands]]";
			File.WriteAllText( Path.Combine( Application.StartupPath, engineprefix + " Console Commands (" + gameprefix + ").txt" ), buffer );
		}
	}
}