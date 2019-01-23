using System;
using System.Collections.Generic;
using System.Text;

namespace Virgo.IP.Models
{
    public class DataBlock
    {
        #region Private Properties
        public int CityID
        {
            get;
            private set;
        }

        public string Region
        {
            get;
            private set;
        }

        public int DataPtr
        {
            get;
            private set;
        }
        #endregion

        #region Constructor
        public DataBlock(int city_id, string region, int dataPtr = 0)
        {
            CityID = city_id;
            Region = region;
            DataPtr = dataPtr;
        }

        public DataBlock(int city_id, string region) : this(city_id, region, 0)
        {
        }
        #endregion

        public override string ToString()
        {
            return $"{CityID}|{Region}|{DataPtr}";
        }

    }
}
