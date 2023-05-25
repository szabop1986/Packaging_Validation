using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Box_validation
{
    class SAP_validation
    {
        internal static bool Validate_SAP_Label_On_Box(string UKL_type, string Scanned_SAP_label_content)
        {
            bool result = false;

            if (Scanned_SAP_label_content.Contains(UKL_type))
            {
                result = true;
            }

            return result;
        }


    }
}
