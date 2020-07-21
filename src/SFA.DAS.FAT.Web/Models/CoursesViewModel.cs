using SFA.DAS.FAT.Domain.Courses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FAT.Web.Models
{
    public class CoursesViewModel
    {
        public List<CourseViewModel> Courses { get; set; }
        public string Keyword { get; set; }
        public int Total { get ; set ; }
        public int TotalFiltered { get ; set ; }
        public List<LevelViewModel> Levels { get; set; }
        public List<SectorViewModel> Sectors { get ; set ; }
        public List<Guid> SelectedSectors { get ; set ; }
        public List<int> SelectedLevels { get ; set ; }
        public OrderBy OrderBy { get; set; } = OrderBy.None;
        public bool ShowFilterOptions =>  ClearSectorLinks.Any() || ClearLevelLinks.Any() || !string.IsNullOrEmpty(Keyword);

        public string TotalMessage => GetTotalMessage();
        public Dictionary<string, string> ClearSectorLinks => BuildClearSectorFilterLinks();
        public string ClearKeywordLink => BuildClearKeywordFilterLink();
        public Dictionary<string, string> ClearLevelLinks => BuildClearLevelFilterLinks();
        public string OrderByName => BuildOrderByLink(OrderBy.Name);
        public string OrderByRelevance => BuildOrderByLink(OrderBy.Relevance);

        private string GetTotalMessage()
        {
            var totalToUse = string.IsNullOrEmpty(Keyword) 
                             && (SelectedSectors == null || !SelectedSectors.Any()) 
                             && (SelectedLevels == null || !SelectedLevels.Any())
                                    ? Total 
                                    : TotalFiltered;

            return $"{totalToUse} result" + (totalToUse!=1 ? "s": "");
        }
        private string BuildOrderByLink(OrderBy order)
        {
            var buildOrderByNameLink = !string.IsNullOrEmpty(Keyword) ? $"?keyword={Keyword}" : "";

            buildOrderByNameLink += !string.IsNullOrEmpty(order.ToString()) ? $"{GetSeparator(buildOrderByNameLink)}orderby={order}" : "";

            buildOrderByNameLink += SelectedSectors != null && SelectedSectors.Any() ? $"{GetSeparator(buildOrderByNameLink)}sectors=" + string.Join("&sectors=", SelectedSectors) : "";
            
            buildOrderByNameLink += SelectedLevels != null && SelectedLevels.Any() ? $"{GetSeparator(buildOrderByNameLink)}levels=" + string.Join("&levels=", SelectedLevels) : "";
            
            return buildOrderByNameLink;
        }

        private string BuildClearKeywordFilterLink()
        {
            var buildClearKeywordFilterLink = SelectedSectors != null && SelectedSectors.Any() 
                ? "?sectors=" + string.Join("&sectors=", SelectedSectors) : "";

            buildClearKeywordFilterLink += SelectedLevels!=null && SelectedLevels.Any() 
                ?  $"{GetSeparator(buildClearKeywordFilterLink)}levels=" + string.Join("&levels=", SelectedLevels) : "";
            
            return buildClearKeywordFilterLink;
        }

        private Dictionary<string, string> BuildClearSectorFilterLinks ( )
        {
            var clearFilterLinks = new Dictionary<string, string>();
            if (SelectedSectors == null)
            {
                return clearFilterLinks;
            }
            
            var levels = SelectedLevels!=null && SelectedLevels.Any() 
                ?  $"&levels=" + string.Join("&levels=", SelectedLevels) : "";
            
            foreach (var selectedSector in SelectedSectors)
            {
                var clearFilterString = BuildClearFilterStringForKeywordAndOrderBy();

                clearFilterString += $"{GetSeparator(clearFilterString)}sectors=" + string.Join("&sectors=", SelectedSectors.Where(c => !c.Equals(selectedSector)));
                clearFilterString += levels;
                
                var sector = Sectors.SingleOrDefault(c => c.Id.Equals(selectedSector));
                
                clearFilterLinks.Add(sector.Route, clearFilterString);
            }

            return clearFilterLinks;
        }

        private Dictionary<string, string> BuildClearLevelFilterLinks()
        {
            var clearLevelLink = new Dictionary<string,string>();
            if (SelectedLevels == null)
            {
                return clearLevelLink;
            }
            var sectors = SelectedSectors != null && SelectedSectors.Any() 
                ? "&sectors=" + string.Join("&sectors=", SelectedSectors) : "";
            
            foreach (var selectedLevel in SelectedLevels)
            {
                var clearFilterString = BuildClearFilterStringForKeywordAndOrderBy();

                clearFilterString += $"{GetSeparator(clearFilterString)}levels=" + string.Join("&levels=", SelectedLevels.Where(c => !c.Equals(selectedLevel)));
                clearFilterString += sectors;
                var sector = Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));
                
                clearLevelLink.Add(sector.Title, clearFilterString);
            }
            
            return clearLevelLink;
        }

        private string BuildClearFilterStringForKeywordAndOrderBy()
        {
            var clearFilterString = "";
            
            if (!string.IsNullOrEmpty(Keyword))
            {
                clearFilterString = $"?keyword={Keyword}";
            }

            if (OrderBy != OrderBy.None)
            {
                clearFilterString += $"{GetSeparator(clearFilterString)}orderby={OrderBy}";
            }

            return clearFilterString;
        }

        private string GetSeparator(string url)
        {
            return string.IsNullOrEmpty(url) ? "?" : "&";
        }
    }
}
