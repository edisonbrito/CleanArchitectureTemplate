using FluentMigrator;
using System;

namespace Praxio.Tools.Arquitetura.Infra.Migrations.Base
{
    public class MigrationBaseAttribute : MigrationAttribute
    {
        public new long Version { get; }

        public MigrationBaseAttribute(int majorVersion, int minorVersion, int revision, string author)
            : base(CalculateValue(majorVersion, minorVersion, revision))
        {
            Author = author;
            Version = CalculateValue(majorVersion, minorVersion, revision);
        }

        public MigrationBaseAttribute(int versionNumber, string author)
            : base(versionNumber)
        {
            Author = author;
            Version = versionNumber;
        }

        public string Author { get; set; }

        private static long CalculateValue(int majorVersion, int minorVersion, int revision)
        {
            var binRev = Convert.ToString(revision, 2).PadLeft(21, '0');
            var binMinor = Convert.ToString(minorVersion, 2).PadLeft(21, '0');
            var binMaj = Convert.ToString(majorVersion, 2).PadLeft(21, '0');

            var binVersion = binMaj + binMinor + binRev;

            return Convert.ToInt64(binVersion, 2);
        }
    }
}
