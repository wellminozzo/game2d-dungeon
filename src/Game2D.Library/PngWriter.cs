using System.IO.Compression;

namespace Game2D.Library;

public static class PngWriter
{
    public static void Write(string path, int width, int height, byte[] rgbaData)
    {
        using var stream = new FileStream(path, FileMode.Create);
        using var writer = new BinaryWriter(stream);

        writer.Write(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A });

        WriteChunk(writer, "IHDR", CreateIhdrData(width, height));
        WriteChunk(writer, "IDAT", CreateIdatData(width, height, rgbaData));
        WriteChunk(writer, "IEND", []);
    }

    private static byte[] CreateIhdrData(int width, int height)
    {
        var data = new byte[13];
        WriteBE(data, 0, width);
        WriteBE(data, 4, height);
        data[8] = 8;
        data[9] = 6;
        data[10] = 0;
        data[11] = 0;
        data[12] = 0;
        return data;
    }

    private static byte[] CreateIdatData(int width, int height, byte[] rgba)
    {
        var raw = new byte[height * (1 + width * 4)];
        for (var y = 0; y < height; y++)
        {
            raw[y * (1 + width * 4)] = 0;
            Buffer.BlockCopy(rgba, y * width * 4, raw, y * (1 + width * 4) + 1, width * 4);
        }

        using var compressed = new MemoryStream();
        compressed.WriteByte(0x78);
        compressed.WriteByte(0x01);

        var crc = Adler32(raw);

        using (var deflate = new DeflateStream(compressed, CompressionLevel.Optimal, true))
        {
            deflate.Write(raw, 0, raw.Length);
        }

        WriteBE(compressed, crc);

        return compressed.ToArray();
    }

    private static void WriteChunk(BinaryWriter writer, string type, byte[] data)
    {
        var length = data.Length;
        var crcInput = new byte[4 + data.Length];
        System.Text.Encoding.ASCII.GetBytes(type).CopyTo(crcInput, 0);
        data.CopyTo(crcInput, 4);

        writer.WriteBE(length);
        writer.Write(System.Text.Encoding.ASCII.GetBytes(type));
        writer.Write(data);
        writer.WriteBE(Crc32(crcInput));
    }

    private static uint Crc32(byte[] data)
    {
        uint crc = 0xFFFFFFFF;
        foreach (var b in data)
        {
            crc ^= b;
            for (var i = 0; i < 8; i++)
                crc = (crc >> 1) ^ ((crc & 1) != 0 ? 0xEDB88320 : 0);
        }
        return crc ^ 0xFFFFFFFF;
    }

    private static uint Adler32(byte[] data)
    {
        uint a = 1, b = 0;
        foreach (var byteVal in data)
        {
            a = (a + byteVal) % 65521;
            b = (b + a) % 65521;
        }
        return (b << 16) | a;
    }

    private static void WriteBE(this BinaryWriter w, int value)
    {
        var bytes = BitConverter.GetBytes(value);
        if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
        w.Write(bytes);
    }

    private static void WriteBE(this BinaryWriter w, uint value)
    {
        w.WriteBE((int)value);
    }

    private static void WriteBE(byte[] buffer, int offset, int value)
    {
        buffer[offset] = (byte)((value >> 24) & 0xFF);
        buffer[offset + 1] = (byte)((value >> 16) & 0xFF);
        buffer[offset + 2] = (byte)((value >> 8) & 0xFF);
        buffer[offset + 3] = (byte)(value & 0xFF);
    }

    private static void WriteBE(MemoryStream stream, uint value)
    {
        stream.WriteByte((byte)((value >> 24) & 0xFF));
        stream.WriteByte((byte)((value >> 16) & 0xFF));
        stream.WriteByte((byte)((value >> 8) & 0xFF));
        stream.WriteByte((byte)(value & 0xFF));
    }

    public static byte[] CreatePlayerPixels()
    {
        var w = 48;
        var h = 24;
        var data = new byte[w * h * 4];

        void SetPx(int x, int y, byte r, byte g, byte b, byte a = 255)
        {
            if (x < 0 || x >= w || y < 0 || y >= h) return;
            var i = (y * w + x) * 4;
            data[i] = r; data[i + 1] = g; data[i + 2] = b; data[i + 3] = a;
        }

        void DrawRect(int rx, int ry, int rw, int rh, byte r, byte g, byte b, byte a = 255)
        {
            for (var x = rx; x < rx + rw; x++)
            for (var y = ry; y < ry + rh; y++)
                SetPx(x, y, r, g, b, a);
        }

        for (var frame = 0; frame < 2; frame++)
        {
            var ox = frame * 24;

            DrawRect(ox + 9, 1, 6, 6, 220, 180, 140);
            DrawRect(ox + 10, 2, 2, 2, 30, 30, 30);
            DrawRect(ox + 14, 2, 1, 1, 30, 30, 30);
            DrawRect(ox + 8, 0, 8, 2, 60, 40, 20);
            DrawRect(ox + 9, 7, 6, 7, 60, 140, 80);
            DrawRect(ox + 8, 8, 2, 4, 220, 180, 140);
            DrawRect(ox + 14, 8, 2, 4, 220, 180, 140);

            if (frame == 0)
            {
                DrawRect(ox + 9, 14, 3, 5, 50, 70, 120);
                DrawRect(ox + 12, 14, 3, 5, 50, 70, 120);
                DrawRect(ox + 9, 18, 3, 1, 40, 30, 20);
                DrawRect(ox + 12, 18, 3, 1, 40, 30, 20);
            }
            else
            {
                DrawRect(ox + 8, 14, 3, 5, 50, 70, 120);
                DrawRect(ox + 13, 14, 3, 5, 50, 70, 120);
                DrawRect(ox + 8, 18, 3, 1, 40, 30, 20);
                DrawRect(ox + 13, 18, 3, 1, 40, 30, 20);
            }
        }

        return data;
    }

    public static byte[] CreateEnemyPixels()
    {
        var w = 44;
        var h = 22;
        var data = new byte[w * h * 4];

        void SetPx(int x, int y, byte r, byte g, byte b, byte a = 255)
        {
            if (x < 0 || x >= w || y < 0 || y >= h) return;
            var i = (y * w + x) * 4;
            data[i] = r; data[i + 1] = g; data[i + 2] = b; data[i + 3] = a;
        }

        void DrawRect(int rx, int ry, int rw, int rh, byte r, byte g, byte b, byte a = 255)
        {
            for (var x = rx; x < rx + rw; x++)
            for (var y = ry; y < ry + rh; y++)
                SetPx(x, y, r, g, b, a);
        }

        for (var frame = 0; frame < 2; frame++)
        {
            var ox = frame * 22;

            DrawRect(ox + 3, 2, 16, 14, 160, 60, 40);
            DrawRect(ox + 4, 0, 14, 3, 160, 60, 40);
            DrawRect(ox + 5, 16, 12, 4, 160, 60, 40);
            DrawRect(ox + 6, 3, 3, 3, 240, 220, 80);
            DrawRect(ox + 13, 3, 3, 3, 240, 220, 80);
            DrawRect(ox + 7, 4, 1, 1, 20, 20, 20);
            DrawRect(ox + 14, 4, 1, 1, 20, 20, 20);

            if (frame == 0)
            {
                DrawRect(ox + 4, 8, 2, 4, 160, 60, 40);
                DrawRect(ox + 16, 8, 2, 4, 160, 60, 40);
            }
            else
            {
                DrawRect(ox + 2, 8, 3, 3, 160, 60, 40);
                DrawRect(ox + 17, 8, 3, 3, 160, 60, 40);
            }
        }

        return data;
    }

    public static byte[] CreateTilesetPixels()
    {
        var ts = 32;
        var cols = 3;
        var w = ts * cols;
        var h = ts;
        var data = new byte[w * h * 4];

        void SetPx(int x, int y, byte r, byte g, byte b, byte a = 255)
        {
            if (x < 0 || x >= w || y < 0 || y >= h) return;
            var i = (y * w + x) * 4;
            data[i] = r; data[i + 1] = g; data[i + 2] = b; data[i + 3] = a;
        }

        void DrawRect(int rx, int ry, int rw, int rh, byte r, byte g, byte b, byte a = 255)
        {
            for (var x = rx; x < rx + rw; x++)
            for (var y = ry; y < ry + rh; y++)
                SetPx(x, y, r, g, b, a);
        }

        void DrawChecker(int ox, int oy, int size, byte r1, byte g1, byte b1, byte r2, byte g2, byte b2)
        {
            for (var x = 0; x < size; x++)
            for (var y = 0; y < size; y++)
            {
                if ((x + y) % 2 == 0)
                    SetPx(ox + x, oy + y, r1, g1, b1);
                else
                    SetPx(ox + x, oy + y, r2, g2, b2);
            }
        }

        DrawRect(0, 0, ts, ts, 0, 0, 0, 0);

        DrawChecker(ts, 0, ts, 50, 50, 55, 45, 45, 50);

        var br = (byte)90; var bg = (byte)80; var bb = (byte)70;
        var dr = (byte)75; var dg = (byte)65; var db = (byte)55;

        DrawRect(ts * 2, 0, ts, ts, br, bg, bb);
        DrawRect(ts * 2 + 1, 0, ts - 2, 1, dr, dg, db);
        DrawRect(ts * 2 + 1, ts - 1, ts - 2, 1, dr, dg, db);
        DrawRect(ts * 2, 1, 1, ts - 2, dr, dg, db);
        DrawRect(ts * 2 + ts - 1, 1, 1, ts - 2, dr, dg, db);
        DrawRect(ts * 2 + 4, 4, 4, 4, dr, dg, db);
        DrawRect(ts * 2 + 14, 8, 4, 4, dr, dg, db);
        DrawRect(ts * 2 + 8, 18, 4, 4, dr, dg, db);
        DrawRect(ts * 2 + 20, 22, 4, 4, dr, dg, db);
        DrawRect(ts * 2 + 4, 26, 4, 4, dr, dg, db);
        DrawRect(ts * 2 + 16, 14, 4, 4, dr, dg, db);

        return data;
    }
}
