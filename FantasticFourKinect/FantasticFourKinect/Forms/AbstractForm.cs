using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace FantasticFourKinect.Forms
{
    abstract class AbstractForm
    {
        protected Principal pM;

        public AbstractForm(Principal pM)
        {
            this.pM = pM;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void UnloadContent();
    }
}
