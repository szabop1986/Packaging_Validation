using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Box_validation
{
    class Create_MES_Log
    {
        public void Create_MES_LOG(string serial)
        {
    
            String current_time = DateTime.Now.ToString("M\\/dd\\/yyyy HH:mm:ss");
            String current_time_1 = DateTime.Now.ToString("yyyyMdd_HHmmss");
            string filename ="P_S" + serial + "_" + current_time_1;
            string Dummy_dir = @"C:\Users\szabop1\Desktop\"+filename + ".TAR";

            string MES_Dir = @"\\HUTISM0PARSERV1\ERICSSON_PREVAS_TESTERS\" + filename + ".TAR";
                                 
            StreamWriter create_txt = new StreamWriter(MES_Dir);
            create_txt.WriteLine("S"+ serial);
            create_txt.WriteLine("CERICSSON_QM");
            create_txt.WriteLine("NPackaging_QC");
            create_txt.WriteLine("PPackaging_QC");
            create_txt.WriteLine("TP");
            create_txt.WriteLine("O" + Environment.UserName);
            create_txt.WriteLine("[" + current_time);
            create_txt.WriteLine("]" + current_time);
            

            create_txt.Close();

        }

    
    
    }
}
