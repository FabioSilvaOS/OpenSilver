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
using System.Text;
using System.Threading.Tasks;

namespace CSHTML5.Internal
{
    internal static class INTERNAL_ListsHelper
    {
        internal static List<object> ConvertToListOfObjectsOrNull(IEnumerable enumerable)
        {
            List<object> result = null;
            if (enumerable != null)
            {
                result = new List<object>();
                foreach (var obj in enumerable)
                {
                    result.Add(obj);
                }
            }
            return result;
        }

    }
}
