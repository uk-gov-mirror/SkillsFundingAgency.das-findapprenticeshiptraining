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
        public string OrderBy { get; set; }

        public string TotalMessage => GetTotalMessage();
        public Dictionary<string, string> ClearSectorLinks => BuildClearSectorFilterLinks();
        public string ClearKeywordLink => BuildClearKeywordFilterLink();
        public Dictionary<string, string> ClearLevelLinks => BuildClearLevelFilterLinks();
        public string OrderByName => BuildOrderByNameLink();
        public string OrderByRelevance => BuildOrderByRelevanceLink();

        private string GetTotalMessage()
        {
            var totalToUse = string.IsNullOrEmpty(Keyword) 
                             && (SelectedSectors == null || !SelectedSectors.Any()) 
                             && (SelectedLevels == null || !SelectedLevels.Any())
                                    ? Total 
                                    : TotalFiltered;

            return $"{totalToUse} result" + (totalToUse!=1 ? "s": "");
        }
        private string BuildOrderByRelevanceLink()
        {
            OrderBy = "relevance";

            var buildOrderByRelevanceLink = Keyword != null ? $"?keyword=" + string.Join("?keywords=", Keyword) : "";

            var separator = string.IsNullOrEmpty(buildOrderByRelevanceLink) ? "?" : "&";

            buildOrderByRelevanceLink += SelectedSectors != null && SelectedSectors.Any() ? $"{separator}sectors=" + string.Join("&sectors=", SelectedSectors) : "";
            buildOrderByRelevanceLink += SelectedLevels != null && SelectedLevels.Any() ? $"{separator}levels=" + string.Join("&levels=", SelectedLevels) : "";
            buildOrderByRelevanceLink += buildOrderByRelevanceLink.Count() > 0 ? $"&orderby=" + string.Join("&orderby=", OrderBy) : "?orderby=" + string.Join($"&orderby=", OrderBy);

            return buildOrderByRelevanceLink;
        }

        private string BuildOrderByNameLink()
        {
            OrderBy = "name";

            var buildOrderByNameLink = Keyword != null ? $"?keyword=" + string.Join("?keywords=", Keyword) : "";

            var separator = string.IsNullOrEmpty(buildOrderByNameLink) ? "?" : "&";

            buildOrderByNameLink += SelectedSectors != null && SelectedSectors.Any() ? $"{separator}sectors=" + string.Join("&sectors=", SelectedSectors) : "";
            buildOrderByNameLink += SelectedLevels != null && SelectedLevels.Any() ? $"{separator}levels=" + string.Join("&levels=", SelectedLevels) : "";
            buildOrderByNameLink += buildOrderByNameLink.Count() > 0 ? $"&orderby=" + string.Join("&orderby=", OrderBy) : "?orderby=" + string.Join($"&orderby=", OrderBy);

            return buildOrderByNameLink;
        }

        private string BuildClearKeywordFilterLink()
        {
            var buildClearKeywordFilterLink = SelectedSectors != null && SelectedSectors.Any() 
                ? "?sectors=" + string.Join("&sectors=", SelectedSectors) : "";

            var separator = string.IsNullOrEmpty(buildClearKeywordFilterLink) ? "?" : "&";
            
            buildClearKeywordFilterLink += SelectedLevels!=null && SelectedLevels.Any() 
                ?  $"{separator}levels=" + string.Join("&levels=", SelectedLevels) : "";
            
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
                var clearFilterString = string.Empty;
                var separator = "?";
                if (!string.IsNullOrEmpty(Keyword))
                {
                    clearFilterString = $"?keyword={Keyword}";
                    separator = "&";
                }

                clearFilterString += $"{separator}sectors=" + string.Join("&sectors=", SelectedSectors.Where(c => !c.Equals(selectedSector)));
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
                var clearFilterString = string.Empty;
                var separator = "?";
                if (!string.IsNullOrEmpty(Keyword))
                {
                    clearFilterString = $"?keyword={Keyword}";
                    separator = "&";
                }

                clearFilterString += $"{separator}levels=" + string.Join("&levels=", SelectedLevels.Where(c => !c.Equals(selectedLevel)));
                clearFilterString += sectors;
                var sector = Levels.SingleOrDefault(c => c.Code.Equals(selectedLevel));
                
                clearLevelLink.Add(sector.Title, clearFilterString);
            }
            
            return clearLevelLink;
        }
    }
}
