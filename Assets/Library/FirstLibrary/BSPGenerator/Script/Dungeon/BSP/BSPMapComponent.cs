using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG {
	public class BSPMapComponent {
		public Rect spaceRect;
		public int display_order;

        public float area
        {
            get
            {
                return spaceRect.width * spaceRect.height;
            }
        }

    }
}