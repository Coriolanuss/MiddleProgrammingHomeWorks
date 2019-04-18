using System;
using System.Collections.Generic;

namespace HomeWork3
{
    public class RoomInfo : IComparable<RoomInfo>
    {
        public readonly int id;
        public readonly string name;
        public readonly int? zipcode;
        public readonly string smart_location;
        public readonly string country;
        public readonly float latitude;
        public readonly float longitude;

        public RoomInfo(int id, string name, int? zipcode, string smart_location, string country, float latitude, float longitude)
        {
            this.id = id;
            this.name = name;
            this.zipcode = zipcode;
            this.smart_location = smart_location;
            this.country = country;
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public static RoomInfo FromCsvRow(string[] csvFields)
        {
            return new RoomInfo(
                id: GetId(idOrUrl: csvFields[0]),
                name: csvFields[1],
                zipcode: GetZipcode(csvFields[2]),
                smart_location: csvFields[3],
                country: csvFields[4],
                latitude: Convert.ToSingle(csvFields[5]),
                longitude: Convert.ToSingle(csvFields[6]));
        }

        public static IEnumerable<RoomInfo> ReadFromCsv(string filePath)
        {
            var parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(filePath)
            {
                TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited,
                Delimiters = new[] { "," },
                HasFieldsEnclosedInQuotes = true,
                CommentTokens = new[] { "id", "listing_url" },
            };

            while (!parser.EndOfData)
            {
                string[] row = parser.ReadFields();
                RoomInfo parsedRoomInfo;
                try
                {
                    parsedRoomInfo = FromCsvRow(row);
                }
                catch (FormatException)
                {
                    // Source data contains some corrupt rows, so just skip it.
                    //Console.WriteLine($"Corrupt row: {string.Join(",", row)}");
                    continue;
                }
                yield return parsedRoomInfo;
            }
        }

        private static int GetId(string idOrUrl) => Convert.ToInt32(idOrUrl.Substring(idOrUrl.LastIndexOf('/') + 1));

        private static int? GetZipcode(string zipOrEmpty) => string.IsNullOrEmpty(zipOrEmpty) ? (int?)null : Convert.ToInt32(zipOrEmpty);

        public int CompareTo(RoomInfo other) => id.CompareTo(other.id);
    }

    public class RoomInfoEqualityComparer : IEqualityComparer<RoomInfo>
    {
        public bool Equals(RoomInfo x, RoomInfo y) => x.id == y.id;

        public int GetHashCode(RoomInfo obj) => obj.id;
    }
}
