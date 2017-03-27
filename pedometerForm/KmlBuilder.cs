using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace pedometerForm
{
    class KmlBuilder
    {
        double longitude;
        double latitude;
        public KmlBuilder(double longitude, double latitude)
        {
            this.longitude = longitude;
            this.latitude = latitude;
        }
        public KmlBuilder()
        {

        }

        public void KMLstreetview(string longitude, string latitude)
        {
            string XMLName = "streetview.kml";
            XmlTextWriter kml = new XmlTextWriter(XMLName, Encoding.UTF8);

            kml.Formatting = Formatting.Indented;
            kml.Indentation = 3;

            kml.WriteStartDocument();

            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteAttributeString("xmlns:gx", "http://www.google.com/kml/ext/2.2");
            kml.WriteAttributeString("xmlns:kml", "http://www.opengis.net/kml/2.2");
            kml.WriteAttributeString("xmlns:atom", "http://www.w3.org/2005/Atom");

            kml.WriteStartElement("gx:Tour");
            kml.WriteElementString("name", "Your Location");
            kml.WriteStartElement("gx:Playlist");
            kml.WriteStartElement("gx:FlyTo");


            kml.WriteElementString("gx:duration", "0.4102521409843121");
            kml.WriteElementString("gx:flyToMode", "smooth");
            kml.WriteStartElement("Camera");
            kml.WriteStartElement("gx:ViewerOptions");
            kml.WriteStartElement("gx:option");
            kml.WriteAttributeString("name", "streetview");
            kml.WriteEndElement();

            kml.WriteEndElement();
            kml.WriteElementString("longitude", longitude);
            kml.WriteElementString("latitude", latitude);
            kml.WriteElementString("tilt", "91.03494795962899");
            kml.WriteElementString("altitudeMode", "relativeToGround");
            kml.WriteElementString("gx:altitudeMode", "relativeToSeaFloor");

            kml.WriteEndElement();
            kml.WriteEndElement();


            kml.WriteEndElement(); // <Point>
            kml.WriteEndElement(); // <Placemark>
            kml.WriteEndDocument(); // <kml>

            kml.Close();

            System.Diagnostics.Process.Start("streetview.kml");



        }

        public void KML(string longitude, string latitude)
        {
            string XMLName = "location.kml";
            XmlTextWriter kml = new XmlTextWriter(XMLName, Encoding.UTF8);

            kml.Formatting = Formatting.Indented;
            kml.Indentation = 3;

            kml.WriteStartDocument();

            kml.WriteStartElement("kml", "http://www.opengis.net/kml/2.2");
            kml.WriteStartElement("Placemark");
            kml.WriteElementString("name", "Your Location");
            kml.WriteStartElement("Point");

            kml.WriteElementString("coordinates", longitude + "," + latitude);

            kml.WriteEndElement(); // <Point>
            kml.WriteEndElement(); // <Placemark>
            kml.WriteEndDocument(); // <kml>

            kml.Close();

            System.Diagnostics.Process.Start("location.kml");

        }

    }
}
