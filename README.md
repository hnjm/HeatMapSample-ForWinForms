# Heat Map Sample for WinForms

### Description

Heat maps is a technique increasingly used in various fields such in biology and other fields. See http://en.wikipedia.org/wiki/Heat_map. They are also used for displaying areas of webs page most frequently scanned by users. http://csscreme.com/heat-maps/.

At ThinkGeo, we are taking this concept to GIS and applying it to geographic maps. Heat maps are a great way to give the users a visually compelling representation of the distribution and intensity of geographic phenomenon. 

Please refer to [Wiki](http://wiki.thinkgeo.com/wiki/map_suite_desktop_for_winforms) for the details.

![Screenshot](https://github.com/ThinkGeo/HeatMapSample-ForWinForms/blob/master/ScreenShot.png)

### Requirements
This sample makes use of the following NuGet Packages

[MapSuite 10.0.0](https://www.nuget.org/packages?q=ThinkGeo)

### About the Code
```csharp
protected override void DrawCore(IEnumerable<Feature> features, GeoCanvas canvas, System.Collections.ObjectModel.Collection<SimpleCandidate> labelsInThisLayer, System.Collections.ObjectModel.Collection<SimpleCandidate> labelsInAllLayers)
{
    Bitmap intensityBitmap = null;
    Bitmap colorBitmap = null;
    GeoImage geoImage = null;
    MemoryStream pngStream = null;

    try
    {
        if (colorPalette.Count != 256)
        {
            colorPalette = GetDefaultColorPalette();
        }

        intensityBitmap = new Bitmap(Convert.ToInt32(canvas.Width), Convert.ToInt32(canvas.Height));
        Graphics Surface = Graphics.FromImage(intensityBitmap);
        Surface.Clear(Color.Transparent);
        Surface.Dispose();

        List<HeatPoint> heatPoints = new List<HeatPoint>();

        foreach (Feature feature in features)
        {
            if (feature.GetWellKnownType() == WellKnownType.Point)
            {
                PointShape pointShape = (PointShape)feature.GetShape();
                ScreenPointF screenPoint = ExtentHelper.ToScreenCoordinate(canvas.CurrentWorldExtent, pointShape, canvas.Width, canvas.Height);

                double realValue;
                if (intensityRangeStart != 0 && intensityRangeEnd != 0 && intensityRangeStart != intensityRangeEnd && intensityColumnName != string.Empty)
                {


                    if (intensityRangeStart < intensityRangeEnd)
                    {
                        realValue = (255 / (intensityRangeEnd - intensityRangeStart)) * (GetDoubleValue(feature.ColumnValues[intensityColumnName], intensityRangeStart, intensityRangeEnd) - intensityRangeStart);
                    }
                    else
                    {
                        realValue = (255 / (intensityRangeEnd - intensityRangeStart)) * (intensityRangeEnd - GetDoubleValue(feature.ColumnValues[intensityColumnName], intensityRangeStart, intensityRangeEnd));
                        
                    }
                }
                else
                {
                    realValue = intensity;
                }

                HeatPoint heatPoint = new HeatPoint(Convert.ToInt32(screenPoint.X), Convert.ToInt32(screenPoint.Y), Convert.ToByte(realValue));
                heatPoints.Add(heatPoint);
            }
        }

        float size = GetPointSize(canvas);
        
        intensityBitmap = CreateIntensityMask(intensityBitmap, heatPoints, Convert.ToInt32(size));

        colorBitmap = Colorize(intensityBitmap, (byte)alpha, colorPalette);

        pngStream = new MemoryStream();
        colorBitmap.Save(pngStream, System.Drawing.Imaging.ImageFormat.Png);
        geoImage = new GeoImage(pngStream);

        canvas.DrawWorldImageWithoutScaling(geoImage, canvas.CurrentWorldExtent.GetCenterPoint().X, canvas.CurrentWorldExtent.GetCenterPoint().Y, DrawingLevel.LevelOne);
    }
    finally
    {
        if (intensityBitmap != null) { intensityBitmap.Dispose(); }
        if (colorBitmap != null) { colorBitmap.Dispose(); }
        if (geoImage != null) { geoImage.Dispose(); }
        if (pngStream != null) { pngStream.Dispose(); }
    }
}
```
### Getting Help

[Map Suite Desktop for Winforms Wiki Resources](http://wiki.thinkgeo.com/wiki/map_suite_desktop_for_winforms)

[Map Suite Desktop for Winforms Product Description](https://thinkgeo.com/ui-controls#desktop-platforms)

[ThinkGeo Community Site](http://community.thinkgeo.com/)

[ThinkGeo Web Site](http://www.thinkgeo.com)

### Key APIs
This example makes use of the following APIs:

- [ThinkGeo.MapSuite.Drawing.GeoCanvas](http://wiki.thinkgeo.com/wiki/api/thinkgeo.mapsuite.drawing.geocanvas)
- [ThinkGeo.MapSuite.Drawing.GeoImage](http://wiki.thinkgeo.com/wiki/api/thinkgeo.mapsuite.drawing.geoimage)
- [ThinkGeo.MapSuite.Shapes.PointShape](http://wiki.thinkgeo.com/wiki/api/thinkgeo.mapsuite.shapes.pointshape)
- [ThinkGeo.MapSuite.Shapes.Feature](http://wiki.thinkgeo.com/wiki/api/thinkgeo.mapsuite.shapes.feature)

### About Map Suite
Map Suite is a set of powerful development components and services for the .Net Framework.

### About ThinkGeo
ThinkGeo is a GIS (Geographic Information Systems) company founded in 2004 and located in Frisco, TX. Our clients are in more than 40 industries including agriculture, energy, transportation, government, engineering, software development, and defense.
