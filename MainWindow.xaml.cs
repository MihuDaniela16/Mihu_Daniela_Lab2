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
using static Mihu_Daniela_Lab2.DoughnutMachine;

namespace Mihu_Daniela_Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int mRaisedGlazed;
        private int mRaisedSugar;
        private int mFilledLemon;
        private int mFilledChocolate;
        private int mFilledVanilla;

        private DoughnutMachine myDoughnutMachine;
        public object txtGlazedRaised;
        private object txtLemonFilled;
        private object txtChocolateFilled;
        private object txtVanillaFilled;

        public MainWindow()
        {
            InitializeComponent();
            CommandBinding cmdl = new CommandBinding();
            cmdl.Command = ApplicationCommands.Print;
            ApplicationCommands.Print.InputGestures.Add(new KeyGesture(Key.I, ModifierKeys.Alt));
            cmdl.Executed += new ExecutedRoutedEventHandler(CtrlP_CommandHandler);
            this.CommandBindings.Add(cmdl);

            CommandBinding cmd2 = new CommandBinding();
            cmd2.Command = CustomCommands.StopCommand.Launch;
            cmd2.Executed += new
            ExecutedRoutedEventHandler(CtrlS_CommandHandler);
            this.CommandBindings.Add(cmd2);

        }

        private void frmMain_Loaded(object sender, RoutedEventArgs e)
        {
            myDoughnutMachine = new DoughnutMachine();
            myDoughnutMachine.DoughnutComplete += new
            DoughnutMachine.DoughnutCompleteDelegate(DoughnutCompleteHandler);

            cmbType.ItemsSource = PriceList;
            cmbType.DisplayMemberPath = "Key";
            cmbType.SelectedValuePath = "Value";
        }

        private void glazedToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            glazedToolStripMenuItem.IsChecked = true;
            sugarToolStripMenuItem.IsChecked = false;
            myDoughnutMachine.MakeDoughnuts(DoughnutMachine.DoughnutType.Glazed);
        }
        private void sugarToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            sugarToolStripMenuItem.IsChecked = true;
            glazedToolStripMenuItem.IsChecked = false;
            myDoughnutMachine.MakeDoughnuts(DoughnutMachine.DoughnutType.Sugar);
        }
        private void DoughnutCompleteHandler()
        {
            switch (myDoughnutMachine.Flavor)
            {
                case DoughnutType.Glazed:
                    mRaisedGlazed++;
                    txtGlazedRaised = mRaisedGlazed.ToString();
                    break;

                case DoughnutType.Sugar:
                    mRaisedSugar++;
                    txtGlazedRaised_.Text = mRaisedSugar.ToString();
                    break;

                case DoughnutType.Lemon:
                    mFilledLemon++;
                    txtLemonFilled_.Text = mFilledLemon.ToString();
                    break;

                case DoughnutType.Chocolate:
                    mFilledChocolate++;
                    txtChocolateFilled_.Text = mFilledChocolate.ToString();
                    break;

                case DoughnutType.Vanilla:
                    mFilledVanilla++;
                    txtVanillaFilled = mFilledVanilla.ToString();
                    break;

            }
        }

        private void stopToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myDoughnutMachine.Enabled = false;
            this.Title = "Virtual Doughnuts Factory";
            e.Handled = true;
        }



        private void txtQuantity_KeyPress(object sender, KeyEventArgs e)
        {
            if (!(e.Key >= Key.D0 && e.Key <= Key.D9))
            {
                MessageBox.Show("Numai cifre se pot introduce!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FilledItems_Click(object sender, RoutedEventArgs e)
        {
            string DoughnutFlavour;

            MenuItem SelectedItem = (MenuItem)e.OriginalSource;
            DoughnutFlavour = SelectedItem.Header.ToString();

            Enum.TryParse(DoughnutFlavour, out DoughnutType myFlavour);
            myDoughnutMachine.MakeDoughnuts(myFlavour);
        }

        private void FilledItemsShow_Click(object sender, RoutedEventArgs e)
        {
            string mesaj;

            MenuItem SelectedItem = (MenuItem)e.OriginalSource;
            mesaj = SelectedItem.Header.ToString() + " doughnuts are being cooked!";
            this.Title = mesaj;
        }

        KeyValuePair<DoughnutType, double>[] PriceList =
        {
            new KeyValuePair<DoughnutType, double>(DoughnutType.Sugar, 2.5),
            new KeyValuePair<DoughnutType, double>(DoughnutType.Glazed, 3),
            new KeyValuePair<DoughnutType, double>(DoughnutType.Chocolate, 4.5),
            new KeyValuePair<DoughnutType, double>(DoughnutType.Vanilla, 4),
            new KeyValuePair<DoughnutType, double>(DoughnutType.Lemon, 3.5)
        };
        DoughnutType selectedDoughnut;

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtPrice.Text = cmbType.SelectedValue.ToString();
            KeyValuePair<DoughnutType, double> selectedEntry = (KeyValuePair<DoughnutType, double>)cmbType.SelectedItem;
            selectedDoughnut = selectedEntry.Key;

        }
        private int ValidateQuantity(DoughnutType selectedDoughnut)
        {
            int q = int.Parse(txtQuantity_.Text);
            int r = 1;

            switch (selectedDoughnut)
            {
                case DoughnutType.Glazed:
                    if (q > mRaisedGlazed)
                        r = 0;
                    break;
                case DoughnutType.Sugar:
                    if (q > mRaisedSugar)
                        r = 0;
                    break;
                case DoughnutType.Chocolate:
                    if (q > mFilledChocolate)
                        r = 0;
                    break;
                case DoughnutType.Lemon:
                    if (q > mFilledLemon)
                        r = 0;
                    break;
                case DoughnutType.Vanilla:
                    if (q > mFilledVanilla)
                        r = 0;
                    break;
            }
            return r;
        }

        private void btnAddToSale_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateQuantity(selectedDoughnut) > 0)
            {
                lstSale_.Items.Add(txtQuantity_.Text + " " + selectedDoughnut.ToString() + ":" + txtPrice.Text + " " + double.Parse(txtQuantity_.Text) * double.Parse(txtPrice.Text));
            }
            else
            {
                MessageBox.Show("Cantitatea introdusa nu este disponibila in stoc!");
            }
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            lstSale_.Items.Remove(lstSale_.SelectedItem);
        }

        private void btnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            txtTotal.Text = (double.Parse(txtTotal.Text) + double.Parse(txtQuantity_.Text) * double.Parse(txtPrice.Text)).ToString();
            foreach (string s in lstSale_.Items)
            {
                switch (s.Substring(s.IndexOf(" ") + 1, s.IndexOf(":") - s.IndexOf(" ") - 1))
                {
                    case "Glazed":
                        mRaisedGlazed = mRaisedGlazed - Int32.Parse(s.Substring(0, s.IndexOf(" ")));
                        txtGlazedRaised = mRaisedGlazed.ToString();
                        break;
                    case "Sugar":
                        mRaisedSugar = mRaisedSugar - Int32.Parse(s.Substring(0, s.IndexOf(" ")));
                        txtSugarRaised_.Text = mRaisedSugar.ToString();
                        break;
                    case "Chocolate":
                        mFilledChocolate = mFilledChocolate - Int32.Parse(s.Substring(0, s.IndexOf(" ")));
                        txtChocolateFilled_.Text = mFilledChocolate.ToString();
                        break;
                    case "Lemon":
                        mFilledLemon = mFilledLemon - Int32.Parse(s.Substring(0, s.IndexOf(" ")));
                        txtLemonFilled_.Text = mFilledLemon.ToString();
                        break;
                    case "Vanilla":
                        mFilledVanilla = mFilledVanilla - Int32.Parse(s.Substring(0, s.IndexOf(" ")));
                        txtVanillaFilled = mFilledVanilla.ToString();
                        break;
                }
            }
        }

        private void CtrlP_CommandHandler(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have in stock:" + mRaisedGlazed + " Glazed," + mRaisedSugar + " Sugar," + mFilledLemon + " Lemon," + mFilledChocolate + " Chocolate," + mFilledVanilla + " Vanilla");
        }


    }   
}
    
