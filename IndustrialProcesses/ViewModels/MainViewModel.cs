using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using IndustrialProcesses.Resources;
using System.Windows;
using System.Windows.Data;

namespace IndustrialProcesses.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Items.Add(new ItemViewModel() { path = "/tank1", description = "Füllstand Tank 1 (in Liter)", value = "203", type = "[int]", min="200", max="260", visibility= Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/tank2", description = "Füllstand Tank 2 (in Liter)", value = "503", type = "[int]", min="500", max="560", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/tank3", description = "Füllstand Tank 3 (in Liter)", value = "1004", type = "[float]", min="1000", max="1007.5", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/pumpe1", description = "Drehzahl Pumpe 1 (in Umdrehungen pro Minute)", value = "123", type = "[int]", min = "120", max = "130", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/pumpe2", description = "Drehzahl Pumpe 2 (in Umdrehungen pro Minute)", value = "123", type = "[int]", min = "120", max = "130", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/pumpe3", description = "Drehzahl Pumpe 3 (in Umdrehungen pro Minute)", value = "123", type = "[int]", min = "120", max = "130", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/adg1", description = "Druck Ausgleichsbehälter 1 (in Bar)", value = "13", type = "[int]", min = "10", max = "15", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/adg2", description = "Druck Ausgleichsbehälter 2 (in Bar)", value = "13", type = "[int]", min = "10", max = "15", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/temp-zulauf", description = "Temperatur Zulauf (in Celsius)", value = "60", type = "[int]", min = "60", max = "70", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/temp-ruecklauf", description = "Temperatur Rücklauf (in Celsius)", value = "56", type = "[int]", min = "50", max = "60", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/spannung-steuerung", description = "Spannung Steuerung (in Volt)", value = "230", type = "[float]", min = "229", max = "231", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/spannung-pumpe1", description = "Spannung Pumpe 1 (in Volt)", value = "400", type = "[float]", min = "398", max = "401", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/spannung-pumpe2", description = "Spannung Pumpe 2 (in Volt)", value = "400", type = "[float]", min = "398", max = "401", visibility = Visibility.Visible });
            this.Items.Add(new ItemViewModel() { path = "/spannung-pumpe3", description = "Spannung Pumpe 3 (in Volt)", value = "400", type = "[float]", min = "398", max = "401", visibility = Visibility.Visible });
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}