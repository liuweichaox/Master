using MaxMind.GeoIP2;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using Virgo.IP.Models;

namespace Virgo.IP.Searcher
{
    public class IpComplexSearcher : IIpSearcher
    {
        private readonly DatabaseReader _search;

        public IpComplexSearcher()
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ip", "data", "GeoLite2-City.mmdb");
            if (IpSettings.LoadInternationalDbToMemory)
            {
                MemoryMappedFile file = MemoryMappedFile.CreateFromFile(dbPath);
                _search = new DatabaseReader(file.CreateViewStream());
            }
            else
            {
                _search = new DatabaseReader(dbPath);
            }
        }

        public IpInfo Search(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                throw new ArgumentException(nameof(ip));
            }
            if (_search.TryCity(ip, out var city))
            {
                var ipinfo = new IpInfo
                {
                    IpAddress = ip,
                    CountryCode = city.Country.IsoCode,
                    Country = city.Country.Name,
                    Province = city.MostSpecificSubdivision.Name,
                    ProvinceCode = city.MostSpecificSubdivision.IsoCode,
                    City = city.City.Name,
                    PostCode = city.Postal.Code,
                    Latitude = city.Location.Latitude,
                    Longitude = city.Location.Longitude,
                    AccuracyRadius = city.Location.AccuracyRadius
                };
                return ipinfo;
            }
            else
            {
                return new IpInfo();
            }

        }

        public IpInfo SearchWithI18N(string ip, string langCode = "")
        {
            if (string.IsNullOrEmpty(ip))
            {
                throw new ArgumentException(nameof(ip));
            }

            if (string.IsNullOrEmpty(langCode))
            {
                langCode = IpSettings.DefaultLanguage;
            }

            if (_search.TryCity(ip, out var city))
            {
                var ipinfo = new IpInfo
                {
                    IpAddress = ip,
                    CountryCode = city.Country.IsoCode,
                    Country = city.Country.Names.ContainsKey(langCode) ? city.Country.Names[langCode] : city.Country.Name,
                    Province = city.MostSpecificSubdivision.Names.ContainsKey(langCode) ? city.MostSpecificSubdivision.Names[langCode] : city.MostSpecificSubdivision.Name,
                    ProvinceCode = city.MostSpecificSubdivision.IsoCode,
                    City = city.City.Names.ContainsKey(langCode) ? city.City.Names[langCode] : city.City.Name,
                    PostCode = city.Postal.Code,
                    Latitude = city.Location.Latitude,
                    Longitude = city.Location.Longitude,
                    AccuracyRadius = city.Location.AccuracyRadius
                };
                return ipinfo;
            }
            else
            {
                return new IpInfo();
            }
        }
    }
}
