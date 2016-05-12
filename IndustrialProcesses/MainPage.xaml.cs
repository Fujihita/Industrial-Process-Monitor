using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using IndustrialProcesses.Resources;
using System.Windows.Media;
using IndustrialProcesses.ViewModels;
using System.Windows.Data;
using System.Threading.Tasks;
using IndustrialProcesses;
using System.Threading;
using System.Windows.Threading;

namespace IndustrialProcesses
{
    public partial class MainPage : PhoneApplicationPage
    {
        Color currentAccentColorHex = (Color)Application.Current.Resources["PhoneAccentColor"];
        Color currentInactiveColorHex = (Color)Application.Current.Resources["PhoneInactiveColor"];

        ProcessUpdater[] updaters = new ProcessUpdater[15];
        // bool[] IsVisible = new Boolean[15];


        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            int i = 0;
            // index items from model into arrays
            foreach (ItemViewModel item in App.ViewModel.Items)
            {
                updaters[i] = new ProcessUpdater(App.ViewModel.Items[i], textBlock_status);
                updaters[i].Update();
                i++; // getting maximum elements in ViewModel
            }
        }

        //Animated effect for visibility panel and dashboard visibility control method
        #region Visibility handler
        private void StackPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock header = (sender as StackPanel).Children.ElementAt(0) as TextBlock;
            TextBlock tooltip = (sender as StackPanel).Children.ElementAt(2) as TextBlock;
            int j = getModel(header.Text);
            if (j == -1)
            {
                return;
            }

            /*SearchVisualTree(Dashboard, header.Text);
            if (control == null)
            {
                header.Foreground = new SolidColorBrush(Colors.Green);
                return;
            }*/
            if ((string)(sender as StackPanel).Tag == "true")
            {
                header.Foreground = new SolidColorBrush(currentInactiveColorHex);
                tooltip.Text = "Tap to start";
                (sender as StackPanel).Tag = "false";
                App.ViewModel.Items[j].visibility = Visibility.Collapsed;
                //IsVisible[j] = false;
            }
            else if ((string)(sender as StackPanel).Tag == "false")
            {
                header.Foreground = new SolidColorBrush(currentAccentColorHex);
                tooltip.Text = "Tap to stop";
                (sender as StackPanel).Tag = "true";
                App.ViewModel.Items[j].visibility = Visibility.Visible;
                //IsVisible[j] = true;
                //if (!updaters[j].flag)
                //{
                //    updaters[j].Update();
                //}
            }
            //(control.Parent as StackPanel).Visibility = model.visibility;
        }
        #endregion

        //LongListSelector query method & ViewModel lookup method
        #region Query methods
        /*private void SearchVisualTree(DependencyObject targetElement, string searchString)
        {
            var count = VisualTreeHelper.GetChildrenCount(targetElement);
            if (count == 0)
                return;

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(targetElement, i);
                if (child is TextBlock)
                {
                    TextBlock targetItem = (TextBlock)child;

                    if (targetItem.Text == searchString)
                    {
                        control = targetItem;
                        return;
                    }
                }
                else
                {
                    SearchVisualTree(child, searchString);
                }
            }
        }*/

        // Determine the index of a matched \path model
        private int getModel(string search)
        {
            int j = -1;
            foreach (ItemViewModel item in App.ViewModel.Items)
            {
                j++;
                if (item.path == search)
                {
                    return j;
                }

            }
            return j;
        }
        #endregion
    }

    #region Asychronous Webclient updater class
    public class ProcessUpdater
    {
        ItemViewModel control;
        WebClient wc = new WebClient();
        TextBlock status;
        private bool flag = false;

        public ProcessUpdater(ItemViewModel item, TextBlock stat)
        {
            control = item; // getter
            status = stat;

            // create 2s dispatch timer event for periodic data update
            DispatcherTimer Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
            Timer.Interval = TimeSpan.FromMilliseconds(500);
            Timer.Start();

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (flag == false)
                Update();
        }

        public void Update()
        {
            if (control.visibility == Visibility.Visible)
            {
                flag = true;
                wc.CancelAsync();
                wc.DownloadStringCompleted += HttpCompleted;
                // Append salt to the uri to prevent response caching
                wc.DownloadStringAsync(new Uri("http://ivlab.azurewebsites.net" + control.path + "?" + DateTime.Now.Ticks.ToString()));
            }
        }
        private void HttpCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                control.value = e.Result;
                flag = false;
                if (status.Text == "Failed to update: " + control.path)
                    status.Text = "";
                // Update();
            }
            else
            {
                wc.CancelAsync();
                status.Text = "Failed to update: " + control.path;
                //MessageBox.Show(control.path + " update failed: " + e.Error);   
                flag = false;
            }

        }
    }
    #endregion
}
