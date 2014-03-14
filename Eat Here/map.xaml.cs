using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Navigation;


using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Shell;

using System.Device.Location;
using Windows.Devices.Geolocation;
using System.Windows.Shapes;
using System.Windows.Media;


namespace Eat_Here
{
    public partial class map : PhoneApplicationPage
    {
        List<GeoCoordinate> MyCoordinates = new List<GeoCoordinate>();
        
        GeocodeQuery Mygeocodequery = null;
        RouteQuery MyQuery = null;
        
        Ellipse myCircle = new Ellipse();
        MapOverlay myLocationOverlay = new MapOverlay();
        MapLayer myLocationLayer = new MapLayer();
       
        public map()
        {
            InitializeComponent();
            this.GetCoordinates();
        }

        private async void GetCoordinates()
        {
            Geolocator MyGeolocator = new Geolocator();

            Mygeocodequery = new GeocodeQuery();

            MyGeolocator.DesiredAccuracyInMeters = 5;
            Geoposition MyGeoPosition = null;
            try
            {
                MyGeoPosition = await MyGeolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10));  // get where I am now
                MyCoordinates.Add(new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude));

                Mygeocodequery.SearchTerm = "250 City Centre Ave, Ottawa, ON";
                Mygeocodequery.GeoCoordinate = new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude);    // Geocode_query_QueryCompleted will be called both for route searches and location searched.  Needs some geolocation (either new GeoCoordinate(0, 0) if we don't have ours, or MyCoordinate )
                
                Mygeocodequery.QueryCompleted += Mygeocodequery_QueryCompleted;
                Mygeocodequery.QueryAsync();

            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Location is disabled in phone settings or capabilities are not checked.");
            }
            catch (Exception ex)
            {
                // Something else happened while acquiring the location.
                MessageBox.Show(ex.Message);
            }
        }

        void Mygeocodequery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Error == null && e.Result.Count > 0)
            {
                MyQuery = new RouteQuery();
                MyCoordinates.Add(e.Result[0].GeoCoordinate);
                MyQuery.Waypoints = MyCoordinates;
                MyQuery.QueryCompleted += MyQuery_QueryCompleted;
                MyQuery.QueryAsync();
                Mygeocodequery.Dispose();


                // Make my current location the center of the Map.
                this.MyMap.Center = MyCoordinates[0];
                this.MyMap.ZoomLevel = 15;

                          /*
                            myCircle.Fill = new SolidColorBrush(Colors.Black);
                            myCircle.Height = 10;
                            myCircle.Width = 10;
                            myCircle.Opacity = 30;

                            // Create a MapOverlay to contain the circle.
                            myLocationOverlay.Content = myCircle;
                            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
                            myLocationOverlay.GeoCoordinate = MyCoordinates[0];
                            myLocationOverlay.GeoCoordinate = MyCoordinates[1];

                            // Create a MapLayer to contain the MapOverlay.
                            myLocationLayer.Add(myLocationOverlay);

                            // Add the MapLayer to the Map.
                            MyMap.Layers.Add(myLocationLayer);
                          */
            }
        }
        private void MyQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (e.Error == null)
            {
                Route MyRoute = e.Result;
                MapRoute MyMapRoute = new MapRoute(MyRoute);
                MyMap.AddRoute(MyMapRoute);
                MyQuery.Dispose();
            }
        }

        private void MapLocationButton_Click(object sender, RoutedEventArgs e)
        {
            GetCoordinates();
        }
    }
}