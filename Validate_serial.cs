using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Box_validation
{
    class Validate_serial

    {      
        public static void Validate_Rautest(string Radio_serial, out string Rautest_status)
        {

            bool is_Balder = false;
            string child_serial = "";
            Rautest_status = "No info";

            List<MyMESServices.OneMESHistoryRow> bh = MyMESServices.LongBoardHistory.Get(Radio_serial);
            
            // Check if radio is a Balder 
            for (int i = 0; i <= bh.Count - 1; i++)
            {
                if (bh[i].Test_Process == "BIRTH / BIRTH")
                {
                    if (bh[i].Assembly.Contains("6365") || bh[i].Assembly.Contains("Balder"))
                    {
                        is_Balder = true;
                        break;
                    }
                }              
            }

            if(is_Balder != true) { Rautest_status = "Not Balder";}

            //Get last child serial
            if (is_Balder == true)  
            {
                for (int i = 0; i <= bh.Count - 1; i++)
                {
                    if (bh[i].TestType == "Child Link")
                    {
                        child_serial = bh[i].ChildSerialNumber;
                        break;
                    }
                }

                if(child_serial == "")
                {
                    MessageBox.Show("Nincs UKZ serial linkelve", "Linkelési hiba");                  
                }


                //Inspect child

                
                List<MyMESServices.OneMESHistoryRow> ch = MyMESServices.LongBoardHistory.Get(child_serial);
                                
                for (int j = 0; j <= ch.Count - 1; j++)
                {
                    if (ch[j].Test_Process == "FVT / RAUTEST" & ch[j].TestType == "TEST")
                    {
                        Rautest_status = ch[j].TestStatus;
                        break;
                    }
                    else Rautest_status = "Not tested";
                }
            }
            
        }




        public static void Validate_Serial(string raw_serial_UKL, string raw_serial_Package, out string Code)
        {
            List<MyMESServices.OneMESHistoryRow> bh = MyMESServices.LongBoardHistory.Get(raw_serial_UKL);
            Code = "NOK";

            for (int i =0 ; i<=bh.Count - 1; i++)
            {
                if (bh[i].TestType == "Child Link")
                {
                    if (bh[i].ChildSerialNumber == raw_serial_UKL & bh[i].ParentSerialNumber == raw_serial_Package)
                    {                       
                        Code = "Linking OK";
                        break;
                    }
                }

                if (bh[i].TestType == "Child Link")
                {
                    if (bh[i].ChildSerialNumber == raw_serial_UKL & bh[i].ParentSerialNumber != raw_serial_Package)
                    {                      
                        MessageBox.Show("A " + raw_serial_UKL + " serial jelenleg ebbe a szériaszámú dobozba van linkelve: " + bh[i].ParentSerialNumber);
                        Code = "NOK";
                    }
                }               
            }


     
        }    
    }
}
