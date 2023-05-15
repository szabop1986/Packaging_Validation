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


        private void BFZ_Serial_Package_KeyDown(object sender, KeyEventArgs e)
        {
            string pressed_key = e.Key.ToString();
            if (pressed_key == "Return")
            {
                UKL_Textbox_RAU.Focus();
                UKL_Textbox_RAU.SelectAll();
            }
        }

        private void UKL_Textbox_Package_KeyDown(object sender, KeyEventArgs e)
        {

            string pressed_key = e.Key.ToString();
            if (pressed_key == "Return")
            {
                UKL_Textbox_RAU.Focus();
                UKL_Textbox_RAU.SelectAll();
            }

        }

        private void UKL_Textbox_RAU_KeyDown(object sender, KeyEventArgs e)
        {
            
            string pressed_key = e.Key.ToString();
           
            if (pressed_key == "Return")
            {

                bool UKLScannedToUKLTextBox = false;

                if (Convert.ToString(UKL_Textbox_RAU.Text).Contains("CR9") || Convert.ToString(UKL_Textbox_RAU.Text).Contains("cr9"))
                {
                    UKLScannedToUKLTextBox = true;                    
                }
                else
                {MessageBox.Show("A scannelt cimke nem tartalmaz CR9 -el kezdődő szériaszámot!");}


                if (UKL_radio_button.IsChecked == false & BFZ_Radio_button.IsChecked == false)
                { MessageBox.Show("Nincs kiválasztva csomagolási szint!"); }

                //UKL case

                if (UKL_radio_button.IsChecked == true & Convert.ToString(UKL_Textbox_Package.Text) != "" & UKLScannedToUKLTextBox==true)
                {
                    string UKL_Textbox_Package_raw = Convert.ToString(UKL_Textbox_Package.Text);
                    string UKL_Textbox_RAU_raw = Convert.ToString(UKL_Textbox_RAU.Text);

                    bool IS_UKL_Package_type = UKL_Textbox_Package_raw.Contains("UKL");
                    bool IS_UKL_RAU_type = UKL_Textbox_RAU_raw.Contains("UKL");

                    int index_of_cr9_in_package_raw = UKL_Textbox_Package_raw.IndexOf("CR9");
                    int index_of_cr9_in_rau_raw = UKL_Textbox_RAU_raw.IndexOf("CR9");

                    string RAU_Scanned_Serial = UKL_Textbox_RAU_raw.Substring(index_of_cr9_in_rau_raw, 10);
                    string Box_Scanned_Serial = UKL_Textbox_Package_raw.Substring(index_of_cr9_in_package_raw, 10);

                    if (RAU_Scanned_Serial.Length == Box_Scanned_Serial.Length)
                    {
                   
                        if (Convert.ToString(RAU_Scanned_Serial) != Convert.ToString(Box_Scanned_Serial))
                        {
                            MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                            User_message_label.Content = "NOK";
                            MessageBox.Show("A bevitt szériaszámok nem egyeznek meg! (Eltérő karakterek) \n" +
                                "Ellenőrizd hogy biztosan jó terméket raksz -e a dobozba!");

                            UKL_Textbox_Package.Focus();
                            UKL_Textbox_Package.SelectAll();
                        }

                        if (Convert.ToString(RAU_Scanned_Serial) == Convert.ToString(Box_Scanned_Serial))
                        {
                          
                            string Rautest_check_result = "";
                            Validate_serial.Validate_Rautest(RAU_Scanned_Serial, out Rautest_check_result);

                            if(Rautest_check_result == "Not Balder" || Rautest_check_result =="Pass")
                            {                            
                                    MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                                    User_message_label.Content = "OK";
                                    Create_MES_Log log = new Create_MES_Log();
                                    log.Create_MES_LOG(Box_Scanned_Serial);
                                    UKL_Textbox_Package.Focus();
                                    UKL_Textbox_Package.SelectAll();                               
                            }

                            if (Rautest_check_result == "Fail" || Rautest_check_result == "Not tested")
                            {
                                MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                User_message_label.Content = "NOK";
                                MessageBox.Show("A termék Rautesten nem tesztelt vagy az utolsó teszt eredménye bukás! \n Értesítsd a csoportvezetőt","Folyamat hiba");
                                BFZ_Serial_Package.Focus();
                                BFZ_Serial_Package.SelectAll();
                            }                               
                            }

                        
                    } else { MessageBox.Show("A bevitt szériaszámok nem egyeznek meg! (Karakterek száma biztosan eltér)");
                        MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        User_message_label.Content = "NOK";

                        UKL_Textbox_Package.Focus();
                        UKL_Textbox_Package.SelectAll();
                    }

                }

                //  BFZ case 

                if (BFZ_Radio_button.IsChecked == true & Convert.ToString(BFZ_Serial_Package.Text) != "" & UKLScannedToUKLTextBox == true)
                {
                    try {
                    string BFZ_Textbox_Package_raw = Convert.ToString(BFZ_Serial_Package.Text);
                    string UKL_Textbox_RAU_raw = Convert.ToString(UKL_Textbox_RAU.Text);

                    bool IS_UKL_Package_type = BFZ_Textbox_Package_raw.Contains("BFZ");
                    bool IS_UKL_RAU_type = UKL_Textbox_RAU_raw.Contains("UKL");

                    int index_of_cr9_in_package_raw = BFZ_Textbox_Package_raw.IndexOf("CR9");
                    int index_of_cr9_in_rau_raw = UKL_Textbox_RAU_raw.IndexOf("CR9");

                    string RAU_Scanned_Serial = UKL_Textbox_RAU_raw.Substring(index_of_cr9_in_rau_raw, 10);
                    string Box_Scanned_Serial = BFZ_Textbox_Package_raw.Substring(index_of_cr9_in_package_raw, 10);


                    String Result_string = "";

                     Validate_serial.Validate_Serial(RAU_Scanned_Serial, Box_Scanned_Serial, out Result_string);
                     

                if(Result_string == "Linking OK")
                    {
                        MESSAGE_TO_USER.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                        User_message_label.Content = "OK";

                        Create_MES_Log log = new Create_MES_Log();
                        log.Create_MES_LOG(Box_Scanned_Serial);

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

                      



                    }
                    catch(Exception ex) { MessageBox.Show("Ellenőrizd a scannelt szériaszámot!" +"\n\n" + ex.Message); }

                }

                    // Itt a BFZ vége
                }
        }

     
    }
}
