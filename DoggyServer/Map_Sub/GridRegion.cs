using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMetaverse;

namespace DoggyServer.Map_Sub
{
    class GridRegion
    {
        private static GridClient client;
        public static void init(GridClient client_new)
        {
            client = client_new;

            client.Grid.GridRegion += new EventHandler<GridRegionEventArgs>(Grid_GridRegion);
            client.Grid.GridItems += new EventHandler<GridItemsEventArgs>(Grid_GridItems);
            client.Grid.GridLayer += new EventHandler<GridLayerEventArgs>(Grid_GridLayer);
        }

        static void Grid_GridLayer(object sender, GridLayerEventArgs e)
        {
            
        }

        static void Grid_GridItems(object sender, GridItemsEventArgs e)
        {
            List<MapItem> items = e.Items;

            foreach (MapItem item in items)
            {
                
            }
        }

        static void Grid_GridRegion(object sender, GridRegionEventArgs e)
        {
            
        }
    }
}
