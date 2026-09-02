namespace MyHub.Utils
{
    public static class FileUtils
    {
        const long BYTES_PER_KIBIBYTE = 1024;
        public static long ByteToMegaByte(long fileSizeByte)
        {
            return fileSizeByte / (long)Math.Pow(BYTES_PER_KIBIBYTE, 2);
        }

        public static long MegaByteToByte(long fileSizeMegabyte)
        {
            return fileSizeMegabyte * (long)Math.Pow(BYTES_PER_KIBIBYTE, 2);
        }

        public static long ByteToGigaByte(long fileSizeByte)
        {
            return fileSizeByte / (long)Math.Pow(BYTES_PER_KIBIBYTE, 3);
        }

        public static long GigaByteToByte(long fileSizeGigabyte)
        {
            return fileSizeGigabyte * (long)Math.Pow(BYTES_PER_KIBIBYTE, 3);
        }
    }
}
