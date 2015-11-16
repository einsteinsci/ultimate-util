using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UltimateUtil.Test
{
	[TestClass]
	public class TextUtil_Test
	{
		[TestMethod, TestCategory("text")]
		public void RemoveWhitespace()
		{
			string str = "a b\tc d";
			Assert.AreEqual("abcd", str.RemoveWhitespace());
		}

		[TestMethod, TestCategory("text")]
		public void RemoveChars()
		{
			string str = "abcdefg";
			Assert.AreEqual("abdfg", str.RemoveChars('c', 'e'));
		}

		[TestMethod, TestCategory("text")]
		public void Alphanumeric()
		{
			char a = 'a';
			char pct = '%';
			char num = '4';

			Assert.IsTrue(a.IsAlphabetic());
			Assert.IsFalse(pct.IsAlphabetic());
			Assert.IsFalse(num.IsAlphabetic());

			Assert.IsFalse(a.IsNumeric());
			Assert.IsFalse(pct.IsNumeric());
			Assert.IsTrue(num.IsNumeric());

			Assert.IsTrue(a.IsAlphaNumeric());
			Assert.IsFalse(pct.IsAlphaNumeric());
			Assert.IsTrue(num.IsAlphaNumeric());
		}

		[TestMethod, TestCategory("text")]
		public void CharacterCase()
		{
			Assert.AreEqual('C', 'c'.ToUpper());
			Assert.AreNotEqual('c', 'c'.ToUpper());

			Assert.AreEqual('c', 'C'.ToLower());
		}

		[TestMethod, TestCategory("text")]
		public void FormatSplit()
		{
			string item = "minecraft:stick=64";
			string[] split = item.FormatSplit("{0}:{1}={2}");

			Assert.AreEqual(3, split.Length);
			Assert.AreEqual("minecraft", split[0]);
			Assert.AreEqual("stick", split[1]);
			Assert.AreEqual("64", split[2]);
		}

		[TestMethod, TestCategory("text")]
		public void Repeat()
		{
			Assert.AreEqual("TestTestTest", "Test".Repeat(3));
		}

		[TestMethod, TestCategory("text")]
		public void Fmt()
		{
			Assert.AreEqual("35 > 10", "{0} > {1}".Fmt(35, 10));
		}

		[TestMethod, TestCategory("text")]
		public void IgnoreCase()
		{
			Assert.IsTrue("one".EqualsIgnoreCase("ONE"));
			Assert.IsTrue("onetwothree".StartsWithIgnoreCase("ONE"));
			Assert.IsTrue("onetwothree".EndsWithIgnoreCase("THREE"));
			Assert.IsTrue("onetwothree".ContainsIgnoreCase("TWO"));
		}

		[TestMethod, TestCategory("text")]
		public void NullOrEmptyOrWhitespace()
		{
			string str = null;

			Assert.IsTrue(str.IsNullOrEmpty());
			Assert.IsTrue(str.IsNullOrWhitespace());

			str = "";
			Assert.IsTrue(str.IsNullOrEmpty());
			Assert.IsTrue(str.IsNullOrWhitespace());

			str = "\n\t";
			Assert.IsFalse(str.IsNullOrEmpty());
			Assert.IsTrue(str.IsNullOrWhitespace());

			str = "stuff";
			Assert.IsFalse(str.IsNullOrEmpty());
			Assert.IsFalse(str.IsNullOrWhitespace());
		}

		[TestMethod, TestCategory("text")]
		public void Reverse()
		{
			string str = "abcde";
			Assert.AreEqual("edcba", str.Reverse());
		}

		[TestMethod, TestCategory("text")]
		public void ThrowIfNullOrEmpty()
		{
			string str = null;
			try
			{
				str.ThrowIfNullOrEmpty();
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{ }

			str = "";
			try
			{
				str.ThrowIfNullOrEmpty();
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{ }

			str = "stuff";
			str.ThrowIfNullOrEmpty();
		}

		[TestMethod, TestCategory("text")]
		public void Shorten()
		{
			string str = "abcdef";
			Assert.AreEqual("abc...", str.Shorten(3));
			Assert.AreEqual("abcdef", str.Shorten(30));
		}
	}
}
