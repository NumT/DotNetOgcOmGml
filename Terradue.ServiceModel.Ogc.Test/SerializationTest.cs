﻿using NUnit.Framework;
using System;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Terradue.ServiceModel.Ogc.Test {
    
    [TestFixture()]
    public class SerializationTest {


        [Test()]
        public void TestCase() {

            XmlSerializer gmlTimePeriodSerializer = new XmlSerializer(typeof(Terradue.ServiceModel.Ogc.Gml321.TimePeriodType));
            FileStream fs = new FileStream("../Samples/GmlTimePeriod.xml", FileMode.Open);
            Terradue.ServiceModel.Ogc.Gml321.TimePeriodType tp = (Terradue.ServiceModel.Ogc.Gml321.TimePeriodType)gmlTimePeriodSerializer.Deserialize(fs);

            Assert.That(tp.Item is Terradue.ServiceModel.Ogc.Gml321.TimeInstantPropertyType);
            Assert.That(tp.Item1 is Terradue.ServiceModel.Ogc.Gml321.TimeInstantPropertyType);

        }


        [Test()]
        public void SerializeFootprint() {

            XmlSerializer sarSerializer = OgcHelpers.Sar21Serializer;
            FileStream fs = new FileStream("../out/sarwithfootprint.xml", FileMode.Create, FileAccess.Write);

            Terradue.ServiceModel.Ogc.Sar21.SarEarthObservationType sarEO = new Terradue.ServiceModel.Ogc.Sar21.SarEarthObservationType();
            sarEO.featureOfInterest = new Terradue.ServiceModel.Ogc.Gml321.FeaturePropertyType();
            sarEO.featureOfInterest.Eop21Footprint = new Terradue.ServiceModel.Ogc.Eop21.FootprintType();
            sarEO.procedure = new Terradue.ServiceModel.Ogc.Om.OM_ProcessPropertyType();
            sarEO.procedure.Eop21EarthObservationEquipment = new Terradue.ServiceModel.Ogc.Eop21.EarthObservationEquipmentType();
            sarEO.procedure.Eop21EarthObservationEquipment.acquisitionParameters = new Terradue.ServiceModel.Ogc.Eop21.AcquisitionPropertyType();
            sarEO.procedure.Eop21EarthObservationEquipment.acquisitionParameters.Acquisition = new Terradue.ServiceModel.Ogc.Sar21.SarAcquisitionType();
            sarEO.procedure.Eop21EarthObservationEquipment.acquisitionParameters.Acquisition.orbitDirection = Terradue.ServiceModel.Ogc.Eop21.OrbitDirectionValueType.DESCENDING;
            sarEO.procedure.Eop21EarthObservationEquipment.acquisitionParameters.Acquisition.orbitDirectionSpecified = true;
            sarEO.procedure.Eop21EarthObservationEquipment.acquisitionParameters.SarAcquisition.polarisationChannels = "HH";

            var msfs = new FileStream("../Samples/Multisurface32.gml", FileMode.Open);

            XmlReader reader = XmlReader.Create(msfs);

            Terradue.ServiceModel.Ogc.Gml321.MultiSurfaceType gml = (Terradue.ServiceModel.Ogc.Gml321.MultiSurfaceType)Terradue.ServiceModel.Ogc.Gml321.GmlHelper.Deserialize(reader);

            msfs.Close();


            sarEO.featureOfInterest.Eop21Footprint.multiExtentOf = new Terradue.ServiceModel.Ogc.Gml321.MultiSurfacePropertyType();
            sarEO.featureOfInterest.Eop21Footprint.multiExtentOf.MultiSurface = gml;

            sarSerializer.Serialize(fs, sarEO);

        }

    }
}

