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
using System.ComponentModel;
using OpenSilver.Internal;

namespace CSHTML5.Internal.Attributes
{
    [Obsolete(Helper.ObsoleteMemberMessage)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class OutputRootPathAttribute : Attribute
    {
        public OutputRootPathAttribute(string outputRootPath)
        {
            this.OutputRootPath = outputRootPath;
        }

        public string OutputRootPath { get; private set; }
    }
}