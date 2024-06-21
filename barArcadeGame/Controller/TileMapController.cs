using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TiledSharp;

namespace barArcadeGame.Managers
{
    public class TileMapController
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly TmxMap _map;
        private readonly Texture2D _tileset;
        private readonly int _tilesetTilesWide;
        private readonly int _tileWidth;
        private readonly int _tileHeight;

        public TileMapController(SpriteBatch spriteBatch, TmxMap map, Texture2D tileset, int tilesetTilesWide, int tileWidth, int tileHeight)
        {
            _spriteBatch = spriteBatch;
            _map = map;
            _tileset = tileset;
            _tilesetTilesWide = tilesetTilesWide;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
        }

        public void Draw(Matrix transformMatrix)
        {
            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                samplerState: SamplerState.PointClamp,
                transformMatrix: transformMatrix);

            foreach (var layer in _map.TileLayers)
            {
                for (var i = 0; i < layer.Tiles.Count; i++)
                {
                    int gid = layer.Tiles[i].Gid;
                    if (gid == 0) continue;

                    int tileFrame = gid - 1;
                    int column = tileFrame % _tilesetTilesWide;
                    int row = tileFrame / _tilesetTilesWide;
                    float x = (i % _map.Width) * _map.TileWidth;
                    float y = (i / _map.Width) * _map.TileHeight;

                    Rectangle tilesetRec = new Rectangle(_tileWidth * column, _tileHeight * row, _tileWidth, _tileHeight);
                    _spriteBatch.Draw(_tileset, new Rectangle((int)x, (int)y, _tileWidth, _tileHeight), tilesetRec, Color.White);
                }
            }

            _spriteBatch.End();
        }
    }
}