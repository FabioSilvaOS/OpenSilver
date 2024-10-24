﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
    /// <summary>
    /// Exposes <see cref="TreeView" /> types to UI
    /// automation.
    /// </summary>
    /// <QualityBand>Stable</QualityBand>
    public partial class TreeViewAutomationPeer : FrameworkElementAutomationPeer, ISelectionProvider
    {
        /// <summary>
        /// Gets the TreeView that owns this TreeViewAutomationPeer.
        /// </summary>
        private TreeView OwnerTreeView
        {
            get { return (TreeView)Owner; }
        }

        /// <summary>
        /// Gets a value indicating whether the UI automation provider
        /// allows more than one child element to be selected at the same time.
        /// </summary>
        /// <value>
        /// True if multiple selection is allowed; otherwise, false.
        /// </value>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        bool ISelectionProvider.CanSelectMultiple
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the UI automation provider
        /// requires at least one child element to be selected.
        /// </summary>
        /// <value>
        /// True if selection is required; otherwise, false.
        /// </value>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        bool ISelectionProvider.IsSelectionRequired
        {
            get { return false; }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TreeViewAutomationPeer" />
        /// class.
        /// </summary>
        /// <param name="owner">
        /// The <see cref="TreeView" /> to associate
        /// with the
        /// <see cref="TreeViewAutomationPeer" />.
        /// </param>
        public TreeViewAutomationPeer(TreeView owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Gets the control type for the
        /// <see cref="TreeView" /> that is associated
        /// with this
        /// <see cref="TreeViewAutomationPeer" />.
        /// This method is called by
        /// <see cref="AutomationPeer.GetAutomationControlType" />.
        /// </summary>
        /// <returns>
        /// The
        /// <see cref="AutomationControlType.Tree" />
        /// enumeration value.
        /// </returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Tree;
        }

        /// <summary>
        /// Gets the name of the
        /// <see cref="TreeView" /> that is associated
        /// with 
        /// <see cref="TreeViewAutomationPeer" />.
        /// This method is called by
        /// <see cref="AutomationPeer.GetClassName" />.
        /// </summary>
        /// <returns>A string that contains TreeView.</returns>
        protected override string GetClassNameCore()
        {
            return "TreeView";
        }

        /// <summary>
        /// Gets a control pattern for the
        /// <see cref="TreeView" /> that is associated
        /// with this
        /// <see cref="TreeViewAutomationPeer" />.
        /// </summary>
        /// <param name="patternInterface">
        /// One of the enumeration values that indicates the control pattern.
        /// </param>
        /// <returns>
        /// The object that implements the pattern interface, or null if the
        /// specified pattern interface is not implemented by this peer.
        /// </returns>
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Selection)
            {
                return this;
            }
            else if (patternInterface == PatternInterface.Scroll)
            {
                ScrollViewer scroll = OwnerTreeView.ItemsControlHelper.ScrollHost;
                if (scroll != null)
                {
                    AutomationPeer peer = FrameworkElementAutomationPeer.CreatePeerForElement(scroll);
                    IScrollProvider provider = peer as IScrollProvider;
                    if (provider != null)
                    {
                        peer.EventsSource = this;
                        return provider;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the collection of child elements of the
        /// <see cref="TreeView" /> that is associated
        /// with this
        /// <see cref="TreeViewAutomationPeer" />.
        /// </summary>
        /// <returns>The collection of child elements.</returns>
        protected override List<AutomationPeer> GetChildrenCore()
        {
            TreeView owner = OwnerTreeView;

            ItemCollection items = owner.Items;
            if (items.Count <= 0)
            {
                return null;
            }

            List<AutomationPeer> peers = new List<AutomationPeer>(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                TreeViewItem element = owner.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                if (element != null)
                {
                    peers.Add(
                        FrameworkElementAutomationPeer.FromElement(element) ??
                        FrameworkElementAutomationPeer.CreatePeerForElement(element));
                }
            }

            return peers;
        }

        /// <summary>
        /// Retrieves a UI automation provider for each child element that is
        /// selected.
        /// </summary>
        /// <returns>An array of UI automation providers.</returns>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        IRawElementProviderSimple[] ISelectionProvider.GetSelection()
        {
            IRawElementProviderSimple[] selection = null;

            TreeViewItem selectedItem = OwnerTreeView.SelectedContainer;
            if (selectedItem != null)
            {
                AutomationPeer peer = FrameworkElementAutomationPeer.FromElement(selectedItem);
                if (peer != null)
                {
                    selection = new IRawElementProviderSimple[] { ProviderFromPeer(peer) };
                }
            }

            return selection ?? new IRawElementProviderSimple[] { };
        }
    }
}
