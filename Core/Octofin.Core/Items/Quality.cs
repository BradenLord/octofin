namespace Octofin.Core.Items
{
    /// <summary>
    /// Specifies the current quality of an item.  Quality degrades one stage (Excellent -> Damaged)
    /// on every death.  If equipment is Damaged at death it is automatically scrapped.  If a piece of
    /// equipment that contains enhancements is automatically scrapped the enhancements are also scrapped
    /// regardless of their quality (an inherent risk of slotting enhancements on damaged equipment).
    /// 
    /// Each quality has a descriptor of the bonus/penalty to combat stats and modifiers it provides.
    /// Quality does not effect set bonuses, other unique bonuses, or counters.
    /// 
    /// </summary>
    public enum Quality
    {
        /// <summary>
        /// 50% Bonus
        /// </summary>
        Excellent = 6, 

        /// <summary>
        /// 25% Bonus
        /// </summary>
        Strong = 5,

        /// <summary>
        /// 10% Bonus
        /// </summary>
        Good = 4,

        /// <summary>
        /// No bonus or penalty (and should not be added to equipment label).
        /// </summary>
        Average = 3,

        /// <summary>
        /// 10% Penalty
        /// </summary>
        Poor = 2,

        /// <summary>
        /// 25% Penalty
        /// </summary>
        Weak = 1,

        /// <summary>
        /// 50% Penalty
        /// </summary>
        Damaged = 0
    }
}
