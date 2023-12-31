﻿using System;

namespace Eliot.Utilities
{
	/// <summary>
	/// 
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="index"></param>
		/// <returns>Left side from index.</returns>
		public static string Left( this string s, int index )
		{
			return s.Substring( 0, index );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="index"></param>
		/// <returns>Mid side from index.</returns>
		public static string Mid( this string s, int index )
		{
			return s.Substring( index, s.Length - index );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static string Right( this string s, int index )
		{
			return s.Substring( 0, s.Length - index );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="i"></param>
		/// <returns></returns>
		public static char FromInt( this Char s, int i )
		{
			return (Char)i;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static int FromChar( this Char s )
		{
			return s;
		}	
	}
}

