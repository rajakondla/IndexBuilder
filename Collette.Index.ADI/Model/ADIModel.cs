using Collette.Index;
using System;
using System.Collections.Generic;
using System.Text;

namespace Collette.Index
{
    public class ADIModel:IIndexModel
    {
        //Included all the properties in the ShopTourDataWebIndexWithPrice_ColleteUS API return JSON
        public long PackageId { get; set; }
        public long PackageDateId { get; set; }
        public long TourId { get; set; }
        public DateTime DepartureDate { get; set; }
        public int AllotmentAvailable { get; set; }
        public int SaleStatusId { get; set; }
        public int SinglePrice { get; set; }
        public int DoublePrice { get; set; }
        public int PriceInterAir { get; set; }
        public int PriceAir { get; set; }
        public int PointsOverrideSingle { get; set; }
        public int PointsOverrideDouble { get; set; }
        public int PriceAdditionalSupplements { get; set; }
        public int PriceGround { get; set; }
        public int SalesCurrencyId { get; set; }


    }
}
