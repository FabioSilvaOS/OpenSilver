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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Runtime.Serialization
{
    internal partial class MemberInformationAndValue
    {
        public MemberInformationAndValue(MemberInformation memberInformation, object memberValue)
        {
            this.MemberInformation = memberInformation;
            this.MemberValue = memberValue;
        }

        public MemberInformation MemberInformation;
        public object MemberValue;
    }
}
