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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using CSHTML5;
using CSHTML5.Internal;

//
// Important: do not rename this class without updating the Simulator as well!
// The class is called via reflection from the Simulator.
//

namespace DotNetForHtml5.Core
{
    internal static class PopupsManager
    {
        private static int CurrentPopupRootIndentifier = 0;
        private static readonly HashSet<PopupRoot> PopupRootIdentifierToInstance = new();

        public static PopupRoot CreateAndAppendNewPopupRoot(Popup popup, Window parentWindow)
        {
            // Generate a unique identifier for the PopupRoot:
            string uniquePopupRootIdentifier = $"INTERNAL_Cshtml5_PopupRoot_{++CurrentPopupRootIndentifier}";

            var popupRoot = new PopupRoot(uniquePopupRootIdentifier, parentWindow, popup);

            //--------------------------------------
            // Create a DIV for the PopupRoot in the DOM tree:
            //--------------------------------------

            var popupRootDiv = INTERNAL_HtmlDomManager.CreatePopupRootDomElementAndAppendIt(popupRoot);
            popupRoot.OuterDiv
                = popupRoot.InnerDiv
                = popupRootDiv;
            popupRoot.IsConnectedToLiveTree = true;
            popupRoot.INTERNAL_AttachToDomEvents();
            popupRoot.UpdateIsVisible();

            //--------------------------------------
            // Remember the PopupRoot for later use:
            //--------------------------------------

            PopupRootIdentifierToInstance.Add(popupRoot);

            return popupRoot;
        }

        public static void RemovePopupRoot(PopupRoot popupRoot)
        {
            if (!PopupRootIdentifierToInstance.Remove(popupRoot))
            {
                throw new InvalidOperationException(
                    $"No PopupRoot with identifier '{popupRoot.UniqueIndentifier}' was found.");
            }

            //--------------------------------------
            // Remove from the DOM:
            //--------------------------------------

            popupRoot.INTERNAL_DetachFromDomEvents();

            string sWindow = InteropImplementation.GetVariableStringForJS(popupRoot.ParentWindow.RootDomElement);

            OpenSilver.Interop.ExecuteJavaScriptFastAsync(
                $@"var popupRoot = document.getElementById(""{popupRoot.UniqueIndentifier}"");
if (popupRoot) {sWindow}.removeChild(popupRoot);");

            popupRoot.OuterDiv = popupRoot.InnerDiv = null;
            popupRoot.IsConnectedToLiveTree = false;
            popupRoot.ParentPopup = null;
        }

        public static IEnumerable GetAllRootUIElements() // IMPORTANT: This is called via reflection from the "Visual Tree Inspector" of the Simulator. If you rename or remove it, be sure to update the Simulator accordingly!
        {
            // Include the main window:
            yield return Window.Current;

            // And all the popups:
            foreach (PopupRoot popupRoot in PopupRootIdentifierToInstance)
            {
                yield return popupRoot;
            }
        }
        
        internal static IEnumerable<PopupRoot> GetActivePopupRoots() => PopupRootIdentifierToInstance;
        
        /// <summary>
        /// Returns the coordinates of the UIElement, relative to the Window that contains it.
        /// </summary>
        /// <param name="element">The element of which the position will be returned.</param>
        /// <returns>The position of the element relative to the Window that contains it.</returns>
        public static Point GetUIElementAbsolutePosition(UIElement element)
        {
            if (INTERNAL_VisualTreeManager.IsElementInVisualTree(element))
            {
                GeneralTransform gt = element.TransformToVisual(Window.GetWindow(element));
                return gt.Transform(new Point(0d, 0d));
            }
            return new Point();
        }
    }
}
