using ESAPIX.Constraints;
using System;
using VMS.TPS.Common.Model.API;

namespace ESAPX_StarterUI.ViewModels
{
    public class CTDateConstraint : IConstraint
    {
        public string Name => "CT Date Constraint";
        public string FullName => "CT < 60 days old";

        public ConstraintResult CanConstrain(PlanningItem pi)
        {
            var pq = new PQAsserter(pi);
            return pq.HasImage()
                .CumulativeResult;
        }

        public ConstraintResult Constrain(PlanningItem pi)
        {
            var ctDate = pi.StructureSet.Image.CreationDateTime;
            return ConstrainCTDate(ctDate.Value);
        }

        /// <summary>
        /// Tests whether or not a CT is older than 60 days. Fail if older
        /// </summary>
        /// <param name="ctDate"></param>
        /// <returns></returns>
        public ConstraintResult ConstrainCTDate(System.DateTime ctDate)
        {
            var daysOld = (DateTime.Now - ctDate).TotalDays;
            var limit = DateTime.Now.AddDays(-60);
            if (ctDate > limit)
            {
                return new ConstraintResult(this, ResultType.PASSED, $"The CT is {daysOld} days old");
            }
            else
            {
                return new ConstraintResult(this, ResultType.ACTION_LEVEL_3, $"The CT is {daysOld} days old");
            }
        }
    }
}