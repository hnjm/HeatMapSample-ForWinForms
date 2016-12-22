using System;
using System.Windows.Forms;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;
using ThinkGeo.MapSuite.WinForms;


namespace  HeatMap
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            // Set the full extent and the background color
            winformsMap1.CurrentExtent = new RectangleShape(-150,64,-90,16); 
            winformsMap1.BackgroundOverlay.BackgroundBrush = new GeoSolidBrush(GeoColor.FromArgb(255, 198, 255, 255));

            //WorldMapKit
            WorldMapKitWmsDesktopOverlay worldMapKitDesktopOverlay = new WorldMapKitWmsDesktopOverlay();
            winformsMap1.Overlays.Add(worldMapKitDesktopOverlay);

            ShapeFileFeatureSource featureSource = new ShapeFileFeatureSource("../../data/swineflu.shp"); 

            HeatLayer heatLayer = new HeatLayer(featureSource);
            //Creates the HeatStyle to set the properties determining the display of the heat map with earth quake data.
            //Notice that each point is treated with an intensity depending on the value of the column "other_magn1".
            //So, in addition to the density of points location, the value for each point is also going to be counted into account
            //for the coloring of the map.
            HeatStyle heatStyle = new HeatStyle();
            heatStyle.Alpha = 255;
            heatStyle.PointRadius = 100; 
            heatStyle.PointRadiusUnit = DistanceUnit.Kilometer;
            heatStyle.Alpha = 180;
            heatStyle.PointIntensity = 10; 
           
            heatStyle.IntensityColumnName = "CONFIRMED";
            heatStyle.IntensityRangeStart = 0;
            heatStyle.IntensityRangeEnd = 638;

            heatLayer.HeatStyle = heatStyle;

            LayerOverlay heatMapOverlay = new LayerOverlay();
            heatMapOverlay.Layers.Add(heatLayer);

            winformsMap1.Overlays.Add("HeatOverlay", heatMapOverlay);
            

            winformsMap1.Refresh();
        }

        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    
    }
}
