﻿
/*===================================================================================
* 
*   Copyright (c) Userware (OpenSilver.net, CSHTML5.com)
*      
*   This file is part of both the OpenSilver Compiler (https://opensilver.net), which
*   is licensed under the MIT license (https://opensource.org/licenses/MIT), and the
*   CSHTML5 Compiler (http://cshtml5.com), which is dual-licensed (MIT + commercial).
*   
*   As stated in the MIT license, "the above copyright notice and this permission
*   notice shall be included in all copies or substantial portions of the Software."
*  
\*====================================================================================*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using OpenSilver.Compiler.Common;

namespace OpenSilver.Compiler
{
    internal sealed class SLCoreTypesConverterVB : CoreTypesConverterBase
    {
        protected override Dictionary<string, Func<string, string>> SupportedCoreTypes { get; }
            = GetSupportedCoreTypes();

        //
        // IMPORTANT: Do not modify this dictionary unless you made changes in the file
        // 'TypeConverterHelper.cs' in the Runtime project. This dictionary and the 
        // dictionary 'TypeConverterHelper.CoreTypeConverters' must stay in sync.
        //
        // ImageSource is the only type to be present in this dictionary and not in the
        // one from the Runtime. This is due to the fact that we need to support the 
        // following XAML syntax : <ImageSource>whatever/you/want</ImageSource> which
        // normally requires the ContentPropertyAttribute to be defined on the type we
        // are trying to instantiate but this is not the case for ImageSource.
        // However, ImageSource has a registered TypeConverter via TypeConverterAttribute,
        // so this is why we do not want to register it in the CoreTypeConverters
        // dictionary, as it would prevent derived types (BitmapSource and BitmapImage)
        // from finding the converter.
        //
        private static Dictionary<string, Func<string, string>> GetSupportedCoreTypes()
        {
            return new Dictionary<string, Func<string, string>>(28)
            {
                ["system.windows.input.cursor"] = (s => CoreTypesHelperVB.ConvertToCursor(s, "Global.System.Windows.Input.Cursor", "Global.System.Windows.Input.Cursors")),
                ["system.windows.media.animation.keytime"] = (s => CoreTypesHelperVB.ConvertToKeyTime(s, "Global.System.Windows.Media.Animation.KeyTime")),
                ["system.windows.media.animation.repeatbehavior"] = (s => CoreTypesHelperVB.ConvertToRepeatBehavior(s, "Global.System.Windows.Media.Animation.RepeatBehavior")),
                ["system.windows.media.animation.keyspline"] = (s => CoreTypesHelperVB.ConvertToKeySpline(s, "Global.System.Windows.Media.Animation.KeySpline", "Global.System.Windows.Point")),
                ["system.windows.media.brush"] = (s => CoreTypesHelperVB.ConvertToBrush(s, "Global.System.Windows.Media.SolidColorBrush", "Global.System.Windows.Media.Color")),
                ["system.windows.media.solidcolorbrush"] = (s => CoreTypesHelperVB.ConvertToBrush(s, "Global.System.Windows.Media.SolidColorBrush", "Global.System.Windows.Media.Color")),
                ["system.windows.media.color"] = (s => CoreTypesHelperVB.ConvertToColor(s, "Global.System.Windows.Media.Color")),
                ["system.windows.media.doublecollection"] = (s => CoreTypesHelperVB.ConvertToDoubleCollection(s, "Global.System.Windows.Media.DoubleCollection")),
                ["system.windows.media.fontfamily"] = (s => CoreTypesHelperVB.ConvertToFontFamily(s, "Global.System.Windows.Media.FontFamily")),
                ["system.windows.media.geometry"] = (s => CoreTypesHelperVB.ConvertToGeometry(s, "Global.System.Windows.Media.Geometry")),
                ["system.windows.media.pathgeometry"] = (s => CoreTypesHelperVB.ConvertToGeometry(s, "Global.System.Windows.Media.PathGeometry")),
                ["system.windows.media.matrix"] = (s => CoreTypesHelperVB.ConvertToMatrix(s, "Global.System.Windows.Media.Matrix")),
                ["system.windows.media.pointcollection"] = (s => CoreTypesHelperVB.ConvertToPointCollection(s, "Global.System.Windows.Media.PointCollection", "Global.System.Windows.Point")),
                ["system.windows.media.transform"] = (s => CoreTypesHelperVB.ConvertToTransform(s, "Global.System.Windows.Media.MatrixTransform", "Global.System.Windows.Media.Matrix")),
                ["system.windows.media.matrixtransform"] = (s => CoreTypesHelperVB.ConvertToTransform(s, "Global.System.Windows.Media.MatrixTransform", "Global.System.Windows.Media.Matrix")),
                ["system.windows.media.cachemode"] = (s => CoreTypesHelperVB.ConvertToCacheMode(s, "Global.System.Windows.Media.CacheMode", "Global.System.Windows.Media.BitmapCache")),
                ["system.windows.cornerradius"] = (s => CoreTypesHelperVB.ConvertToCornerRadius(s, "Global.System.Windows.CornerRadius")),
                ["system.windows.duration"] = (s => CoreTypesHelperVB.ConvertToDuration(s, "Global.System.Windows.Duration")),
                ["system.windows.fontweight"] = (s => CoreTypesHelperVB.ConvertToFontWeight(s, "Global.System.Windows.FontWeight", "Global.System.Windows.FontWeights")),
                ["system.windows.gridlength"] = (s => CoreTypesHelperVB.ConvertToGridLength(s, "Global.System.Windows.GridLength", "Global.System.Windows.GridUnitType")),
                ["system.windows.point"] = (s => CoreTypesHelperVB.ConvertToPoint(s, "Global.System.Windows.Point")),
                ["system.windows.propertypath"] = (s => CoreTypesHelperVB.ConvertToPropertyPath(s, "Global.System.Windows.PropertyPath")),
                ["system.windows.rect"] = (s => CoreTypesHelperVB.ConvertToRect(s, "Global.System.Windows.Rect")),
                ["system.windows.size"] = (s => CoreTypesHelperVB.ConvertToSize(s, "Global.System.Windows.Size")),
                ["system.windows.thickness"] = (s => CoreTypesHelperVB.ConvertToThickness(s, "Global.System.Windows.Thickness")),
                ["system.windows.fontstretch"] = (s => CoreTypesHelperVB.ConvertToFontStretch(s, "Global.System.Windows.FontStretch")),
                ["system.windows.fontstyle"] = (s => CoreTypesHelperVB.ConvertToFontStyle(s, "Global.System.Windows.FontStyle", "Global.System.Windows.FontStyles")),
                ["system.windows.textdecorationcollection"] = (s => CoreTypesHelperVB.ConvertToTextDecorationCollection(s, "Global.System.Windows.TextDecorationCollection", "Global.System.Windows.TextDecorations")),
                ["system.windows.media.imagesource"] = (s => CoreTypesHelperVB.ConvertToImageSource(s, "Global.System.Windows.Media.ImageSource", "Global.System.Windows.Media.Imaging.BitmapImage")),
            };
        }
    }

    internal static class CoreTypesHelperVB
    {
        public const string TypeFromStringConvertersFullName = "Global.DotNetForHtml5.Core.TypeFromStringConverters";

        public static string ConvertFromInvariantStringHelper(string source, string destinationType)
        {
            return string.Format(
                "CType({1}.ConvertFromInvariantString(GetType({0}), {2}), {0})",
                destinationType, TypeFromStringConvertersFullName, Escape(source)
            );
        }

        internal static string ConvertToCursor(string source, string destinationType, string cursorsTypeFullName)
        {
            return $"{cursorsTypeFullName}.{source}";
        }

        internal static string ConvertToKeyTime(string source, string destinationType)
        {
            string stringValue = source.Trim();

            if (stringValue == "Paced")
            {
                throw new XamlParseException(
                    $"The '{destinationType}.{stringValue}' property is not supported yet."
                );
            }
            else if (stringValue.Length > 0 &&
                     stringValue[stringValue.Length - 1] == '%')
            {
                throw new XamlParseException(
                    $"Percentage values for '{destinationType}' are not supported yet."
                );
            }
            else if (stringValue == "Uniform")
            {
                return $"{destinationType}.Uniform";
            }
            else
            {
                return $"{destinationType}.FromTimeSpan({SystemTypesHelper.VisualBasic.ConvertFromInvariantString(stringValue, "system.timespan")})";
            }
        }

        internal static string ConvertToRepeatBehavior(string source, string destinationType)
        {
            const char _iterationCharacter = 'x';

            string stringValue = source.Trim().ToLowerInvariant();

            if (stringValue == "forever")
            {
                return string.Format($"{destinationType}.Forever");
            }
            else if (stringValue.Length > 0 &&
                     stringValue[stringValue.Length - 1] == _iterationCharacter)
            {
                string stringDoubleValue = stringValue.TrimEnd(_iterationCharacter);

                return $"New {destinationType}({stringDoubleValue.TrimEnd()})";
            }

            return SystemTypesHelper.VisualBasic.ConvertFromInvariantString(stringValue, "system.timespan");
        }

        internal static string ConvertToKeySpline(string source, string destinationType, string pointTypeName)
        {
            if (string.IsNullOrEmpty(source))
            {
                return $"New {destinationType}()";
            }

            string[] split = source.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 4)
            {
                return $"New {destinationType} With {{.ControlPoint1 = New {pointTypeName}({split[0]}, {split[1]}), .ControlPoint2 = New {pointTypeName}({split[2]}, {split[3]})}}";
            }

            throw GetConvertException(source, destinationType);
        }

        internal static string ConvertToBrush(string source, string destinationType, string colorTypeName)
        {
            return $"New {destinationType}({ConvertToColor(source, colorTypeName)})";
        }

        internal static string ConvertToColor(string source, string destinationType)
        {
            const int s_zeroChar = (int)'0';
            const int s_aLower = (int)'a';
            const int s_aUpper = (int)'A';

            string MatchColor(string colorString, out bool isKnownColor, out bool isNumericColor, out bool isScRgbColor)
            {
                string trimmedString = colorString.Trim();

                if (((trimmedString.Length == 4) ||
                    (trimmedString.Length == 5) ||
                    (trimmedString.Length == 7) ||
                    (trimmedString.Length == 9)) &&
                    (trimmedString[0] == '#'))
                {
                    isNumericColor = true;
                    isScRgbColor = false;
                    isKnownColor = false;
                }
                else if (trimmedString.StartsWith("sc#", StringComparison.Ordinal))
                {
                    isNumericColor = false;
                    isScRgbColor = true;
                    isKnownColor = false;
                }
                else
                {
                    isNumericColor = false;
                    isScRgbColor = false;
                    isKnownColor = true;
                }

                return trimmedString;
            }

            int ParseHexChar(char c)
            {
                int intChar = (int)c;

                if ((intChar >= s_zeroChar) && (intChar <= (s_zeroChar + 9)))
                {
                    return (intChar - s_zeroChar);
                }

                if ((intChar >= s_aLower) && (intChar <= (s_aLower + 5)))
                {
                    return (intChar - s_aLower + 10);
                }

                if ((intChar >= s_aUpper) && (intChar <= (s_aUpper + 5)))
                {
                    return (intChar - s_aUpper + 10);
                }
                throw new FormatException("Token is not valid.");
            }

            string ParseHexColor(string trimmedColor)
            {
                int a, r, g, b;
                a = 255;

                if (trimmedColor.Length > 7)
                {
                    a = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
                    r = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
                    g = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
                    b = ParseHexChar(trimmedColor[7]) * 16 + ParseHexChar(trimmedColor[8]);
                }
                else if (trimmedColor.Length > 5)
                {
                    r = ParseHexChar(trimmedColor[1]) * 16 + ParseHexChar(trimmedColor[2]);
                    g = ParseHexChar(trimmedColor[3]) * 16 + ParseHexChar(trimmedColor[4]);
                    b = ParseHexChar(trimmedColor[5]) * 16 + ParseHexChar(trimmedColor[6]);
                }
                else if (trimmedColor.Length > 4)
                {
                    a = ParseHexChar(trimmedColor[1]);
                    a = a + a * 16;
                    r = ParseHexChar(trimmedColor[2]);
                    r = r + r * 16;
                    g = ParseHexChar(trimmedColor[3]);
                    g = g + g * 16;
                    b = ParseHexChar(trimmedColor[4]);
                    b = b + b * 16;
                }
                else
                {
                    r = ParseHexChar(trimmedColor[1]);
                    r = r + r * 16;
                    g = ParseHexChar(trimmedColor[2]);
                    g = g + g * 16;
                    b = ParseHexChar(trimmedColor[3]);
                    b = b + b * 16;
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}.FromArgb(CByte({1}), CByte({2}), CByte({3}), CByte({4}))",
                    destinationType, a, r, g, b
                );
            }

            string ParseScRgbColor(string trimmedColor)
            {
                if (!trimmedColor.StartsWith("sc#", StringComparison.Ordinal))
                {
                    throw new FormatException("Token is not valid.");
                }

                string tokens = trimmedColor.Substring(3, trimmedColor.Length - 3);

                char[] separator = new char[2] { ',', ' ' };
                string[] split = tokens.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                if (split.Length == 3)
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}.FromScRgb({1}F, {2}F, {3}F, {4}F)",
                        destinationType,
                        1.0f,
                        Convert.ToSingle(split[0], CultureInfo.InvariantCulture),
                        Convert.ToSingle(split[1], CultureInfo.InvariantCulture),
                        Convert.ToSingle(split[2], CultureInfo.InvariantCulture)
                    );
                }
                else if (split.Length == 4)
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "{0}.FromScRgb({1}F, {2}F, {3}F, {4}F)",
                        destinationType,
                        Convert.ToSingle(split[0], CultureInfo.InvariantCulture),
                        Convert.ToSingle(split[1], CultureInfo.InvariantCulture),
                        Convert.ToSingle(split[2], CultureInfo.InvariantCulture),
                        Convert.ToSingle(split[3], CultureInfo.InvariantCulture)
                    );
                }

                throw new FormatException("Token is not valid.");
            }

            string ParseColor(string colorString)
            {
                string trimmedColor = MatchColor(
                    colorString, out bool isPossibleKnowColor, out bool isNumericColor, out bool isScRgbColor
                );

                //Is it a number?
                if (isNumericColor)
                {
                    return ParseHexColor(trimmedColor);
                }
                else if (isScRgbColor)
                {
                    return ParseScRgbColor(trimmedColor);
                }
                else
                {
                    Debug.Assert(isPossibleKnowColor);

                    if (Enum.TryParse(trimmedColor, true, out ColorsEnum namedColor))
                    {
                        int color = (int)namedColor;

                        return string.Format(
                            CultureInfo.InvariantCulture,
                            "{0}.FromArgb(CByte({1}), CByte({2}), CByte({3}), CByte({4}))",
                            destinationType,
                            (color >> 0x18) & 0xff,
                            (color >> 0x10) & 0xff,
                            (color >> 8) & 0xff,
                            color & 0xff
                        );
                    }
                }

                throw GetConvertException(colorString, destinationType);
            }

            return ParseColor(source);
        }

        internal static string ConvertToDoubleCollection(string source, string destinationType)
        {
            string[] split = source.Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            var sb = new StringBuilder();

            sb.Append($"New {destinationType}() From ");
            sb.Append("{");
            
            if (split != null && split.Length > 0)
                sb.Append(String.Join(",", split));

            sb.Append("}");

            return sb.ToString();
        }

        internal static string ConvertToFontFamily(string source, string destinationType)
        {
            string fontName = Escape(source.Trim());

            return $"New {destinationType}({fontName})";
        }

        internal static string ConvertToGeometry(string source, string destinationType)
        {
            return ConvertFromInvariantStringHelper(source, destinationType);
        }

        internal static string ConvertToMatrix(string source, string destinationType)
        {
            if (source == "Identity")
            {
                return $"{destinationType}.Identity";
            }

            string[] split = source.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 6)
            {
                return string.Format(
                    "New {0}({1}, {2}, {3}, {4}, {5}, {6})",
                    destinationType, split[0], split[1], split[2], split[3], split[4], split[5]
                );
            }

            throw GetConvertException(source, destinationType);
        }

        internal static string ConvertToPointCollection(string source, string destinationType, string pointTypeFullName)
        {
            string[] split = source.Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Points count needs to be an even number
            if (split.Length % 2 == 1)
            {
                throw GetConvertException(source, destinationType);
            }

            if (split.Length == 0)
            {
                return $"New {destinationType}()";
            }

            var sb = new StringBuilder();

            sb.Append($"New {destinationType}() From ");
            sb.Append("{");

            sb.Append(ConvertPointHelper(split[0], split[1], pointTypeFullName));
            for (int i = 2; i < split.Length; i += 2)
            {
                sb.Append(", ")
                  .Append(ConvertPointHelper(split[i], split[i + 1], pointTypeFullName));
            }

            sb.Append("}");

            return sb.ToString();
        }

        internal static string ConvertToTransform(string source, string destinationType, string matrixTypeFullName)
        {
            return $"New {destinationType}({ConvertToMatrix(source, matrixTypeFullName)})";
        }

        internal static string ConvertToCacheMode(string source, string destinationType, string bitmapCacheTypeFullName)
        {
            if (source.Equals("BitmapCache", StringComparison.OrdinalIgnoreCase))
            {
                return $"New {bitmapCacheTypeFullName}()";
            }

            throw GetConvertException(source, destinationType);
        }

        internal static string ConvertToCornerRadius(string source, string destinationType)
        {
            char[] separator = new char[2] { ',', ' ' };
            string[] split = source.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            switch (split.Length)
            {
                case 1:
                    return $"New {destinationType}({split[0]})";

                case 4:
                    return $"New {destinationType}({split[0]}, {split[1]}, {split[2]}, {split[3]})";
            }

            throw GetConvertException(source, destinationType);
        }

        internal static string ConvertToDuration(string source, string destinationType)
        {
            string stringValue = source.Trim();

            if (stringValue.Equals("Automatic", StringComparison.OrdinalIgnoreCase))
            {
                return $"{destinationType}.Automatic";
            }
            else if (stringValue.Equals("Forever", StringComparison.OrdinalIgnoreCase))
            {
                return $"{destinationType}.Forever";
            }
            else
            {
                return SystemTypesHelper.VisualBasic.ConvertFromInvariantString(stringValue, "system.timespan");
            }
        }

        internal static string ConvertToFontWeight(string source, string destinationType, string fontWeightsTypeFullName)
        {
            if (Enum.TryParse(source, true, out FontWeightsCode fontCode))
            {
                return $"{fontWeightsTypeFullName}.{fontCode}";
            }
            else if (ushort.TryParse(source, out ushort code))
            {
                string fontName = Enum.GetName(typeof(FontWeightsCode), code);
                if (fontName != null)
                {
                    return $"{fontWeightsTypeFullName}.{fontName}";
                }
            }

            throw GetConvertException(source, destinationType);
        }

        internal static string ConvertToGridLength(string source, string destinationType, string gridUnitTypeFullName)
        {
            string ReadDouble(string seq, string defaultValue)
            {
                // flag used to keep track of dots in the sequence.
                // If we weet more than 1 dot, just ignore the rest of the sequence.
                bool isFloat = false;

                int i = 0;
                for (; i < seq.Length; i++)
                {
                    char c = seq[i];
                    if (c == '.')
                    {
                        if (isFloat)
                        {
                            break;
                        }

                        isFloat = true;
                        continue;
                    }
                    else if (!char.IsDigit(c))
                    {
                        break;
                    }
                }

                if (i == 0)
                {
                    return defaultValue;
                }
                else if (i == 1 && seq[0] == '.')
                {
                    return "0D";
                }

                return seq.Substring(0, i);
            }

            string value;
            string unit;
            
            string stringValue = source.Trim().ToLower();
            if (stringValue == "auto")
            {
                value = "1D";
                unit = $"{gridUnitTypeFullName}.Auto";
            }
            else if (stringValue.EndsWith("*"))
            {
                value = ReadDouble(stringValue, "1D");
                unit = $"{gridUnitTypeFullName}.Star";
            }
            else
            {
                value = ReadDouble(stringValue, "0D");
                unit = $"{gridUnitTypeFullName}.Pixel";
            }

            return $"New {destinationType}({value}, {unit})";
        }

        internal static string ConvertToPoint(string source, string destinationType)
        {
            string[] split = source.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 2)
            {
                return ConvertPointHelper(split[0], split[1], destinationType);
            }

            throw GetConvertException(source, destinationType);
        }

        internal static string ConvertPointHelper(string x, string y, string pointTypeFullName)
        {
            return $"New {pointTypeFullName}({x}, {y})";
        }

        internal static string ConvertToPropertyPath(string source, string destinationType)
        {
            return $"New {destinationType}({Escape(source)})";
        }

        internal static string ConvertToRect(string source, string destinationType)
        {
            string[] split = source.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 4)
            {
                return string.Format(
                    "New {0}({1}, {2}, {3}, {4})",
                    destinationType, split[0], split[1], split[2], split[3]
                );
            }

            throw GetConvertException(source, destinationType);
        }

        internal static string ConvertToSize(string source, string destinationType)
        {
            string[] split = source.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 2)
            {
                return string.Format(
                    "New {0}({1}, {2})",
                    destinationType, split[0], split[1]
                );
            }

            throw GetConvertException(source, destinationType);
        }

        internal static string ConvertToThickness(string source, string destinationType)
        {
            char[] separator = new char[2] { ',', ' ' };

            string[] split = source.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            switch (split.Length)
            {
                case 1:
                    return $"New {destinationType}({split[0]})";

                case 2:
                    return $"New {destinationType}({split[0]}, {split[1]}, {split[0]}, {split[1]})";

                case 4:
                    return $"New {destinationType}({split[0]}, {split[1]}, {split[2]}, {split[3]})";
            }

            throw GetConvertException(source, destinationType);
        }

        internal static string ConvertToFontStretch(string source, string destinationType)
        {
            return $"New {destinationType}()";
        }

        internal static string ConvertToFontStyle(string source, string destinationType, string fontStylesFullTypeName)
        {
            if (source.Equals("Normal", StringComparison.OrdinalIgnoreCase))
            {
                return $"{fontStylesFullTypeName}.Normal";
            }
            else if (source.Equals("Oblique", StringComparison.OrdinalIgnoreCase))
            {
                return $"{fontStylesFullTypeName}.Oblique";
            }
            else if (source.Equals("Italic", StringComparison.OrdinalIgnoreCase))
            {
                return $"{fontStylesFullTypeName}.Italic";
            }

            throw GetConvertException(source, destinationType);
        }

        internal static string ConvertToTextDecorationCollection(string source, string destinationType, string textDecorationsTypeFullName)
        {
            switch (source.Trim().ToLower())
            {
                case "underline":
                    return $"{textDecorationsTypeFullName}.Underline";
                case "strikethrough":
                    return $"{textDecorationsTypeFullName}.Strikethrough";
                case "overline":
                    return $"{textDecorationsTypeFullName}.OverLine";
                //case "baseline":
                //    return $"{textDecorationsTypeFullName}.Baseline";
                case "none":
                    return "Nothing";

                default:
                    throw GetConvertException(source, destinationType);
            }
        }

        internal static string ConvertToImageSource(string source, string destinationType, string bitmapImageTypeFullName)
        {
            string uriKind;
            if (source.Contains(":/"))
            {
                uriKind = "Global.System.UriKind.Absolute";
            }
            else
            {
                uriKind = "Global.System.UriKind.Relative";
            }

            return $"New {bitmapImageTypeFullName}(New Global.System.Uri({Escape(source)}, {uriKind}))";
        }

        private static Exception GetConvertException(string value, string destinationTypeFullName)
        {
            return new XamlParseException(
                $"Cannot convert '{value}' to '{destinationTypeFullName}'."
            );
        }

        private static string Escape(string s)
        {
            return string.Concat("\"", s.Replace("\"", "\"\""), "\"");
        }
    }
}
