using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Box_validation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {     
        public MainWindow()
        {
            InitializeComponent();
            // MessageBox.Show("Először a doboz tartalmat scanneld be, majd a terméket!");
        }

        private void UKL_Textbox_RAU_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void UKL_radio_button_Checked(object sender, RoutedEventArgs e)
        {
            BFZ_Serial_Package.IsEnabled = false;
            UKL_Textbox_Package.IsEnabled = true;
            UKL_Textbox_Package.Focus();
        }

        private void BFZ_Radio_button_Checked(object sender, RoutedEventArgs e)
        {
            BFZ_Serial_Package.IsEnabled = true;
            UKL_Textbox_Package.IsEnabled = false;
            BFZ_Serial_Package.Focus();
        }

      
        string Type_of_BFZ_on_Package = "";
        string BFZ_level_Box_Scanned_Serial = "";

        private void BFZ_Serial_Package_KeyDown(object sender, KeyEventArgs e)
        {
            string pressed_key = e.Key.ToString();
            if (pressed_key == "Return")
            {
                try {

                    string BFZ_Textbox_Package_raw = Convert.ToString(BFZ_Serial_Package.Text).Replace('ö', '0').Replace('-', '/').Replace('Y', 'Z').Trim();
                   // MessageBox.Show(BFZ_Textbox_Package_raw);

                    bool IS_BFZ_Package_type = BFZ_Textbox_Package_raw.Contains("BFZ");
                    int index_of_cr9_in_package_raw = BFZ_Textbox_Package_raw.IndexOf("SCR9")+1;
                    int index_of_BFZ_string = BFZ_Textbox_Package_raw.IndexOf("BFZ");
                    int index_of_21P_string = BFZ_Textbox_Package_raw.IndexOf("21P");

                    int BFZ_code_length = index_of_21P_string - index_of_BFZ_string;

                    Type_of_BFZ_on_Package = BFZ_Textbox_Package_raw.Substring(index_of_BFZ_string, BFZ_code_length);
                    BFZ_level_Box_Scanned_Serial = BFZ_Textbox_Package_raw.Substring(index_of_cr9_in_package_raw,10);
                    BFZ_Serial_Package.Text = BFZ_level_Box_Scanned_Serial;

                 //   MessageBox.Show(BFZ_level_Box_Scanned_Serial);
                 //   MessageBox.Show(Type_of_BFZ_on_Package);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " \n Valószínűleg nem a PDF 417 kódot scannelted be");
                }

                SAP_Label_Textbox.Focus();
                SAP_Label_Textbox.SelectAll();
            }
        }
          
        


        string Type_of_UKL_on_Package = "";
        string UKL_level_Box_Scanned_Serial = "";
       
        private void UKL_Textbox_Package_KeyDown(object sender, KeyEventArgs e)
        {
            string pressed_key = e.Key.ToString();
            if (pressed_key == "Return")
            {
                try
                {
                string UKL_Textbox_Package_raw = Convert.ToString(UKL_Textbox_Package.Text).Replace('ö', '0').Replace('-', '/').Replace('Y', 'Z').Trim();
                
                int index_of_UKL_string = UKL_Textbox_Package_raw.IndexOf("UKL");
                int index_of_21P_string = UKL_Textbox_Package_raw.IndexOf("21P");
                int UKL_code_length = UKL_Textbox_Package_raw.IndexOf("21P") - UKL_Textbox_Package_raw.IndexOf("UKL");

                int index_of_cr9_in_package_raw = UKL_Textbox_Package_raw.IndexOf("SCR9")+1;
                UKL_level_Box_Scanned_Serial = UKL_Textbox_Package_raw.Substring(index_of_cr9_in_package_raw, 10).Replace('ö', '0');
                Type_of_UKL_on_Package = UKL_Textbox_Package_raw.Substring(index_of_UKL_string, UKL_code_length);

             
                UKL_Textbox_Package.Text = UKL_level_Box_Scanned_Serial;

                SAP_Label_Textbox.Focus();
                SAP_Label_Textbox.SelectAll();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message + " \n Valószínűleg nem a PDF 417 kódot scannelted be");
                }
            }

        }


        string SAP_label_content = "";

        private void SAP_Label_Textbox_KeyDown(object sender, KeyEventArgs e)
        {
            string pressed_key = e.Key.ToString();
            if (pressed_key == "Return")
            {             
                SAP_label_content = Convert.ToString(SAP_Label_Textbox.Text).Replace('ö', '0').Replace('-', '/').Replace('Y', 'Z').Trim();

                if (SAP_label_content[0].Equals('P')){

                    SAP_label_content = SAP_label_content.Substring(2);
                }
                else
                {
                    SAP_label_content = SAP_label_content.Substring(1);
                }

                SAP_Label_Textbox.Text = SAP_label_content;

                UKL_Textbox_RAU.Focus();
                UKL_Textbox_RAU.SelectAll();
            }       
        }


        private void UKL_Textbox_RAU_KeyDown(object sender, KeyEventArgs e)
        {          
            string pressed_key = e.Key.ToString();
           
            if (pressed_key == "Return")
            {
                // Termék UKL cimke adatok kinyerése
                int index_of_cr9_in_rau_raw = UKL_Textbox_RAU.Text.IndexOf("CR9");
               
                string RAU_Scanned_Serial =Convert.ToString(UKL_Textbox_RAU.Text).Substring(index_of_cr9_in_rau_raw, 10).Replace('ö', '0');

                bool Is_RAU_UKL_Label_Scanned = false;

                if (UKL_Textbox_RAU.Text.Contains("MINI") || UKL_Textbox_RAU.Text.Contains("LINK") || UKL_Textbox_RAU.Text.Contains("TRX"))
                {
                    Is_RAU_UKL_Label_Scanned = true;
                }
                else
                {
                    MessageBox.Show("Úgy tűnik nem a Radio unit UKL cimkéjét scannelted be!");
                    UKL_Textbox_RAU.Focus();
                    UKL_Textbox_RAU.SelectAll(); 
                }


                UKL_Textbox_RAU.Text = RAU_Scanned_Serial;

               

                bool UKLScannedToUKLTextBox = false;

                if (Convert.ToString(UKL_Textbox_RAU.Text).Contains("CR9") || Convert.ToString(UKL_Textbox_RAU.Text).Contains("cr9"))
                {
                    UKLScannedToUKLTextBox = true;                    
                }
                else
                {
                    MessageBox.Show("A scannelt cimke nem tartalmaz CR9 -el kezdődő szériaszámot!");
                }


                if (UKL_radio_button.IsChecked == false & BFZ_Radio_button.IsChecked == false)
                {
                MessageBox.Show("Nincs kiválasztva csomagolási szint!");
                }

                //UKL case
               if (UKL_radio_button.IsChecked == true && UKLScannedToUKLTextBox==true && Is_RAU_UKL_Label_Scanned ==true )
                {
                                      
                    bool Is_SAP_label_matching_to_UKL = SAP_validation.Validate_SAP_Label_On_Box(Type_of_UKL_on_Package, Convert.ToString(SAP_Label_Textbox.Text));


                    if (RAU_Scanned_Serial.Length == UKL_level_Box_Scanned_Serial.Length)
                    {
                   
                        if (Convert.ToString(RAU_Scanned_Serial) != Convert.ToString(UKL_level_Box_Scanned_Serial))
                        {
                            MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            User_message_label.Content = "NOK";
                            MessageBox.Show("A bevitt szériaszámok nem egyeznek meg! (Eltérő karakterek) \n" +
                                "Ellenőrizd hogy biztosan jó terméket raksz -e a dobozba!");

                            UKL_Textbox_Package.Focus();
                            UKL_Textbox_Package.SelectAll();
                        }

                        if ((Convert.ToString(RAU_Scanned_Serial) == Convert.ToString(UKL_level_Box_Scanned_Serial)) && (Is_SAP_label_matching_to_UKL == true))
                        {                          
                            string Rautest_check_result = "";
                            Validate_serial.Validate_Rautest(RAU_Scanned_Serial, out Rautest_check_result);

                            if(Rautest_check_result == "Not Balder" || Rautest_check_result =="Pass")
                            {                            
                                    MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                                    User_message_label.Content = "OK";
                                    Create_MES_Log log = new Create_MES_Log();
                                    log.Create_MES_LOG(UKL_level_Box_Scanned_Serial);
                                    UKL_Textbox_Package.Focus();
                                    UKL_Textbox_Package.SelectAll();                               
                            }

                            if (Rautest_check_result == "Fail" || Rautest_check_result == "Not tested")
                            {
                                MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                User_message_label.Content = "NOK";
                                MessageBox.Show("A termék Rautesten nem tesztelt vagy az utolsó teszt eredménye bukás! \n Értesítsd a csoportvezetőt","Folyamat hiba");
                                UKL_Textbox_Package.Focus();
                                UKL_Textbox_Package.SelectAll();
                            }                               
                        }

                        if (Is_SAP_label_matching_to_UKL == false)
                        {
                            MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            User_message_label.Content = "NOK";
                            MessageBox.Show("Az SAP cimke tartalma és a termék cikkszáma nem egyezik meg! \n Értesítsd a csoportvezetőt!");
                            UKL_Textbox_Package.Focus();
                            UKL_Textbox_Package.SelectAll();
                        }                       
                    }
                    else
                    {
                        MessageBox.Show("A bevitt szériaszámok nem egyeznek meg! (Karakterek száma biztosan eltér)");
                        MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        User_message_label.Content = "NOK";

                        UKL_Textbox_Package.Focus();
                        UKL_Textbox_Package.SelectAll();
                    }

                }


             // BFZ case 
                if (BFZ_Radio_button.IsChecked == true && UKLScannedToUKLTextBox == true && Is_RAU_UKL_Label_Scanned == true)
                {
                    bool Is_SAP_label_matching_to_BFZ = false;
                    //MessageBox.Show(Type_of_BFZ_on_Package);
                    //MessageBox.Show(SAP_label_content);
                    Is_SAP_label_matching_to_BFZ = SAP_validation.Validate_SAP_Label_On_Box(Type_of_BFZ_on_Package, SAP_label_content);

                  //  MessageBox.Show(Convert.ToString(Is_SAP_label_matching_to_BFZ));

                    try
                    {                 
                        String Result_string = "";
                        Validate_serial.Validate_Serial(RAU_Scanned_Serial, BFZ_level_Box_Scanned_Serial, out Result_string);

                        if ((Result_string == "Linking OK") && (Is_SAP_label_matching_to_BFZ==true))
                        {
                            MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                            User_message_label.Content = "OK";

                            Create_MES_Log log = new Create_MES_Log();
                            log.Create_MES_LOG(BFZ_level_Box_Scanned_Serial);

                            BFZ_Serial_Package.Focus();
                            BFZ_Serial_Package.SelectAll();
                        }


                        if (Result_string == "NOK")
                        {
                            MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            User_message_label.Content = "NOK";

                            BFZ_Serial_Package.Focus();
                            BFZ_Serial_Package.SelectAll();
                        }

                        if(Is_SAP_label_matching_to_BFZ == false)
                        {
                            MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            User_message_label.Content = "NOK";
                            MessageBox.Show("Az SAP cimke tartalma és a termék cikkszáma nem egyezik meg! \n Értesítsd a csoportvezetőt!");
                            BFZ_Serial_Package.Focus();
                            BFZ_Serial_Package.SelectAll();
                        }

                    }
                    catch (Exception ex)
                    {
                    MessageBox.Show("Ellenőrizd a scannelt szériaszámot!" + "\n\n" + ex.Message);
                    }

                }
            }
        }

       
    }
}
