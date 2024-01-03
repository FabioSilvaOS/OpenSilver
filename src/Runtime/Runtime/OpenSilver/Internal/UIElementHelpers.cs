﻿
/*===================================================================================
* 
*   Copyright (c) Userware/OpenSilver.net
*      
*   This file is part of the OpenSilver Runtime (https://opensilver.net), which is
*   licensed under the MIT license: https://opensource.org/licenses/MIT
*   
*   As stated in the MIT license, "the above copyright notice and this permission
*   notice shall be included in all copies or substantial portions of the Software."
*  
\*====================================================================================*/

using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Threading.Tasks;
using System;
using CSHTML5.Internal;

namespace OpenSilver.Internal;

internal static class UIElementHelpers
{
    internal static void SetCharacterSpacing(this UIElement uie, int cSpacing)
    {
        double value = cSpacing / 1000.0;
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.letterSpacing = $"{value.ToInvariantString()}em";
    }

    internal static void SetFontFamily(this UIElement uie, FontFamily font)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.fontFamily = font.GetFontFace(uie).CssFontName;
    }

    internal static void SetFontStyle(this UIElement uie, FontStyle fontStyle)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.fontStyle = fontStyle.ToString().ToLower();
    }

    internal static void SetFontWeight(this UIElement uie, FontWeight fontWeight)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.fontWeight = fontWeight.ToOpenTypeWeight().ToInvariantString();
    }

    internal static void SetForeground(this UIElement uie, Brush oldForeground, Brush newForeground)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        switch ((oldForeground, newForeground))
        {
            case (GradientBrush, SolidColorBrush scb):
                style.backgroundImage = string.Empty;
                style.backgroundClip = string.Empty;
                style.color = scb.ToHtmlString();
                break;

            case (_, SolidColorBrush scb):
                style.color = scb.ToHtmlString();
                break;

            case (GradientBrush, LinearGradientBrush lgb):
                style.backgroundImage = lgb.ToHtmlString(uie);
                break;

            case (_, LinearGradientBrush lgb):
                style.backgroundImage = lgb.ToHtmlString(uie);
                style.color = "transparent";
                style.backgroundClip = "text";
                break;

            case (GradientBrush, RadialGradientBrush rgb):
                style.backgroundImage = rgb.ToHtmlString(uie);
                break;

            case (_, RadialGradientBrush rgb):
                style.backgroundImage = rgb.ToHtmlString(uie);
                style.color = "transparent";
                style.backgroundClip = "text";
                break;

            case (SolidColorBrush, null):
                style.color = string.Empty;
                break;

            case (GradientBrush, null):
                style.backgroundImage = string.Empty;
                style.backgroundClip = string.Empty;
                style.color = string.Empty;
                break;

            default:
                // ImageBrush and custom brushes are not supported. Keep using old brush.
                break;
        }
    }

    internal static async Task SetBackgroundAsync(this UIElement uie, Brush brush)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.background = brush switch
        {
            Brush => await brush.GetDataStringAsync(uie),
            _ => string.Empty,
        };
    }

    internal static void SetLineHeight(this UIElement uie, double lineHeight)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.lineHeight = lineHeight switch
        {
            0.0 => "normal",
            _ => $"{lineHeight.ToInvariantString()}px",
        };
    }

    internal static void SetPadding(this UIElement uie, Thickness padding)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.padding = $"{padding.Top.ToInvariantString()}px {padding.Right.ToInvariantString()}px {padding.Bottom.ToInvariantString()}px {padding.Left.ToInvariantString()}px";
    }

    internal static void SetTextAlignment(this UIElement uie, TextAlignment textAlignment)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.textAlign = textAlignment switch
        {
            TextAlignment.Center => "center",
            TextAlignment.Right => "end",
            TextAlignment.Justify => "justify",
            _ => "start",
        };
    }

    internal static void SetTextDecorations(this UIElement uie, TextDecorationCollection tdc)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.textDecoration = tdc?.ToHtmlString() ?? string.Empty;
    }

    internal static void SetTextTrimming(this UIElement uie, TextTrimming textTrimming)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.textOverflow = textTrimming switch
        {
            TextTrimming.WordEllipsis or TextTrimming.CharacterEllipsis => "ellipsis",
            _ => "clip",
        };
    }

    internal static void SetTextWrapping(this UIElement uie, TextWrapping textWrapping)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        switch (textWrapping)
        {
            case TextWrapping.Wrap:
                style.whiteSpace = "pre-wrap";
                style.overflowWrap = "break-word";
                break;

            case TextWrapping.NoWrap:
            default:
                style.whiteSpace = "pre";
                style.overflowWrap = string.Empty;
                break;
        }
    }

    internal static void SetFontSize(this UIElement uie, double fontSize)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.fontSize = $"{fontSize.ToInvariantString()}px";
    }

    internal static void SetTextSelection(this UIElement uie, bool enabled)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.userSelect = enabled ? "auto" : "none";
    }

    internal static void SetInnerText(this UIElement uie, string text)
    {
        string sDiv = CSHTML5.InteropImplementation.GetVariableStringForJS(uie.OuterDiv);
        string escapedText = INTERNAL_HtmlDomManager.EscapeStringForUseInJavaScript(text);
        Interop.ExecuteJavaScriptVoidAsync($"{sDiv}.innerText = \"{escapedText}\";");
    }

    internal static void SetOpacity(this UIElement uie, double opacity)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.opacity = Math.Round(opacity, 3).ToInvariantString();
    }

    internal static void SetTransform(this UIElement uie, Transform transform)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.transform = transform switch
        {
            Transform when !transform.IsIdentity => MatrixTransform.MatrixToHtmlString(transform.ValueInternal),
            _ => string.Empty,
        };
    }

    internal static void SetTransformOrigin(this UIElement uie, Point origin)
    {
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.transformOrigin = $"{(origin.X * 100).ToInvariantString()}% {(origin.Y * 100).ToInvariantString()}%";
    }

    internal static void SetZIndex(this UIElement uie, int value)
    {
        Debug.Assert(uie is not null);
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.zIndex = value.ToInvariantString();
    }

    internal static void SetCursor(this UIElement uie, Cursor cursor)
    {
        Debug.Assert(uie is not null);
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.cursor = cursor?.ToHtmlString() ?? string.Empty;
    }

    internal static void SetBorderRadius(this UIElement uie, CornerRadius radius)
    {
        Debug.Assert(uie is not null);
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.borderRadius = $"{radius.TopLeft.ToInvariantString()}px {radius.TopRight.ToInvariantString()}px {radius.BottomRight.ToInvariantString()}px {radius.BottomLeft.ToInvariantString()}px";
    }

    internal static void SetBorderWidth(this UIElement uie, Thickness width)
    {
        Debug.Assert(uie is not null);
        var style = INTERNAL_HtmlDomManager.GetDomElementStyleForModification(uie.OuterDiv);
        style.boxSizing = "border-box";
        style.borderStyle = "solid";
        style.borderWidth = $"{width.Top.ToInvariantString()}px {width.Right.ToInvariantString()}px {width.Bottom.ToInvariantString()}px {width.Left.ToInvariantString()}px";
    }

    internal static double GetBaseLineOffset(this UIElement uie)
    {
        Debug.Assert(uie is not null);
        if (uie.OuterDiv is not null)
        {
            string sDiv = CSHTML5.InteropImplementation.GetVariableStringForJS(uie.OuterDiv);
            return Interop.ExecuteJavaScriptDouble($"document.getBaseLineOffset({sDiv});");
        }

        return 0.0;
    }

    internal static void InvalidateMeasureOnFontFamilyChanged(UIElement uie, FontFamily font)
    {
        var face = font.GetFontFace(uie);
        if (face.IsLoaded)
        {
            InvalidateMeasure(uie);
        }
        else
        {
            face.RegisterForMeasure(uie);
            _ = face.LoadAsync();
        }
    }

    internal static void InvalidateMeasure(UIElement uie)
    {
        DependencyObject d = uie;
        while (d is not null)
        {
            if (d is FrameworkElement fe)
            {
                if (fe is TextBlock tb)
                {
                    tb.InvalidateCacheAndMeasure();
                }
                else
                {
                    fe.InvalidateMeasure();
                }
                return;
            }

            d = VisualTreeHelper.GetParent(d);
        }
    }
}