using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WST.Domain
{
    public class HomeModel
    {
        public List<AreaDto> storeRegional { get; set; }

        public List<Typetem> storeType { get; set; }
        public List<StoreItem> storeList { get; set; }

    }

    public class StoreItem
    {
        public string storeId { get; set; }
        public string storeName { get; set; }
        public string storeAdress { get; set; }
        public string storeImgUrl { get; set; }
        public int storeCollect { get; set; }
        public string storeAreaId { get; set; }
        public string storeTypeId { get; set; }
    }

    public class AreaDto
    {
        public string storeCityValue { get; set; }

        public List<AreaItem> storeArea { get; set; }
    }

    public class AreaItem
    {
        public string storeAreaValue { get; set; }
        public string storeAreaId { get; set; }
        public int areaAll { get; set; }
    }
    public class Typetem
    {
        public string storeTypeValue { get; set; }
        public string storeTypeId { get; set; }
        public int typeAll { get; set; }
    }
}
