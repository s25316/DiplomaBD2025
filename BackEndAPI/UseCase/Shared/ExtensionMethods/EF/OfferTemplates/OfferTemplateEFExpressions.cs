using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.ExtensionMethods.EF.OfferTemplates
{
    public static class OfferTemplateEFExpressions
    {
        public static Expression<Func<OfferTemplate, bool>> SearchTextExpression(
            IEnumerable<string> searchWords)
        {
            return ot =>
                !searchWords.Any() ||
                searchWords.Any(word =>
                    ot.Name.Contains(word) ||
                    ot.Description.Contains(word) ||
                    (ot.Company.Name != null && ot.Company.Name.Contains(word)) ||
                    (ot.Company.Description != null && ot.Company.Description.Contains(word)));
        }

        public static Expression<Func<OfferTemplate, bool>> SkillsExpression(
           IEnumerable<int> skillIds)
        {
            return ot =>
                !skillIds.Any() ||
                skillIds.Any(skillId =>
                    ot.OfferSkills.Any(skill =>
                            skill.SkillId == skillId
                ));
        }
    }
}
