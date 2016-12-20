using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using BKDelivery.Courier.Model;
using BKDelivery.Domain.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Maps.MapControl.WPF;
using Address = BKDelivery.Domain.Model.Address;

namespace BKDelivery.Courier.ViewModel.Pages
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class RouteViewModel : ViewModelBase
    {
        private RelayCommand _cleanupCommand;
        private IDialogService _dialogService;
        private ObservableCollection<Order> _ordersCollection;
        public ObservableCollection<Order> OrdersCollection
        {
            get { return _ordersCollection; }
            set { Set(() => OrdersCollection, ref _ordersCollection, value); }
        }

        public RouteViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            OrdersCollection = new ObservableCollection<Order>();
        }

        public RelayCommand CleanupCommand
        {
            get
            {
                return _cleanupCommand
                       ?? (_cleanupCommand = new RelayCommand(
                           () =>
                           {
                               var dialog = _dialogService.Show(Helpers.DialogType.BusyWaiting, "Updating route");
                               _myBingMap = (Map)Application.Current.FindResource("MyBingMap");

                               //TODO Download from API

                               var temp = new ObservableCollection<Order>();


                               for (int i = 0; i < 10; i++)
                               {
                                   Order futureOrder = new Order
                                   {
                                       ToAddress =
                                           new Address
                                           {
                                               Street = "Dickensa",
                                               BuildingNumber = "8",
                                               FlatNumber = "",
                                               ZipCode = 01234,
                                               Country = "Poland",
                                               City = "Warszawa",
                                               Voivodeship = "Mazowieckie"
                                           },
                                       TimeInterval =
                                           new TimeInterval
                                           {
                                               Start = DateTime.Now,
                                               End = DateTime.Now
                                           },
                                   };
                                   OrdersCollection.Add(futureOrder);
                               }

                               //DO NOT DELETE
                               _homeAddress = $"Ulica%20{OrdersCollection[0].ToAddress.Street}%20{OrdersCollection[0].ToAddress.BuildingNumber}%20{OrdersCollection[0].ToAddress.City}%20PL";
                               dialog.Hide();
                               GetRoute();
                           }));
            }
        }

        // BING
        private Map _myBingMap;

        private readonly string _bingMapsKey = "Apt92vXYsHTba9UMYF2aSIED288GerifRDi8U531OqE8frFBeRWrtCy0X_VoYgRB";
        private string _destinationAddress= "Ulica%20Koszykowa%2075%20Warszawa%20PL";
        private  string _homeAddress;
        private Dictionary<MapLayer, ItineraryItem> _routeItems;

        public XmlDocument Geocode(string address)
        {
            string geocodeRequest = "http://dev.virtualearth.net/REST/V1/Routes/Driving?o=xml&wp.0=" + _destinationAddress +
                                    "&wp.1=" + address + "&rpo=Points&key=" + _bingMapsKey;
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);

            return geocodeResponse;
        }

        private XmlDocument GetXmlResponse(string requestUrl)
        {
            var request = WebRequest.Create(requestUrl) as HttpWebRequest;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    return null;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return xmlDoc;
            }
        }

        private void DisplayRoute(XmlDocument document)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            XmlNodeList NodeList = document.SelectNodes("//rest:ManeuverPoint", nsmgr);

            if (NodeList.Count == 0)
            {
                MessageBox.Show("Zepsuło się!");
                return;
            }

            var locations = new LocationCollection();
            for (int i = 0; i < NodeList.Count; i++)
            {
                Microsoft.Maps.MapControl.WPF.Location Location = new Microsoft.Maps.MapControl.WPF.Location(
                    double.Parse(NodeList[i]["Latitude"].InnerText.Replace('.', ',')),
                    double.Parse(NodeList[i]["Longitude"].InnerText.Replace('.', ',')));
                locations.Add(Location);
            }

            var myPoliLine = new MapPolyline()
            {
                Locations = locations,
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(Colors.Blue)
            };

            AddPushPin(double.Parse(NodeList[NodeList.Count - 1]["Latitude"].InnerText.Replace('.', ',')),
                double.Parse(NodeList[NodeList.Count - 1]["Longitude"].InnerText.Replace('.', ',')));
            _myBingMap.Children.Add(myPoliLine);
            _myBingMap.ZoomLevel = 12;
            _myBingMap.Focus();
        }

        private void AddPushPin(double Latitude, double Longitude)
        {
            var myPushPin = new Pushpin {Location = new Microsoft.Maps.MapControl.WPF.Location(Latitude, Longitude)};
            _myBingMap.Children.Add(myPushPin);
        }

        private void Route(string key, Action<Response> callback)
        {
            var requestUri =
                new Uri(
                    $"http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={_destinationAddress}&wp.1={_homeAddress}&rpo=Points&key={key}");
            GetResponse(requestUri, callback);
        }

        private void GetResponse(Uri uri, Action<Response> callback)
        {
            try
            {
                var request = WebRequest.Create(uri) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                using (Stream stream = response.GetResponseStream())
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Response));

                    callback?.Invoke(ser.ReadObject(stream) as Response);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void GetKey(Action<string> callback)
        {
            if (callback != null)
            {
                _myBingMap.CredentialsProvider.GetCredentials((c) => { callback(c.ApplicationId); });
            }
        }

        private void GetRoute()
        {
            GetKey(c =>
            {
                Route(c, r =>
                {
                    if (r != null &&
                        r.ResourceSets != null &&
                        r.ResourceSets.Length > 0 &&
                        r.ResourceSets[0].Resources != null &&
                        r.ResourceSets[0].Resources.Length > 0)
                    {
                        var bingRoute = r.ResourceSets[0].Resources[0] as Route;
                        _routeItems = new Dictionary<MapLayer, ItineraryItem>();

                        //for (int i = 0; i < Route.RouteLegs[0].ItineraryItems.Length; i++)
                        //    RouteItems.Add(Route.RouteLegs[0].ItineraryItems[i],
                        //        Route.RouteLegs[0].ItineraryItems[i].ManeuverPoint.Coordinates);

                        double[][] routePath = bingRoute.RoutePath.Line.Coordinates;
                        var locations = new LocationCollection();

                        for (int i = 0; i < routePath.Length; i++)
                        {
                            if (routePath[i].Length >= 2)
                                locations.Add(new Microsoft.Maps.MapControl.WPF.Location(routePath[i][0],
                                    routePath[i][1]));

                            if (i == routePath.Length - 1)
                                AddPushPin(routePath[i][0], routePath[i][1]);
                        }

                        var routeLine = new MapPolyline()
                        {
                            Locations = locations,
                            Stroke = new SolidColorBrush(Colors.Blue),
                            StrokeThickness = 3
                        };

                        _myBingMap.Children.Add(routeLine);

                        foreach (var V in bingRoute.RouteLegs[0].ItineraryItems)
                        {
                            var mapImage = new Image();
                            var imageLayer = new MapLayer();

                            var bitMap = new BitmapImage();

                            bitMap.BeginInit();
                            bitMap.UriSource = new Uri(@"../Resources/Point.png", UriKind.Relative);
                            bitMap.DecodePixelHeight = 7;
                            bitMap.DecodePixelWidth = 7;
                            bitMap.EndInit();

                            mapImage.Stretch = Stretch.None;
                            mapImage.Source = bitMap;
                            mapImage.Opacity = 0.9;
                            _routeItems.Add(imageLayer, V);

                            imageLayer.AddChild(mapImage, new Microsoft.Maps.MapControl.WPF.Location(
                                V.ManeuverPoint.Coordinates[0], V.ManeuverPoint.Coordinates[1]), PositionOrigin.Center);

                            _myBingMap.Children.Add(imageLayer);
                        }

                        _myBingMap.SetView(locations, new Thickness(30), 0);
                    }
                    else
                    {
                        MessageBox.Show("No Results found.");
                    }
                });
            });
        }
    }
}