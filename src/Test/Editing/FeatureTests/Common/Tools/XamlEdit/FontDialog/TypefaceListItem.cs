// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;

namespace FontDialogSample
{
    internal class TypefaceListItem : TextBlock, IComparable
    {
        private string _displayName;
        private bool _simulated;

        public TypefaceListItem(Typeface typeface)
        {
            _displayName = GetDisplayName(typeface);
            _simulated = typeface.IsBoldSimulated || typeface.IsObliqueSimulated;

            this.FontFamily = typeface.FontFamily;
            this.FontWeight = typeface.Weight;
            this.FontStyle = typeface.Style;
            this.FontStretch = typeface.Stretch;

            string itemLabel = _displayName;

            if (_simulated)
            {
                string formatString = FontDialogSample.Properties.Resources.ResourceManager.GetString(
                    "simulated", 
                    CultureInfo.CurrentUICulture
                    );
                itemLabel = string.Format(formatString, itemLabel);
            }

            this.Text = itemLabel;
            this.ToolTip = itemLabel;

            // In the case of symbol font, apply the default message font to the text so it can be read.
            if (FontFamilyListItem.IsSymbolFont(typeface.FontFamily))
            {
                TextRange range = new TextRange(this.ContentStart, this.ContentEnd);
                range.ApplyPropertyValue(TextBlock.FontFamilyProperty, SystemFonts.MessageFontFamily);
            }
        }

        public override string ToString()
        {
            return _displayName;
        }

        public Typeface Typeface
        {
            get { return new Typeface(FontFamily, FontStyle, FontWeight, FontStretch); }
        }

        int IComparable.CompareTo(object obj)
        {
            TypefaceListItem item = obj as TypefaceListItem;
            if (item == null)
            {
                return -1;
            }

            // Sort all simulated faces after all non-simulated faces.
            if (_simulated != item._simulated)
            {
                return _simulated ? 1 : -1;
            }

            // If weight differs then sort based on weight (lightest first).
            int difference = FontWeight.ToOpenTypeWeight() - item.FontWeight.ToOpenTypeWeight();
            if (difference != 0)
            {
                return difference > 0 ? 1 : -1;
            }

            // If style differs then sort based on style (Normal, Italic, then Oblique).
            FontStyle thisStyle = FontStyle;
            FontStyle otherStyle = item.FontStyle;

            if (thisStyle != otherStyle)
            {
                if (thisStyle == FontStyles.Normal)
                {
                    // This item is normal style and should come first.
                    return -1;
                }
                else if (otherStyle == FontStyles.Normal)
                {
                    // The other item is normal style and should come first.
                    return 1;
                }
                else
                {
                    // Neither is normal so sort italic before oblique.
                    return (thisStyle == FontStyles.Italic) ? -1 : 1;
                }
            }

            // If stretch differs then sort based on stretch (Normal first, then numerically).
            FontStretch thisStretch = FontStretch;
            FontStretch otherStretch = item.FontStretch;

            if (thisStretch != otherStretch)
            {
                if (thisStretch == FontStretches.Normal)
                {
                    // This item is normal stretch and should come first.
                    return -1;
                }
                else if (otherStretch == FontStretches.Normal)
                {
                    // The other item is normal stretch and should come first.
                    return 1;
                }
                else
                {
                    // Neither is normal so sort numerically.
                    return thisStretch.ToOpenTypeStretch() < otherStretch.ToOpenTypeStretch() ? -1 : 0;
                }
            }

            // They're the same.
            return 0;
        }

        internal static string GetDisplayName(Typeface typeface)
        {
            return NameDictionaryHelper.GetDisplayName(typeface.FaceNames);
        }
    }
}
