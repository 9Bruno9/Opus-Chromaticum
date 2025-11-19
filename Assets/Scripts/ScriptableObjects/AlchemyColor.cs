using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    public abstract class AlchemyColor : ScriptableObject
    {
        public enum ColorTypeEnum
        {
            Primary,Secondary,Complex
        }
        public Color itemColor;

        public string LatinName;

        public ColorType colorType;
    
        /// <summary>
        /// Hit this instance of InteractableAlchemyColor with another InteractableAlchemyColor
        /// </summary>
        /// <param name="otherColor">The Alchemy color of the one who hit</param>
        /// <returns>The multiplier to the damage dealt</returns>
        public virtual float HitThisWith(AlchemyColor otherColor)
        {
            throw new NotImplementedException();
        }
        public virtual float HitThisWith(PrimaryAlchemyColor otherColor)
        {
            return HitThisWith((AlchemyColor)otherColor);
        }
        public virtual float HitThisWith(SecondaryAlchemyColor otherColor)
        {
            return HitThisWith((AlchemyColor)otherColor);
        }
        public virtual float HitThisWith(ComplexAlchemyColor otherColor)
        {
            return HitThisWith((AlchemyColor)otherColor);
        }

        /// <summary>
        /// Hit the given InteractableAlchemyColor with this instance of InteractableAlchemyColor
        /// </summary>
        /// <param name="targetHit">The Alchemy color that is beign hit</param>
        /// <returns>The multiplier to the damage dealt</returns>
        public virtual float HasHit(AlchemyColor targetHit)
        {
            throw new NotImplementedException();
        }
        public virtual float HasHit(PrimaryAlchemyColor targetHit)
        {
            return HasHit((AlchemyColor)targetHit);
        }
        public virtual float HasHit(SecondaryAlchemyColor targetHit)
        {
            return HasHit((AlchemyColor)targetHit);
        }
        public virtual float HasHit(ComplexAlchemyColor targetHit)
        {
            return HasHit((AlchemyColor)targetHit);
        }

        /// <summary>
        /// Returns the components of this InteractableAlchemyColor
        /// </summary>
        /// <returns>The list of AlchemyColors that compose this InteractableAlchemyColor</returns>
        public virtual List<AlchemyColor> GetComponenti()
        {
            throw new NotImplementedException();
        }
    }
}
