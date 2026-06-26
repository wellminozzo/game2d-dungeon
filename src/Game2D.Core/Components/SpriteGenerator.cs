using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2D.Core;

public static class SpriteGenerator
{
    public static Texture2D CreatePlayerTexture(GraphicsDevice device)
    {
        var w = 48;
        var h = 24;
        var data = new Color[w * h];

        void SetPixel(int x, int y, Color c)
        {
            if (x >= 0 && x < w && y >= 0 && y < h)
                data[y * w + x] = c;
        }

        void DrawRect(int rx, int ry, int rw, int rh, Color c)
        {
            for (var x = rx; x < rx + rw; x++)
            for (var y = ry; y < ry + rh; y++)
                SetPixel(x, y, c);
        }

        var skin = new Color(220, 180, 140);
        var shirt = new Color(60, 140, 80);
        var pants = new Color(50, 70, 120);
        var hair = new Color(60, 40, 20);
        var eye = new Color(30, 30, 30);
        var shoe = new Color(40, 30, 20);

        for (var frame = 0; frame < 2; frame++)
        {
            var ox = frame * 24;

            DrawRect(ox + 9, 1, 6, 6, skin);      // head
            DrawRect(ox + 10, 2, 2, 2, eye);       // eyes
            DrawRect(ox + 14, 2, 1, 1, eye);
            DrawRect(ox + 8, 0, 8, 2, hair);       // hair
            DrawRect(ox + 9, 7, 6, 7, shirt);       // body
            DrawRect(ox + 8, 8, 2, 4, skin);        // left arm
            DrawRect(ox + 14, 8, 2, 4, skin);       // right arm

            if (frame == 0)
            {
                DrawRect(ox + 9, 14, 3, 5, pants);   // left leg
                DrawRect(ox + 12, 14, 3, 5, pants);  // right leg
                DrawRect(ox + 9, 18, 3, 1, shoe);
                DrawRect(ox + 12, 18, 3, 1, shoe);
            }
            else
            {
                DrawRect(ox + 8, 14, 3, 5, pants);   // left leg forward
                DrawRect(ox + 13, 14, 3, 5, pants);  // right leg back
                DrawRect(ox + 8, 18, 3, 1, shoe);
                DrawRect(ox + 13, 18, 3, 1, shoe);
            }
        }

        var tex = new Texture2D(device, w, h);
        tex.SetData(data);
        return tex;
    }

    public static Texture2D CreateEnemyTexture(GraphicsDevice device)
    {
        var w = 44;
        var h = 22;
        var data = new Color[w * h];

        void SetPixel(int x, int y, Color c)
        {
            if (x >= 0 && x < w && y >= 0 && y < h)
                data[y * w + x] = c;
        }

        void DrawRect(int rx, int ry, int rw, int rh, Color c)
        {
            for (var x = rx; x < rx + rw; x++)
            for (var y = ry; y < ry + rh; y++)
                SetPixel(x, y, c);
        }

        var body = new Color(160, 60, 40);
        var eye = new Color(240, 220, 80);
        var pupil = new Color(20, 20, 20);

        for (var frame = 0; frame < 2; frame++)
        {
            var ox = frame * 22;

            DrawRect(ox + 3, 2, 16, 14, body);       // body
            DrawRect(ox + 4, 0, 14, 3, body);        // top curve
            DrawRect(ox + 5, 16, 12, 4, body);       // bottom
            DrawRect(ox + 6, 3, 3, 3, eye);          // left eye
            DrawRect(ox + 13, 3, 3, 3, eye);         // right eye
            DrawRect(ox + 7, 4, 1, 1, pupil);        // left pupil
            DrawRect(ox + 14, 4, 1, 1, pupil);       // right pupil

            if (frame == 0)
            {
                DrawRect(ox + 4, 8, 2, 4, body);      // left arm
                DrawRect(ox + 16, 8, 2, 4, body);     // right arm
            }
            else
            {
                DrawRect(ox + 2, 8, 3, 3, body);      // left arm forward
                DrawRect(ox + 17, 8, 3, 3, body);     // right arm forward
            }
        }

        var tex = new Texture2D(device, w, h);
        tex.SetData(data);
        return tex;
    }

    public static Texture2D CreateTileset(GraphicsDevice device)
    {
        var ts = 32;
        var cols = 3;
        var w = ts * cols;
        var h = ts;
        var data = new Color[w * h];

        void SetPixel(int x, int y, Color c)
        {
            if (x >= 0 && x < w && y >= 0 && y < h)
                data[y * w + x] = c;
        }

        void DrawRect(int rx, int ry, int rw, int rh, Color c)
        {
            for (var x = rx; x < rx + rw; x++)
            for (var y = ry; y < ry + rh; y++)
                SetPixel(x, y, c);
        }

        void DrawChecker(int ox, int oy, int size, Color a, Color b)
        {
            for (var x = 0; x < size; x++)
            for (var y = 0; y < size; y++)
                SetPixel(ox + x, oy + y, (x + y) % 2 == 0 ? a : b);
        }

        var voidC = Color.Transparent;
        var floorA = new Color(50, 50, 55);
        var floorB = new Color(45, 45, 50);
        var wallA = new Color(90, 80, 70);
        var wallB = new Color(75, 65, 55);

        DrawRect(0, 0, ts, ts, voidC);

        DrawChecker(ts, 0, ts, floorA, floorB);

        DrawRect(ts * 2, 0, ts, ts, wallA);
        DrawRect(ts * 2 + 1, 0, ts - 2, 1, wallB);
        DrawRect(ts * 2 + 1, ts - 1, ts - 2, 1, wallB);
        DrawRect(ts * 2, 1, 1, ts - 2, wallB);
        DrawRect(ts * 2 + ts - 1, 1, 1, ts - 2, wallB);
        DrawRect(ts * 2 + 4, 4, 4, 4, wallB);
        DrawRect(ts * 2 + 14, 8, 4, 4, wallB);
        DrawRect(ts * 2 + 8, 18, 4, 4, wallB);
        DrawRect(ts * 2 + 20, 22, 4, 4, wallB);
        DrawRect(ts * 2 + 4, 26, 4, 4, wallB);
        DrawRect(ts * 2 + 16, 14, 4, 4, wallB);

        var tex = new Texture2D(device, w, h);
        tex.SetData(data);
        return tex;
    }
}
